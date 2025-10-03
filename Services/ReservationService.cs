using Npgsql;
using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for reservation-related database operations
    /// </summary>
    public class ReservationService : BaseService
    {
        public ReservationService(PostgreSqlDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all reservations from the database with customer information
        /// </summary>
        public async Task<List<Reservation>> GetAllAsync()
        {
            var reservations = new List<Reservation>();
            var sql = $@"
                SELECT r.id, r.reservation_date, r.reservation_time, r.number_of_people, r.notes, r.customer_id,
                       c.first_name, c.last_name
                FROM {ApplicationConstants.ReservationsTable} r
                LEFT JOIN {ApplicationConstants.CustomersTable} c ON r.customer_id = c.id
                ORDER BY r.id";
            
            using var reader = await ExecuteReaderAsync(sql);
            
            while (await reader.ReadAsync())
            {
                reservations.Add(MapReservationFromReader(reader));
            }
            
            return reservations;
        }

        /// <summary>
        /// Gets a reservation by ID
        /// </summary>
        public async Task<Reservation?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var sql = $@"
                SELECT r.id, r.reservation_date, r.reservation_time, r.number_of_people, r.notes, r.customer_id,
                       c.first_name, c.last_name
                FROM {ApplicationConstants.ReservationsTable} r
                LEFT JOIN {ApplicationConstants.CustomersTable} c ON r.customer_id = c.id
                WHERE r.id = @id";
            
            using var reader = await ExecuteReaderAsync(sql, CreateParameter("id", id));
            
            if (await reader.ReadAsync())
            {
                return MapReservationFromReader(reader);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new reservation
        /// </summary>
        public async Task<OperationResult<Reservation>> CreateAsync(Reservation reservation)
        {
            try
            {
                var validation = ValidationService.ValidateReservation(reservation);
                if (!validation.IsValid)
                {
                    return OperationResult<Reservation>.Failure(validation.GetErrorsAsString());
                }

                // Verify customer exists
                var customerExists = await CustomerExistsAsync(reservation.CustomerId);
                if (!customerExists)
                {
                    return OperationResult<Reservation>.Failure("El cliente especificado no existe");
                }

                var sql = $@"
                    INSERT INTO {ApplicationConstants.ReservationsTable} (reservation_date, reservation_time, number_of_people, notes, customer_id) 
                    VALUES (@reservationDate, @reservationTime, @numberOfPeople, @notes, @customerId) 
                    RETURNING id";

                var parameters = new[]
                {
                    CreateParameter("reservationDate", reservation.ReservationDate),
                    CreateParameter("reservationTime", reservation.ReservationTime),
                    CreateParameter("numberOfPeople", reservation.NumberOfPeople),
                    CreateParameter("notes", string.IsNullOrWhiteSpace(reservation.Notes) ? null : reservation.Notes),
                    CreateParameter("customerId", reservation.CustomerId)
                };

                var result = await ExecuteScalarAsync(sql, parameters);
                if (result != null)
                {
                    reservation.Id = Convert.ToInt32(result);
                    return OperationResult<Reservation>.Success(reservation, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<Reservation>.Failure(ApplicationConstants.ErrorMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<Reservation>.Failure($"Error al crear reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing reservation
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(Reservation reservation)
        {
            try
            {
                var validation = ValidationService.ValidateReservation(reservation);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(validation.GetErrorsAsString());
                }

                // Verify customer exists
                var customerExists = await CustomerExistsAsync(reservation.CustomerId);
                if (!customerExists)
                {
                    return OperationResult<bool>.Failure("El cliente especificado no existe");
                }

                var sql = $@"
                    UPDATE {ApplicationConstants.ReservationsTable} 
                    SET reservation_date = @reservationDate, reservation_time = @reservationTime, 
                        number_of_people = @numberOfPeople, notes = @notes, customer_id = @customerId 
                    WHERE id = @id";

                var parameters = new[]
                {
                    CreateParameter("id", reservation.Id),
                    CreateParameter("reservationDate", reservation.ReservationDate),
                    CreateParameter("reservationTime", reservation.ReservationTime),
                    CreateParameter("numberOfPeople", reservation.NumberOfPeople),
                    CreateParameter("notes", string.IsNullOrWhiteSpace(reservation.Notes) ? null : reservation.Notes),
                    CreateParameter("customerId", reservation.CustomerId)
                };

                var rowsAffected = await ExecuteNonQueryAsync(sql, parameters);
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al actualizar reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a reservation by ID
        /// </summary>
        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de reserva inválido");
                }

                var sql = $"DELETE FROM {ApplicationConstants.ReservationsTable} WHERE id = @id";
                var rowsAffected = await ExecuteNonQueryAsync(sql, CreateParameter("id", id));
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al eliminar reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a customer exists
        /// </summary>
        private async Task<bool> CustomerExistsAsync(int customerId)
        {
            var sql = $"SELECT COUNT(1) FROM {ApplicationConstants.CustomersTable} WHERE id = @id";
            var result = await ExecuteScalarAsync(sql, CreateParameter("id", customerId));
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Maps a data reader row to a Reservation object
        /// </summary>
        private static Reservation MapReservationFromReader(NpgsqlDataReader reader)
        {
            return new Reservation
            {
                Id = reader.GetInt32(0),
                ReservationDate = reader.GetDateTime(1),
                ReservationTime = reader.GetTimeSpan(2),
                NumberOfPeople = reader.GetInt32(3),
                Notes = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                CustomerId = reader.GetInt32(5),
                Customer = new Customer
                {
                    Id = reader.GetInt32(5),
                    FirstName = reader.GetString(6),
                    LastName = reader.GetString(7)
                }
            };
        }
    }
}

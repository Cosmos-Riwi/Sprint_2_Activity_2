# Sistema de Restaurante

## Descripción
Sistema completo de gestión de restaurante desarrollado como aplicación web con ASP.NET Core MVC y PostgreSQL. El sistema maneja todas las operaciones diarias del restaurante incluyendo clientes, meseros, platos, pedidos y reservas a través de una interfaz web moderna con tema oscuro.

## Características

### Entidades Gestionadas
1. **Clientes** - Información personal de los clientes (nombre, apellido, correo, teléfono)
2. **Meseros** - Personal de servicio con turnos y años de experiencia
3. **Platos** - Menú del restaurante con precios y categorías (entrada, plato fuerte, postre, bebida)
4. **Pedidos** - Órdenes de clientes con estados (pendiente, servido, cancelado)
5. **Reservas** - Reservas de mesa con fecha, hora y número de personas

### Funcionalidad CRUD Completa
- **Crear** - Agregar nuevos registros con validaciones
- **Leer** - Consultar y listar registros con paginación
- **Actualizar** - Modificar registros existentes
- **Eliminar** - Remover registros con confirmación
- **Detalles** - Ver información completa de cada registro

### Validaciones Implementadas
- Precios de platos deben ser mayores a 0
- Estados de pedidos deben ser válidos (pendiente, servido, cancelado)
- Número de personas en reservas debe ser mayor a 0 y menor a 50
- Validación de formato de correo electrónico
- Validación de categorías de platos (entrada, plato fuerte, postre, bebida)
- Validación de formato de teléfono
- Validaciones de longitud de campos para todos los campos
- Validaciones de fechas (no fechas futuras para pedidos, no fechas pasadas para reservas)

### Relaciones de Base de Datos
- **Customers → Orders** (Uno a Muchos)
- **Customers → Reservations** (Uno a Muchos)

## Configuración de Base de Datos

### Credenciales de Conexión
- **Host:** 168.119.183.3
- **Port:** 5432
- **Username:** root
- **Password:** s7cq453mt2jnicTaQXKT
- **Database:** restaurante_jeronimo

### Tablas Creadas Automáticamente
El sistema crea automáticamente las siguientes tablas:
- `customers` - Información de clientes
- `waiters` - Información de meseros
- `dishes` - Menú del restaurante
- `orders` - Pedidos de clientes
- `reservations` - Reservas de mesa

## Instalación y Ejecución

### Prerrequisitos
- .NET 8.0 SDK
- Acceso a base de datos PostgreSQL
- Base de datos `restaurante_jeronimo` creada en pgAdmin

### Pasos para Ejecutar
1. Clonar o descargar el proyecto
2. Abrir terminal en la carpeta del proyecto
3. Restaurar paquetes NuGet:
   ```bash
   dotnet restore
   ```
4. Compilar el proyecto:
   ```bash
   dotnet build
   ```
5. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```
6. Abrir navegador en: http://localhost:5000

## Estructura del Proyecto

```
Sprint_2_Activity_2/
├── Models/                          # Modelos de entidades
│   ├── Customer.cs                  # Entidad Cliente
│   ├── Waiter.cs                    # Entidad Mesero
│   ├── Dish.cs                      # Entidad Plato con categorías
│   ├── Order.cs                     # Entidad Pedido con estados
│   ├── Reservation.cs               # Entidad Reserva
│   ├── OperationResult.cs           # Wrapper de resultados de operaciones
│   └── ErrorViewModel.cs            # Modelo de errores
├── Data/                            # Capa de base de datos
│   └── PostgreSqlDbContext.cs       # Contexto de base de datos PostgreSQL
├── Services/                        # Servicios de lógica de negocio
│   ├── BaseService.cs               # Servicio base con operaciones comunes
│   ├── CustomerService.cs           # Operaciones CRUD de clientes
│   ├── WaiterService.cs             # Operaciones CRUD de meseros
│   ├── DishService.cs               # Operaciones CRUD de platos
│   ├── OrderService.cs              # Operaciones CRUD de pedidos
│   ├── ReservationService.cs        # Operaciones CRUD de reservas
│   └── ValidationService.cs         # Servicio de validación de datos
├── Controllers/                     # Controladores MVC
│   ├── HomeController.cs            # Controlador principal
│   ├── CustomerController.cs        # Controlador de clientes
│   ├── WaiterController.cs          # Controlador de meseros
│   ├── DishController.cs            # Controlador de platos
│   ├── OrderController.cs           # Controlador de pedidos
│   └── ReservationController.cs     # Controlador de reservas
├── Views/                           # Vistas Razor
│   ├── Shared/                      # Vistas compartidas
│   │   ├── _Layout.cshtml           # Layout principal con tema oscuro
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Home/                        # Vistas del dashboard
│   │   └── Index.cshtml
│   ├── Customer/                    # Vistas de clientes
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Delete.cshtml
│   │   └── Details.cshtml
│   ├── Waiter/                      # Vistas de meseros
│   ├── Dish/                        # Vistas de platos
│   ├── Order/                       # Vistas de pedidos
│   └── Reservation/                 # Vistas de reservas
├── Configuration/                   # Clases de configuración
│   ├── DatabaseConfiguration.cs     # Configuración de conexión a BD
│   └── ApplicationConstants.cs      # Constantes de la aplicación
├── Program.cs                       # Punto de entrada de la aplicación
└── Sprint_2_Activity_2.csproj      # Configuración del proyecto
```

## Uso del Sistema

### Interfaz Web
1. **Dashboard Principal**: Vista general con acceso a todas las entidades
2. **Navegación**: Menú superior para acceso rápido a cada sección
3. **Operaciones CRUD**: Para cada entidad se puede:
   - Listar todos los registros
   - Crear nuevos registros
   - Ver detalles de registros
   - Editar registros existentes
   - Eliminar registros

### Características de la Interfaz
- **Tema Oscuro**: Interfaz completamente en tema oscuro con colores neutros
- **Responsive**: Diseño adaptable a diferentes tamaños de pantalla
- **Validaciones en Tiempo Real**: Validaciones tanto del lado cliente como servidor
- **Mensajes de Confirmación**: Feedback claro para todas las operaciones
- **Navegación Intuitiva**: Interfaz fácil de usar y navegar

## Mensajes de Confirmación
Todas las operaciones CRUD muestran mensajes de confirmación:
- **SUCCESS** - Operación exitosa
- **ERROR** - Error en la operación
- **WARNING** - Mensajes de advertencia
- **INFO** - Mensajes informativos
- **Validaciones** - Validaciones de datos antes de guardar

## Características Técnicas

### Mejoras de Arquitectura
- **Arquitectura Limpia**: Separación de responsabilidades con capas de Models, Services, Controllers y Views
- **Inyección de Dependencias**: Inyección adecuada de servicios y gestión del ciclo de vida
- **Patrón Base Service**: Operaciones comunes de base de datos en clase base
- **Patrón Operation Result**: Manejo consistente de errores y reporte de resultados
- **Gestión de Configuración**: Configuración centralizada y constantes

### Calidad del Código
- **Nomenclatura en Inglés**: Todo el código, clases y métodos en inglés
- **Documentación Completa**: Documentación XML para todos los miembros públicos
- **Manejo de Errores**: Manejo robusto de excepciones y mensajes de error amigables
- **Validación**: Validación completa de entrada con mensajes de error detallados
- **Async/Await**: Programación completamente asíncrona para mejor rendimiento
- **Gestión de Recursos**: Disposición adecuada de conexiones de base de datos y recursos

### Características de Base de Datos
- **Creación Automática de Tablas**: Tablas creadas automáticamente en el primer ejecución
- **Índices**: Índices de rendimiento en columnas consultadas frecuentemente
- **Restricciones**: Restricciones a nivel de base de datos para integridad de datos
- **Claves Foráneas**: Relaciones adecuadas con eliminación en cascada
- **Pool de Conexiones**: Gestión eficiente de conexiones de base de datos

## Tecnologías Utilizadas
- **ASP.NET Core 8.0** - Framework web MVC
- **C# .NET 8.0** - Lenguaje de programación
- **PostgreSQL** con **Npgsql 8.0.5** - Base de datos
- **Bootstrap 5** - Framework CSS
- **Razor Pages** - Motor de vistas (.cshtml)
- **Programación Asíncrona** (async/await)
- **Validación de Datos Personalizada**
- **Interfaz Web Responsiva** con UX mejorada
- **Tema Oscuro Personalizado**

## Notas de Desarrollo
- Sistema diseñado para ser robusto y manejar errores de conexión graciosamente
- Las validaciones aseguran integridad de datos tanto a nivel de aplicación como de base de datos
- La interfaz es intuitiva y fácil de usar con mensajes de error claros
- Todas las operaciones son asíncronas para mejor rendimiento
- Separación limpia de responsabilidades para mantenibilidad
- Manejo completo de errores y logging
- Interfaz completamente en tema oscuro con colores neutros
- Placeholders y texto en blanco para máxima legibilidad
- Navegación consistente y funcional en todas las secciones

## Estado del Proyecto  
- **Compilación**: Exitosa sin errores ni advertencias
- **Funcionalidad**: 100% operativa
- **Interfaz**: Completamente implementada con tema oscuro
- **Base de Datos**: Configurada y funcionando
- **CRUD**: Implementado completamente para las 5 entidades
- **Validaciones**: Implementadas tanto en cliente como servidor
- **Relaciones**: Funcionando correctamente entre entidades
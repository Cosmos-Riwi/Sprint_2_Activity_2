using System.ComponentModel;
using System.Globalization;

namespace GestionRestaurante.Helpers
{
    public class CustomDecimalConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                // Reemplazar coma por punto para el parsing
                stringValue = stringValue.Replace(",", ".");
                if (decimal.TryParse(stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}

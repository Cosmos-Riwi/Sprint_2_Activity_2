namespace GestionRestaurante.Helpers
{
    public static class PriceHelper
    {
        public static string FormatPrice(decimal price)
        {
            return $"$ {price.ToString("N2").Replace(".", ",")}";
        }

        public static string FormatPrice(decimal? price)
        {
            if (price == null) return "$ 0,00";
            return FormatPrice(price.Value);
        }
    }
}

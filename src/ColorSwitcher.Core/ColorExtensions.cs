using System.Text.RegularExpressions;

namespace ColorSwitcher.Core
{
    public static class ColorExtensions
    {
        private static readonly Regex HexRegex = new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$");

        public static bool IsHexFormat(this string text)
        {
            return HexRegex.IsMatch(text);
        }
    }
}
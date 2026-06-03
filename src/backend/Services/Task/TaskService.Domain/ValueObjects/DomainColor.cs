namespace TaskService.Domain.ValueObjects
{
    public readonly record struct DomainColor(byte R, byte G, byte B, byte A = 255)
    {
        public static DomainColor FromHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentNullException(nameof(hex));

            hex = hex.TrimStart('#');
            if (hex.Length is not 6 and not 8)
                throw new FormatException($"Hex must be 6 or 8 chars. Got: {hex}");

            if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var argb))
                throw new FormatException("Invalid hex characters.");

            var a = hex.Length == 8 ? (byte)(argb >> 24) : (byte)255;
            var r = (byte)((argb >> 16) & 0xFF);
            var g = (byte)((argb >> 8) & 0xFF);
            var b = (byte)(argb & 0xFF);

            return new DomainColor(r, g, b, a);
        }

        public string ToHex(bool includeAlpha = false) =>
            includeAlpha ? $"#{A:X2}{R:X2}{G:X2}{B:X2}" : $"#{R:X2}{G:X2}{B:X2}";

        public static implicit operator string(DomainColor c) => c.ToHex();
    }
}

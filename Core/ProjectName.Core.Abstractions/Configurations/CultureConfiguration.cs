using System.Collections.Generic;

namespace ProjectName.Core.Abstractions.Configurations
{
    public class CultureConfiguration
    {
        private static readonly string[] AvailableCultures = { "en", "ru", "uz" };
        private static readonly string DefaultRequestCulture = "en";

        public CultureConfiguration()
        {
            Cultures = new List<string>();
            Cultures.AddRange(AvailableCultures);
        }

        public List<string> Cultures { get; }
        public string DefaultCulture => DefaultRequestCulture;
    }
}
namespace Expensely.Application.Core.Options
{
    /// <summary>
    /// Represents the caching options.
    /// </summary>
    public class CachingOptions
    {
        /// <summary>
        /// The caching options settings key.
        /// </summary>
        public const string SettingsKey = "Caching";

        /// <summary>
        /// Gets or sets a value indicating whether or not caching is enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}

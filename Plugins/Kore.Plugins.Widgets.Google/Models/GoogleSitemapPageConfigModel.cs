using Kore.Plugins.Widgets.Google.Data.Domain;

namespace Kore.Plugins.Widgets.Google.Models
{
    public class GoogleSitemapPageConfigModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Priority { get; set; }
    }
}
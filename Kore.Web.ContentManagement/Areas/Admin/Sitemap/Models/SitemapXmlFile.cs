using System.Collections.Generic;
using System.Xml.Serialization;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Models
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SitemapXmlFile
    {
        public SitemapXmlFile()
        {
            Urls = new List<UrlElement>();
        }

        [XmlElement("url")]
        public List<UrlElement> Urls { get; set; }
    }

    public class UrlElement
    {
        [XmlElement("loc")]
        public string Location { get; set; }

        /// <summary>
        /// Format: dd-MM-yyyy
        /// </summary>
        [XmlElement("lastmod")]
        public string LastModified { get; set; }

        [XmlElement("changefreq")]
        public ChangeFrequency ChangeFrequency { get; set; }

        [XmlElement("priority")]
        public float Priority { get; set; }
    }
}
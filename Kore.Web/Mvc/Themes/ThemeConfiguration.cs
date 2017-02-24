using System.Xml;

namespace Kore.Web.Mvc.Themes
{
    public class ThemeConfiguration
    {
        public ThemeConfiguration(string themeName, XmlDocument doc)
        {
            ThemeName = themeName;
            var node = doc.SelectSingleNode("Theme");
            if (node != null)
            {
                ConfigurationNode = node;

                var attribute = node.Attributes["title"];
                ThemeTitle = attribute == null ? string.Empty : attribute.Value;

                attribute = node.Attributes["supportRTL"];
                SupportRtl = attribute == null ? false : bool.Parse(attribute.Value);

                attribute = node.Attributes["mobileTheme"];
                MobileTheme = attribute == null ? false : bool.Parse(attribute.Value);

                attribute = node.Attributes["previewImageUrl"];
                PreviewImageUrl = attribute == null ? string.Empty : attribute.Value;

                attribute = node.Attributes["previewText"];
                PreviewText = attribute == null ? string.Empty : attribute.Value;

                attribute = node.Attributes["defaultLayoutPath"];
                DefaultLayoutPath = attribute == null ? string.Empty : attribute.Value;
            }
        }

        public XmlNode ConfigurationNode { get; protected set; }

        public string DefaultLayoutPath { get; protected set; }

        public string PreviewImageUrl { get; protected set; }

        public string PreviewText { get; protected set; }

        public bool SupportRtl { get; protected set; }

        public bool MobileTheme { get; protected set; }

        public string ThemeName { get; protected set; }

        public string ThemeTitle { get; protected set; }
    }
}
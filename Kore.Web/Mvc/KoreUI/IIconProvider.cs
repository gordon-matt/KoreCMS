//namespace Kore.Web.Mvc.KoreUI
//{
//    public interface IIconProvider
//    {
//        string SetName { get; }

//        string GetIconCssClass(string icon);
//    }

//    public class Bootstrap3IconProvider : IIconProvider
//    {
//        #region IIconProvider Members

//        public string SetName
//        {
//            get { return "Bootstrap 3 Glyphicons"; }
//        }

//        public string GetIconCssClass(string icon)
//        {
//            switch (icon)
//            {
//                case "check": return "glyphicon glyphicon-ok";
//                case "check-circle": return "glyphicon glyphicon-ok-circle";
//                case "config": return "glyphicon glyphicon-cog";
//                case "plugins": return "glyphicon glyphicon-gift";
//                case "refresh": return "glyphicon glyphicon-refresh";
//                case "scheduled-tasks": return "glyphicon glyphicon-time";
//                case "search": return "glyphicon glyphicon-search";
//                case "settings": return "glyphicon glyphicon-cogs";
//                case "themes": return "glyphicon glyphicon-tint";
//                default: return null;
//            }
//        }

//        #endregion
//    }

//    public class FontAwesomeIconProvider : IIconProvider
//    {
//        #region IIconProvider Members

//        public string SetName
//        {
//            get { return "Font Awesome"; }
//        }

//        public string GetIconCssClass(string icon)
//        {
//            switch (icon)
//            {
//                case "check": return "fa fa-check";
//                case "check-circle": return "fa fa-check-circle";
//                case "config": return "fa fa-cog";
//                case "plugins": return "fa fa-puzzle-piece";
//                case "refresh": return "fa fa-refresh";
//                case "scheduled-tasks": return "fa fa-clock-o";
//                case "search": return "fa fa-search";
//                case "settings": return "fa fa-cogs";
//                case "themes": return "fa fa-tint";
//                default: return null;
//            }
//        }

//        #endregion
//    }
//}
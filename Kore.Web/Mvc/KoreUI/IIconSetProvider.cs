//using System.Linq;
//using Kore.Infrastructure;

//namespace Kore.Web.Mvc.KoreUI
//{
//    public interface IIconSet
//    {
//        string Name { get; }

//        string GetIconCssClass(string icon);
//    }

//    public abstract class BaseIconSet : IIconSet
//    {
//        public abstract string Name { get; }

//        public virtual string GetIconCssClass(string icon)
//        {
//            var iconProviders = EngineContext.Current.ResolveAll<IIconProvider>()
//                .Where(x => x.SetName == Name);

//            foreach (var provider in iconProviders)
//            {
//                string result = provider.GetIconCssClass(icon);
//                if (!string.IsNullOrEmpty(result))
//                {
//                    return result;
//                }
//            }
//            return icon;
//        }
//    }

//    public class Bootstrap3IconSet : BaseIconSet
//    {
//        public override string Name
//        {
//            get { return "Bootstrap 3 Glyphicons"; }
//        }
//    }

//    public class FontAwesomeIconSet : BaseIconSet
//    {
//        public override string Name
//        {
//            get { return "Font Awesome"; }
//        }
//    }
//}
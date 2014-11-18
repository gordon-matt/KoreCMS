//using System.Collections.Generic;

//namespace Kore.Web.Mvc.Optimization
//{
//    public class BundleCollection
//    {
//        private static readonly IDictionary<string, IEnumerable<string>> scripts = new Dictionary<string, IEnumerable<string>>();
//        private static readonly IDictionary<string, IEnumerable<string>> styles = new Dictionary<string, IEnumerable<string>>();

//        public static IDictionary<string, IEnumerable<string>> Scripts
//        {
//            get { return scripts; }
//        }

//        public static IDictionary<string, IEnumerable<string>> Styles
//        {
//            get { return styles; }
//        }

//        public static void AddScript(string bundleName, params string[] files)
//        {
//            scripts.Add(bundleName, new List<string>(files));
//        }

//        public static void AddStyle(string bundleName, params string[] files)
//        {
//            styles.Add(bundleName, new List<string>(files));
//        }
//    }
//}
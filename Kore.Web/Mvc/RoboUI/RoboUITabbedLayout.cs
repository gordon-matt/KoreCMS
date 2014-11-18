using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUITabbedLayout<TModel>
    {
        private readonly IList<RoboUIGroupedLayout<TModel>> groups;

        public RoboUITabbedLayout(string title)
        {
            Title = title;
            groups = new List<RoboUIGroupedLayout<TModel>>();
        }

        public IList<RoboUIGroupedLayout<TModel>> Groups { get { return groups; } }

        public string Title { get; set; }

        public RoboUIGroupedLayout<TModel> AddGroup(string title = null)
        {
            var group = new RoboUIGroupedLayout<TModel>(title);
            groups.Add(group);
            return group;
        }
    }
}
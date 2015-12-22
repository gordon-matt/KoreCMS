﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Plugins.Messaging.Forums.Models
{
    public class TopicMoveModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int Id { get; set; }

        public int ForumSelected { get; set; }

        public string TopicSeName { get; set; }

        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}
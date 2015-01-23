﻿using System;
using Kore.Data;

namespace Kore.Security.Membership
{
    public class KoreUserProfileEntry : IEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new[] { Id }; }
        }

        #endregion IEntity Members
    }
}
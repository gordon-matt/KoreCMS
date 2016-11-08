﻿using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain
{
    public class MessageTemplate : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the owner of template
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the template is enable
        /// </summary>
        public bool Enabled { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class MessageTemplateMap : EntityTypeConfiguration<MessageTemplate>, IEntityTypeConfiguration
    {
        public MessageTemplateMap()
        {
            ToTable(CmsConstants.Tables.MessageTemplates);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Subject).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Body).IsRequired().IsMaxLength().IsUnicode(true);
            Property(x => x.Enabled).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}
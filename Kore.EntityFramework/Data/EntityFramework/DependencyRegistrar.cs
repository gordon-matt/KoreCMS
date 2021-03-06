﻿using System;
using System.Linq;
using Autofac;
using Kore.Infrastructure;

namespace Kore.Data.EntityFramework
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var entityTypeConfigurations = typeFinder
                .FindClassesOfType(typeof(IEntityTypeConfiguration))
                .ToHashSet();

            foreach (var configuration in entityTypeConfigurations)
            {
                if (configuration.IsGenericType)
                {
                    continue;
                }

                var isEnabled = (Activator.CreateInstance(configuration) as IEntityTypeConfiguration).IsEnabled;

                if (isEnabled)
                {
                    builder.RegisterType(configuration).As(typeof(IEntityTypeConfiguration)).InstancePerLifetimeScope();
                }
            }
        }

        public int Order => 0;

        #endregion IDependencyRegistrar Members
    }
}
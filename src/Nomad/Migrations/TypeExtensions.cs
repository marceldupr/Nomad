using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nomad.Framework;

namespace Nomad.Migrations
{
    public static class TypeExtensions
    {
        public static IList<ChangeSet> GetChangeSetsSince(this Assembly assembly, int version)
        {
            var types = assembly.GetTypes().Where(x => typeof (Migration).IsAssignableFrom(x));
            
            return types
                .Select(x => new ChangeSet { Version = GetVersion(x), Change = Activator.CreateInstance(x) as Migration})
                .Where(x=>x.Version > version)
                .OrderBy(x=>x.Version)
                .ToList();
        }

        private static long GetVersion(Type x)
        {
            var attribute = x.GetCustomAttributes(typeof(MigrationAttribute), true)
                                .FirstOrDefault() as MigrationAttribute;

            if (attribute != null)
                return attribute.Version;

            return -1;
        }
    }
}
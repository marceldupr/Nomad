using System;

namespace Nomad.Framework
{
    public class MigrationAttribute : Attribute
    {
        private readonly long version;

        public MigrationAttribute(long version)
        {
            this.version = version;
        }

        public long Version
        {
            get { return version; }
        }
    }
}

using System;

namespace Nomad.Framework
{
    public class MigrationAttribute : Attribute
    {
        private readonly int version;

        public MigrationAttribute(int version)
        {
            this.version = version;
        }

        public int Version
        {
            get { return version; }
        }
    }
}

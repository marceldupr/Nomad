using Nomad.Framework;

namespace Nomad.Migrations
{
    public class ChangeSet
    {
        public long Version { get; set; }
        public Migration Change { get; set; }
    }
}

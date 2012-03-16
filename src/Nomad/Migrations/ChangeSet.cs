using Nomad.Framework;

namespace Nomad.Migrations
{
    public class ChangeSet
    {
        public int Version { get; set; }
        public Migration Change { get; set; }
    }
}

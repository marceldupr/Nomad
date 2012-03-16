namespace Nomad.Framework
{
    public abstract class Migration
    {
        public abstract void Execute();
        public IDatabase Database { get; set; }
    }
}

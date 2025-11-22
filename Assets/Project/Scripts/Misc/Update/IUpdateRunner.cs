namespace Misc
{
    public interface IUpdateRunner
    {
        public void Register(IUpdatable updatable);

        public void Unregister(IUpdatable updatable);
    }
}

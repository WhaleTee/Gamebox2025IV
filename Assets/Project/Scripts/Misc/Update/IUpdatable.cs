namespace Misc
{
    public interface IUpdatable
    {
        public bool Enabled { get; }

        public void Update(float deltaTime);
    }
}

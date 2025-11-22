namespace Combat.Projectiles.Behaviours
{
    public struct Ricochet
    {
        public int Count;
        public float TimeSinceStart;
        public float Deadline;

        public void Hit()
        {
            Count++;
        }
    }
}
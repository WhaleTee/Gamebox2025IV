namespace Combat
{
    public interface IDamageSource
    {
        public bool TryGetComponent<T>(out T comp);
    }
}
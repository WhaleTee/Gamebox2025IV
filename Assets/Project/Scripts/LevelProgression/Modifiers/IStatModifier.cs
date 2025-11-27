namespace LevelProgression
{
    public interface IStatModifier<T>
    {
        T Apply(T value);
    }
}
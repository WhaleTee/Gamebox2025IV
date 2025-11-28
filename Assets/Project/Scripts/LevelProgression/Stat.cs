using System.Collections.Generic;

namespace LevelProgression
{
    public class Stat<T>
    {
        public T BaseValue { get; private set; }
        public T CalculatedValue { get; private set; }

        private List<IStatModifier<T>> modifiers = new();

        public Stat(T value) => SetBase(value);

        public Stat<T> SetBase(T value)
        {
            BaseValue = value;
            Recalculate();
            return this;
        }

        public void Add(IStatModifier<T> modifier)
        {
            modifiers.Add(modifier);
            Recalculate();
        }

        public void Remove(IStatModifier<T> modifier)
        {
            modifiers.Remove(modifier);
            Recalculate();
        }

        private void Recalculate()
        {
            T value = BaseValue;
            foreach (var mod in modifiers)
                value = mod.Apply(value);

            CalculatedValue = value;
        }
    }
}
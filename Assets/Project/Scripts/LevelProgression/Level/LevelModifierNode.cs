using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelProgression
{
    /// <summary>
    /// Прост хранит список модификаторов в кучке.
    /// </summary>
    [Serializable]
    public class LevelModifierNode : IEnumerable<ModifierBase>
    {
        [field: SerializeField] public ModifierBase[] Modifiers;
        public ModifierBase this[int index] => Modifiers[index];

        public IEnumerator<ModifierBase> GetEnumerator()
        {
            if (Modifiers == null)
                yield break;

            foreach (var m in Modifiers)
                yield return m;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
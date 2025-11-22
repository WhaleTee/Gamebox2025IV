using System;
using UnityEngine;
using UnityEditor;
using Combat.Projectiles.Behaviours;

namespace Assets.Editor.Combat
{
    [CustomPropertyDrawer(typeof(BehavioursAttribute))]
    public class BehavioursDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var behavioursProp = property.FindPropertyRelative("Behaviours");
            var configsProp = property.FindPropertyRelative("Configs");
            BehaviourDrawerUtil.DrawBehaviourSelector(property.serializedObject, behavioursProp, configsProp);
            EditorGUI.EndProperty();
        }
    }

    public static class BehaviourDrawerUtil
    {
        public static void DrawBehaviourSelector(
            SerializedObject serializedObject,
            SerializedProperty behavioursProp,
            SerializedProperty configsProp)
        {
            EditorGUILayout.PropertyField(behavioursProp);

            BehaviourType behaviours = (BehaviourType)behavioursProp.intValue;
            SyncConfigsWithFlags(configsProp, behaviours);

            EditorGUI.indentLevel++;
            int configIndex = 0;

            for (int bit = 0; bit < 32; bit++)
            {
                BehaviourType flag = (BehaviourType)(1 << bit);
                if (!behaviours.HasFlag(flag))
                    continue;

                if (configIndex >= configsProp.arraySize)
                    break;

                var element = configsProp.GetArrayElementAtIndex(configIndex);

                string label = ObjectNames.NicifyVariableName(flag.ToString()) + " Config";
                Type requiredType = GetConfigTypeForFlag(flag);

                var obj = element.objectReferenceValue;
                element.objectReferenceValue =
                    EditorGUILayout.ObjectField(label, obj, requiredType, false);

                configIndex++;
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        private static void SyncConfigsWithFlags(SerializedProperty configsProp, BehaviourType behaviours)
        {
            int active = CountFlags(behaviours);

            while (configsProp.arraySize < active)
                configsProp.InsertArrayElementAtIndex(configsProp.arraySize);
            while (configsProp.arraySize > active)
                configsProp.DeleteArrayElementAtIndex(configsProp.arraySize - 1);
        }

        private static int CountFlags(BehaviourType type)
        {
            int c = 0; ulong b = (ulong)type;
            while (b >= 1) { c += (int)(b & 1L); b >>= 1; }
            return c;
        }

        private static Type GetConfigTypeForFlag(BehaviourType flag)
        {
            return flag switch
            {
                BehaviourType.Sticky => typeof(StickyConfig),
                BehaviourType.Luminous => typeof(LuminousConfig),
                BehaviourType.Piercing => typeof(PiercingConfig),
                BehaviourType.Ricochet => typeof(RicochetConfig),
                BehaviourType.Homing => typeof(HomingConfig),
                BehaviourType.Boomerang => typeof(BoomerangConfig),
                BehaviourType.Joint => typeof(JointConfig),
                BehaviourType.ChangeTarget => typeof(ChangeTargetConfig),
                _ => typeof(BehaviourBaseConfig)
            };
        }
    }
}
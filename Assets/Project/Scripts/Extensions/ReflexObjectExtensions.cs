using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extensions
{
    public static class ReflexObjectExtensions
    {
        public static T InjectAttributes<T>(this T obj) where T : class
        {
            AttributeInjector.Inject(obj, SceneManager.GetActiveScene().GetSceneContainer());
            return obj;
        }

        public static GameObject InjectGameObject(this GameObject obj)
        {
            GameObjectInjector.InjectObject(obj, SceneManager.GetActiveScene().GetSceneContainer());
            return obj;
        }

        public static T InjectGameObject<T>(this T obj) where T : Component
        {
            GameObjectInjector.InjectObject(obj.gameObject, SceneManager.GetActiveScene().GetSceneContainer());
            return obj;
        }

        public static T InjectRecursive<T>(this T obj) where T : Component
        {
            GameObjectInjector.InjectRecursive(obj.gameObject, SceneManager.GetActiveScene().GetSceneContainer());
            return obj;
        }
    }
}
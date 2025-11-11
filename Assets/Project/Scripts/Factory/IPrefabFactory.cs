using UnityEngine;

namespace Factory {
  public interface IPrefabFactory<out TOut> : IFactory<GameObject, TOut> { }
}
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public DestructibleSize size = DestructibleSize.Small;

    // Вызывается, когда игрок пытается разрушить объект
    public bool TryDestroyBy(GameObject instigator)
    {
        var abilities = instigator.GetComponent<PlayerAbilities>();
        if (abilities != null && abilities.CanDestroy(size))
        {
            Destroy(gameObject);
            Debug.Log($"Разрушен объект {name} (size={size})");
            return true;
        }

        Debug.Log($"Нельзя разрушить {name} (size={size}) — нет способности");
        return false;
    }
}

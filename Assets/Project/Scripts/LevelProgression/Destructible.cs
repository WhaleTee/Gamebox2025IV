using UnityEngine;

/// <summary>
/// Разрушаемый объект
/// </summary>
public class Destructible : MonoBehaviour
{
    public DestructibleType type = DestructibleType.Plant;

    /// <summary>
    /// Игрок пытается разрушить объект
    /// </summary>
    public bool TryDestroyBy(GameObject instigator)
    {
        var abilities = instigator.GetComponent<PlayerAbilities>();
        if (abilities != null && abilities.CanDestroy(type))
        {
            Destroy(gameObject);
            Debug.Log($"Разрушен объект {name} (size={type})");
            return true;
        }

        Debug.Log($"Нельзя разрушить {name} (size={type}) — нет способности");
        return false;
    }
}

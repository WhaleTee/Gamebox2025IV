using UnityEngine;

public enum DestructibleSize { Small, Large }

public class PlayerAbilities : MonoBehaviour
{
    [HideInInspector] public bool canDestroySmall = false;
    [HideInInspector] public bool canDestroyLarge = false;

    public bool CanDestroy(DestructibleSize size)
    {
        switch (size)
        {
            case DestructibleSize.Small: return canDestroySmall;
            case DestructibleSize.Large: return canDestroyLarge;
            default: return false;
        }
    }
}

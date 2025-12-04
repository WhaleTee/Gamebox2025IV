using UnityEngine;
using Reflex.Attributes;
using Characters;

namespace Spawn
{
    public class PlayerSpawn : MonoBehaviour
    {
        [Inject] private PlayerInjectionData playerData;

        private void Start()
        {
            playerData.PlayerStart.GetComponent<SpriteRenderer>().enabled = false;
            var playerCharacter = Instantiate(playerData.Prefab);
            playerCharacter.transform.SetPositionAndRotation(playerData.PlayerStart.position, playerData.PlayerStart.rotation);
            playerCharacter.Init();
        }
    }
}

using UnityEngine;
using Reflex.Attributes;
using Characters;
using Extensions;

namespace Spawn
{
    public class PlayerSpawn : MonoBehaviour
    {
        [Inject] private PlayerInjectionData playerData;

        private void Start()
        {
            playerData.PlayerStart.GetComponent<SpriteRenderer>().enabled = false;
            var playerCharacter = GameObject.Instantiate(playerData.Prefab).InjectGameObject();
            playerCharacter.transform.SetPositionAndRotation(playerData.PlayerStart.position, playerData.PlayerStart.rotation);
            playerCharacter.Init();
        }
    }
}

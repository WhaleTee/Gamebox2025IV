using Characters;
using Reflex.Attributes;
using UnityEngine;
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
        }

        //[Inject]
        //public void Inject(PlayerInjectionData playerData)
        //{
        //    playerData.PlayerStart.GetComponent<SpriteRenderer>().enabled = false;
        //    var playerCharacter = GameObject.Instantiate(playerData.Prefab);
        //    playerCharacter.transform.SetPositionAndRotation(playerData.PlayerStart.position, playerData.PlayerStart.rotation);
        //}
    }
}

using Characters;
using Reflex.Attributes;
using UnityEngine;

namespace Spawn
{
    public class PlayerSpawn
    {
        [Inject]
        public void Construct(Hero playerCharacterPrefab, Transform playerStart)
        {
            playerStart.GetComponent<SpriteRenderer>().enabled = false;
            var playerCharacter = GameObject.Instantiate(playerCharacterPrefab);
            playerCharacter.transform.SetPositionAndRotation(playerStart.position, playerStart.rotation);
        }
    }
}

using UnityEngine;

public class SetShape : MonoBehaviour
{
    private SpriteRenderer render;
    private ParticleSystem[] particles;

    private void Start()
    {
        render = GetComponentInChildren<SpriteRenderer>();
        particles = GetComponentsInChildren<ParticleSystem>();

        foreach (var p in particles)
        {
            var shape = p.shape;
            shape.spriteRenderer = render;
        }
    }
}
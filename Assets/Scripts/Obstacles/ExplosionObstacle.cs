using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Explosion Obstacles
public class ExplosionObstacle : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void StopExplosion()
    {
        Destroy(gameObject);
    }

    void EnableCollider()
    {
        boxCollider.enabled = true;
    }
    void DisableCollider()
    {
        boxCollider.enabled = false;
    }
}

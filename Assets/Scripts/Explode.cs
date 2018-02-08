using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private Animator animator;

    void StopExplosion()
    {
        Destroy(gameObject);
    }

}

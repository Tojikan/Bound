using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    //destroy at end of animation
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lethal")
        {
            Debug.Log("OhMyGodWeKilledKenny");
        }
    }
}

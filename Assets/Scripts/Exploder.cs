using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public int countdown;
    public int loopLength;
    public static float millisecond = 0.01f;

    public GameObject explosionType;

    public int timeleft;
    
    void Start()
    {
        BeginSequence();
    }

    public void Initialize(int countdown,  GameObject explosionType, int loopLength)
    {
        this.countdown = countdown;
        this.loopLength = loopLength; 
        this.explosionType = explosionType;
    }


    public void BeginSequence()
    {
        StartCoroutine("Timer", loopLength);
    }


    IEnumerator Timer(float loop)
    {
        timeleft = 0;

        while (true)
        {
            yield return new WaitForSeconds(millisecond);
            timeleft += 1;

            if (timeleft >= loopLength)
            {
                
                timeleft = 0;
            }

            if (timeleft == countdown)
            {
                Instantiate(explosionType, transform.position, transform.rotation);
            }
        }
    }

}

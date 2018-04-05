using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider2D boxCollide;
    private SpriteRenderer spriteRenderer;
    public float projectileSpeed = 10f;
    private Vector3 directionVector;
    

    private Vector3 startPosition;
    public Vector3 StartPosition
    {
        get { return startPosition; }
        set
        {
            startPosition = value;
            transform.position = startPosition;
        }
    }

    private bool isFiring;
    public bool IsFiring
    {
        get { return isFiring; }
        set
        {
            isFiring = value;
            if (isFiring)
            {
                EnableProjectile();
                ResetPosition();
            }
            else if (!isFiring)
            {
                DisableProjectile();
                ResetPosition();
            }
        }
    }

    private int rotation;
    public int Rotation
    {
        get { return rotation;  }
        set
        {
            rotation = value;
            Flip(rotation);
        }
    }

    private void Start()
    {
        boxCollide = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisableProjectile();
    }

    private void Update()
    {
        LaunchProjectile();
    }


    public void Flip(int direction)
    {
        Transform shape = this.transform;
        switch (direction)
        {
            case 0:
                shape.rotation = Quaternion.Euler(0, 0, 180);
                directionVector.Set(0, -1, 0);
                break;
            case 1:
                shape.rotation = Quaternion.Euler(0, 0, 90);
                directionVector.Set(-1, 0, 0);
                break;
            case 2:
                shape.rotation = Quaternion.Euler(0, 0, 0);
                directionVector.Set(0, 1, 0);
                break;
            case 3:
                shape.rotation = Quaternion.Euler(0, 0, 270);
                directionVector.Set(1, 0, 0);
                break;
            default:
                shape.rotation = Quaternion.Euler(0, 0, 0);
                directionVector.Set(0, -1, 0);
                break;
        }
    }

    public void LaunchProjectile()
    {
        if (isFiring)
        {
            transform.position += directionVector * Time.deltaTime * projectileSpeed;
        }
    }

    public void ResetPosition()
    {
        if (!spriteRenderer.isVisible)
        {          
            transform.position = startPosition;
        }
    }

    public void DisableProjectile()
    {
        spriteRenderer.enabled = false;
        boxCollide.enabled = false;
    }

    public void EnableProjectile()
    {
        spriteRenderer.enabled = true;
        boxCollide.enabled = true;
    }
}

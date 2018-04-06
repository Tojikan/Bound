using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Projectile script for the child object attached to a projectile trap. Doesn't need to inherit from Obstacle as it is not its own obstacle. 
//Projectiles should ALWAYS be attached as a child under a projectile trap. Put the projectile as a prefab into a set
public class Projectile : MonoBehaviour
{
    private Collider2D boxCollide;                                              //collider on this object
    private SpriteRenderer spriteRenderer;                                      //spriterender on this object
    public float projectileSpeed = 10f;                                         //Set the movement speed of the projectile
    private Vector3 directionVector;                                            //Sets the direction of the projectile. Multiples the movement vector's x and y by 0 and +-1 to do this
    

    private Vector3 startPosition;                                              //Original start position of the projectile. Should be set on start on the parent obstacle
    public Vector3 StartPosition                                                //Use the property to make sure the transform position is actually the same as the parent
    {
        get { return startPosition; }
        set
        {
            startPosition = value;
            transform.position = startPosition;
        }
    }

    private bool isFiring;                                                      //Bool to both trigger firing and also check if we're firing.
    public bool IsFiring                                                        //Property that tells the projectile to move. By setting the bool, we enable/disable the projectile's sprite and collider
    {
        get { return isFiring; }
        set
        {
            isFiring = value;
            if (isFiring)
            {
                EnableProjectile();
            }
            else if (!isFiring)
            {
                DisableProjectile();
            }
        }
    }

    private int rotation;                                                       //Int that sets our rotation. Based on an enum set in the parent obstacle. Defaults to facing down
    public int Rotation
    {
        get { return rotation;  }
        set
        {
            rotation = value;
            Flip(rotation);
        }
    }                                                      //This property is set in Start() of parent obstacle. It will flip the projectile in the corresponding direction and also set tthe direction vector

    private void Start()
    {
        boxCollide = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Makes sure we're not firing upon start and only when triggered by the parent
        DisableProjectile();
    }

    private void Update()
    {
        //first we check if projectile is offscreen every frame. Then update its position to move it
        CheckProjectilePosition();
        LaunchProjectile();
    }

    //Called in the rotation property. Sets the rotation of the game object as well as the direction of the projectile movement
    private void Flip(int direction)
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


    #region Projectile functionality

    //Moves the projectile by direction vector and speed, frame independent. Gets called on Update() and only works if isFiring is set to true
    private void LaunchProjectile()
    {
        if (isFiring)
        {
            transform.position += directionVector * Time.deltaTime * projectileSpeed;
        }
    }

    //Checks to see if the projectile is off the camera screen. Disable via property and reset position if so.
    private void CheckProjectilePosition()
    {
        if (!spriteRenderer.isVisible)
        {
            IsFiring = false;
            ResetPosition();
        }
    }

    //Move us back to the start position
    private void ResetPosition()
    {     
       transform.position = startPosition;
    }

    //Disables sprite and collider
    private void DisableProjectile()
    {
        spriteRenderer.enabled = false;
        boxCollide.enabled = false;
    }

    //Enables sprite and collider
    private void EnableProjectile()
    {
        spriteRenderer.enabled = true;
        boxCollide.enabled = true;
    }
    #endregion
}

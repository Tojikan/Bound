using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        public Player playerParent;
        public CapsuleCollider2D footCollider;
        public CapsuleCollider2D bodyCollider; 

        public void OnDeathAnimationEnd()
        {
            playerParent.Respawn();
        }

        public void SetFootVertical()
        {
            ChangeColliderSize(ref footCollider, -0.02f, 0.56f, footCollider.offset.y, footCollider.size.y);
        }

        public void SetFootLeft()
        {
            ChangeColliderSize(ref footCollider, 0.1f, 0.4f, footCollider.offset.y, footCollider.size.y);
        }

        public void SetFootRight()
        {
            ChangeColliderSize(ref footCollider, -0.1f, 0.4f, footCollider.offset.y, footCollider.size.y);
        }


        public void SetBodyVertical()
        {
            ChangeColliderSize(ref bodyCollider, -0.02f, 0.5257f, -0.14836f, 0.6043f);
        }

        public void SetBodyLeft()
        {
            ChangeColliderSize(ref bodyCollider, -0.11f, 0.47f, -0.72f, 0.34f);
        }

        public void SetBodyRight()
        {
            ChangeColliderSize(ref bodyCollider, .09f, 0.47f, -0.72f, 0.34f);
        }


        private void ChangeColliderSize(ref CapsuleCollider2D target,float xoffset, float xsize, float yoffset, float ysize)
        {
            Vector2 newOffset = new Vector2(xoffset, yoffset);
            Vector2 newSize = new Vector2(xsize, ysize);
            target.offset = newOffset;
            target.size = newSize;
        }
    }
}

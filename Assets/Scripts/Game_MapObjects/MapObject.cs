using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundEngine
{ 
    public class MapObject : MonoBehaviour
    {
        private Vector2 position;
        public bool startsEnabled;
        public bool showBox;
        private Collider2D collide;



        #region Constructor
        public virtual void ConstructObject(Vector3 pos)
        {
            transform.position = pos;
        }
        #endregion



        public virtual void DestroyObject()
        {
            Destroy(gameObject);
        }


        private void OnDrawGizmos()
        {
            if (showBox == true)
            {
                Gizmos.color = Color.blue;
                BoxCollider2D boundary = GetComponent<BoxCollider2D>();
                Gizmos.DrawCube(transform.position, boundary.size);
            }
        }

    }
}

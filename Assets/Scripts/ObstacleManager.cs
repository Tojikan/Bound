using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;

namespace BoundEngine
{
    public class ObstacleManager : MonoBehaviour
    {
        private Transform thisTransform;
        private List<Exploder> explosionList;

        void Start()
        {
            thisTransform = GetComponent<Transform>();
        }

        
        public void CreateExploders(List<ExplosionData> explosions, ExplosionSet set)
        {
            explosionList = new List<Exploder>();

            foreach (ExplosionData data in explosions)
            {
                GameObject newBomb = Instantiate(set.ExplosionPrefabs[data.explodeType], data.position, transform.rotation, thisTransform);
                Exploder exploder = newBomb.GetComponent<Exploder>();               
                exploder.Initialize(data.explodeTime, data.loopLength);
                explosionList.Add(exploder);
            }
        }

        public void StartExplosions()
        {
            foreach(Exploder exploder in explosionList)
            {
                exploder.BeginSequence();
            }
        }

        public void StopExplosions()
        {
            foreach (Exploder exploder in explosionList)
            {
                exploder.StopSequence();
            }
        }

        public void ClearObstacles()
        {
            foreach (Exploder child in explosionList)
            {
                Destroy(child.gameObject);
            }
        }

    }
}

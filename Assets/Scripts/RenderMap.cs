using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BoundMaps;

namespace BoundEngine
{
    //Class to render our map tiles and set our start/finish points
    public class RenderMap : MonoBehaviour
    {
        public Tilemap groundLayer;
        public Tilemap wallLayer;

        public GameObject spawnPoint;
        public GameObject finishPoint;


        //Function to render tiles into our game. Takes in a level, a tileset, and a boundary area. Renders over two layers currently.
        //Iterates over our level data to translate the stored integer into a tile from our tileset. Then populates an array with the matched tilebase
        //Then uses a boxfill to fill over the bounded area
        public void LoadTiles(LevelData level, TileSet tileSet, BoundsInt gameArea)
        {
            //Calls our check tileset function and returns if false
            if (CheckTilemap() == false)
            {
                return;
            }

            ClearMap();

            //Creates arrays for our layers with size equal to our game board
            TileBase[] groundArray = new TileBase[gameArea.size.x * gameArea.size.y * gameArea.size.z];
            TileBase[] wallArray = new TileBase[gameArea.size.x * gameArea.size.y * gameArea.size.z];

            //iterates over the ground array and populates it with tilebases from our file
            for (int index = 0; index < groundArray.Length; index++)
            {

                //If the value is -1, then it means there was no tile in that position
                if (level.groundTiles[index] != -1)
                {
                    //Sets our ground tile array by matching the value stored in the tile data of the current level with our tileset's array
                    groundArray[index] = tileSet.tilesArray[level.groundTiles[index]];
                }
            }

            //iterates over the wall array and populates it with tilebases from our file
            for (int index = 0; index < wallArray.Length; index++)
            {
                //Checks if there is a tile. If it's -1, then no tile
                if (level.wallTiles[index] != -1)
                {
                    //Sets our wall tile array by matching the value stored in the tile data of the current level with our tileset's array
                    wallArray[index] = tileSet.tilesArray[level.wallTiles[index]];
                }
            }

            //Sets the tiles on the map layer by layer
            groundLayer.SetTilesBlock(gameArea, groundArray);
            wallLayer.SetTilesBlock(gameArea, wallArray);
        }

        //Sets our start and end points on the map. 
        public void SetBeacons(Vector2 start, Vector2 end)
        {
            //If beacons don't exist, return
            if (CheckBeacons() == false)
            {
                return;
            }

            spawnPoint.transform.position = start;
            finishPoint.transform.position = end;
        }


        //Clears both our layers
        public void ClearMap()
        {
            groundLayer.ClearAllTiles();
            wallLayer.ClearAllTiles();
        }

       
        //Checks if our beacons exist or not
        public bool CheckBeacons()
        {
            if (!spawnPoint || !finishPoint)
            {
                Debug.Log("Missing Spawn or Finish Beacon");
                return false;
            }

            return true;
        }


        //Checks if our tileset is set
        public bool CheckTilemap()
        {
            if (!groundLayer || !wallLayer)
            {
                Debug.Log("Missing Tile Layer. Map Load failed");
                return false;
            }

            return true;
        }

    }
}

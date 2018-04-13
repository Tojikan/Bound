using UnityEngine;
using UnityEngine.Tilemaps;

//Tile4Bit ScriptableObject to create new Grass Tiles and set the sprite accordingly
//Based on a 4bit Bitmapping scheme - minimum 15 tiles needed (2 more for extra decorations)
//Careful not to mess up the loops. This directlty modifies the Unity Editor, so you can freeze up the Editor if you don't do the loops right


//Create a new asset via AssetMenu. Former code to create a new asset is unnecessary and Unity handles new asset creation under the hood
[CreateAssetMenu(fileName = "NewTile4", menuName = "Tiles/Bitmasked Tiles/Tile4Bit", order = 1)]
public class Tile4Bit : Tile
{

    //Adds an array field for us to store all our sprites
    [SerializeField]
    private Sprite[] tileSprites;

    //Function to check surrounding tiles. Checks to see if they are grass tiles and if so, refreshes the tile
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //Loop over neighboring tiles
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                //Gets the position of the neighboring tile
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                //Checks if the correct tile type, and then refreshes. 
                if (CheckTile(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }

            }
        }
    }


    //This overrides the GetTileData to let us update each tile according to neighbors
    //If the neighboring tile is same tile, add to our bitmap to map out the tile we use. 
    //4 Bit Bitmap = North - West - South - East : 1 - 2 - 4 - 8

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Call GetTileData in order to let us set TileCollider if it's included in the current layer
        base.GetTileData(position, tilemap, ref tileData);

        //Bitmap counter 
        int bitmask = 0;

        tileData.colliderType = ColliderType.None;

        //Positions for the 4 cardinal directions around tile
        Vector3Int eastSide = new Vector3Int(position.x + 1, position.y, position.z);
        Vector3Int southSide = new Vector3Int(position.x, position.y - 1, position.z);
        Vector3Int westSide = new Vector3Int(position.x - 1, position.y, position.z);
        Vector3Int northSide = new Vector3Int(position.x, position.y + 1, position.z);

        //Calculates our bitmap. Does a boolcheck on the neighboring tiles and adding the value if returns true
        bitmask += CheckTile(tilemap, northSide) ? 1 : 0;
        bitmask += CheckTile(tilemap, westSide) ? 2 : 0;
        bitmask += CheckTile(tilemap, southSide) ? 4 : 0;
        bitmask += CheckTile(tilemap, eastSide) ? 8 : 0;


        //Returns a random decoration sprite for plain grass tiles
        if (bitmask == 15)
        {
            int randomVal = Random.Range(15, 18);
            tileData.sprite = tileSprites[randomVal];
            return;
        }

        //Set the sprite of our tile according to the bitmask value
        tileData.sprite = tileSprites[bitmask];

    }

    //Checks if neighboring tiles are the same tile. Returns true if so and false if not 
    private bool CheckTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }
}


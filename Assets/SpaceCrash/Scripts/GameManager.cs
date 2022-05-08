using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Builder")]

    [Space(8)]

    public GameObject tilePrefab;

    public int levelWidth;
    public int levelLength;
    public Transform tilesHolder;
    public float tileSize = 1;
    public float tileEndHeight = 1;

    [Space(8)]

    //This is the grid
    public TileObject[,] tileGrid = new TileObject [0, 0];

    [Header("Resourses")]

    [Space(8)]

    public GameObject metalPrefab;
    public GameObject carbonPrefab;
    public GameObject gasPrefab;
    public Transform resourcesHolder;

    [Range(0,1)]
    public float obstacleChance =0.3f;

    public int xBounds = 3;
    public int zBounds = 3;

    [Space(8)]

    //Debug method (selected building)
    public BuildingObject buildingToPlace;

    public static GameManager Instance;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        Createlevel();
    }

    /// <summary>
    ///Create our grid depending on our level width and length.
    /// </summary>

    public void Createlevel()
    {
        List<TileObject> visualGrid = new List<TileObject>();

        for (int x = 0; x < levelWidth; x++)
        {
            for (int z = 0; z < levelLength; z++)
            {
                //Directly Spawns a tile
                TileObject spawnedTile = SpawnTile(x * tileSize,z * tileSize);

                //Set the TileObject world space data
                spawnedTile.xPos = x;
                spawnedTile.zPos = z;

                //Checks whenever we can spawn and obstacle inside a tile, using the Bounds parameters
                if (x < xBounds || z < zBounds || z >= (levelLength - zBounds) || x >= (levelWidth - xBounds))
                {
                    //We can spawn an obstacle in there
                    spawnedTile.data.StarterTileValue(false);
                }

                if (spawnedTile.data.CanSpawnObstacle)
                {
                    bool spawnObstacle = Random.value <= obstacleChance;

                    if(spawnObstacle)
                    {
                        spawnedTile.data.SetOccupied(Tile.ObstacleType.Resource);

                        ObstacleObject tmpObstacle = SpawnObstacle(spawnedTile.transform.position.x, spawnedTile.transform.position.z);
                        tmpObstacle.SetTileReference(spawnedTile);
                    }
                }

                //Add the spawned visual tileObject inside the list
                visualGrid.Add(spawnedTile);

            }
        }

        CreateGrid(visualGrid);
    }
    
    /// <summary>
    /// Spawns and return a tileObject
    /// </summary>
    /// <param name="xPos">X Position inside the world</param>
    /// <param name="zPos">Z Position inside the world</param>
    /// <returns></returns>

    TileObject SpawnTile(float xPos, float zPos)
    {
        //This will spawn the tile
        GameObject tmpTile = Instantiate(tilePrefab);

        tmpTile.transform.position = new Vector3(xPos, 0, zPos);
        tmpTile.transform.SetParent(tilesHolder);

        tmpTile.name = "Tile " + xPos + " - " + zPos;

        //Check if the tile is able to hold an obstacle.


        return tmpTile.GetComponent<TileObject>();
    }

    /// <summary>
    /// Will spawn a resource obstacle directly in thr coordinates
    /// </summary>
    /// <param name="xPos">X Position of the osbtacle</param>
    /// <param name="zPos">Z Position of the osbtacle</param>
    ObstacleObject SpawnObstacle(float xPos, float zPos)
    {
        //It has a 50 percent of spawning a metal obstacle
        bool isMetal = Random.value <=0.5f;

        GameObject spawnedObstacle = null;

        //Check spawn a metal or carbon obstacle
        if(isMetal)
        {
            spawnedObstacle = Instantiate(metalPrefab);
            spawnedObstacle.name = "Metal" + xPos + " - " + zPos;

        }
        else
        {
            spawnedObstacle = Instantiate(carbonPrefab);
            spawnedObstacle.name = "Carbon" + xPos + " - " + zPos;            
        }
        
        //Sets the position and the parent of the resources.
        spawnedObstacle.transform.position = new Vector3(xPos, tileEndHeight, zPos);
        spawnedObstacle.transform.SetParent(resourcesHolder);

        return spawnedObstacle.GetComponent<ObstacleObject>();
    }

    /// <summary>
    /// Create tile grid to add buildings
    /// </summary>
    public void CreateGrid(List<TileObject> refVisualGrid)
    {
        //Set the size of the tile grid
        tileGrid = new TileObject[levelWidth, levelLength];

        //Iterates through all the tile grid
        for (int x = 0; x < levelWidth; x++)
        {
            for (int z = 0; z < levelLength; z++)
            {
                //Connects the tile grid to visual grid
                tileGrid[x,z] = refVisualGrid.Find(v => v.xPos == x && v.zPos == z);
                //Debug.Log(tileGrid[x, z].gameObject.name);
            }

        }
    }

    /// <summary>
    /// Handles the placing of the building
    /// </summary>
    /// <param name="building">Building to place</param>
    /// <param name="tile">Tile to place the building to</param>
    public void SpawnBuilding(BuildingObject building, List<TileObject> tiles)
    {
        GameObject spawnedBuilding = Instantiate(building.gameObject);
        float sumX = 0;
        float sumZ = 0;

        //Old position
        //Vector3 position = new Vector3(tile.xPos, tileEndHeight, tile.zPos);
        //Sum value of X position of all tiles
        //Sum value of Z position of all tiles
        for (int i = 0; i < tiles.Count; i++)
        {
           sumX += tiles[i].xPos;
           sumZ += tiles[i].zPos;

           tiles[i].data.SetOccupied(Tile.ObstacleType.Building); 
           Debug.Log("placed building in " + tiles[i].xPos + " - " + tiles[i].zPos);   
    
        }

        //Sets the correct position
        Vector3 position = new Vector3((sumX / tiles.Count), building.data.yPadding, (sumZ / tiles.Count));

        spawnedBuilding.transform.position = position;
    }
}

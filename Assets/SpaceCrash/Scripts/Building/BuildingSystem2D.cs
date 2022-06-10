using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem2D : MonoBehaviour
{
    public static BuildingSystem2D Instance;

    //Grid reference
    public GridLayout gridLayout;

    //Tilemap reference
    [SerializeField] public Tilemap MainTilemap;
    [SerializeField] private TileBase takenTile;

    //Reference of a Placeable object
    private PlaceableObject2D objectToPlace;

    #region Unity methods

    private void Awake() 
    {
        Instance = this;
    }

    #endregion

    #region Tilemap Management

    //Return a Tile Base array and accepts a bound integer area and a tilemap.
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileBase tileBase, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        FillTiles(tileArray, tileBase);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileBase tileBase)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBase;
        }
    }

    public void ClearArea(BoundsInt area, Tilemap tilemap)
    {
        SetTilesBlock(area, null, tilemap);
    }

    #endregion

    #region Building Placement

    //Initialize the object in the world space
    public GameObject InitializeWithObject(GameObject building, Vector3 pos)
    {
        //Calculate the position to place the object
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        //Instantiate an object
        GameObject obj = Instantiate(building, position, Quaternion.identity);
        obj.gameObject.AddComponent<ObjectDrag2D>();

        //The camera follow the object
        PanZoom.Instance.FollowObject(obj.transform);

        return obj;
    }

    //Can be place the object in the world space or not
    public bool CanTakeArea(BoundsInt area)
    {

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == takenTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, takenTile, MainTilemap);
    }

    #endregion

}

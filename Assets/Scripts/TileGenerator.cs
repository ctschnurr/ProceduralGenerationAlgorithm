using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fullWallPrefab;
    public GameObject cornerAPrefab;

    public int dungeonDimensionsRows;
    public int dungeonDimensionsColumns;

    static Tile fullWall = new Tile();
    static Tile cornerA = new Tile();
    static Tile cornerB = new Tile();
    static Tile cornerC = new Tile();
    static Tile cornerD = new Tile();
    static Tile hallwayH = new Tile();
    static Tile hallwayV = new Tile();
    static Tile intersection = new Tile();

    List<Tile> tileList = new List<Tile> { fullWall, cornerA, cornerB, cornerC, cornerD, hallwayH, hallwayV, intersection };

    Tile[,] tiles;
    void Start()
    {
        tiles = new Tile[dungeonDimensionsRows,dungeonDimensionsColumns];
        SetupTiles();
        DrawBorder();
        PickStart();

    }

    void SetupTiles()
    {
        fullWall.exitN = false;
        fullWall.exitE = false;
        fullWall.exitS = false;
        fullWall.exitW = false;
        fullWall.tileObject = fullWallPrefab;

        cornerA.exitN = false;
        cornerA.exitE = true;
        cornerA.exitS = true;
        cornerA.exitW = false;
        cornerA.tileObject = cornerAPrefab;

        cornerB.exitN = false;
        cornerB.exitE = false;
        cornerB.exitS = true;
        cornerB.exitW = true;

        cornerC.exitN = true;
        cornerC.exitE = false;
        cornerC.exitS = false;
        cornerC.exitW = true;

        cornerD.exitN = true;
        cornerD.exitE = true;
        cornerD.exitS = false;
        cornerD.exitW = false;

        hallwayH.exitN = false;
        hallwayH.exitE = true;
        hallwayH.exitS = false;
        hallwayH.exitW = true;

        hallwayV.exitN = true;
        hallwayV.exitE = false;
        hallwayV.exitS = true;
        hallwayV.exitW = false;

        intersection.exitN = true;
        intersection.exitE = true;
        intersection.exitS = true;
        intersection.exitW = true;
    }
    void DrawBorder()
    {
        for(int x = 0; x < dungeonDimensionsColumns; x++)
        {
            if (tiles[0,x] == null)
            {
                tiles[0,x] = fullWall;
                Instantiate(fullWallPrefab, new Vector3(x * 5, 0, 0), Quaternion.identity);
            }
        }

        for (int x = 0; x < dungeonDimensionsColumns; x++)
        {
            if (tiles[dungeonDimensionsRows - 1, x] == null)
            {
                tiles[dungeonDimensionsRows - 1, x] = fullWall;
                Instantiate(fullWallPrefab, new Vector3(x * 5, 0, -(dungeonDimensionsRows -1) * 5), Quaternion.identity);
            }
        }

        for (int z = 0; z < dungeonDimensionsRows; z++)
        {
            if (tiles[z, 0] == null)
            {
                tiles[z, 0] = fullWall;
                Instantiate(fullWallPrefab, new Vector3(0, 0, -z * 5), Quaternion.identity);
            }
        }
        
        for (int z = 0; z < dungeonDimensionsRows; z++)
        {
            if (tiles[z, dungeonDimensionsColumns - 1] == null)
            {
                tiles[z, dungeonDimensionsColumns - 1] = fullWall;
                Instantiate(fullWallPrefab, new Vector3((dungeonDimensionsColumns - 1) * 5, 0, -z * 5), Quaternion.identity);
            }
        }

    }

    void PickStart()
    {
        Tile picked = tiles[0, 0];
        int randomRow = -1;
        int randomCol = -1;

        while(tiles != null)
        {
            randomRow = Random.Range(1, dungeonDimensionsRows);
            randomCol = Random.Range(1, dungeonDimensionsColumns);

            picked = tiles[randomRow, randomCol];
        }

        PickTile(randomRow, randomCol);
    }

    void PickTile(int row, int col)
    {
        List<Tile> possibles = tileList;
        List<Tile> remove = new List<Tile>();

        if (tiles[row -1, col].exitS == false)
        {
            foreach(Tile tile in tileList)
            {
                if (tile.exitN == true) remove.Add(tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Tile
{
    public bool exitN;
    public bool exitS;
    public bool exitE;
    public bool exitW;

    public GameObject tileObject;
}

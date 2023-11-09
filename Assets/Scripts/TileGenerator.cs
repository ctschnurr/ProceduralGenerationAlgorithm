using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fullWallPrefab;
    public GameObject cornerAPrefab;
    public GameObject cornerBPrefab;
    public GameObject cornerCPrefab;
    public GameObject cornerDPrefab;
    public GameObject hallwayHPrefab;
    public GameObject hallwayVPrefab;
    public GameObject intersectionPrefab;
    public GameObject startAPrefab;
    public GameObject startBPrefab;
    public GameObject startCPrefab;
    public GameObject startDPrefab;
    public GameObject tCornerAPrefab;
    public GameObject tCornerBPrefab;
    public GameObject tCornerCPrefab;
    public GameObject tCornerDPrefab;

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
    static Tile startA = new Tile();
    static Tile startB = new Tile();
    static Tile startC = new Tile();
    static Tile startD = new Tile();
    static Tile tCornerA = new Tile();
    static Tile tCornerB = new Tile();
    static Tile tCornerC = new Tile();
    static Tile tCornerD = new Tile();

    List<Tile> tileList = new List<Tile> { cornerA, cornerB, cornerC, cornerD, hallwayH, hallwayV, intersection, startA, startB, startC, startD };
    List<Tile> endsTileList = new List<Tile> { startA, startB, startC, startD };

    static int startX;
    static int startY;

    static int endX;
    static int endY;

    protected Queue<Tile> nextTiles = new Queue<Tile>();

    Tile[,] tiles;

    void SetupTiles()
    {
        fullWall.exitN = false;
        fullWall.exitE = false;
        fullWall.exitS = false;
        fullWall.exitW = false;
        fullWall.tileObject = fullWallPrefab;

        cornerA.exitN = true;
        cornerA.exitE = true;
        cornerA.exitS = false;
        cornerA.exitW = false;
        cornerA.tileObject = cornerAPrefab;

        cornerB.exitN = false;
        cornerB.exitE = true;
        cornerB.exitS = true;
        cornerB.exitW = false;
        cornerB.tileObject = cornerBPrefab;

        cornerC.exitN = true;
        cornerC.exitE = false;
        cornerC.exitS = false;
        cornerC.exitW = true;
        cornerC.tileObject = cornerCPrefab;

        cornerD.exitN = true;
        cornerD.exitE = false;
        cornerD.exitS = false;
        cornerD.exitW = true;
        cornerD.tileObject = cornerDPrefab;

        hallwayH.exitN = false;
        hallwayH.exitE = true;
        hallwayH.exitS = false;
        hallwayH.exitW = true;
        hallwayH.tileObject = hallwayHPrefab;

        hallwayV.exitN = true;
        hallwayV.exitE = false;
        hallwayV.exitS = true;
        hallwayV.exitW = false;
        hallwayV.tileObject = hallwayVPrefab;

        intersection.exitN = true;
        intersection.exitE = true;
        intersection.exitS = true;
        intersection.exitW = true;
        intersection.tileObject = intersectionPrefab;

        startA.exitN = false;
        startA.exitE = true;
        startA.exitS = false;
        startA.exitW = false;
        startA.tileObject = startAPrefab;

        // START B POINTS NORTH
        startB.exitN = true;
        startB.exitE = false;
        startB.exitS = false;
        startB.exitW = false;
        startB.tileObject = startBPrefab;

        startC.exitN = false;
        startC.exitE = false;
        startC.exitS = false;
        startC.exitW = true;
        startC.tileObject = startCPrefab;

        startD.exitN = false;
        startD.exitE = false;
        startD.exitS = true;
        startD.exitW = false;
        startD.tileObject = startDPrefab;

        tCornerA.exitN = true;
        tCornerA.exitE = true;
        tCornerA.exitS = true;
        tCornerA.exitW = false;
        tCornerA.tileObject = tCornerAPrefab;

        tCornerB.exitN = true;
        tCornerB.exitE = true;
        tCornerB.exitS = false;
        tCornerB.exitW = true;
        tCornerB.tileObject = tCornerBPrefab; ;

        tCornerC.exitN = true;
        tCornerC.exitE = false;
        tCornerC.exitS = true;
        tCornerC.exitW = true;
        tCornerC.tileObject = tCornerCPrefab;

        tCornerD.exitN = false;
        tCornerD.exitE = true;
        tCornerD.exitS = true;
        tCornerD.exitW = true;
        tCornerD.tileObject = tCornerDPrefab;
    }
    void Start()
    {
        tiles = new Tile[dungeonDimensionsRows, dungeonDimensionsColumns];
        SetupTiles();
        CreateBase();
        PickStart();
        //BuildMaze(tiles[startX, startY]);



        // Tile next = nextTiles.Dequeue();
        // BuildMaze(next);
        // PickTile(next, tileList);

        InstantiateDungeon();
        // DrawBorder();
        // BuildMaze(tiles[startX, startY]);

        // while (nextTiles.Count > 0)
        // {
        //     Tile next = nextTiles.Dequeue();
        //     BuildMaze(next);
        // }
    }

    void CreateBase()
    {
        for (int x = 0; x < dungeonDimensionsRows; x++)
        {
            for (int y  = 0; y <  dungeonDimensionsColumns; y++)
            {
                Tile newTile = new Tile();
                newTile = fullWall;
                newTile.xVal = x; 
                newTile.yVal = y;
                tiles[x, y] = newTile;
            }
        }
    }

    void InstantiateDungeon()
    {
        for (int x = 0; x < dungeonDimensionsRows; x++)
        {
            for (int y = 0; y < dungeonDimensionsColumns; y++)
            {
                GameObject newObj = Instantiate(tiles[x, y].tileObject, new Vector3(x * 5, 0, y * 5), Quaternion.identity);
                // tiles[x, y] = newObj.AddComponent<Tile>();
            }
        }
    }

    void PickStart()
    {
        int randomRow = Random.Range(2, dungeonDimensionsRows);
        int randomCol = Random.Range(2, dungeonDimensionsColumns);

        startX = randomRow;
        startY = randomCol;

        Debug.Log(startX);
        Debug.Log(startY);

        Debug.Log(tiles[startX, startY].tileObject.name);

        Debug.Log(tiles[startX, startY].xVal);
        Debug.Log(tiles[startX, startY].yVal);
        PickTile(tiles[startX, startY], endsTileList);
    }

    void BuildMaze(Tile subjectTile)
    {
        if (subjectTile.exitN == true)
        {
            nextTiles.Enqueue(tiles[subjectTile.xVal + 1, subjectTile.yVal]);
        }

        if (subjectTile.exitE == true) //  && tiles[subjectTile.x - 1, subjectTile.y].edge == false
        {
            nextTiles.Enqueue(tiles[subjectTile.xVal, subjectTile.yVal - 1]);
        }
        
        if (subjectTile.exitS == true)
        {
            nextTiles.Enqueue(tiles[subjectTile.xVal - 1, subjectTile.yVal]);
        }
        
        if (subjectTile.exitW == true)
        {
            nextTiles.Enqueue(tiles[subjectTile.xVal, subjectTile.yVal + 1]);
        }
    }

    void PickTile(Tile input, List<Tile> possibles)
    {
        List<Tile> remove = new List<Tile>();

        if (input.xVal > 1)
        {
            Debug.Log(tiles[startX, startY].tileObject.name);
            Debug.Log(tiles[startX, startY].xVal);
            Debug.Log(tiles[startX, startY].yVal);
            Debug.Log("X: " + input.xVal + " Y: " + input.yVal);
            Debug.Log(tiles[input.xVal + 1, input.yVal].tileObject.name);

            if (tiles[input.xVal + 1, input.yVal].exitS == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitN == false) remove.Add(tile);
                }
            }
        }

        if (input.xVal < dungeonDimensionsRows - 1)
        {
            if(tiles[input.xVal - 1, input.yVal].exitN == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitS == false) remove.Add(tile);
                }
            }
        }

        if (input.yVal > 1)
        {
            if (tiles[input.xVal, input.yVal - 1].exitE == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitW == false) remove.Add(tile);
                }
            }

        }

        if (input.yVal < dungeonDimensionsColumns - 1)
        {
            if(tiles[input.xVal, input.yVal + 1].exitW == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitE == false) remove.Add(tile);
                }
            }
        }

        foreach (Tile tile in remove)
        {
            possibles.Remove(tile);
        }

        Tile chosen = possibles[Random.Range(0, possibles.Count - 1)];
        tiles[input.xVal, input.yVal] = chosen;
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

    public bool edge = false;

    public int xVal;
    public int yVal;

    public GameObject tileObject;
}

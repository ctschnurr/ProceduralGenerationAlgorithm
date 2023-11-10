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

    List<Tile> tileList;
    List<Tile> endsTileList;
    List<GameObject> allTiles;

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

        cornerA.exitN = false;
        cornerA.exitE = true;
        cornerA.exitS = true;
        cornerA.exitW = false;
        cornerA.tileObject = cornerAPrefab;

        cornerB.exitN = false;
        cornerB.exitE = false;
        cornerB.exitS = true;
        cornerB.exitW = true;
        cornerB.tileObject = cornerBPrefab;

        cornerC.exitN = true;
        cornerC.exitE = false;
        cornerC.exitS = false;
        cornerC.exitW = true;
        cornerC.tileObject = cornerCPrefab;

        cornerD.exitN = true;
        cornerD.exitE = true;
        cornerD.exitS = false;
        cornerD.exitW = false;
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
        startA.exitE = false;
        startA.exitS = true;
        startA.exitW = false;
        startA.tileObject = startAPrefab;

        startB.exitN = false;
        startB.exitE = true;
        startB.exitS = false;
        startB.exitW = false;
        startB.tileObject = startBPrefab;

        startC.exitN = true;
        startC.exitE = false;
        startC.exitS = false;
        startC.exitW = false;
        startC.tileObject = startCPrefab;

        startD.exitN = false;
        startD.exitE = false;
        startD.exitS = false;
        startD.exitW = true;
        startD.tileObject = startDPrefab;

        tCornerA.exitN = false;
        tCornerA.exitE = true;
        tCornerA.exitS = true;
        tCornerA.exitW = true;
        tCornerA.tileObject = tCornerAPrefab;

        tCornerB.exitN = true;
        tCornerB.exitE = true;
        tCornerB.exitS = true;
        tCornerB.exitW = false;
        tCornerB.tileObject = tCornerBPrefab; ;

        tCornerC.exitN = true;
        tCornerC.exitE = true;
        tCornerC.exitS = false;
        tCornerC.exitW = true;
        tCornerC.tileObject = tCornerCPrefab;

        tCornerD.exitN = true;
        tCornerD.exitE = false;
        tCornerD.exitS = true;
        tCornerD.exitW = true;
        tCornerD.tileObject = tCornerDPrefab;

        tileList = new List<Tile> { hallwayH, hallwayV, hallwayH, hallwayV, cornerA, cornerB, cornerC, cornerD, tCornerA, tCornerB, tCornerC, tCornerD, intersection };

        endsTileList = new List<Tile> { startA, startB, startC, startD };

        allTiles = new ();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dungeonDimensionsRows = 15;
            dungeonDimensionsColumns = 10;
            RunAgain();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dungeonDimensionsRows = 20;
            dungeonDimensionsColumns = 15;
            RunAgain();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dungeonDimensionsRows = 25;
            dungeonDimensionsColumns = 20;
            RunAgain();
        }

    }
    void Start()
    {
        dungeonDimensionsRows = 15;
        dungeonDimensionsColumns = 10;
        tiles = new Tile[dungeonDimensionsRows, dungeonDimensionsColumns];
        SetupTiles();
        CreateBase();
        PickStart();

        while (nextTiles.Count > 0)
        {
            Tile next = nextTiles.Dequeue();
            List<Tile> tempList = new List<Tile>(tileList);
            PickTile(next.xVal, next.yVal, tempList);
        }

        InstantiateDungeon();
    }

    void RunAgain()
    {
        foreach(GameObject tile in allTiles)
        {
            Destroy(tile);
        }

        tiles = new Tile[dungeonDimensionsRows, dungeonDimensionsColumns];
        SetupTiles();
        CreateBase();
        PickStart();

        while (nextTiles.Count > 0)
        {
            Tile next = nextTiles.Dequeue();
            List<Tile> tempList = new List<Tile>(tileList);
            PickTile(next.xVal, next.yVal, tempList);
        }

        InstantiateDungeon();
    }

    void CreateBase()
    {
        for (int x = 0; x < dungeonDimensionsRows; x++)
        {
            for (int y = 0; y < dungeonDimensionsColumns; y++)
            {
                Tile newTile = new Tile();
                newTile.tileObject = fullWall.tileObject;
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
                GameObject newObj = Instantiate(tiles[x, y].tileObject, new Vector3((x - dungeonDimensionsRows / 2) * 5, 0, (y - dungeonDimensionsColumns / 2) * 5), Quaternion.identity);
                newObj.name = tiles[x, y].xVal + "/" + tiles[x, y].yVal;

                allTiles.Add(newObj);
            }
        }
    }

    void PickStart()
    {
        int randomRow = Random.Range(3, dungeonDimensionsRows - 2);
        int randomCol = Random.Range(3, dungeonDimensionsColumns - 2);

        startX = randomRow;
        startY = randomCol;

        Debug.Log(startX + "/" + startY);

        PickTile(startX, startY, endsTileList);
    }

    void PickTile(int x, int y, List<Tile> possibles)
    {
        if (x < 1 || x > dungeonDimensionsRows - 2) return;
        if (y < 1 || y > dungeonDimensionsColumns - 2) return;

        List<Tile> remove = new List<Tile>();

        if (tiles[x + 1, y].tileObject != fullWallPrefab && tiles[x + 1, y].exitW == true)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitE == false) remove.Add(tile);
            }
        }

        if (tiles[x + 1, y].tileObject != fullWallPrefab && tiles[x + 1, y].exitW == false)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitE == true) remove.Add(tile);
            }
        }

        if (tiles[x - 1, y].tileObject != fullWallPrefab && tiles[x - 1, y].exitE == true)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitW == false) remove.Add(tile);
            }
        }

        if (tiles[x - 1, y].tileObject != fullWallPrefab && tiles[x - 1, y].exitE == false)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitW == true) remove.Add(tile);
            }
        }

        if (tiles[x, y - 1].tileObject != fullWallPrefab && tiles[x, y - 1].exitN == true)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitS == false) remove.Add(tile);
            }
        }

        if (tiles[x, y - 1].tileObject != fullWallPrefab && tiles[x, y - 1].exitN == false)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitS == true) remove.Add(tile);
            }
        }

        if (tiles[x, y + 1].tileObject != fullWallPrefab && tiles[x, y + 1].exitS == true)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitN == false) remove.Add(tile);
            }
        }

        if (tiles[x, y + 1].tileObject != fullWallPrefab && tiles[x, y + 1].exitS == false)
        {
            foreach (Tile tile in possibles)
            {
                if (tile.exitN == true) remove.Add(tile);
            }
        }

        foreach (Tile tile in remove)
        {
            possibles.Remove(tile);
        }

        Tile temp = tiles[x, y];
        if (possibles.Count > 0) temp = possibles[Random.Range(0, possibles.Count)];
        temp.xVal = tiles[x, y].xVal;
        temp.yVal = tiles[x, y].yVal;
        tiles[x, y] = temp;


        BuildMaze(tiles[x, y]);
     
    }

    void BuildMaze(Tile subjectTile)
    {
        if (subjectTile.exitE == true && subjectTile.xVal < dungeonDimensionsRows - 1)
        {
            Debug.Log("EAST CHECK");

            if (tiles[subjectTile.xVal + 1, subjectTile.yVal].tileObject == fullWallPrefab)
            {
                nextTiles.Enqueue(tiles[subjectTile.xVal + 1, subjectTile.yVal]);
                Debug.Log("EAST FIRE");
            }
        }

        if (subjectTile.exitS == true && subjectTile.yVal > 1)
        {
            Debug.Log("SOUTH CHECK");

            if (tiles[subjectTile.xVal, subjectTile.yVal - 1].tileObject == fullWallPrefab)
            {
                nextTiles.Enqueue(tiles[subjectTile.xVal, subjectTile.yVal - 1]);
                Debug.Log("SOUTH FIRE");
            }
        }

        if (subjectTile.exitW == true && subjectTile.xVal > 1)
        {
            Debug.Log("WEST CHECK");

            if (tiles[subjectTile.xVal - 1, subjectTile.yVal].tileObject == fullWallPrefab)
            {
                nextTiles.Enqueue(tiles[subjectTile.xVal - 1, subjectTile.yVal]);
                Debug.Log("WEST FIRE");
            }

        }

        if (subjectTile.exitN == true && subjectTile.yVal < dungeonDimensionsColumns - 1)
        {
            Debug.Log("NORTH CHECK");

            if (tiles[subjectTile.xVal, subjectTile.yVal + 1].tileObject == fullWallPrefab)
            {
                nextTiles.Enqueue(tiles[subjectTile.xVal, subjectTile.yVal + 1]);
                Debug.Log("NORTH FIRE");
            }
        }

        Debug.Log(nextTiles.Count);

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

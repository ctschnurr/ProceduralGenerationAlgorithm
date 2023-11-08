using System.Collections;
using System.Collections.Generic;
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
        DrawBorder();
        PickStart();
        BuildMaze(tiles[startX, startY]);

        while (nextTiles.Count > 0)
        {
            Tile next = nextTiles.Dequeue();
            BuildMaze(next);
        }
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
                Instantiate(fullWallPrefab, new Vector3(x * 5, 0, (dungeonDimensionsRows -1) * 5), Quaternion.identity);
            }
        }
        
        for (int z = 0; z < dungeonDimensionsRows; z++)
        {
            if (tiles[z, 0] == null)
            {
                tiles[z, 0] = fullWall;
                Instantiate(fullWallPrefab, new Vector3(0, 0, z * 5), Quaternion.identity);
            }
        }
        
        for (int z = 0; z < dungeonDimensionsRows; z++)
        {
            if (tiles[z, dungeonDimensionsColumns - 1] == null)
            {
                tiles[z, dungeonDimensionsColumns - 1] = fullWall;
                Instantiate(fullWallPrefab, new Vector3((dungeonDimensionsColumns - 1) * 5, 0, z * 5), Quaternion.identity);
            }
        }

    }

    void PickStart()
    {
        Tile picked = tiles[0, 0];
        int randomRow = -1;
        int randomCol = -1;

        while(picked != null)
        {
            randomRow = Random.Range(1, dungeonDimensionsRows);
            randomCol = Random.Range(1, dungeonDimensionsColumns);

            picked = tiles[randomRow, randomCol];
        }

        startX = randomRow;
        startY = randomCol;

        PickTile(startX, startY, endsTileList);
    }

    void PickEnd()
    {
        Tile picked = tiles[0, 0];
        int randomRow = -1;
        int randomCol = -1;

        while (picked != null)
        {
            randomRow = Random.Range(1, dungeonDimensionsRows);
            randomCol = Random.Range(1, dungeonDimensionsColumns);

            picked = tiles[randomRow, randomCol];
        }

        endX = randomRow;
        endY = randomCol;

        PickTile(randomRow, randomCol, endsTileList);
    }

    void BuildMaze(Tile subjectTile)
    {
        Debug.Log(subjectTile.tileObject.name);

        if (subjectTile.exitN == true && tiles[subjectTile.x, subjectTile.y + 1] == null) 
        {
            PickTile(subjectTile.x, subjectTile.y + 1, tileList);
            nextTiles.Enqueue(tiles[subjectTile.x, subjectTile.y + 1]);
        }

        // if (subjectTile.exitE == true && tiles[subjectTile.x - 1, subjectTile.y] == null)
        // {
        //     PickTile(subjectTile.x, subjectTile.y + 1, tileList);
        //     nextTiles.Enqueue(tiles[subjectTile.x - 1, subjectTile.y]);
        // }
        // 
        // if (subjectTile.exitS == true && tiles[subjectTile.x, subjectTile.y - 1] == null)
        // {
        //     PickTile(subjectTile.x, subjectTile.y + 1, tileList);
        //     nextTiles.Enqueue(tiles[subjectTile.x, subjectTile.y - 1]);
        // }
        // 
        // if (subjectTile.exitW == true && tiles[subjectTile.x + 1, subjectTile.y] == null)
        // {
        //     PickTile(subjectTile.x, subjectTile.y + 1, tileList);
        //     nextTiles.Enqueue(tiles[subjectTile.x + 1, subjectTile.y]);
        // }
    }

    void PickTile(int row, int col, List<Tile> possibles)
    {
        List<Tile> remove = new List<Tile>();

        if (tiles[row, col + 1] != null)
        {
            if (tiles[row, col + 1].exitS == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitN == false) remove.Add(tile);
                }
            }
        }

        if (tiles[row + 1, col] != null)
        {
            if (tiles[row + 1, col].exitE == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitW == false) remove.Add(tile);
                }
            }
        }

        if (tiles[row - 1, col] != null)
        {
            if (tiles[row - 1, col].exitW == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitE == false) remove.Add(tile);
                }
            }
        }

        if (tiles[row, col - 1] != null)
        {
            if (tiles[row, col - 1].exitN == true)
            {
                foreach (Tile tile in possibles)
                {
                    if (tile.exitS == false) remove.Add(tile);
                }
            }
        }

        foreach (Tile tile in remove)
        {
            possibles.Remove(tile);
        }

        Tile chosen = possibles[Random.Range(0, possibles.Count - 1)];
        chosen.x = row;
        chosen.y = col;
        tiles[row, col] = chosen;
        Instantiate(chosen.tileObject, new Vector3(col * 5, 0, row * 5), chosen.tileObject.transform.rotation);
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

    public int x;
    public int y;

    public GameObject tileObject;
}

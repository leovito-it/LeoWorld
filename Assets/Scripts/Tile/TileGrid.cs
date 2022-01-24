using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [HideInInspector] public int Rows;
    [HideInInspector] public int Cols;
    [HideInInspector] public GameObject TilePrefab;

    public Color TileColor_Default = new Color(0.86f, 0.83f, 0.83f);
    public Color TileColor_Expensive = new Color(0.19f, 0.65f, 0.43f);
    public Color TileColor_Infinity = new Color(0.37f, 0.37f, 0.37f);
    public Color TileColor_Start = Color.green;
    public Color TileColor_End = Color.red;
    public Color TileColor_Path = new Color(0.73f, 0.0f, 1.0f);
    public Color TileColor_Visited = new Color(0.75f, 0.55f, 0.38f);
    public Color TileColor_Frontier = new Color(0.4f, 0.53f, 0.8f);

    [HideInInspector] public Tile[] Tiles;
    [HideInInspector] public Tile tileSelected;


    private IEnumerator _pathRoutine;
    private Tile _start, _end;

    public void CreateTilemap(Transform parent)
    {
        Tiles = new Tile[Rows * Cols];
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                Tile tile = new Tile(this, r, c, Values.TileWeight_Default);
                tile.InitGameObject(parent, TilePrefab);
                tile.GetButton().onClick.AddListener(delegate { GameController.HandleTileClicked(tile); });

                int index = GetTileIndex(r, c);
                Tiles[index] = tile;
            }
        }
        ResetGrid();
    }

    public void SetStartPos(Tile tile)
    {
        if (tile.Weight == Values.TileWeight_Default)
        {
            _start = tile;
            tile.SetColor(TileColor_Start);
        }
    }

    public bool IsStart(Tile tile)
    {
        return tile == _start;
    }

    public bool IsEnd(Tile tile)
    {
        return tile == _end;
    }

    public bool IsRunningState()
    {
        return (_start != null) && (_end != null) && (_start != _end);
    }

    public void ClearStart_End()
    {     
        _start.SetColor(TileColor_Default);    
        _end.SetColor(TileColor_Default);
        _start = null;
        _end = null;
    }

    public void SetEndPos(Tile tile)
    {
        if (tile.Weight == Values.TileWeight_Default)
        {
            _end = tile;
            tile.SetColor(TileColor_End);
        }
    }

    public void AstarFindWay()
    {
        Debug.Log("Searching");
        StopPathCoroutine();
        _pathRoutine = FindPath(_start, _end, PathFinder.FindPath_AStar);
        StartCoroutine(_pathRoutine);
    }

    private void StopPathCoroutine()
    {
        if (_pathRoutine != null)
        {
            StopCoroutine(_pathRoutine);
            _pathRoutine = null;
        }
    }

    public void CreateExpensiveArea(int row, int col, int width, int height, int weight)
    {
        for (int r = row; r < row + height; r++)
        {
            for (int c = col; c < col + width; c++)
            {
                Tile tile = GetTile(r, c);
                if (tile != null)
                {
                    tile.Weight = weight;
                }
            }
        }
        ResetGrid();
    }

    public void CreateExpensive(int row, int col)
    {
        Tile tile = GetTile(row, col);
        if (tile != null)
        {
            tile.Weight = Values.TileWeight_Expensive;
            tile.SetColor(TileColor_Expensive);
        }
    }

    public void RemoveExpensive(int row, int col)
    {
        Tile tile = GetTile(row, col);
        if (tile != null)
        {
            tile.Weight = Values.TileWeight_Default;
            tile.SetColor(TileColor_Default);
        }
    }

    public void ResetGrid()
    {
        foreach (var tile in Tiles)
        {
            tile.Cost = 0;
            tile.PrevTile = null;
            tile.SetText("");

            switch (tile.Weight)
            {
                case Values.TileWeight_Default:
                    tile.SetColor(TileColor_Default);
                    break;
                case Values.TileWeight_Expensive:
                    tile.SetColor(TileColor_Expensive);
                    break;
                case Values.TileWeight_Infinity:
                    tile.SetColor(TileColor_Infinity);
                    break;
            }
        }
    }

    private IEnumerator FindPath(Tile start, Tile end, Func<TileGrid, Tile, Tile, List<IVisualStep>, List<Tile>> pathFindingFunc)
    {
        ResetGrid();

        List<IVisualStep> steps = new List<IVisualStep>();
        pathFindingFunc(this, start, end, steps);

        foreach (var step in steps)
        {
            step.Execute();
            yield return new WaitForFixedUpdate();
        }
    }

    public Tile GetTile(int row, int col)
    {
        if (!IsInBounds(row, col))
        {
            return null;
        }

        return Tiles[GetTileIndex(row, col)];
    }

    public IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        Tile right = GetTile(tile.Row, tile.Col + 1);
        if (right != null)
        {
            yield return right;
        }

        Tile up = GetTile(tile.Row - 1, tile.Col);
        if (up != null)
        {
            yield return up;
        }

        Tile left = GetTile(tile.Row, tile.Col - 1);
        if (left != null)
        {
            yield return left;
        }

        Tile down = GetTile(tile.Row + 1, tile.Col);
        if (down != null)
        {
            yield return down;
        }
    }

    private bool IsInBounds(int row, int col)
    {
        bool rowInRange = row >= 0 && row < Rows;
        bool colInRange = col >= 0 && col < Cols;
        return rowInRange && colInRange;
    }

    private int GetTileIndex(int row, int col)
    {
        return row * Cols + col;
    }
}
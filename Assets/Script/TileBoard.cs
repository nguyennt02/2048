using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefabs;
    public TileState[] states;
    public GameObjectPooling tilePooling;

    private TileGrid grid;
    private List<Tile> lstTiles;

    private IInputManager inputManager;
    private DIRECTION directionInput;

    private bool waiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        lstTiles = new List<Tile>(16);
    }
    private void Start(){
        inputManager = InputWindows.Intance;
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }
        lstTiles.Clear();
        tilePooling.ClearPoolingTile();
    }

    public void CreateTile()
    {
        Tile tile = tilePooling.GetPoolingTile().GetComponent<Tile>();
        tile.gameObject.SetActive(true);
        tile.SetState(states[0], 2);
        tile.Spawn(grid.GetRandomEmptyCell());
        lstTiles.Add(tile);
    }

    private void Update()
    {
        if (waiting) return;
        InputManager();
    }
    private void InputManager()
    {
        directionInput = inputManager.GetInput();
        if (directionInput == DIRECTION.NULL)
        {
            return;
        }
        else if (directionInput == DIRECTION.UP)
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (directionInput == DIRECTION.DOWN)
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
        }
        else if (directionInput == DIRECTION.LEFT)
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (directionInput == DIRECTION.RIGHT)
        {
            MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }
        if (changed)
        {
            StartCoroutine(WaitForChanges());
            AudioManager.Instance.PlaySFX("Click");
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);
        while (adjacentCell != null)
        {
            if (adjacentCell.occupied)
            {
                if (CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }
                break;
            }
            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.looked;
    }

    private void Merge(Tile a, Tile b)
    {
        lstTiles.Remove(a);
        a.Meger(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, states.Length - 1);
        int number = b.number * 2;

        b.SetState(states[index], number);
        GameManager.Instance.IncreaseScore(number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (state == states[i])
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in lstTiles)
        {
            tile.looked = false;
        }

        if (lstTiles.Count != grid.size)
        {
            CreateTile();
        }

        if(CheckGameOver()){
            AudioManager.Instance.musicSource.Stop();
            AudioManager.Instance.PlaySFX("GameOver");
            GameManager.Instance.GameOver();
        }
    }

    private bool CheckGameOver()
    {
        if (lstTiles.Count != grid.size)
        {
            return false;
        }
        foreach (var cell in grid.cells)
        {
            TileCell adjacentCellUp = grid.GetAdjacentCell(cell, Vector2Int.up);
            if (adjacentCellUp != null && CanMerge(cell.tile, adjacentCellUp.tile))
            {
                return false;
            }

            TileCell adjacentCellDown = grid.GetAdjacentCell(cell, Vector2Int.down);
            if (adjacentCellDown != null && CanMerge(cell.tile, adjacentCellDown.tile))
            {
                return false;
            }

            TileCell adjacentCellLeft = grid.GetAdjacentCell(cell, Vector2Int.left);
            if (adjacentCellLeft != null && CanMerge(cell.tile, adjacentCellLeft.tile))
            {
                return false;
            }

            TileCell adjacentCellRight = grid.GetAdjacentCell(cell, Vector2Int.right);
            if (adjacentCellRight != null && CanMerge(cell.tile, adjacentCellRight.tile))
            {
                return false;
            }
        }
        return true;
    }
}

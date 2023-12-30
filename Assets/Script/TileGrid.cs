using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;

    private void Awake(){
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start(){
        AssignCoordinates();
    }
    
    void AssignCoordinates(){
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                rows[x].cells[y].coordinates = new Vector2Int(y,x);
            }
        }
    }

    public TileCell GetCell(int x, int y){
        if(x >= 0 && x < width && y >= 0 && y < height){
            return rows[y].cells[x];
        }else{
            return null;
        }
    }

    public TileCell GetCell(Vector2Int coordinates){
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction){
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public TileCell GetRandomEmptyCell(){
        int index = Random.Range(0,size);
        int startingIndex = index;
        while(cells[index].occupied){
            index++;
            if(index >= size){
                index = 0;
            }
            if(index == startingIndex){
                return null;
            }
        }
        return cells[index];
    }
}

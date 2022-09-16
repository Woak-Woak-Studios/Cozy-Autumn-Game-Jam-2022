using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSurface : MonoBehaviour
{
    public enum SurfaceType {Floor, Wall, Shelf, Counter};

    public SurfaceType surfType = SurfaceType.Shelf;
    public int gridX;
    public int gridY;
    GridData[,] gridSlots;
    public static float gridScale = 0.1f;

    struct GridData{
        public HeldObject heldObject;
        public bool taken;
    }

    // Start is called before the first frame update
    void Start()
    {
        gridSlots = new GridData[gridX,gridY];
    }

    void OnValidate(){
        transform.localScale = new Vector3(gridX*gridScale,gridY*gridScale,1f);
    }
    public void PickUp(HeldObject ho){
        Vector2Int gridPos = ho.mySurfaceGridPos;
        Vector2Int gridFootprint = ho.getFootprint();
        Vector2Int footprintCenter = new Vector2Int((int)(gridFootprint.x/2f),(int)(gridFootprint.y/2f));
        for (int i = gridPos.x-footprintCenter.x; i < gridPos.x-footprintCenter.x+gridFootprint.x;i++){
            for (int j = gridPos.y-footprintCenter.y; j < gridPos.y-footprintCenter.y+gridFootprint.y;j++){
                if (!gridSlots[i,j].taken){
                    Debug.LogError("SOMETHING HAS GONE RLY WRONG HAAAAP");
                }
                gridSlots[i,j].taken = false;
                gridSlots[i,j].heldObject = null;
            }
        }
        ho.mySurface = null;
        ho.mySurfaceGridPos = Vector2Int.zero;
    }

    public bool AttemptPlacement(Vector3 worldPosHover, HeldObject ho){
        Vector2Int gridPos;
        Vector3 worldPos = GetPositionOnGrid(worldPosHover,ho,out gridPos);
        if (!Vector3.Equals(worldPos,Vector3.negativeInfinity)){
            Vector2Int gridFootprint = ho.getFootprint();
            Vector2Int footprintCenter = new Vector2Int((int)(gridFootprint.x/2f),(int)(gridFootprint.y/2f));
            for (int i = gridPos.x-footprintCenter.x; i < gridPos.x-footprintCenter.x+gridFootprint.x;i++){
                for (int j = gridPos.y-footprintCenter.y; j < gridPos.y-footprintCenter.y+gridFootprint.y;j++){
                    if (gridSlots[i,j].taken){
                        Debug.LogError("SOMETHING HAS GONE RLY WRONG HAAAAP");
                    }
                    gridSlots[i,j].taken = true;
                    gridSlots[i,j].heldObject = ho;
                }
            }
            ho.transform.position = worldPos;
            ho.mySurface = this;
            ho.mySurfaceGridPos = gridPos;
            return true;
        }


        return false;
    }

    public Vector3 GetPositionOnGrid(Vector3 worldPos, HeldObject ho){
        Vector2Int throwing;
        return GetPositionOnGrid(worldPos,ho,out throwing);
    }

    public Vector3 GetPositionOnGrid(Vector3 worldPos, HeldObject ho, out Vector2Int finalGridPos){
        Vector2Int gridPos = WorldToGrid(worldPos);
        Vector2Int gridFootprint = ho.getFootprint();
        Vector2Int invalidVector = Vector2Int.zero;
        if (CheckGridPlace(gridPos,gridFootprint,out invalidVector)){
            finalGridPos = gridPos;
            return GridToWorld(gridPos);
        }else{
            Vector2Int throwing;
            if (CheckGridPlace(gridPos-invalidVector,gridFootprint,out throwing)){
                finalGridPos = gridPos-invalidVector;
                return GridToWorld(gridPos-invalidVector);
            }
        }

        finalGridPos = Vector2Int.zero;
        return Vector3.negativeInfinity;
    }

    bool CheckGridPlace (Vector2Int gridPos, Vector2Int gridFootprint,out Vector2Int invalidVector){
        invalidVector = Vector2Int.zero;
        Vector2Int footprintCenter = new Vector2Int((int)(gridFootprint.x/2f),(int)(gridFootprint.y/2f));
        bool validPlace = true;
        for (int i = gridPos.x-footprintCenter.x; i < gridPos.x-footprintCenter.x+gridFootprint.x;i++){
            for (int j = gridPos.y-footprintCenter.y; j < gridPos.y-footprintCenter.y+gridFootprint.y;j++){
                if (i <0 || i >= gridX || j < 0 || j >= gridY || gridSlots[i,j].taken){
                    validPlace = false;
                    invalidVector += new Vector2Int(i-gridPos.x,j-gridPos.y);
                }
            }
        }
        return validPlace;
    }

    public Vector2Int WorldToGrid(Vector3 pos){
        Vector3 localPos = transform.InverseTransformPoint(pos);
        localPos = new Vector3(localPos.x*transform.localScale.x,localPos.y*transform.localScale.y,0f);
        localPos /= gridScale;
        Vector2Int gridPos = new Vector2Int((int)(localPos.x+gridX/2f),(int)(localPos.y+gridY/2f));
        gridPos = new Vector2Int(Mathf.RoundToInt(Mathf.Clamp(gridPos.x,0,gridX-1)),Mathf.RoundToInt(Mathf.Clamp(gridPos.y,0,gridY-1)));
        return gridPos;
    }

    public Vector3 GridToWorld(Vector2Int grid){
        Vector3 pos = new Vector3(grid.x+0.5f-gridX/2f,grid.y+0.5f-gridY/2f,0f);
        pos *= gridScale;
        pos = new Vector3(pos.x/transform.localScale.x,pos.y/transform.localScale.y,0f);
        return transform.TransformPoint(pos);
    }

    public Vector3 CenterOnGrid(Vector3 pos){
        return GridToWorld(WorldToGrid(pos));
    }
}

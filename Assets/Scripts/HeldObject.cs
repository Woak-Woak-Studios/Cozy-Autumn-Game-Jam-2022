using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour
{
    public enum ObjectOrientation {North,East,South,West};

    public int footprintX;
    public int footprintY;
    public ObjectOrientation orientation = ObjectOrientation.North;

    [HideInInspector]
    public PlaceSurface mySurface;
    [HideInInspector]
    public Vector2Int mySurfaceGridPos;

    public Vector2Int getFootprint(){
        if (orientation == ObjectOrientation.North || orientation == ObjectOrientation.South){
            return new Vector2Int(footprintX,footprintY);
        }else{
            return new Vector2Int(footprintY,footprintX);
        }
    }
}

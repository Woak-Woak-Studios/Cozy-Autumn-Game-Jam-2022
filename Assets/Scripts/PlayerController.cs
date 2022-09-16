using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    Vector2 cursorPos;
    Vector2 cursorPosLast;
    Vector2 cursorPosRead;
    bool panning;

    LayerMask maskPlaceSurfaces;
    LayerMask maskClickableObjects;
    bool holding;
    HeldObject heldObject;

    public float hoverHeight = 0.03f;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cursorPos = Vector2.zero;
        cursorPosLast = Vector2.zero;

        maskPlaceSurfaces = LayerMask.GetMask("Place Surface");
        maskClickableObjects = LayerMask.GetMask("Clickable Objects");
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosLast = cursorPos;
        cursorPos = cursorPosRead;
        Panning();

        Holding();
    }

//INPUT CALLBACKS
    public void CursorPositionChanged(InputAction.CallbackContext callback){
        cursorPosRead = callback.ReadValue<Vector2>();
    }

    public void OnPanClick(InputAction.CallbackContext callback){
        panning = callback.ReadValueAsButton();
    }

    public void ScrollDeltaChanged(InputAction.CallbackContext callback){
        Zoom(callback.ReadValue<Vector2>());
    }

    public void OnSelectClick(InputAction.CallbackContext callback){
        bool val = callback.ReadValueAsButton();
        if (val && callback.started){
            Select();
        }
    }

//SELECTION CODE
    void Select(){
        if (!holding){
            Ray ray = cam.ScreenPointToRay(cursorPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100,maskClickableObjects)){
                HeldObject ho = hit.transform.parent.GetComponent<HeldObject>();
                if (ho != null){
                    if (ho.mySurface != null){
                        ho.mySurface.PickUp(ho);
                    }
                    holding = true;
                    heldObject = ho;
                    return;
                }
                BoxDispenser bd = hit.transform.parent.GetComponent<BoxDispenser>();
                if (bd != null){
                    bd.ClickMe();
                }
            }
        }else{
            RaycastHit hit;
            PlaceSurface surf = CastForSurface(out hit);
            if(surf != null){
                bool success = surf.AttemptPlacement(hit.point,heldObject);
                if (success){
                    holding = false;
                    heldObject = null;
                }
            }
        }
    }

    void Holding(){
        if ( holding && heldObject != null){
            heldObject.transform.position = GetMouseWorldPosition(cursorPos,5f);

            RaycastHit hit;
            PlaceSurface surf = CastForSurface(out hit);
            if(surf != null){
                Vector3 gridPosition = surf.GetPositionOnGrid(hit.point,heldObject);
                if (!Vector3.Equals(gridPosition,Vector3.negativeInfinity)){
                   heldObject.transform.position = gridPosition + heldObject.transform.up * hoverHeight;
                }
             }
        }
    }

    PlaceSurface CastForSurface(out RaycastHit hit){
        Ray ray = cam.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(ray, out hit, 100,maskPlaceSurfaces)){
            PlaceSurface surf = hit.transform.GetComponent<PlaceSurface>();
            return surf;
        }
        return null;
    }

//ZOOMINGING CODE

    void Zoom(Vector2 scroll){
        cam.orthographicSize += -scroll.y*0.001f;
        if (cam.orthographicSize < 0.5f){
            cam.orthographicSize = 0.5f;
        }
    }

//PANNING CODE
    void Panning(){
        if (panning){
            MoveScreen();
        }
    }

    void MoveScreen(){
        MoveScreen(5f);
    }

    void MoveScreen (float dist){
        Vector3 oldPos = GetMouseWorldPosition(cursorPosLast,dist);
        Vector3 newPos = GetMouseWorldPosition(cursorPos,dist);
        Vector3 delta = oldPos-newPos;
        transform.position += transform.right * Vector3.Dot(transform.right,delta);
        transform.position += transform.up * Vector3.Dot(transform.up,delta);
    }

    Vector3 GetMouseWorldPosition(Vector2 position, float dist){
        Vector3 pos3D = new Vector3(position.x,position.y,dist);
        return cam.ScreenToWorldPoint(pos3D);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDispenser : MonoBehaviour
{
    public Sprite spriteTopOpen;
    public Sprite spriteBottomOpen;

    GameObject[] objects;
    string[] names;
    string[] colors;

    bool opened = false;
    int dispenseIndex = 0;
    HeldObject lastSpawned;
    
    void Start()
    {
        
    }

    public void Setup(GameObject[] o, string[] n, string[] c){
        objects = o;
        names = n;
        colors = c;
    }

    public void ClickMe(){
        if (!opened){
            Open();
            return;
        }
        if (dispenseIndex == objects.Length){
            Destroy(this.gameObject);
            return;
        }
        if (lastSpawned != null && lastSpawned.mySurface == null){
            return;//Pick up the last one i spat out first ya noodle
        }
        lastSpawned = Instantiate (objects[dispenseIndex],transform.position+transform.up*0.5f, Quaternion.identity).GetComponent<HeldObject>();
        lastSpawned.name = names[dispenseIndex];
        lastSpawned.ChangeColor(colors[dispenseIndex]);
        dispenseIndex++;
    }

    void Open(){
        opened = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteTopOpen;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = spriteBottomOpen;
    }
}

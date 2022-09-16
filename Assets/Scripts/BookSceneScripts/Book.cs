using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Book
{

    [TextArea(4, 10)]
    public string[] leftPages;

    [TextArea(4, 10)]
    public string[] rightPages;
}

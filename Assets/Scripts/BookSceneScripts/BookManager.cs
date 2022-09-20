using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{

    public Text leftText;
    public Text rightText;
    
    private Queue<string> leftPages; //declaration
    private Queue<string> rightPages;
    
    // Start is called before the first frame update
    void Start()
    {
        leftPages = new Queue<string>(); //initialization
        rightPages = new Queue<string>();
    }

    public void StartBook (Book book)
    {

        leftPages.Clear();
        rightPages.Clear();

        foreach (string leftPage in book.leftPages)
        {
            leftPages.Enqueue(leftPage);
        }

        foreach (string rightPage in book.rightPages)
        {
            rightPages.Enqueue(rightPage);
        }

        DisplayNextPage();
    }

    public void DisplayNextPage ()
    {
        if(leftPages.Count == 0 && rightPages.Count == 0)
        {
            EndBook();
            return;
        } 
        else if (rightPages.Count == 0 && leftPages.Count > 0)
        {
            rightPages.Clear();
            string rightPage = ("The End.");
            rightText.text = rightPage;

            string leftPage = leftPages.Dequeue();
            leftText.text = leftPage;
        }
        else
        {
        string leftPage = leftPages.Dequeue();
        leftText.text = leftPage;

        string rightPage = rightPages.Dequeue();
        rightText.text = rightPage;
        }
    }


    void EndBook()
    {
        Debug.Log("End of book.");
    }

}

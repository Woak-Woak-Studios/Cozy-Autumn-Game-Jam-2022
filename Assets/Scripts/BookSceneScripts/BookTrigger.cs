using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTrigger : MonoBehaviour
{
    public Book book;

    public void TriggerBook ()
    {
        FindObjectOfType<BookManager>().StartBook(book);
    }
}

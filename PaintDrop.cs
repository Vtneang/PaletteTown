using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintDrop : MonoBehaviour
{
    public int value; 
    public PlayerScript.PaintColors color;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || ( collision.transform.parent && collision.transform.parent.CompareTag("Player")))
        {
            collision.transform.GetComponentInParent<PlayerScript>().addColor(value, color);
            Destroy(this.gameObject);

        }
        





    }
}

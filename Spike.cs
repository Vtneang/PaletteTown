using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Spike") || !collision.gameObject.CompareTag("Floor"))
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Fake Player")
                || collision.transform.parent.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerScript>().Hurt(3);
            }
            else
            {
                Destroy(collision.gameObject);
            }   
        }
    }
}

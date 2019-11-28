using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BlackPaint : MonoBehaviour
{
    #region Editor Variables
    public float damage_amount;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        try
        {
            if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
            {
                if (collision.transform.GetComponentInParent<PlayerScript>().m_health - damage_amount <= 0)
                {
                    Debug.Log("OH NIII");
                    StartCoroutine(GameObject.FindGameObjectWithTag("End").GetComponent<EndLevel>().Restart());
                }
                collision.transform.GetComponentInParent<PlayerScript>().Hurt(damage_amount);
                Destroy(this.gameObject);
            }
        } catch(Exception e) {
        
        }




    }
}

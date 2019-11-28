using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPaint : MonoBehaviour
{
    #region Editor Variables
    public int damage_amount;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
    	if (collision.transform.CompareTag("Enemy")) {
    		collision.transform.GetComponent<EnemyScript>().Damage(damage_amount);
    	} else if (collision.gameObject.CompareTag("DestroyableBlock"))
        {
            Debug.Log("HIIIII");
            Destroy(collision.gameObject);
        }
	}
}

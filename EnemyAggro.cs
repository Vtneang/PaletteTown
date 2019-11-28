using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public Transform player;
    public float shootDelay;
    public float lastShot;
    // Start is called before the first frame update
    void Start()
    {
        lastShot = shootDelay; 
    }

    // Update is called once per frame
    void Update()
    {
        if (player && lastShot >= shootDelay)
        {
            float x_diff = player.position.x - player.position.x;
            Debug.Log("shooting");
            GetComponentInParent<EnemyScript>().Shoot(x_diff > 0);
            lastShot = 0;
        } else
        {
            lastShot += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            player = col.transform;
            lastShot = shootDelay;
            //float x_diff = col.transform.position.x - transform.position.x;
            //Debug.Log("x_diff: " + x_diff);
            //GetComponentInParent<EnemyScript>().Shoot(x_diff > 0);

        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            player = null;
        }
    }
}

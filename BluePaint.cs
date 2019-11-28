using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePaint : MonoBehaviour {
	#region paint_vars
	public float slip_amount = .75f;
  #endregion



  #region functions
  private void OnTriggerEnter2D(Collider2D collision) {
    Debug.Log("This is Blue collision." + collision.transform.tag);
    if (collision.transform.CompareTag("PlayerFeet") || collision.transform.CompareTag("Player")) {
        PlayerScript ps = collision.transform.GetComponentInParent<PlayerScript>();
        if (ps.touchingBlue == 0){
            ps.movespeed += slip_amount;
            Debug.Log("speed is now " + collision.transform.GetComponentInParent<PlayerScript>().movespeed);
        }
        ps.touchingBlue += 1;

    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    Debug.Log("You exit collision." + collision.transform.tag);
    if (collision.transform.CompareTag("PlayerFeet") || collision.transform.CompareTag("Player")) {
            PlayerScript ps = collision.transform.GetComponentInParent<PlayerScript>();
            ps.touchingBlue -= 1;
        if (ps.touchingBlue == 0){
            ps.movespeed -= slip_amount;
            Debug.Log("speed is now " + collision.transform.GetComponentInParent<PlayerScript>().movespeed);

        }
    }
  }
    #endregion
    private void Update()
    {
        Debug.Log("velocity: " + GetComponent<Rigidbody2D>().velocity);
    }
}

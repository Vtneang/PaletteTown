using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
	public int numCollided = 0;
	public GameObject player;

	#region speed_vars
	public float slow_amount;
	public float slip_amount;
	#endregion

    // Returns whether the obj is a floor, platform, or wall
	bool isFloor(GameObject obj) {
		return obj.tag == "Floor" || obj.tag == "DestroyableBlock" || obj.tag == "Platform";
	}
    void OnCollisionEnter2D(Collision2D collision) {
        GetComponentInParent<PlayerScript>().feetContact = isFloor(collision.gameObject) ? 1 : 0;
        GetComponentInParent<PlayerScript>().jumped = false;
        if (collision.gameObject.CompareTag("Platform")) {
        	player.transform.parent = collision.gameObject.transform;
        }
        GetComponentInParent<PlayerScript>().doublejump = false;
    }

    void OnCollisionExit2D(Collision2D collision) {
        GetComponentInParent<PlayerScript>().feetContact = isFloor(collision.gameObject) ? 0 : 1;
        GetComponentInParent<PlayerScript>().doublejump = true;
        if (collision.gameObject.CompareTag("Platform")) {
        	player.transform.parent = null;
        }
    }
}

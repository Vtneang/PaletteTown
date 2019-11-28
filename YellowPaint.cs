using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Flips Player and slows it down when touching vertical surfaces with yellow paint 
 */

public class YellowPaint : MonoBehaviour
{
    #region paint_vars
	public float slow_amount = 1;
    #endregion

    #region functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("This is Yellow collision. " + collision.tag);

        if (collision.transform.CompareTag("PlayerHead")) {
            Debug.Log("touching head");
            GetComponentInParent<PlayerScript>().feetContact = 1;
            if (collision.transform.GetComponent<HeadScript>().numCollided == 0) {
                //while (true)
                //{

                //}
                //collision.transform.GetComponentInParent<PlayerScript>().GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                //collision.transform.GetComponentInParent<PlayerScript>().freeze = true;
                collision.GetComponentInParent<PlayerScript>().jumped = false;
                collision.GetComponentInParent<PlayerScript>().doublejump = false;
                collision.transform.GetComponentInParent<PlayerScript>().headFirst = true;
                 // change gravity
                 Debug.Log("change gravity");
                 collision.transform.GetComponentInParent<PlayerScript>().GravityChange(false);
                 // flip player
                 collision.transform.GetComponentInParent<PlayerScript>().FlipPlayerY();

            }
            collision.transform.GetComponent<HeadScript>().numCollided += 1;
        }

        //if (collision.transform.CompareTag("PlayerFeet")) {
        //    Debug.Log("touching feet");
        //    if (collision.transform.GetComponent<FeetScript>().numCollided == 0) {
        //        collision.transform.GetComponentInParent<PlayerScript>().movespeed -= slow_amount;
        //        Debug.Log("Yellow speed is now " + collision.transform.GetComponentInParent<PlayerScript>().movespeed);
        //    }
        //    collision.transform.GetComponent<FeetScript>().numCollided += 1;
        //}

        if (collision.transform.CompareTag("PlayerRight") || collision.transform.CompareTag("PlayerLeft")) {
            Debug.Log("Touched side of player");
            PlayerScript playerScript = collision.transform.GetComponentInParent<PlayerScript>();
            Rigidbody2D playerRB = playerScript.playerRB;
            playerScript.feetContact = 1;
            playerScript.jumped = false;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            playerScript.doublejump = false;
            if (playerScript.numCollided == 0) {
                playerRB.gravityScale = 0f;
                //playerScript.movespeed -= slow_amount;
                playerScript.onWall = true;

                if (collision.transform.CompareTag("PlayerRight")){
                    //animate a flip to the right
                    Debug.Log("touched right");
                    playerScript.rightCollided += 1;

                }

                if (collision.transform.CompareTag("PlayerLeft")) {
                    //animate a flip to the left
                    Debug.Log("touched right");
                    playerScript.leftCollided += 1; 
                }
            }
            playerRB.velocity = Vector2.zero;
            playerScript.numCollided += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("You exit collision.");
        if (collision.transform.CompareTag("PlayerHead")) {
            PlayerScript playerScript = collision.transform.GetComponentInParent<PlayerScript>();
            playerScript.feetContact = 0;
            playerScript.doublejump = true;
            if (!playerScript.onWall)
            {
                collision.transform.GetComponent<HeadScript>().numCollided -= 1;
            }
         }

        if (collision.transform.CompareTag("PlayerFeet"))
        {
            PlayerScript playerScript = collision.transform.GetComponentInParent<PlayerScript>();
            FeetScript fscript = collision.transform.GetComponent<FeetScript>();
            fscript.numCollided -= 1;
            playerScript.feetContact = 0;
            playerScript.doublejump = true;

            if (fscript.numCollided == 0) {
                 if (playerScript.headFirst) {
                     playerScript.GravityChange(true);
                     playerScript.FlipPlayerY();
                     //playerScript = false;
                 }
                
                playerScript.movespeed += 1;
                
                Debug.Log("foot: speed is now " + playerScript.movespeed);
            }
        }

        if (collision.transform.CompareTag("PlayerRight") || collision.transform.CompareTag("PlayerLeft")) {
            
            PlayerScript playerScript = collision.transform.GetComponentInParent<PlayerScript>();
            playerScript.numCollided -= 1;
            playerScript.feetContact = 0;
            playerScript.doublejump = true;

            if (collision.transform.CompareTag("PlayerRight"))
            {
                playerScript.rightCollided -= 1; 
            }
            if (collision.transform.CompareTag("PlayerLeft"))
            {
                playerScript.leftCollided -= 1;

            }


            Rigidbody2D playerRB = playerScript.playerRB;

            if (playerScript.numCollided == 0 && playerScript.onWall) {
                Debug.Log("side: speed is now " + playerScript.movespeed);
                playerRB.gravityScale = 1f;
                //playerScript.movespeed += slow_amount;
                playerScript.onWall = false;   
            }
            //playerRB.velocity =;      
        }
    }
    #endregion
}

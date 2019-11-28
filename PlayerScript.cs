using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //tbh prob better to make this in a separate class later
    public enum PaintColors
    {
        Red,
        Blue,
        Yellow,
        Orange,
        Green,
        Purple,
        Black
    }

    public static Dictionary<PaintColors, string> paintPrefabs = new Dictionary<PaintColors, string>()
        {
            { PaintColors.Blue, "Blue" },
            { PaintColors.Red, "Red" },
            { PaintColors.Yellow, "Yellow" },
            { PaintColors.Orange, "Orange" },
            { PaintColors.Green, "Green" },
            { PaintColors.Purple, "Purple" },
            { PaintColors.Black, "Black" }
        };
    public bool freeze;
    public PaintColors currColor; 

    #region Player variables
    [SerializeField]
    [Tooltip("Amount of health the player has.")]
    public float m_health;

    [SerializeField]
    [Tooltip("The amount of starting red")]
    public float r_val;

    [SerializeField]
    [Tooltip("The amount of starting blue")]
    public float b_val;

    [SerializeField]
    [Tooltip("The amount of starting yellow")]
    public float y_val;

    [SerializeField]
    [Tooltip("The amount of starting orange")]
    public float o_val;

    [SerializeField]
    [Tooltip("The amount of starting green")]
    public float g_val;

    [SerializeField]
    [Tooltip("The amount of starting purple")]
    public float p_val;
    #endregion

    #region attack_variables
    public float damage;
    public float attackspeed;
    float attackTimer;
    public float hitboxTiming;
    public float endAnimationTiming;
    bool isPainting;
    Vector2 currDirection;
    #endregion

	#region animation_components
    Animator anim;
    #endregion

	#region movement_vars
	public float movespeed;
	public float jumpforce;
	public int feetContact;
    public bool jumped;
    public bool doublejump;
	public int touchingBlue;
	public bool onWall;
	public int numCollided;
    public int leftCollided;
    public int rightCollided;
	float x_input;
	float y_input;
    public bool menuon;
	#endregion

	#region gravity_vars
    public bool gravity; // true is normal, false is flipped gravity
    public bool headFirst;
    #endregion

    public bool facingRight;
    public Camera cam;

    #region physics_components
    public Rigidbody2D playerRB;
    public GameObject sidePaintSpawner;
    public GameObject ps;
    public GameObject ui;
    #endregion

    #region Unity_functions
    //called once on creation
    private void Awake()
    {
        menuon = false;
    	touchingBlue = 0;
        feetContact = 0;
        jumped = false;
    	playerRB = GetComponent<Rigidbody2D>();
    	anim = GetComponent<Animator>();
    	gravity = true;
    	headFirst = false;
    	
        facingRight = true;
        freeze = false; // for debugging
        sidePaintSpawner = GameObject.FindGameObjectWithTag("SidePaintSpawner");
        ps = GameObject.FindGameObjectWithTag("MasterPaintSpawner");
        ui.GetComponent<HealthUI>().ResetBar();
        ui.GetComponent<HealthUI>().ChangeBar(paintPrefabs[currColor]);
    }
    //only call this first time player presses a 1,2,3
    private void ChangeColor()
    {
        ui.GetComponent<HealthUI>().ResetBar();
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Alpha3))
        {
            Debug.Log("Black not implemented");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            /*if (Input.GetKey(KeyCode.Alpha2))
            {
                Debug.Log("switching to purple");
                currColor = PaintColors.Purple;
            } else if (Input.GetKey(KeyCode.Alpha3))
            {
                Debug.Log("switching to Green");
                currColor = PaintColors.Green;
            }
            else
            {*/
            Debug.Log("switching to blue");
            currColor = PaintColors.Blue;
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) )
        {
            /*if (Input.GetKey(KeyCode.Alpha3))
            {
                Debug.Log("switching to Orange");
                currColor = PaintColors.Orange;
            }
            else
            {*/
            Debug.Log("switching to red");
            currColor = PaintColors.Red;
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("switching to yellow");
            currColor = PaintColors.Yellow;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("switching to Orange");
            currColor = PaintColors.Orange;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("switching to Green");
            currColor = PaintColors.Green;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            Debug.Log("switching to purple");
            currColor = PaintColors.Purple;
        }
        ui.GetComponent<HealthUI>().ChangeBar(paintPrefabs[currColor]);
    }

// Called every frame
private void Update() 
{
        if (freeze)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)
            || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6)) {
            ChangeColor();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!menuon)
            {
                menuon = true;
                ui.GetComponent<HealthUI>().activatemenu();
            } else
            {
                menuon = false;
                ui.GetComponent<HealthUI>().deactivatemenu();
            }
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Open_Game");
        }
        if (Input.GetKeyDown("space")) {
        	Debug.Log("Pressed space");
			Jump();
        } 
        else if (canJump() || onWall) {
			Move();
		} else if ((!onWall && feetContact == 0) &&
                        (!ps.GetComponent<PaintSpawner>().is_orange || !doublejump)) {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 oldVelocity = playerRB.velocity;
                oldVelocity.x = 1 * movespeed;
                playerRB.velocity = oldVelocity;
                //Debug.Log("updated velocity " + playerRB.velocity);

                currDirection = Vector2.right;
            }
            //If player is pressing 'A'
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 oldVelocity = playerRB.velocity;
                oldVelocity.x = -1 * movespeed;
                playerRB.velocity = oldVelocity;
                Debug.Log("updated velocity " + playerRB.velocity);

                currDirection = Vector2.left;
            }
        }

        if (playerRB.velocity.y > 6.0f) {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 6.0f);
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        //if the cursor goes to the left of the paintspawner's reflection on the x side
        //else if is the opposite if facing left 
        //not combining the loops so its easier to read/debug
        if (facingRight && mousePos.x < sidePaintSpawner.transform.position.x - 2 * sidePaintSpawner.transform.localPosition.x)
        {
            //Debug.Log("Mouse: " + mousePos.x + " : " + "hinge and local pos" + hinge.transform.position.x + " " +  hinge.transform.localPosition.x + "calc: " + (hinge.transform.position.x - 2 * hinge.transform.localPosition.x));
            FlipPlayerX(sidePaintSpawner);
        } else if (!facingRight && mousePos.x > sidePaintSpawner.transform.position.x + 2 * sidePaintSpawner.transform.localPosition.x * -1 )
        {
            //Debug.Log("Mouse: " + mousePos.x + " : " + "hinge and local pos" + hinge.transform.position.x + " " +  hinge.transform.localPosition.x + "calc: " + (hinge.transform.position.x + 2 * hinge.transform.localPosition.x));

            FlipPlayerX(sidePaintSpawner);
        }

    }

    private void FlipPlayerX(GameObject sidePaintSpawner, bool shouldFlipX = true)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (shouldFlipX) {
        	sr.flipX = !sr.flipX;
    	}
        facingRight = !facingRight;
        //localPosition is a pass by copy struct
        Vector3 spawnerPos = sidePaintSpawner.transform.localPosition;
        sidePaintSpawner.transform.localPosition = new Vector3(spawnerPos.x * -1, spawnerPos.y, spawnerPos.z);
    }

    public void FlipPlayerY()
    {
    	// Rotate upside down
    	transform.Rotate(0f, 0f, 180f, Space.Self);
    	transform.position = transform.position + new Vector3(0,-0.07f,0);

    	// turn the player the right way (right or left)
    	FlipPlayerX(sidePaintSpawner, false);
    }
    #endregion

    # region movement_functions
    //Moves the player based on WASD inputs and 'movespeed'
    private void Move()
    {
    	anim.SetBool("Moving", true);
        //Debug.Log("Calling move " + playerRB.velocity);

        //If player is pressing 'D'
        if (x_input > 0)
        {
            Vector3 oldVel = playerRB.velocity;
            oldVel.x = 1 * movespeed;
            playerRB.velocity = oldVel;

            currDirection = Vector2.right;
            //Debug.Log("updated velocity REEE" + playerRB.velocity);

        }
        //If player is pressing 'A'
        else if (x_input < 0)
        {
            Vector3 oldVel = playerRB.velocity;
            oldVel.x = -1 * movespeed;
            playerRB.velocity = oldVel;
            currDirection = Vector2.left;
            //Debug.Log("updated velocity REEEE" + playerRB.velocity);

        }

        else if (onWall) {
	        //If player is pressing 'W'
	        if (y_input > 0)
	        {
	           playerRB.velocity = Vector2.up * movespeed;
	           currDirection = Vector2.up;
	        }
	        //If player is pressing 'S'
	        else if (y_input < 0)
	        {
	          playerRB.velocity = Vector2.down * movespeed;
	           currDirection = Vector2.down;
	        }
    	}
        else
        {
            playerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
    }

    public void GravityChange(bool direction) {
    	Debug.Log("Gravity: " + gravity + " Direction: " + direction);
    	if (gravity ^ direction) { // direction must be opposite to change the gravity
    		playerRB.gravityScale *= -1;
    		gravity = direction;    		
    	}

    	
    }

    bool canJump() {
    	return feetContact != 0 && !jumped;
    }

    private void Jump() {
    	if (canJump() || onWall) {
            feetContact = 0;
            Debug.Log("Jumping");
            //feetContact = false;
            jumped = true;
            doublejump = true;
        	StartCoroutine(JumpSeq());
    	} else if (doublejump)
        {
            doublejump = false;
            StartCoroutine(doublejumper());
        }

    	//if (onWall) {
    	//	feetContact = false;
     //   	StartCoroutine(JumpSeq());
    	//}
    }

    public IEnumerator WaitSeq(string an, float length) {
        anim.SetBool(an, true);
        yield return new WaitForSeconds(length);
        anim.SetBool(an, false);
    }

    IEnumerator doublejumper()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
        Debug.Log("Here's the new y" + playerRB.velocity.y);
        playerRB.AddForce(new Vector2(0f, jumpforce * playerRB.gravityScale));
        yield return null;
    }

    IEnumerator JumpSeq()
    {
        anim.SetBool("Jumping", true);
        //playerRB.velocity = new Vector2 (playerRB.velocity.x, 0);
        float velx = 0f;
        float yforce = jumpforce * playerRB.gravityScale;
        if (onWall)
        {
            yforce = jumpforce;

            if (leftCollided > 0)
            {
                velx = 25;
            } else if (rightCollided > 0)
            {
                velx = -25;
            }
        }
        Debug.Log("y force is: " + yforce);
        playerRB.AddForce ( new Vector2(velx, yforce));

        //yield return null;
        yield return new WaitForSeconds(1f);
        //yield return new WaitUntil(() => (playerRB.transform.position.y <= playerY));
        //Debug.Log("jumping is false");
        anim.SetBool("Jumping", false);
    }
    #endregion

    #region HealthFuncs
    public void Heal (float amount) {
    	m_health += amount;
    	Debug.Log("Healed! Health is: " + m_health);
    }

    public void Hurt(float amount) {
        m_health -= amount;
        ui.GetComponent<HealthUI>().UpdateHearts(- amount);

        if (m_health <= 0) {
            freeze = true;
            StartCoroutine(DeathSeq());
        } else {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            StartCoroutine(flashing(s.color, new Color(.82f, .08f, .08f, 1f), 0));
            Debug.Log("Hit! Heath is: " + m_health);
        }

        
    }

    IEnumerator DeathSeq() {
        Debug.Log("Dead");
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    IEnumerator flashing(Color org, Color changer, int i)
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        {
            if (i < 2)
            {
                while (s.color != changer)
                {
                    s.color = Color.Lerp(s.color, changer, .90f);
                    yield return null;
                }
                while (s.color != org)
                {
                    s.color = Color.Lerp(s.color, org, .90f);
                    yield return null;
                }
                StartCoroutine(flashing(org, changer, i + 1));
            }
        }
    }
    #endregion

    public void addColor(float amount, PaintColors color)
    {
        if (color == PaintColors.Yellow) {
            y_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Yellow", y_val);
        }
        if (color == PaintColors.Blue)
        {
            b_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Blue", b_val);

        }
        if (color == PaintColors.Red)
        {
            r_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Red", r_val);

        }

        if (color == PaintColors.Orange)
        {
            o_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Orange", o_val);
        }
        if (color == PaintColors.Purple)
        {
            p_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Purple", p_val);

        }
        if (color == PaintColors.Green)
        {
            g_val += amount;
            ui.GetComponent<HealthUI>().IncreasePaint("Green", g_val);

        }
    }

}

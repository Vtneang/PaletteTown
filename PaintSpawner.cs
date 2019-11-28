using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintSpawner : MonoBehaviour
{
    public Camera cam;
    public bool is_colored;
    public bool is_orange;
    public bool is_green;
    public bool is_purple;
    private float forceMultiplier = .1f;
    private float maxForceMagnitude = .5f;
    GameObject p;
    SpriteRenderer rendy;
    IEnumerator tracker;
    //private float minDistance = 1f; 
    HashSet<PlayerScript.PaintColors> shootableColors = new HashSet<PlayerScript.PaintColors>() 
        { PlayerScript.PaintColors.Red, PlayerScript.PaintColors.Blue, PlayerScript.PaintColors.Yellow };
    public GameObject sideSpawner;
    public GameObject topSpawner;
    public GameObject bottomSpawner;
    public GameObject tracer;

    private LinkedList<GameObject> tracers;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main;
        is_colored = is_orange = is_green = is_purple = false;
        p = GameObject.FindGameObjectWithTag("Player");
        rendy = p.GetComponent<SpriteRenderer>();
        tracers = new LinkedList<GameObject>();
    }
    private void ResetTracers()
    {
         foreach (GameObject t in tracers)
        {
            Destroy(t);
        }
        tracers = new LinkedList<GameObject>();
    }
    //void testPredictionLine()
    //{
    //    PlayerScript.PaintColors color = GetComponentInParent<PlayerScript>().currColor;

    //    Vector3 point = new Vector3();

    //    Debug.Log("mouse pos = " + Input.mousePosition.x + " " + Input.mousePosition.y);

    //    point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    //    Debug.Log("Point: " + point);
    //    PlayerScript player = GetComponentInParent<PlayerScript>();
       
    //    bool facingRight = player.facingRight;
    //    GameObject paintSpawnerToUse = null;
    //    if ((facingRight && point.x < sideSpawner.transform.position.x) || !facingRight && point.x > sideSpawner.transform.position.x)
    //    {
    //        float paintspawnerDist = Mathf.Abs(sideSpawner.transform.position.x - player.transform.position.x);
    //        if (Mathf.Abs(point.x - player.transform.position.x) < paintspawnerDist)
    //        {
    //            if (point.y > topSpawner.transform.position.y)
    //            {
    //                paintSpawnerToUse = topSpawner;
    //            }
    //            else if (point.y < bottomSpawner.transform.position.y && player.feetContact != 0)
    //            {
    //                paintSpawnerToUse = bottomSpawner;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        paintSpawnerToUse = sideSpawner;
    //    }

    //    if (shootableColors.Contains(color) && paintSpawnerToUse != null)
    //    {

    //        Vector3 force = Vector3.Normalize(point - paintSpawnerToUse.transform.position) * Mathf.Min(Vector3.Magnitude(point - paintSpawnerToUse.transform.position) * forceMultiplier, maxForceMagnitude);
    //        force.z = 0;

    //    }
    //}
    void PredictionLine(Vector3 force, Vector2 spawnerPos)
    {
        float throwForce = Vector3.Magnitude(force);
        //Debug.Log("throw force: " + throwForce);
        //Debug.Log("direction: " + direction);
        //Debug.Log("position: " + spawnerPos);
        float vi = throwForce;
        //calulate initial x and y velocity using power level and angle
        //float vx = direction.x * vi;
        float vx = force.x / .01f;
        //Debug.Log("X Vel: " + vx);
        //float vyi = direction.y * vi;
        float vyi = force.y / .01f;

        //Debug.Log("Y Vel: " + vy);

        //calculate next position after some time
        float freq = 1 / (throwForce * 200); //how frequently to sample
        int samples = 10; //num of samples

        float gravity = Physics2D.gravity.y * 3;
        float vy1 = vyi;
        float vy2 = vyi + gravity * freq;

        Vector2 currentLocation = spawnerPos; //initial previous location

        for (int i = 0; i < samples; i++)
        {
            if ((Physics2D.OverlapCircle(currentLocation, .3f, 1 << 11) != null))
            {
                vy1 = vy2;
                vy2 = vyi + gravity * freq * (i + 1);

                currentLocation = new Vector2(currentLocation.x + vx * freq, currentLocation.y + (vy2 + vy1) / 2 * freq);
                //Debug.Log((Physics2D.OverlapCircle(currentLocation, .2f, 1 << 11)).name);
                continue;
            }

            tracers.AddLast(Instantiate(tracer, currentLocation, Quaternion.identity));
            
            vy1 = vy2;
            vy2 = vyi + gravity * freq * (i + 1);

            currentLocation = new Vector2(currentLocation.x + vx * freq, currentLocation.y + (vy2 + vy1) / 2 * freq);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //too lazy to refactor this
        ResetTracers();
        Vector3 point = new Vector3();
        PlayerScript.PaintColors color = GetComponentInParent<PlayerScript>().currColor;

        //Debug.Log("mouseDown = " + Input.mousePosition.x + " " + Input.mousePosition.y);

        point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //Debug.Log("Point: " + point);
        PlayerScript player = GetComponentInParent<PlayerScript>();
        //Debug.Log("Self: " + transform.position);
        //Debug.Log("Magnitude: " + Vector3.Magnitude(point - transform.position));
        bool facingRight = player.facingRight;
        GameObject paintSpawnerToUse = null;
        if ((facingRight && point.x < sideSpawner.transform.position.x) || !facingRight && point.x > sideSpawner.transform.position.x)
        {
            float paintspawnerDist = Mathf.Abs(sideSpawner.transform.position.x - player.transform.position.x);
            if (Mathf.Abs(point.x - player.transform.position.x) < paintspawnerDist)
            {
                if (point.y > topSpawner.transform.position.y)
                {
                    paintSpawnerToUse = topSpawner;
                }
                else if (point.y < bottomSpawner.transform.position.y && player.feetContact == 0)
                {
                    Debug.Log("feet contact: " + player.feetContact);
                    paintSpawnerToUse = bottomSpawner;
                    //Debug.Log("USINGBOT SPAWNER!");
                }
            }
        }
        else
        {
            paintSpawnerToUse = sideSpawner;
        }

        if (shootableColors.Contains(color) && paintSpawnerToUse != null)
        {
            Vector3 force = Vector3.Normalize(point - paintSpawnerToUse.transform.position) * Mathf.Min(Vector3.Magnitude(point - paintSpawnerToUse.transform.position) * forceMultiplier, maxForceMagnitude);
            force.z = 0;

            PredictionLine(force, paintSpawnerToUse.transform.position);

            if (Input.GetMouseButtonDown(0))
            {
            
                string prefabName = PlayerScript.paintPrefabs[color];

                if (p.GetComponent<PlayerScript>().ui.GetComponent<HealthUI>().DecreasePaint(prefabName))
                {
                    StartCoroutine(GetComponentInParent<PlayerScript>().WaitSeq("Attack", 0.5f));
                    
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/" + prefabName), paintSpawnerToUse.transform.position, paintSpawnerToUse.transform.rotation);

                    //Debug.Log("Force: " + force);

                    obj.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

                }
            }
        }





            //left click
            //testPredictionLine();


        //right click
        if (Input.GetMouseButtonDown(1)) {
            StartCoroutine(SelfCastSeq());
        }

    }

    IEnumerator SelfCastSeq() {
        PlayerScript player = GetComponentInParent<PlayerScript>();
        PlayerScript.PaintColors color = GetComponentInParent<PlayerScript>().currColor;

        

        if (!is_colored)
        {
            StartCoroutine(GetComponentInParent<PlayerScript>().WaitSeq("SelfCast", 0.5f));

            yield return new WaitForSeconds(0.5f);

            bool go = true;
            string prefabName = PlayerScript.paintPrefabs[color];
            if (color == PlayerScript.PaintColors.Orange)
            {
                if (p.GetComponent<PlayerScript>().o_val <= 0) {
                    go = false;
                } else
                {
                    tracker = p.GetComponent<OrangePaint>().Change(50, rendy.color, rendy);
                }
            } else if (color == PlayerScript.PaintColors.Green)
            {
                if (p.GetComponent<PlayerScript>().g_val <= 0) {
                    go = false;
                } else
                {
                    tracker = p.GetComponent<GreenPaint>().Change(50, rendy.color, rendy);
                }
            } else if (color == PlayerScript.PaintColors.Purple)
            {
                if (p.GetComponent<PlayerScript>().p_val <= 0 ) {
                    go = false;
                } else
                {
                    tracker = p.GetComponent<PurplePaint>().Change(50, rendy.color, rendy);
                }
            }
            if (go)
            {
                StartCoroutine(tracker); // Needs to be fixed for other/ curr color.
                Debug.Log("OKKKK");
                p.GetComponent<PlayerScript>().ui.GetComponent<HealthUI>().DecreasePaint(prefabName);
            }
        }
        
    }
}
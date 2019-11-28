using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HosePaintSpawner : MonoBehaviour
{
    public GameObject paintStartPoint;
    public float hoseForce;
    public PlayerScript.PaintColors curColor;
    public float delay;
    public float timeSinceShot;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceShot = 0; 
        //curColor = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceShot > delay) { 
            Shoot(curColor);
            timeSinceShot = 0;
        }
        else
        {
            timeSinceShot += Time.deltaTime;
        }
    }
    void Shoot(PlayerScript.PaintColors color)
    {
        string prefabName = PlayerScript.paintPrefabs[color];

        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/" + prefabName), transform.position, transform.rotation);
        Vector3 force = Vector3.Normalize(transform.position - paintStartPoint.transform.position) * hoseForce; 
        obj.GetComponent<Rigidbody2D>().AddForce(force);
        //Debug.Log("shooting" + force.x + ", " + force.y + ", " + force.z);
    }
}

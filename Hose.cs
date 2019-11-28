using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
    public float turnSpeed;
    //public float anglesToTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 bottomLeft = GetComponent<SpriteRenderer>().bounds.
        //if (anglesToTurn < 0)
        //{
        //    anglesToTurn - turnSpeed * Time.deltaTime);

        //}

    }

    public void turnCW(float deltaTime)
    {
        Debug.Log("turning CW");
        transform.Rotate(0, 0, -turnSpeed * deltaTime);
    }

    public void turnCCW(float deltaTime)
    {
        Debug.Log("turning ccw");

        transform.Rotate(0, 0, turnSpeed * deltaTime);
    }
}

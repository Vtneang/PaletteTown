using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HoseTurner : MonoBehaviour
{
    public bool isActive;
    public Hose hose;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                hose.turnCCW(Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.R))
            {
                hose.turnCW(Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
            {
                isActive = true;

            }
        }
        catch (Exception e)
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
            {
                isActive = false;

            }
        }
        catch (Exception e)
        {

        }
    }
}

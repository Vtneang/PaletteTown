using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //1 if true 
    public int facingRight = -1;
    public GameObject blackPaint;
    public bool shot;
    public float bulletSpd = .5f;
    public int hp; 
    // Start is called before the first frame update
    void Start()
    {
        shot = true;
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shot);

        //if (shot)
        //{
        //    Shoot();
        //}
        //shot = false;
        //Debug.Log(shot);

    }
    void FlipEnemyX()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = !sr.flipX;
        facingRight = facingRight * - 1;
    }

   public void Shoot(bool right)
    {
        if (right && facingRight == -1 || !right && facingRight == 1) {
            FlipEnemyX();
        }
        Debug.Log("Shooting");
        Vector3 spawnPos = transform.position + Vector3.right * facingRight;

        GameObject obj = (GameObject)Instantiate(blackPaint, spawnPos, transform.rotation);

        obj.GetComponent<Rigidbody2D>().velocity = new Vector3(facingRight * bulletSpd, 0, 0); 
    }

    public void Damage(int damage)
    {
        hp = hp - damage;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
            {
                collision.transform.GetComponentInParent<PlayerScript>().Hurt(9999);

            }
        }
        catch (Exception e)
        {

        }
    }

}

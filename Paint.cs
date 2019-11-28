using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Paint : MonoBehaviour
{
    public PlayerScript.PaintColors color;
    private Vector2 oldVelocity;
    public Dictionary<PlayerScript.PaintColors, Color> paintPrefabs = 
        new Dictionary<PlayerScript.PaintColors, Color>()
            {
                { PlayerScript.PaintColors.Blue, new Color(0, 0.63f, 1) },
                { PlayerScript.PaintColors.Red, new Color(1, 0.45f, 0.45f) },
                { PlayerScript.PaintColors.Yellow, new Color(0.96f, 1, 0.45f) },
            };
    //public Dictionary<PlayerScript.PaintColors, MonoBehaviour> paintScripts =
        //new Dictionary<PlayerScript.PaintColors, MonoBehaviour>()
            //{
            //    { PlayerScript.PaintColors.Blue, BluePaint },
            //    { PlayerScript.PaintColors.Red, RedPaint },
            //    { PlayerScript.PaintColors.Yellow, YellowPaint },
            //};

    // Start is called before the first frame update
    void Start()
    {
        oldVelocity = transform.GetComponent<Rigidbody2D>().velocity;
    }

    // Update is called once per frame
    void Update()
    {
        oldVelocity = transform.GetComponent<Rigidbody2D>().velocity;
        Debug.Log("trans velocity: " + transform.GetComponent<Rigidbody2D>().velocity);
        Debug.Log("velocity: " + GetComponent<Rigidbody2D>().velocity);


        //Debug.Log("Velocity: " + transform.GetComponent<Rigidbody2D>().velocity);

    }
    //reason why this function is so weird:
    //Contact points and physics raycast would sometimes give points outside the tile..
    //not sure why maybe a bug with tilemap
    //so i implemented it myself 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy paint if it hits player
        try
        {
            if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
            {
                Destroy(this.gameObject);
                return;

            }
        }
        catch (Exception e)
        {

        }
        //GameObject tilemapGameObject = GameObject.FindGameObjectWithTag("Floor");

        //Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();

        //Vector3 pos = Vector3.zero;
        //pos.x = 4.2f;
        //pos.y = .7f;
        //pos.z = 0f;
        //Debug.Log("Position: " + tilemap.WorldToCell(pos));
        //return;
        Vector3 contactPos = Vector3.zero;
        Vector3 hitPosition = Vector3.zero;
        Debug.Log("Paint New Collision");
        GameObject tilemapGameObject = GameObject.FindGameObjectWithTag("Floor");
        Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();
        bool found = false;
        Debug.Log("collided");
        if (tilemap != null && tilemapGameObject == collision.gameObject)
        {
            //first, given contact point, try extending it by velocity and see if it hits something 
            //use oldVelocity here b/c velocity is changed by this point
            //look for a hitposition that has a tile, idk why theres multiple tbh
            for (int i = 0; i < collision.contactCount; i++)
            {
                Debug.Log("Contact Point" + collision.GetContact(0).point.x + " , " + collision.GetContact(0).point.y);

                contactPos.x = collision.GetContact(i).point.x;
                contactPos.y = collision.GetContact(i).point.y;
                hitPosition.x = collision.GetContact(0).point.x;
                hitPosition.y = collision.GetContact(0).point.y;
                if (tilemap.HasTile(tilemap.WorldToCell(hitPosition)))
                {
                    found = true;
                    break;
                }

                //RaycastHit2D hits = Physics2D.Raycast(contactPos, oldVelocity);
                //hitPosition.x = hits.point.x + .05f * oldVelocity.normalized.x;
                //hitPosition.y = hits.point.y + .05f * oldVelocity.normalized.y;

                //hitPosition.z = 0;
                    
                //Debug.Log("Hit Position: " + hitPosition.x + " , " + hitPosition.y);

                //Debug.Log("Position: " + tilemap.WorldToCell(hitPosition));
                //Debug.Log("Has tile: " + tilemap.HasTile(tilemap.WorldToCell(hitPosition)));

                //move it 0 to .1f units in velocity forwards, see if it collides
                for (float f = 0;  f <= .1f; f += .005f)
                {
                    hitPosition.x = contactPos.x + f * oldVelocity.normalized.x;
                    hitPosition.y = contactPos.y + f * oldVelocity.normalized.y;
                    hitPosition.z = 0;
                    if (tilemap.HasTile(tilemap.WorldToCell(hitPosition)))
                    {
                        found = true;
                        break;
                    }
                }




            //Vector3Int tilePos = tilemap.WorldToCell(hitPosition);
            //tilePos.x += oldVelocity.x >= 0 ? 1 : 0;
            //tilePos.y += oldVelocity.y >= 0 ? 1 : -1;

            }
            if (!found)
            {
                //if not found, just draw circles until it hits something
                for (float dist = .01f; dist < .1f; dist += .01f)
                {
                    for (float radians = 0f; radians < Mathf.PI; radians += .05f)
                    {
                        Vector3 circleRay = Vector3.zero;
                        circleRay.x = dist * Mathf.Cos(radians);
                        circleRay.y = dist * Mathf.Sin(radians);
                        circleRay.z = 0;
                        hitPosition = contactPos + circleRay;

                    }
                    if (tilemap.HasTile(tilemap.WorldToCell(hitPosition)))
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                Debug.Log("No position with tile found...");
                Destroy(this.gameObject);

                return;
            }

            tilemap.SetTileFlags(tilemap.WorldToCell(hitPosition), TileFlags.None);
            tilemap.SetColor(tilemap.WorldToCell(hitPosition), paintPrefabs[color]);
            //Tile t = (Tile) tilemap.GetTile(tilemap.WorldToCell(hitPosition));
            //GameObject g = tilemap.GetInstantiatedObject(tilemap.WorldToCell(hitPosition));
            Vector3Int cellPosition = tilemap.layoutGrid.WorldToCell(hitPosition);
            //Debug.Log("cell position is: " + cellPosition);
            Vector3 pos = tilemap.layoutGrid.CellToWorld(cellPosition);
            //pos.z = 1;
            pos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(hitPosition));
            pos.z = 0;
            //not efficient, but too lazy to change
            GameObject.Find("SpawnedPaintManager").GetComponent<SpawnedPaintManager>().SpawnPaint(pos, color);
            //pos.x -= .05f;
            //pos.y -= .05f; 
            //Debug.Log("Collission point is: " + hitPosition);
            //Debug.Log("tile pos is: " + pos);


            //tilemap.SetTile(tilemap.WorldToCell(hitPosition), t);
            tilemap.RefreshAllTiles();
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerHead")
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy" && color == PlayerScript.PaintColors.Red)
        {
            collision.gameObject.GetComponent<EnemyScript>().Damage(9999);
        }
        if (collision.gameObject.CompareTag("DestroyableBlock") && color == PlayerScript.PaintColors.Red)
        {
            Destroy(collision.gameObject);
        }
        Destroy(this.gameObject);



        //Debug.Log(collision.gameObject);
        //Tile t = collision.gameObject.GetComponent<Tile>();
        //Debug.Log(t.transform);
        //Debug.Log(collision.contacts[0].point);
        //t.color = Color.red;
    }
}

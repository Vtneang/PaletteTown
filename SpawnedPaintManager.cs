using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPaintManager : MonoBehaviour
{
    public GameObject emptyPrefab;
    public GameObject bluePrefab;

    public Dictionary<Vector3, GameObject> spawnedPaints = new Dictionary<Vector3, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //obj in this case is always the empty prefab
    public void SpawnPaint(Vector3 pos, PlayerScript.PaintColors color)
    {
        Debug.Log("spawning paint at position: " + pos);
        if (spawnedPaints.ContainsKey(pos))
        {
            Destroy(spawnedPaints[pos]);
            spawnedPaints.Remove(pos);
        }
        

        if (color == PlayerScript.PaintColors.Blue)
        {
            Debug.Log("spwaning blue");
            GameObject g = Instantiate(bluePrefab, pos, Quaternion.identity);
            spawnedPaints.Add(pos, g);
            //g.AddComponent<BluePaint>();
        } else {
            GameObject g = Instantiate(emptyPrefab, pos, Quaternion.identity);
            spawnedPaints.Add(pos, g);

            if (color == PlayerScript.PaintColors.Yellow)
            {
                g.tag = "Yellow";
                g.AddComponent<YellowPaint>();
            }
            if (color == PlayerScript.PaintColors.Red)
            {
                Debug.Log("here");
                g.AddComponent<RedPaint>();
            }
        }
    }
}

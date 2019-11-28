using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour
{

    public Transform pos1, pos2; // pos1 is leftmost, pos2 is rightmost position of the block
    public float speed;
    public Transform startPos;

    Vector3 nextPos;

    void Start() {
        nextPos = startPos.position;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    /*
        The switch is flipped left. This means the block will move towards pos1, the leftmost pos.
    */
    public void Left() {
        nextPos = pos1.position;
    }

    /*
        The switch is flipped right. This means the block will move towards pos2, the rightmost pos.
    */
    public void Right() {
        nextPos = pos2.position;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}

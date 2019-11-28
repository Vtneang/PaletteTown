using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlockSwitch : MonoBehaviour
{
    public Sprite leftSwitch;
    public Sprite rightSwitch;
    public bool curDirection; // 0 is right, 1 is left.

    void Start() {
        if (curDirection) {
            GetComponent<SpriteRenderer>().sprite = rightSwitch;
        } else {
            GetComponent<SpriteRenderer>().sprite = leftSwitch;
        }
    }

    void Switch() {
        if (curDirection) {
            GetComponent<SpriteRenderer>().sprite = rightSwitch;
            GetComponentInParent<MoveableBlock>().Right();
        } else {
            GetComponent<SpriteRenderer>().sprite = leftSwitch;
            GetComponentInParent<MoveableBlock>().Left();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Paint"))
        {
            curDirection = !curDirection;
            Switch();
            Destroy(collision.gameObject);
        }

    }

}

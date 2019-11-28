using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurplePaint : MonoBehaviour
{
    // Start is called before the first frame update

    #region Public Variables
    public Color purp;
    public float wait_time;
    public GameObject person;
    #endregion

    #region Private Variables
    private Color tracker;
    private SpriteRenderer render;
    #endregion

    #region Functions
    public IEnumerator Change(float speed, Color keep, SpriteRenderer rendy)
    {
        tracker = keep;
        render = rendy;
        Color curColor = tracker;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_purple = true;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = true;
        while (curColor != purp)
        {
            person.tag = "Fake Player";
            curColor = Color.Lerp(curColor, purp, speed / 100);
            render.color = curColor;
            yield return null;
        }
        StartCoroutine(GetComponent<PlayerScript>().ui.GetComponent<HealthUI>().SelfCastingTime(wait_time));
        yield return new WaitForSeconds(wait_time);
        StartCoroutine(Relive(Color.white, 50));
    }

    IEnumerator Relive(Color c, float speed)
    {
        Color curColor = render.color;
        while (curColor != c)
        {
            curColor = Color.Lerp(curColor, c, speed / 100);
            render.color = curColor;
            yield return null;
        }
        person.tag = "Player";
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_purple = false;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = false;
    }
    #endregion
}

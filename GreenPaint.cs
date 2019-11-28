using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPaint : MonoBehaviour
{
    #region Public Variables
    public Color gree;
    public float wait_time;
    public float heal_amount;
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
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_green = true;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = true;
        while (curColor != gree)
        {
            curColor = Color.Lerp(curColor, gree, speed / 100);
            render.color = curColor;
            yield return null;
        }
        StartCoroutine(Healing(heal_amount));
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
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_green = false;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = false;
    }

    IEnumerator Healing(float amount)
    {
        Debug.Log("Start Healing"); //need another varaible for current and max!
        if (GetComponent<PlayerScript>().m_health >= 3) //max health for now
        {
            if (GetComponent<PlayerScript>().m_health > 3)
            {
                GetComponent<PlayerScript>().m_health = 3;
            }
            yield break;
        }
        float pace = amount / wait_time;
        while (GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_green && amount > 0)
        {
            float diff = 3 - GetComponent<PlayerScript>().m_health;
            if (pace > diff)
            {
                GetComponent<PlayerScript>().ui.GetComponent<HealthUI>().UpdateHearts(diff);
                GetComponent<PlayerScript>().m_health += diff;
            }
            else
            {
                GetComponent<PlayerScript>().ui.GetComponent<HealthUI>().UpdateHearts(pace);
                GetComponent<PlayerScript>().m_health += pace;
            }
            amount -= pace;
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePaint : MonoBehaviour
{

    #region Public Variables
    public float wait_time = 3;
    public float add_jump = 500;
    public Color Orange;
    #endregion

    #region Private Variables
    private Color tracker;
    private SpriteRenderer render;
    private float old_jump;
    #endregion

    #region Functions
    public IEnumerator Change(float speed, Color keep, SpriteRenderer rendy)
    {
        tracker = keep;
        render = rendy;
        Color curColor = tracker;
        old_jump = GetComponent<PlayerScript>().jumpforce;
        GetComponent<PlayerScript>().jumpforce = old_jump + add_jump;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_orange = true;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = true;
        while (curColor != Orange)
		{
			curColor = Color.Lerp(curColor, Orange, speed / 100);
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
        GetComponent<PlayerScript>().jumpforce = old_jump;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_orange = false;
        GetComponent<PlayerScript>().ps.GetComponent<PaintSpawner>().is_colored = false;
    }
    #endregion

}

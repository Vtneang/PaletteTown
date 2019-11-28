using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The list of text")]
    private RectTransform[] list_text;

    [SerializeField]
    [Tooltip("Displayed Text")]
    public RectTransform dis;

    [SerializeField]
    [Tooltip("Duration of each text")]
    private float sec;
    #endregion

    IEnumerator tellplayer()
    {
        for (int i = 0; i < list_text.Length; i ++)
        {
            dis.GetComponent<Text>().text = list_text[i].GetComponent<Text>().text;
            yield return new WaitForSeconds(sec);
        }
        dis.GetComponent<Text>().text = "";
    }

    private void Awake()
    {
        StartCoroutine(tellplayer());
    }

}

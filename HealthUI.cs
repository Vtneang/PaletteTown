using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Player object")]
    private PlayerScript person;

    [SerializeField]
    [Tooltip("The list of hearts")]
    private Image [] list_h;

    [SerializeField]
    [Tooltip("The Red paint UI")]
    private RectTransform red;

    [SerializeField]
    [Tooltip("The Blue paint UI")]
    private RectTransform blue;

    [SerializeField]
    [Tooltip("The Yellow paint UI")]
    private RectTransform yellow;

    [SerializeField]
    [Tooltip("The Purple paint UI")]
    private RectTransform purple;

    [SerializeField]
    [Tooltip("The Green paint UI")]
    private RectTransform green;

    [SerializeField]
    [Tooltip("The Orange paint UI")]
    private RectTransform orange;

    [SerializeField]
    [Tooltip("The Red paint Text")]
    private RectTransform t_red;

    [SerializeField]
    [Tooltip("The Blue paint Text")]
    private RectTransform t_blue;

    [SerializeField]
    [Tooltip("The Yellow paint Text")]
    private RectTransform t_yellow;

    [SerializeField]
    [Tooltip("The Purple paint Text")]
    private RectTransform t_purple;

    [SerializeField]
    [Tooltip("The Orange paint Text")]
    private RectTransform t_orange;

    [SerializeField]
    [Tooltip("The Green paint Text")]
    private RectTransform t_green;

    [SerializeField]
    [Tooltip("Bar showing cur color")]
    private RectTransform curr;

    [SerializeField]
    [Tooltip("The timer of the selfcasting colors")]
    private Text timer;

    [SerializeField]
    [Tooltip("The win text")]
    private Text win_text;

    [SerializeField]
    [Tooltip("The amount to multiple the bars by")]
    private float f;

    [SerializeField]
    [Tooltip("Menu image")]
    private Image menu;

    [SerializeField]
    [Tooltip("Menu text")]
    private Text t_menu;
    #endregion

    #region Private Variables
    private float remaining;
    private float p_RedWidth;
    private float p_BlueWidth;
    private float p_YelloWidth;
    private float p_PurpleWidth;
    private float p_GreenWidth;
    private float p_OrangeWidth;
    //private float c_red;
    //private float c_blue;
    //private float c_yellow;
    private Color oran;
    private int TextWidth = 150;
    #endregion

    private void Awake()
    {
        person = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        timer.enabled = false;
        remaining = person.GetComponent<PlayerScript>().m_health;
        p_RedWidth = TextWidth;
        p_BlueWidth = TextWidth;
        p_YelloWidth = TextWidth;
        p_OrangeWidth = TextWidth;
        p_GreenWidth = TextWidth;
        p_PurpleWidth = TextWidth;
        UpdateHearts(0);
        string color = PlayerScript.paintPrefabs
            [person.GetComponent<PlayerScript>().currColor];
        ChangeBar(color);
        float c_red = person.GetComponent<PlayerScript>().r_val;
        float c_blue = person.GetComponent<PlayerScript>().b_val;
        float c_yellow = person.GetComponent<PlayerScript>().y_val;
        t_red.GetComponent<Text>().text = c_red.ToString();
        t_blue.GetComponent<Text>().text = c_blue.ToString();
        t_yellow.GetComponent<Text>().text = c_yellow.ToString();

        t_purple.GetComponent<Text>().text = person.GetComponent<PlayerScript>().p_val.ToString();
        t_green.GetComponent<Text>().text = person.GetComponent<PlayerScript>().g_val.ToString();
        t_orange.GetComponent<Text>().text = person.GetComponent<PlayerScript>().o_val.ToString();
        oran = person.GetComponent<OrangePaint>().Orange;
    }
    // Update is called once per frame

    public void UpdateHearts(float amount)
    {
        remaining += amount;
        Debug.Log(remaining);
        if (remaining <= 0)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
            StartCoroutine(Restart());
        }
        else if (remaining <= .5)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
            list_h[3].enabled = true;
        }
        if (remaining > 0.5 && remaining <= 1)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
            list_h[0].enabled = true;
        }
        if (remaining <= 1.5 && remaining > 1)
        {
            foreach (Image h in list_h)
            {
                h.enabled = true;
            }
            list_h[1].enabled = false;
            list_h[2].enabled = false;
            list_h[5].enabled = false;
        }
        if (remaining > 1.5 && remaining <= 2)
        {
            foreach (Image h in list_h)
            {
                h.enabled = true;
            }
            list_h[2].enabled = false;
            list_h[5].enabled = false;
        }
        if (remaining <= 2.5 && remaining > 2)
        {
            foreach (Image h in list_h)
            {
                h.enabled = true;
            }
            list_h[2].enabled = false;
        }
        if (remaining > 2.5)
        {
            foreach (Image h in list_h)
            {
                h.enabled = true;
            }
        }
    }

    public void SetHearts(float amount)
    {
        if (amount <= 0)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
        }
        else if (amount <= .5)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
            list_h[3].enabled = true;
        }
        if (amount > 0.5 && amount <= 1)
        {
            foreach (Image h in list_h)
            {
                h.enabled = false;
            }
            list_h[0].enabled = true;
        }
        if (amount <= 1.5 && amount > 1)
        {
            list_h[4].enabled = true;
            list_h[1].enabled = false;
            list_h[2].enabled = false;
            list_h[5].enabled = false;
        }
        if (amount > 1.5 && amount <= 2)
        {
            list_h[1].enabled = true;
            list_h[2].enabled = false;
            list_h[5].enabled = false;
        }
        if (amount <= 2.5 && amount > 2)
        {
            list_h[5].enabled = true;
            list_h[2].enabled = false;
        }
        if (amount > 2.5)
        {
            list_h[2].enabled = true;
        }
    }

    public void ChangeBar(string s)
    {
        if (s == "Red")
        {
            curr.GetComponent<Image>().color = Color.red;
            red.sizeDelta = new Vector2(p_RedWidth * f, red.sizeDelta.y);
            t_red.sizeDelta = new Vector2(TextWidth * f, t_red.sizeDelta.y);
            Color r = red.GetComponent<Image>().color;
            red.GetComponent<Image>().color = new Color(r.r, r.g, r.b, 1f);
        } else if (s == "Blue")
        {
            curr.GetComponent<Image>().color = Color.blue;
            blue.sizeDelta = new Vector2(p_BlueWidth * f, blue.sizeDelta.y);
            t_blue.sizeDelta = new Vector2(TextWidth * f, t_blue.sizeDelta.y);
            Color b = blue.GetComponent<Image>().color;
            blue.GetComponent<Image>().color = new Color(b.r, b.g, b.b, 1f);
        } else if (s == "Yellow")
        {
            curr.GetComponent<Image>().color = Color.yellow;
            yellow.sizeDelta = new Vector2(p_YelloWidth * f, yellow.sizeDelta.y);
            t_yellow.sizeDelta = new Vector2(TextWidth * f, t_yellow.sizeDelta.y);
            Color y = yellow.GetComponent<Image>().color;
            yellow.GetComponent<Image>().color = new Color(y.r, y.g, y.b, 1f);
        } else if (s == "Purple")
        {
            curr.GetComponent<Image>().color = Color.magenta;
            purple.sizeDelta = new Vector2(p_PurpleWidth * f, purple.sizeDelta.y);
            t_purple.sizeDelta = new Vector2(TextWidth * f, t_purple.sizeDelta.y);
            Color p = purple.GetComponent<Image>().color;
            purple.GetComponent<Image>().color = new Color(p.r, p.g, p.b, 1f);
        } else if (s == "Orange")
        {
            curr.GetComponent<Image>().color = oran;
            orange.sizeDelta = new Vector2(p_OrangeWidth * f, orange.sizeDelta.y);
            t_orange.sizeDelta = new Vector2(TextWidth * f, t_orange.sizeDelta.y);
            Color o = orange.GetComponent<Image>().color;
            orange.GetComponent<Image>().color = new Color(o.r, o.g, o.b, 1f);
        } else if (s == "Green")
        {
            curr.GetComponent<Image>().color = Color.green;
            green.sizeDelta = new Vector2(p_GreenWidth * f, green.sizeDelta.y);
            t_green.sizeDelta = new Vector2(TextWidth * f, t_green.sizeDelta.y);
            Color g = green.GetComponent<Image>().color;
            green.GetComponent<Image>().color = new Color(g.r, g.g, g.b, 1f);
        } else
        {
            curr.GetComponent<Image>().color = Color.black;
            red.sizeDelta = new Vector2(p_RedWidth * f, red.sizeDelta.y);
            t_red.sizeDelta = new Vector2(TextWidth * f, t_red.sizeDelta.y);
            yellow.sizeDelta = new Vector2(p_YelloWidth * f, yellow.sizeDelta.y);
            t_yellow.sizeDelta = new Vector2(TextWidth * f, t_yellow.sizeDelta.y);
            blue.sizeDelta = new Vector2(p_BlueWidth * f, blue.sizeDelta.y);
            t_blue.sizeDelta = new Vector2(TextWidth * f, t_blue.sizeDelta.y);
            green.sizeDelta = new Vector2(p_GreenWidth * f, green.sizeDelta.y);
            t_green.sizeDelta = new Vector2(TextWidth * f, t_green.sizeDelta.y);
            orange.sizeDelta = new Vector2(p_OrangeWidth * f, orange.sizeDelta.y);
            t_orange.sizeDelta = new Vector2(TextWidth * f, t_orange.sizeDelta.y);
            purple.sizeDelta = new Vector2(p_PurpleWidth * f, purple.sizeDelta.y);
            t_purple.sizeDelta = new Vector2(TextWidth * f, t_purple.sizeDelta.y);
        }
    }

    public void ResetBar()
    {
        red.sizeDelta = new Vector2(TextWidth, red.sizeDelta.y);
        t_red.sizeDelta = new Vector2(TextWidth, t_red.sizeDelta.y);
        yellow.sizeDelta = new Vector2(TextWidth, yellow.sizeDelta.y);
        t_yellow.sizeDelta = new Vector2(TextWidth, t_yellow.sizeDelta.y);
        blue.sizeDelta = new Vector2(TextWidth, blue.sizeDelta.y);
        t_blue.sizeDelta = new Vector2(TextWidth, t_blue.sizeDelta.y);
        orange.sizeDelta = new Vector2(TextWidth, orange.sizeDelta.y);
        t_orange.sizeDelta = new Vector2(TextWidth, t_green.sizeDelta.y);
        green.sizeDelta = new Vector2(TextWidth, green.sizeDelta.y);
        t_green.sizeDelta = new Vector2(TextWidth, t_green.sizeDelta.y);
        purple.sizeDelta = new Vector2(TextWidth, purple.sizeDelta.y);
        t_purple.sizeDelta = new Vector2(TextWidth, t_purple.sizeDelta.y);
        LowerAlpa();
    }

    public void LowerAlpa()
    {
        Color r = red.GetComponent<Image>().color;
        red.GetComponent<Image>().color = new Color(r.r, r.g, r.b, 0.2f);
        Color b = blue.GetComponent<Image>().color;
        blue.GetComponent<Image>().color = new Color(b.r, b.g, b.b, 0.2f);
        Color y = yellow.GetComponent<Image>().color;
        yellow.GetComponent<Image>().color = new Color(y.r, y.g, y.b, 0.2f);
        Color g = green.GetComponent<Image>().color;
        green.GetComponent<Image>().color = new Color(g.r, g.g, g.b, 0.2f);
        Color o = orange.GetComponent<Image>().color;
        orange.GetComponent<Image>().color = new Color(o.r, o.g, o.b, 0.2f);
        Color p = purple.GetComponent<Image>().color;
        purple.GetComponent<Image>().color = new Color(p.r, p.g, p.b, 0.2f);
    }

    //this is dumb but call this instead of the player paint for now
    public bool DecreasePaint(string s)
    {
        if (s == "Red")
        {
            if ( person.r_val <= 0)
            {
                return false;
            }
            person.r_val -= 1;
            t_red.GetComponent<Text>().text = person.r_val.ToString();
        } else if (s == "Blue")
        {
            if (person.b_val <= 0)
            {
                return false;
            }
            person.b_val -= 1;
            t_blue.GetComponent<Text>().text = person.b_val.ToString();
        } else if (s == "Yellow")
        {
            if (person.y_val <= 0)
            {
                return false;
            }
            person.y_val -= 1;
            t_yellow.GetComponent<Text>().text = person.y_val.ToString();
        } else if (s == "Orange")
        {
            if (person.o_val <= 0)
            {
                return false;
            }
            person.o_val -= 1;
            t_orange.GetComponent<Text>().text = person.o_val.ToString();
        } else if (s == "Green")
        {
            if (person.g_val <=0)
            {
                return false;
            }
            person.g_val -= 1;
            t_green.GetComponent<Text>().text = person.g_val.ToString();
        } else if (s == "Purple")
        {
            if (person.p_val<= 0)
            {
                return false;
            }
            person.p_val -= 1;
            t_purple.GetComponent<Text>().text = person.p_val.ToString();
        } else
        {
            Debug.Log("invalid paint.. theres an error ");
            return false;
        }
        //t_orange.GetComponent<Text>().text = Mathf.Min(c_yellow, c_red).ToString();
        //t_green.GetComponent<Text>().text = Mathf.Min(c_yellow, c_blue).ToString();
        //t_purple.GetComponent<Text>().text = Mathf.Min(c_blue, c_red).ToString();
        //person.GetComponent<PlayerScript>().r_val = c_red;
        //person.GetComponent<PlayerScript>().b_val = c_blue;
        //person.GetComponent<PlayerScript>().y_val = c_yellow;
        return true;
    }
    //prereq: modified in player already
    public void IncreasePaint(string s, float value)
    {
        if (s == "Red")
        {
            t_red.GetComponent<Text>().text = value.ToString();
        }
        else if (s == "Blue")
        {

            t_blue.GetComponent<Text>().text = value.ToString();
        }
        else if (s == "Yellow")
        {
            t_yellow.GetComponent<Text>().text = value.ToString();

        }
        else if (s == "Orange")
        {
            t_orange.GetComponent<Text>().text = value.ToString();

        }
        else if (s == "Purple")
        {
            t_purple.GetComponent<Text>().text = value.ToString();

        }
        else if (s == "Green")
        {
            t_green.GetComponent<Text>().text = value.ToString();

        }

        else
        {
            Debug.Log("Invalid color: " + s);
        }
        
    }

        public IEnumerator SelfCastingTime(float n)
    {
        timer.text = n.ToString();
        timer.enabled = true;
        while (n >= 0)
        {
            yield return new WaitForSeconds(1);
            n -= 1;
            timer.text = n.ToString();
        }
        timer.enabled = false;
    }

    public void win()
    {
        win_text.gameObject.SetActive(true);
    }

    public void activatemenu()
    {
        menu.enabled = true;
        t_menu.enabled = true;
    }

    public void deactivatemenu()
    {
        menu.enabled = false;
        t_menu.enabled = false;
    }

    public IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

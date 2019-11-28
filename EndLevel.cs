using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Learning Text")]
    private GameObject t_ui;
    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yay u win");
        if (collision.gameObject.CompareTag("Player") || collision.transform.parent.CompareTag("Player"))
        {
            if (t_ui != null)
            {
                t_ui.GetComponent<TutorialUI>().dis.GetComponent<Text>().enabled = false;
            }
            GameObject.Find("UI").GetComponent<HealthUI>().win();
            StartCoroutine(SwitchScene());

        }

    }

    public IEnumerator Restart()
    {
        Debug.Log("restarting");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SwitchScene()
    {
        if (nextScene != null && nextScene != "")
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(nextScene);
        }
    }
}

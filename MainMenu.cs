using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Play Buttons Methods
    public void PlayArena()
    {
        SceneManager.LoadScene("Tutorial");
    }
    #endregion

    #region General Application Button Methods
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}

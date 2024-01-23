using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelect : MonoBehaviour
{
    public Button GM;
    public Button SS;

    void Start()
    {
        GM.onClick.AddListener(GMCall);
        SS.onClick.AddListener(SSCall);
    }
    void GMCall()
    {
        SceneManager.LoadScene("GrotonMaze");
    }
    void SSCall()
    {
        SceneManager.LoadScene("SymbolSubstitution");
    }
}

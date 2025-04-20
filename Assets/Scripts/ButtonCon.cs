using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ButtonCon : MonoBehaviour
{
   

    public TextMeshProUGUI bestScore;
    public int maxscore = 0;
    public TextMeshProUGUI playername;
    private void Awake()
    {
      
    }
    private void Start()
    {
        bestScore.text = maxscore.ToString();  
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
Application.Quit();
#endif
    }

}



﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play from MainMenu.cs");
        SceneManager.LoadScene("LevelOneScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

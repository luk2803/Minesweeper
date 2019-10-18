using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionScript : MonoBehaviour
{
    private GameController controllerScript;
    private GameObject gameController;

    public void Update()
    {
        bool isEscKeyDown = Input.GetKeyUp(KeyCode.Escape);
        if(isEscKeyDown)
            Continue();
    }

    public void Awake()
    {
        gameController = GameObject.Find("GameController");
        controllerScript = gameController.GetComponent<GameController>();
    }
    
    public void ResetBoard()
    {
        SceneManager.LoadScene(GameController.GameSceneBuildIndex);
        controllerScript.ResetGame();
    }

    public void BackToMenu()
    {
        Destroy(gameController);
        SceneManager.LoadScene(GameController.MenüSceneBuildIndex);
    }

    public void Continue()
    {
        SceneManager.LoadScene(GameController.GameSceneBuildIndex);
    }
}

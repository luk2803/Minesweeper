using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvasScript : MonoBehaviour
{
    //TODO 
    // Back und reset button in eigenes Menü legen.
    
    private GameController controllerScript;
    private Button toggleFlagButton; 
    private Color white = new Color(1,1,1);
    private Color grey = new Color(0.5283019f, 0.5283019f, 0.5283019f);
    private GameObject gameController;
    void Start()
    {
        gameController = GameObject.Find("GameController");
        controllerScript = gameController.GetComponent<GameController>();
        toggleFlagButton = GameObject.Find("ToggleFlagButton").GetComponent<Button>();
    }

    public void ToggleFlag()
    {
        controllerScript.isFlagMode = !controllerScript.isFlagMode;
        Color color = controllerScript.isFlagMode ? grey : white;
        SetButtonColor(color);
    }

    private void SetButtonColor(Color color)
    {
        var colors = toggleFlagButton.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        toggleFlagButton.colors = colors;
    }

    public void ResetBoard()
    {
        SetButtonColor(white);
        controllerScript.ResetGame();
        controllerScript.ResetMines();
    }

    public void BackToMenu()
    {
        Destroy(gameController);
        SceneManager.LoadScene(0);
    }
    
}

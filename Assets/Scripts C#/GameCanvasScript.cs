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
    public static Color white = new Color(1,1,1);
    public static Color grey = new Color(0.5283019f, 0.5283019f, 0.5283019f);
    private GameObject gameController;
    public bool ButtonActivatedByToggle { get; private set; } = false;

    void Start()
    {
        gameController = GameObject.Find("GameController");
        controllerScript = gameController.GetComponent<GameController>();
        toggleFlagButton = GameObject.Find("ToggleFlagButton").GetComponent<Button>();
        controllerScript.LoadGameCanvas();
    }

    public void ToggleFlag()
    {
       
        controllerScript.SetIsFlagMode(!controllerScript.GetIsFlagMode());
        
        if(controllerScript.GetIsFlagMode())
            ButtonActivatedByToggle = true;
        else 
            ButtonActivatedByToggle = false;
        
        Color color = controllerScript.GetIsFlagMode() ? grey : white;
        SetButtonColor(color);
    }

    public void SetButtonColor(Color color)
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
    }

    public void BackToMenu()
    {
        Destroy(gameController);
        SceneManager.LoadScene(0);
    }
    
}

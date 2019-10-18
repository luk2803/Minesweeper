using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = System.Object;

public class MenüScript : MonoBehaviour
{
    public GameObject gameController;
    private GameController controllerScript;
    private List<InputField> spielFeldData = new List<InputField>();
    private int newSceneIndex;
    private Scene spielfeld;
    private ErrorMessage errorMessage;
    void Awake()
    {
        spielFeldData.Add(GameObject.Find("Input_X").GetComponent<InputField>());
        spielFeldData.Add(GameObject.Find("Input_Y").GetComponent<InputField>());
        spielFeldData.Add(GameObject.Find("Input_AnzMines").GetComponent<InputField>());

        controllerScript = gameController.GetComponent<GameController>();
        TextMeshProUGUI errorMessageLabel = GameObject.Find("ErrorMessage").GetComponent<TextMeshProUGUI>();
        errorMessage = new ErrorMessage(errorMessageLabel);
    }

    public void PlayGame()
    {
        if (CheckAllCorrect())
        {
            newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(newSceneIndex);
            Scene game = SceneManager.GetSceneByBuildIndex(newSceneIndex);
           
            controllerScript.length_x = int.Parse(spielFeldData[0].text);
            controllerScript.length_y = int.Parse(spielFeldData[1].text);
            controllerScript.anzmines = int.Parse(spielFeldData[2].text);
            
            DontDestroyOnLoad(gameController);
            SceneManager.UnloadScene(newSceneIndex-1);
        }
        else
            ResetInputFields();
    }

    public bool CheckNumber(string input)
    {
        foreach (char letter in input)
        {
            if (!Char.IsDigit(letter))
                return false;
        }

        return true;
    }

    public bool CheckAllCorrect()
    {
        foreach (InputField input in spielFeldData)
        {
            if (!CheckAvailable(input.text))
            {
                errorMessage.Show(ErrorMessage.NODATA);
                return false;
            }

            if (!CheckNumber(input.text))
            {
                errorMessage.Show(ErrorMessage.INVALIDDATA);
                return false;
            }

            if (input.text == "0" || input.text == "1")
            {
                errorMessage.Show(ErrorMessage.IMPOSSIBLEGAME);
                return false;
            }
        }

        if (!CheckValidNumbers())
        {
            errorMessage.Show(ErrorMessage.IMPOSSIBLEGAME);
            return false;
        }

        return true;
    }

    public bool CheckAvailable(string input) => input != "";
    public bool CheckValidNumbers()
    {
        int lengthx = int.Parse(spielFeldData[0].text);
        int lenghty = int.Parse(spielFeldData[1].text);
        int anzBombs = int.Parse(spielFeldData[2].text);

        int Percent80 = lenghty * lengthx / 100 * 80;
        
        if(anzBombs > Percent80)
            return false;
            
        return true;
    }

    public void ResetInputFields()
    {
        foreach (InputField input in spielFeldData)
            input.text = "";
    }

    public void Preset5Times5()
    {  
        spielFeldData[0].text = "5"; 
        spielFeldData[1].text = "5"; 
        spielFeldData[2].text = "5";
    }
    

    public void Preset16Times16()
    {
        spielFeldData[0].text = "16";
        spielFeldData[1].text = "16";
        spielFeldData[2].text = "40";
    }

    public void Preset30Times30()
    {
        spielFeldData[0].text = "30";
        spielFeldData[1].text = "30";
        spielFeldData[2].text = "90";
    }
    
    public void Preset50Times50()
    {
        spielFeldData[0].text = "50";
        spielFeldData[1].text = "50";
        spielFeldData[2].text = "250";
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

public class MenüScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject gameController;
    private GameController controllerScript;
    private List<InputField> spielFeldData = new List<InputField>();
    private int newSceneIndex;
    private Scene spielfeld;
    
    void Awake()
    {
        spielFeldData.Add(GameObject.Find("Input_X").GetComponent<InputField>());
        spielFeldData.Add(GameObject.Find("Input_Y").GetComponent<InputField>());
        spielFeldData.Add(GameObject.Find("Input_AnzMines").GetComponent<InputField>());

        controllerScript = gameController.GetComponent<GameController>();
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

            
            // SceneManager.MoveGameObjectToScene(gameController, game);
            DontDestroyOnLoad(gameController);
            SceneManager.UnloadScene(newSceneIndex-1);
        }
        else
            ResetInputFields();
        
    }

    public bool CheckAvailable(string input)
    {
        return input != "";
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
                Debug.Log(input.name +": Available");
                return false;
            }

            if (!CheckNumber(input.text))
            {
                Debug.Log(input.name + ": Number");
                return false;
            }

            if (input.text == "0" || input.text == "1")
            {
                Debug.Log(input.name + ": 01");
                return false;
            }
                              
        }

        if (!CheckValidNumbers())
            return false;
        

        return true;
    }

    public bool CheckValidNumbers()
    {
        int lengthx = int.Parse(spielFeldData[0].text);
        int lenghty = int.Parse(spielFeldData[1].text);
        int anzBombs = int.Parse(spielFeldData[2].text);

        if(lenghty * lengthx <= anzBombs)
            return false;

        return true;
    }

    public void ResetInputFields()
    {
        foreach (InputField input in spielFeldData)
            input.text = "";
    }

}

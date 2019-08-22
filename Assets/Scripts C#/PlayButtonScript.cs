using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
{

    private string _length_X_text;
    private string _length_Y_text;
    private string _anzMines_text;

    public string Length_X_text
    {
        get => _length_X_text;
        private set
        {
            int Eingabe = CheckInt(value);
            if (Eingabe == 0)
            {
                ProveClick = false;
            }
            else
            {
                ProveClick = true;
            }

            Length_X_text = value;
        }
    }

    public string Length_Y_text { get; private set; }
    public string AnzMines_text { get; private set; }

    public InputField length_X;
    public InputField length_Y;
    public InputField anzMines;


    public bool ProveClick { get; private set; } = false;
    public string ErrorMessage { get; private set; } = "Eingabe ungültig";


    public int CheckInt(string Eingabe)
    {
        try
        {
            return int.Parse(Eingabe);
        }
        catch
        {
            return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        length_X = GameObject.Find("Input_X").GetComponent<InputField>();
        length_Y = GameObject.Find("Input_Y").GetComponent<InputField>();
        anzMines = GameObject.Find("Input_AnzMines").GetComponent<InputField>();
    }

    void OnClick()
    {

    }

  

    // Update is called once per frame
    void Update()
    {
        
    }
}

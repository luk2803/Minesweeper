using System;
using System.Collections.Generic;
using UnityEngine;
//using GameController;

public class Mine : MonoBehaviour
{
    public enum MineState
    {
        open = 0,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        clicked,
        mine,
        bomb,
        redbomb


    }

    public int position;
    public bool IsMine { get; set; } = false;

    private BoxCollider2D boxCollider;
    public int Minesinnear { get; set; } = 0;
    private Sprite openbomb;
    private Sprite openredbomb;
    private Sprite mineClicked;
    private Sprite mine;
    //public bool IsSave { get; private set; } = false;
    public bool IsSave = false;

    private GameObject gameController;
    private GameController controllerScript;
    private bool messagenear = false;

    //alle publics mit gettern/settern ersetzen 
    //algorithmus erstellen
    //bilder für zahlen erstellen
    //bombem filtern die nicht herausfindbar sind
    //start/endscreen
    //selbsteintscheidung von größe des feldes und anz der bomben
    private MineState state;
    public MineState State
    {
        get { return state; }
        set
        {
            switch (value)
            {
                case MineState.open:
                    {
                        state = MineState.open;

                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[0];
                        break;
                    }
                case MineState.one:
                    {
                        state = MineState.one;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[1];
                        break;
                    }
                case MineState.two:
                    {
                        state = MineState.two;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[2];
                        break;
                    }
                case MineState.three:
                    {
                        state = MineState.three;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[3];
                        break;
                    }
                case MineState.four:
                    {
                        state = MineState.four;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[4];
                        break;
                    }
                case MineState.five:
                    {
                        state = MineState.five;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[5];
                        break;
                    }
                case MineState.six:
                    {
                        state = MineState.six;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[6];
                        break;
                    }
                case MineState.seven:
                    {
                        state = MineState.seven;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[7];
                        break;
                    }
                case MineState.eight:
                    {
                        state = MineState.eight;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.openMines[8];
                        break;
                    }
                case MineState.clicked:
                    {
                        state = MineState.clicked;
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.advancedMines[0];
                        break;
                    }
                case MineState.mine:
                    {                                          
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.advancedMines[1];
                        state = MineState.mine;
                        break;
                    }
                case MineState.bomb:
                    {
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.advancedMines[2];
                        state = MineState.bomb;
                        break;
                    }
                case MineState.redbomb:
                    {
                        this.GetComponent<SpriteRenderer>().sprite = controllerScript.advancedMines[3];
                        state = MineState.redbomb;
                        break;
                    }

            }


        }
    }



    private bool exit = false;

    private Mine m = null;
    private bool first = true;

    public void Fill(int i, int j)
    {

        if (first)
        {
            m.Click();
            if (Minesinnear != 0)
                return;

            first = false;
        }
        else
        {
            try
            {
                m = controllerScript.GetSpielFeld()[i, j];
            }
            catch
            {
                return;
            }

            if (m == null)
            {
                return;
            }

            if (m.State != MineState.mine)
            {

                return;
            }

            m.Click();
            if (m.Minesinnear != 0)
            {

                return;
            }
        }

        Fill(i + 1, j);
        Fill(i, j + 1);
        Fill(i - 1, j);
        Fill(i, j - 1);
        Fill(i + 1, j +1);
        Fill(i + 1, j -1);
        Fill(i - 1, j -1);
        Fill(i -1, j +1);
    }

    public static bool Compare(Mine m, Mine mine)
    {
        if (m.position == mine.position)
            return true;
        return false;
    }




    public bool Click()
    {
        if (!IsMine)
        {
            controllerScript.AddSpielZug();

            State = (MineState)Minesinnear;
          

            if (controllerScript.GetSpielZüge() ==
                controllerScript.GetSpielFeld().Length - controllerScript.anzmines)
            {
                controllerScript.gewonnen = true;
                controllerScript.gameOver = true;
            }
        }
        else
        {
            controllerScript.gameOver = true;
            controllerScript.openBombs();
            State = MineState.redbomb;
        }
        //Debug.Log(controllerScript.Spielzüge + " Spielzüge");
        return true;
    }

    public void ProveClick()
    {

        if (Input.GetMouseButton(0))
        {

            if (controllerScript.Alreadyclicked)
                return;
            if (State == MineState.mine)
            {
                State = MineState.clicked;

                controllerScript.Alreadyclicked = true;
            }
        }
        else
        {
            if (State == MineState.clicked)
            {
                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                #region isSave
                if (controllerScript.GetAndNegateFirstClick())
                    {
                        try
                        {
                            controllerScript.GetSpielFeld()[i, j].IsSave = true;
                        }
                        catch
                        {
                        }

                        try
                        {
                            controllerScript.GetSpielFeld()[i + 1, j].IsSave = true;
                        }
                        catch
                        {
                        }

                        try
                        {
                            controllerScript.GetSpielFeld()[i - 1, j].IsSave = true;
                        }
                        catch
                        {
                        }

                        try
                        {
                            controllerScript.GetSpielFeld()[i, j + 1].IsSave = true;
                        }
                        catch
                        {
                        }

                        try
                        {
                            controllerScript.GetSpielFeld()[i, j - 1].IsSave = true;
                        }
                        catch
                        {
                        }


                        controllerScript.GameStart();
                    }

                    #endregion
                Fill(i, j);     
                controllerScript.Alreadyclicked = false;
            }
            else
            {
                controllerScript.Alreadyclicked = false;
            }
        }
    }

    void OnMouseOver()
    {       
        int x, y;
        controllerScript.GetMinePosIJ(this, out x, out y);       
        ProveClick();

    }

    void OnMouseExit()
    {
        exit = true;
        messagenear = false;

    }

    public void SetIsMine()
    {
        IsMine = true;
    }


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
      
        gameController = GameObject.Find("GameController");
        controllerScript = gameController.GetComponent<GameController>();    
        m = this;   
        State = MineState.mine;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (controllerScript.gameOver && !controllerScript.message)
        {
            if (controllerScript.gewonnen)
                Debug.Log("Gewonnen!");
            else
                Debug.Log("Verloren!");
            controllerScript.message = true;

            return;
        }

        if (exit)
        {
            if (!Input.GetMouseButton(0))
            {
                exit = false;
                if (State == MineState.clicked)
                {
                    State = MineState.mine;

                }
            }
            else
            {
                return;
            }
        }



    }
}

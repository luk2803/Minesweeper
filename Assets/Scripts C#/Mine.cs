using System;
using Assets.Scripts_C_;
using UnityEngine;

//using GameController;

//TODO:
//refactor Fill/ alle try catch Blöcke
//bombem filtern die nicht herausfindbar sind
//start/endscreen

public class Mine : MonoBehaviour
{
    private MineData MineData { get; set; }
    private GameController controllerScript;

    private bool exit = false;
    private bool firstFillCall = true;
    private Mine fillMineInstance;

    public bool GetIsMine()
    {
        return MineData.IsMine;
    }

    public void AddOneToMinesInNear(int bombcount)
    {
        MineData.MinesInNear += bombcount;
    }

    public MineState GetState()
    {
        return MineData.State;
    }

    public void SetState(MineState state)
    {
        MineData.State = state;
    }

    public bool GetIsNotAllowedToBeBomb()
    {
        return MineData.IsNotAllowedToBeBomb;
    }

    public void Reset()
    {
        MineData.IsMine = false;
        MineData.State = MineState.mine;
        MineData.MinesInNear = 0;
    }

    public void SetPosition(int position)
    {
        MineData.Position = position;
    }
    
    public void Fill(int i, int j)
    {
        if (firstFillCall)
        {
            fillMineInstance.Open();
            if (MineData.MinesInNear != 0)
                return;

            firstFillCall = false;
        }
        else
        {
            SetFillMineInstance(i,j);
            if (DemolitionConditionsAndMineOpen())
                return;
        }

        Fill(i + 1, j);
        Fill(i, j + 1);
        Fill(i - 1, j);
        Fill(i, j - 1);
        Fill(i + 1, j + 1);
        Fill(i + 1, j - 1);
        Fill(i - 1, j - 1);
        Fill(i - 1, j + 1);
    }

    public void SetFillMineInstance(int i, int j)
    {
        try
        {
            fillMineInstance = controllerScript.GetSpielFeld()[i, j];
        }
        catch
        {
            fillMineInstance = null;
        }
    }
    
    public bool DemolitionConditionsAndMineOpen()
    {
        if (controllerScript.gameOver == true)
            return true;
        if (fillMineInstance == null)
            return true;
        if (fillMineInstance.MineData.State != MineState.mine)
            return true;
        if (fillMineInstance.MineData.State == MineState.mineFlag)
            return true;
       

        fillMineInstance.Open();

        if (fillMineInstance.MineData.MinesInNear != 0)
            return true;

        return false;
    }

    public static bool Compare(Mine m, Mine mine)
    {
        if (m.MineData.Position == mine.MineData.Position)
            return true;
        return false;
    }


    public void Open()
    {
        if (!MineData.IsMine)
        {
            controllerScript.AddSpielZug();

            MineData.State = (MineState) MineData.MinesInNear;


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
            controllerScript.openAllBombs();
            MineData.State = MineState.redbomb;
        }

        //Debug.Log(controllerScript.Spielzüge + " Spielzüge");
        return;
    }

    public void ProveClick()
    {
        if (Input.GetMouseButton(0))
        {
            if (controllerScript.Alreadyclicked)
                return;

            if (MineData.State == MineState.mine)
            {
                if (controllerScript.isFlagMode)
                    MineData.State = MineState.flagclicked;
                else
                    MineData.State = MineState.clicked;
                
                controllerScript.Alreadyclicked = true;
            }
            else if (MineData.State == MineState.mineFlag && controllerScript.isFlagMode)
            {
             RemoveFlag(); 
             controllerScript.Alreadyclicked = true;
            }
        }
        else
        {
            if (MineData.State == MineState.clicked)
            {
                if (controllerScript.isFlagMode)
                {
                    SetFlag();
                    return;
                }

                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                
                if (controllerScript.GetAndNegateFirstClick())
                    SetIsNotAllowedToBeBomb(i, j);
                
                Fill(i, j);
                controllerScript.Alreadyclicked = false;
            }else if (MineData.State == MineState.flagclicked)
            {
                SetFlag();
                controllerScript.Alreadyclicked = false;
            }
            else
            {
                controllerScript.Alreadyclicked = false;
            }
        }
    }


    private void SetFlag()
    {
        MineData.State = MineState.mineFlag;
    }

    private void RemoveFlag()
    {
        MineData.State = MineState.mine;
            Debug.Log("remove");
    }

    private void SetIsNotAllowedToBeBomb(int i, int j)
    {
        SafeSetIsNotAllowedToBeBomb(i,j);
        SafeSetIsNotAllowedToBeBomb(i + 1, j + 1);
        SafeSetIsNotAllowedToBeBomb(i +1, j);
        SafeSetIsNotAllowedToBeBomb(i - 1, j);
        SafeSetIsNotAllowedToBeBomb(i, j + 1);
        SafeSetIsNotAllowedToBeBomb(i, j - 1);
        SafeSetIsNotAllowedToBeBomb(i - 1, j - 1);
        SafeSetIsNotAllowedToBeBomb(i + 1, j - 1); 
        SafeSetIsNotAllowedToBeBomb(i - 1, j + 1);

        controllerScript.GameStart();
        
    }

    public void SafeSetIsNotAllowedToBeBomb(int i, int j)
    {
        try
        {
            controllerScript.GetSpielFeld()[i, j].MineData.IsNotAllowedToBeBomb = true;
        }
        catch
        {
        }
    }

    void OnMouseOver()
    {
        if (controllerScript.spielendeMessage)
            return;

        ProveClick();
    }

    void OnMouseExit()
    {
        exit = true;
    }

    public void SetIsMine()
    {
        MineData.IsMine = true;
    }


    private void Awake()
    {
        controllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        MineData = new MineData(this, controllerScript);
        MineData.State = MineState.mine;
        fillMineInstance = this;
    }

    // Start is called before the firstFillCall frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerScript.gameOver && !controllerScript.spielendeMessage)
        {
            if (controllerScript.gewonnen)
            {
                Debug.Log("Gewonnen!");
                controllerScript.openAllBombs();
            }
            else
                Debug.Log("Verloren!");


            controllerScript.spielendeMessage = true;

            return;
        }

        if (exit)
        {
            if (!Input.GetMouseButton(0))
            {
                exit = false;
                if (MineData.State == MineState.clicked)
                {
                    MineData.State = MineState.mine;
                }
            }
            else
            {
                return;
            }
        }
    }
}
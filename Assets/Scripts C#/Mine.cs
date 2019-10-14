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

    public bool GetIsMine() => MineData.IsMine;
    public void AddOneToMinesInNear(int bombcount) => MineData.MinesInNear += bombcount;
    public MineState GetState() => MineData.State;
    public void SetState(MineState state)=> MineData.State = state;
    public bool GetIsNotAllowedToBeBomb() => MineData.IsNotAllowedToBeBomb;
    public void SetPosition(int position) => MineData.Position = position;
    private void SetFlag() => MineData.State = MineState.mineFlag;
    private void RemoveFlag() => MineData.State = MineState.mine;
    
    public void ResetMine() 
    {
        MineData.IsMine = false;
        MineData.State = MineState.mine;
        MineData.MinesInNear = 0;
        MineData.IsNotAllowedToBeBomb = false;
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
    }

    public void ProveClick()
    {
        if(controllerScript.GetFirstClick())
            if (controllerScript.isFlagMode)
                return;
        
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
                {
                    SetIsNotAllowedToBeBomb(i, j);
                    controllerScript.InsertMines();
                }

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
        if (controllerScript.gameOverMessage)
            return;

        ProveClick();
    }

    void OnMouseExit()
    {
        exit = true;
    }

    public void SetIsMine() =>  MineData.IsMine = true;
    private void Awake()
    {
        controllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        MineData = new MineData(this, controllerScript);
        MineData.State = MineState.mine;
        fillMineInstance = this;
    }
    void Update()
    {
        if (controllerScript.gameOver && !controllerScript.gameOverMessage)
        {
            if (controllerScript.gewonnen)
            {
                Debug.Log("Gewonnen!");
                controllerScript.openAllBombs();
            }
            else
                Debug.Log("Verloren!");
            
            controllerScript.gameOverMessage = true;
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
        }
    }
}
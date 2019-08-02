using Assets.Scripts_C_;
using UnityEngine;
//using GameController;

//TODO:
//bombem filtern die nicht herausfindbar sind
//start/endscreen

public class Mine : MonoBehaviour
{

    public MineData MineData { get; private set; }
    private GameController controllerScript;
    
    private bool exit = false;
    private bool firstFillCall = true;
    private Mine fillMineInstance;

    public void Fill(int i, int j)
    {

        if (firstFillCall)
        {
            fillMineInstance.Click();
            if (MineData.MinesInNear != 0)
                return;

            firstFillCall = false;
        }
        else
        {
            try
            {
                fillMineInstance = controllerScript.GetSpielFeld()[i, j];
            }
            catch
            {
                return;
            }

            if (fillMineInstance == null)
            {
                return;
            }

            if (fillMineInstance.MineData.State != MineState.mine)
            {

                return;
            }

            fillMineInstance.Click();
            if (fillMineInstance.MineData.MinesInNear != 0)
            {

                return;
            }
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

    public static bool Compare(Mine m, Mine mine)
    {
        if (m.MineData.Position == mine.MineData.Position)
            return true;
        return false;
    }
    

    public bool Click()
    {     

        if (!MineData.IsMine)
        {
            controllerScript.AddSpielZug();

            MineData.State = (MineState)MineData.MinesInNear;


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
        return true;
    }

    public void ProveClick()
    {

        if (Input.GetMouseButton(0))
        {

            if (controllerScript.Alreadyclicked)
                return;
            if (MineData.State == MineState.mine)
            {
                MineData.State = MineState.clicked;

                controllerScript.Alreadyclicked = true;
            }
        }
        else
        {
            if (MineData.State == MineState.clicked)
            {
                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                #region isNotAllowedToBeBomb
                if (controllerScript.GetAndNegateFirstClick())
                {
                    try
                    {
                        controllerScript.GetSpielFeld()[i, j].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }

                    try
                    {
                        controllerScript.GetSpielFeld()[i + 1, j].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }

                    try
                    {
                        controllerScript.GetSpielFeld()[i - 1, j].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }

                    try
                    {
                        controllerScript.GetSpielFeld()[i, j + 1].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }

                    try
                    {
                        controllerScript.GetSpielFeld()[i, j - 1].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }

                    try
                    {
                        controllerScript.GetSpielFeld()[i - 1, j - 1].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }
                    try
                    {
                        controllerScript.GetSpielFeld()[i + 1, j - 1].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }
                    try
                    {
                        controllerScript.GetSpielFeld()[i - 1, j + 1].MineData.IsNotAllowedToBeBomb = true;
                    }
                    catch
                    {
                    }
                    try
                    {
                        controllerScript.GetSpielFeld()[i + 1, j + 1].MineData.IsNotAllowedToBeBomb = true;
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
        MineData= new MineData(this, controllerScript);
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

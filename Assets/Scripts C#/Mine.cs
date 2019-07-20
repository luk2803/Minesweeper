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
    private Sprite mineOpen;
    private Sprite mine;
    private Sprite openmine_1;
    private Sprite openmine_2;
    private Sprite openmine_3;
    private Sprite openmine_4;
    private Sprite openmine_5;
    private Sprite openmine_6;
    private Sprite openmine_7;
    private Sprite openmine_8;

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

                        this.GetComponent<SpriteRenderer>().sprite = mineOpen;
                        break;
                    }
                case MineState.one:
                    {
                        state = MineState.one;
                        this.GetComponent<SpriteRenderer>().sprite = openmine_1;
                        break;
                    }
                case MineState.two:
                    {
                        state = MineState.two;
                        this.GetComponent<SpriteRenderer>().sprite = openmine_2;
                        break;
                    }
                case MineState.three:
                    {
                        state = MineState.three;
                        this.GetComponent<SpriteRenderer>().sprite = openmine_3;
                        break;
                    }
                case MineState.four:
                    {
                        state = MineState.four;
                        this.GetComponent<SpriteRenderer>().sprite = openmine_4;
                        break;
                    }
                case MineState.five:
                    {
                        state = MineState.five;
                        this.GetComponent<SpriteRenderer>().sprite = openmine_5;
                        break;
                    }
                case MineState.six:
                {
                    state = MineState.six;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_6;
                    break;
                }
                case MineState.seven:
                {
                    state = MineState.seven;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_7;
                    break;
                }
                case MineState.eight:
                {
                    state = MineState.eight;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_8;
                    break;
                }
                case MineState.clicked:
                    {
                        state = MineState.clicked;
                        this.GetComponent<SpriteRenderer>().sprite = mineClicked;
                        break;
                    }
                case MineState.mine:
                    {
                        this.GetComponent<SpriteRenderer>().sprite = mine;
                        state = MineState.mine;
                        break;
                    }
                case MineState.bomb:
                    {
                        this.GetComponent<SpriteRenderer>().sprite = openbomb;
                        state = MineState.bomb;
                        break;
                    }
                case MineState.redbomb:
                    {
                        this.GetComponent<SpriteRenderer>().sprite = openredbomb;
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


    }

    public static bool Compare(Mine m, Mine mine)
    {
        if (m.name == mine.name)
            return true;
        return false;
    }




    public bool Click()
    {



        if (!IsMine)
        {
            if (Minesinnear != 0 && controllerScript.spielzüge == 0)
            {
                controllerScript.GameStart();
                Debug.Log("Neustart!");
                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                Fill(i, j);
                return false;
            }
            controllerScript.spielzüge++;

            State = (MineState)Minesinnear;
            Debug.Log(controllerScript.spielzüge);
            Debug.Log(controllerScript.GetSpielFeld().Length - controllerScript.anzmines);

            if (controllerScript.spielzüge ==
                controllerScript.GetSpielFeld().Length - controllerScript.anzmines)
            {
                controllerScript.gewonnen = true;
                controllerScript.gameOver = true;
            }
        }
        else
        {
            if (controllerScript.spielzüge == 0)
            {
                controllerScript.GameStart();
                Debug.Log("Neustart!");
                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                Fill(i, j);
                return true;
            }
            controllerScript.gameOver = true;

            controllerScript.openBombs();

            State = MineState.redbomb;

        }
        //Debug.Log(controllerScript.spielzüge + " spielzüge");

        return true;
    }

    public void ProveClick()
    {

        if (controllerScript.gameOver)
        {
            if (controllerScript.gewonnen)
            {
                if (!controllerScript.message)
                {
                    Debug.Log("Gewonnen!");
                    controllerScript.message = true;
                }

            }
            return;
        }

        if (!messagenear)
        {
            //Debug.Log("mines in near: " + Minesinnear);
            messagenear = true;
        }
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
                if (!IsMine)
                {

                    int i, j;
                    controllerScript.GetMinePosIJ(this, out i, out j);

                    // Debug.Log("fill!!!!!!");
                    Fill(i, j);
                    //Click();


                }
                else
                {
                    if (controllerScript.spielzüge == 0)
                    {
                        controllerScript.GameStart();
                        int i, j;
                        controllerScript.GetMinePosIJ(this, out i, out j);
                        Debug.Log("Neustart!");
                        Fill(i, j);
                        //Click();
                        return;
                    }
                    controllerScript.gameOver = true;

                    controllerScript.openBombs();

                    State = MineState.redbomb;

                }

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
        ProveClick();
        Debug.Log("HALDLWUI)RFWAP");
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
        State = MineState.mine;


        gameController = GameObject.Find("GameController");
        controllerScript = gameController.GetComponent<GameController>();
        mineOpen = controllerScript.mineOpen;
        mineClicked = controllerScript.mineClicked;
        mine = controllerScript.mine;
        openredbomb = controllerScript.openredbomb;
        openbomb = controllerScript.openbomb;
        openmine_1 = controllerScript.openmine_1;
        openmine_2 = controllerScript.openmine_2;
        openmine_3 = controllerScript.openmine_3;
        openmine_4 = controllerScript.openmine_4;
        openmine_5 = controllerScript.openmine_5;
        openmine_6 = controllerScript.openmine_6;
        openmine_7 = controllerScript.openmine_7;
        openmine_8 = controllerScript.openmine_8;
        m = this;

        Debug.Log(this.transform.localPosition.x +": x");
        Debug.Log(this.transform.localPosition.y + ": y");


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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

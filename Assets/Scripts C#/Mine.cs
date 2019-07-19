using UnityEngine;
//using GameController;

public class Mine : MonoBehaviour
{
    public enum Mine_State
    {
        one = 1,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        clicked,
        open,
        mine,
        bomb,
        redbomb


    }

    public int position;
    public bool IsMine { get; set; } = false;

    private BoxCollider2D boxCollider;
    public int minesinnear = 0;
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

    private GameObject gameController;
    private GameController controllerScript;
    private bool messagenear = false;

    //alle publics mit gettern/settern ersetzen 
    //algorithmus erstellen
    //bilder für zahlen erstellen
    //bombem filtern die nicht herausfindbar sind
    //start/endscreen
    //selbsteintscheidung von größe des feldes und anz der bomben

    public Mine_State state = Mine_State.mine;
    private bool exit = false;

    private Mine m = null;
    private bool first = true;

    public void Fill(int i, int j)
    {

        if (first)
        {
            m.Click();
            if (minesinnear != 0)
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

            if (m.state != Mine_State.mine)
            {

                return;
            }

            m.Click();
            if (m.minesinnear != 0)
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

    public void SetOwnSprite(int number)
    {


        switch (number)
        {
            case 0:
                {
                    state = Mine_State.open;

                    this.GetComponent<SpriteRenderer>().sprite = mineOpen;
                    break;
                }
            case 1:
                {
                    state = Mine_State.one;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_1;
                    break;
                }
            case 2:
                {
                    state = Mine_State.two;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_2;
                    break;
                }
            case 3:
                {
                    state = Mine_State.three;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_3;
                    break;
                }
            case 4:
                {
                    state = Mine_State.four;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_4;
                    break;
                }
            case 5:
                {
                    state = Mine_State.five;
                    this.GetComponent<SpriteRenderer>().sprite = openmine_5;
                    break;
                }
        }
    }

    public void SetOwnSprite(Sprite mine)
    {
        this.GetComponent<SpriteRenderer>().sprite = mine;

        switch (mine.name)
        {
            case "mine_open":
                {

                    state = Mine_State.open;
                    break;
                }
            case "mine_clicked":
                {

                    state = Mine_State.clicked;
                    break;
                }
            case "mine_closed":
                {

                    state = Mine_State.mine;
                    break;
                }
            case "mineopenwithbomb":
                {
                    state = Mine_State.bomb;
                    break;
                }
            case "redbomb":
                {
                    state = Mine_State.redbomb;
                    break;
                }
        }
    }

    public bool Click()
    {
        
        if (!IsMine)
        {
            if (minesinnear != 0 && controllerScript.spielzüge == 0)
            {
                controllerScript.GameStart();
                Debug.Log("Neustart!");
                int i, j;
                controllerScript.GetMinePosIJ(this, out i, out j);
                Fill(i, j);
                return false;
            }
            controllerScript.spielzüge++;

            SetOwnSprite(minesinnear);
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

            SetOwnSprite(openredbomb);

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
            //Debug.Log("mines in near: " + minesinnear);
            messagenear = true;
        }
        if (Input.GetMouseButton(0))
        {

            if (controllerScript.Alreadyclicked)
                return;
            if (state == Mine_State.mine)
            {
                SetOwnSprite(mineClicked);

                controllerScript.Alreadyclicked = true;
            }
        }
        else
        {

            if (state == Mine_State.clicked)
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

                    SetOwnSprite(openredbomb);

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
        m = this;



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
                if (state == Mine_State.clicked)
                {
                    SetOwnSprite(mine);

                }
            }
            else
            {
                return;
            }
        }

    }
}

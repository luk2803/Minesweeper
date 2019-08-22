using System.Collections.Generic;
using Assets.Scripts_C_;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public bool gameOver = false;
    //public Sprite mineClicked;
    //public Sprite mine;
    //public Sprite openbomb;
    //public Sprite openredbomb;
    public List<Sprite> advancedMines = new List<Sprite>();
    public List<Sprite> openMines = new List<Sprite>();

    private Mine[,] spielfeld;
    private GameObject gameController;
    public bool Alreadyclicked { get; set; } = false;
    public int anzmines;
    static Random rnd = new Random();
    public int length_x = 0;
    public int length_y = 0;
    private int Spielzüge = 0;
    public bool gewonnen = false;
    public bool spielendeMessage = false;
    public GameObject myPrefab;
    private CameraScript cameraScript;
    private bool firstClick = true;

    public List<Sprite> GetAdvancedMines()
    {
        return advancedMines;
    }
    public List<Sprite> GetOpenMines()
    {
        return openMines;
    }

    public bool GetAndNegateFirstClick()
    {
        if (firstClick)
        {
            firstClick = false;
            return true;
        }

        return false;
    }

    public int GetSpielZüge()
    {
        return Spielzüge;
    }

    public void AddSpielZug()
    {
        Spielzüge++;
    }

    public Mine[,] GetSpielFeld()
    {
        return spielfeld;
    }

    public bool GetMinePosIJ(Mine m, out int i, out int j)
    {
        j = 0;

        for (i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (j = 0; j < spielfeld.GetLength(1); j++)
            {
                if (Mine.Compare(spielfeld[i, j], m))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void openAllBombs()
    {
        foreach (var m in spielfeld)
        {
            if (m.MineData.IsMine)
            {
                m.MineData.State = MineState.bomb;

            }

        }
    }

    public int isbomb(Mine m)
    {
        if (m.MineData.IsMine)
            return 1;
        return 0;
    }
    public void SetNumbers()
    {
        for (int i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (int j = 0; j < spielfeld.GetLength(1); j++)
            {
                if (spielfeld[i, j].MineData.IsMine)
                {
                    continue;
                }

                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i + 1, j]);
                }
                catch
                { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i - 1, j]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i + 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i - 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i + 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i - 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].MineData.MinesInNear += isbomb(spielfeld[i, j - 1]);
                }
                catch { }


            }
        }
    }

    public void resetMines()
    {
        foreach (var m in spielfeld)
        {
            m.MineData.IsMine = false;
            m.MineData.State = MineState.mine;
            m.MineData.MinesInNear = 0;
        }
    }

    public Mine GetMine(int pos)
    {
        int anz = 0;
        for (int i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (int j = 0; j < spielfeld.GetLength(1); j++)
            {
                if (anz == pos)
                    return spielfeld[i, j];
                anz++;
            }
        }

        return null;
    }

    public void GameStart()
    {

        Spielzüge = 0;
        int c = spielfeld.Length;


        resetMines();
        for (int i = 0; i < anzmines; i++)
        {
            Mine m = GetMine(rnd.Next(0, c));
            if (!m.MineData.IsMine)
                if (m != m.MineData.IsNotAllowedToBeBomb)
                    m.SetIsMine();
                else
                    i--;
            else
                i--;

        }

        SetNumbers();

    }

    // Start is called before the first frame update
    void Start()
    {
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        gameController = this.gameObject;

        spielfeld = new Mine[length_x, length_y];
        int zählerInsgesamt = 0;
        int zählerLänge = 0;
        int zählerBreite = 0;
        float längeBreiteDerMine = 1.2f;


        for (float x = 0; zählerLänge < length_x; x += längeBreiteDerMine)
        {
            zählerBreite = 0;
            for (float y = 0; zählerBreite < length_y; y += längeBreiteDerMine)
            {
                spielfeld[zählerLänge, zählerBreite] = Instantiate(myPrefab, new Vector3(x, y, 0), Quaternion.identity, gameController.transform).GetComponent<Mine>();
                spielfeld[zählerLänge, zählerBreite].MineData.Position = zählerInsgesamt;

                zählerInsgesamt++;
                zählerBreite++;
            }
            zählerLänge++;
        }

        
        cameraScript.createCameraSettings(length_x, length_y);

    }


    void Awake()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }
}

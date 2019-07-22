using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public bool gameOver = false;
    //public Sprite mineClicked;
    //public Sprite mine;
    //public Sprite openbomb;
    //public Sprite openredbomb;
    public List<Sprite> advancedMines;
    public List <Sprite> openMines;
    private Mine[,] spielfeld;
    private GameObject gameController;
    public bool Alreadyclicked { get; set; } = false;
    public int anzmines;
    static Random rnd = new Random();
    public int length_x = 0;
    public int length_y = 0;
    private int Spielzüge = 0;
    public bool gewonnen = false;
    public bool message = false;
    public GameObject myPrefab;
    private GameObject camera;
    private bool firstClick();

    public GetAnd

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

    public void openBombs()
    {
        foreach (var m in spielfeld)
        {
            if (m.IsMine)
            {
                m.State = Mine.MineState.bomb;

            }

        }
    }

    public int isbomb(Mine m)
    {
        if (m.IsMine)
            return 1;
        return 0;
    }
    public void SetNumbers()
    {
        for (int i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (int j = 0; j < spielfeld.GetLength(1); j++)
            {
                if (spielfeld[i, j].IsMine)
                {
                    continue;
                }

                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i + 1, j]);
                }
                catch
                { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i - 1, j]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i + 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i - 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i + 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i - 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].Minesinnear += isbomb(spielfeld[i, j - 1]);
                }
                catch { }


            }
        }
    }

    public void resetmines()
    {
        foreach (var m in spielfeld)
        {

            m.IsMine = false;
            m.State = Mine.MineState.mine;
            m.Minesinnear = 0;
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

    
        resetmines();
        for (int i = 0; i < anzmines; i++)
        {
            Mine m;
            if (((m = GetMine(rnd.Next(0, c)))).IsMine)        
                i--;                        
            else
                m.SetIsMine();
        }

        SetNumbers();

    }

    // Start is called before the first frame update
    void Start()
    {
    }


    void Awake()
    {
        camera = GameObject.Find("Main Camera");
        gameController = this.gameObject;
        //alle Minen in eine Liste packen dann in 2d array hauen. wurzel aus anz ziehen und länge festlegen.
        List<GameObject> allmines = new List<GameObject>();    
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    allmines.Add(transform.GetChild(i).gameObject);
        //}
        //Debug.Log(allmines.Count + " allmines");
        //Debug.Log(transform.childCount + " childcount");


        if (length_x == 0 || length_y == 0)
            length_x = length_y = (int)Math.Sqrt(allmines.Count);

        spielfeld = new Mine[length_x, length_y];
        int c = 0;
        int i = 0;
        int j = 0;



        for (float y = 0; i< length_x ; y+=1.2f)
        {
            j = 0;

            for (float x = 0; j< length_y; x += 1.2f)
            {           
               
                spielfeld[i ,j]= Instantiate(myPrefab, new Vector3(x, y, 0), Quaternion.identity,gameController.transform).GetComponent<Mine>();
                spielfeld[i, j].position = c;
               
                c++;
                j++;
            }
            i++;          
        }

        camera.transform.localPosition = new Vector3((float)(length_x * 1.2 / 2), (float)(length_y * 1.2 / 2));
        camera.GetComponent<Camera>().orthographicSize = (length_x < length_y) ? length_y : length_x;

       
    }


    // Update is called once per frame
    void Update()
    {

    }
}

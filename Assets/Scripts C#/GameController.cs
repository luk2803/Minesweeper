using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public bool gameOver = false;
    private Sprite ownSprite;
    public Sprite mineClicked;
    public Sprite mineOpen;
    public Sprite mine;
    public Sprite openbomb;
    public Sprite openredbomb;
    public Sprite openmine_1;
    public Sprite openmine_2;
    public Sprite openmine_3;
    public Sprite openmine_4;
    public Sprite openmine_5;
    private Mine[,] spielfeld;
    private Transform transform;
    private GameObject gameController;
    public bool Alreadyclicked { get; set; } = false;
    public int anzmines;
    static Random rnd = new Random();
    public int length_x = 0;
    public int length_y = 0;
    public int spielzüge = 0;
    public bool gewonnen = false;
    public bool message = false;

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
        foreach (var VARIABLE in spielfeld)
        {
            if (VARIABLE.IsMine)
            {
                VARIABLE.SetOwnSprite(openbomb);

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
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i + 1, j]);
                }
                catch
                { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i - 1, j]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i + 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i - 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i + 1, j - 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i - 1, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i, j + 1]);
                }
                catch { }
                try
                {
                    spielfeld[i, j].minesinnear += isbomb(spielfeld[i, j - 1]);
                }
                catch { }


            }
        }
    }

    public void resetmines()
    {
        foreach (var VARIABLE in spielfeld)
        {

            VARIABLE.IsMine = false;
            VARIABLE.SetOwnSprite(mine);
            VARIABLE.minesinnear = 0;
        }
    }

    public Mine GetMine(int pos)
    {
        int anz = 1;
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

        spielzüge = 0;
        int c = spielfeld.GetLength(0)* spielfeld.GetLength(1);

    
        resetmines();
        for (int i = 0; i < anzmines; i++)
        {
            Mine m;
            if (((m = GetMine(rnd.Next(1, c + 1)))).IsMine)
            {
                i--;
                continue;
            }
            else
                m.SetIsMine();
        }

        SetNumbers();

    }

    // Start is called before the first frame update
    void Start()
    {
        GameStart();

    }


    void Awake()
    {

        gameController = this.gameObject;
        //alle Minen in eine Liste packen dann in 2d array hauen. wurzel aus anz ziehen und länge festlegen.
        List<GameObject> allmines = new List<GameObject>();
        transform = gameController.transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            allmines.Add(transform.GetChild(i).gameObject);
        }
        Debug.Log(allmines.Count + " allmines");
        Debug.Log(transform.childCount + " childcount");


        if (length_x == 0 || length_y == 0)
            length_x = length_y = (int)Math.Sqrt(allmines.Count);

        Debug.Log(length_x+ "     " +length_y);
      
        spielfeld = new Mine[length_x, length_y];
        int c = 0;
        for (int i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (int j = 0; j < spielfeld.GetLength(1); j++)
            {
                spielfeld[i, j] = allmines[c].GetComponent<Mine>();
                ;
                spielfeld[i, j].position = c;
                spielfeld[i, j].SetOwnSprite(mine);
                c++;
            }
        }


    }


    // Update is called once per frame
    void Update()
    {

    }
}

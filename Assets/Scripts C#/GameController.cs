using System;
using System.Collections.Generic;
using Assets.Scripts_C_;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    //TODO
    // Messagebox für Errors  / ErrorMessages im Menü
    // Flaggen
    // Zoom in richtung
    // Zähler von Bomben
    // Design überarbeiten
    
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
    public bool isFlagMode = false;

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
            if (m.GetIsMine())
            {
                m.SetState(MineState.bomb);

            }

        }
    }

    public int isbomb(Mine m)
    {
        if (m.GetIsMine())
            return 1;
        return 0;
    }
    public void SetNumbers()
    {
        for (int i = 0; i < spielfeld.GetLength(0); i++)
        {
            for (int j = 0; j < spielfeld.GetLength(1); j++)
            {
                if (spielfeld[i, j].GetIsMine())
                {
                    continue;
                }

                SetMinesInNear(i, j);


            }
        }
    }

    private void SetMinesInNear(int i, int j)
    {
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i + 1, j]));
        }
        catch
        { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i - 1, j]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i + 1, j + 1]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i - 1, j - 1]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i + 1, j - 1]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i - 1, j + 1]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i, j + 1]));
        }
        catch { }
        try
        {
            spielfeld[i, j].AddOneToMinesInNear(isbomb(spielfeld[i, j - 1]));
        }
        catch { }
    }

    public void ResetMines()
    {
        foreach (var m in spielfeld)
        {
            m.Reset();
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


        ResetMines();
        for (int i = 0; i < anzmines; i++)
        {
            Mine m = GetMine(rnd.Next(0, c));
            if (!m.GetIsMine())
                if (m != m.GetIsNotAllowedToBeBomb())
                    m.SetIsMine();
                else
                    i--;
            else
                i--;

        }

        SetNumbers();

    }

    // Start is called before the first frame update
    public void Load() //wird in CameraScript aufgerufen
    {
        this.cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        
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
                spielfeld[zählerLänge, zählerBreite].SetPosition(zählerInsgesamt);

                zählerInsgesamt++;
                zählerBreite++;
            }
            zählerLänge++;
        }
        cameraScript.createCameraSettings(length_x, length_y);

    }


    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}

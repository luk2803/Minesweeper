using System;
using System.Collections.Generic;
using Assets.Scripts_C_;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    //TODO
    // Messagebox für Errors  / ErrorMessages im Menü
    // Flaggen
    // Zoom in richtung
    // Zähler von Bomben
    // Design überarbeiten
    
    public List<Sprite> advancedMines = new List<Sprite>();
    public List<Sprite> openMines = new List<Sprite>();
    static Random rnd = new Random();

    public bool gameOver;
    public int anzmines;
    public int length_x = 0;
    public int length_y = 0;
    public bool gewonnen;
    public bool gameOverMessage;
    private bool firstClick;
    public bool isFlagMode;
    public bool Alreadyclicked { get; set; } 
    private int Spielzüge;
    
    private Mine[,] spielfeld;
    private GameObject gameController;
    public GameObject myPrefab;
    private CameraScript cameraScript;
  

    public bool GetAndNegateFirstClick()
    {
        if (firstClick)
        {
            firstClick = false;
            return true;
        }
        return false;
    }
    public List<Sprite> GetAdvancedMines() => advancedMines;
    public List<Sprite> GetOpenMines() => openMines;
    public bool GetFirstClick() => firstClick;
    public Mine[,] GetSpielFeld() => spielfeld;
    public int GetSpielZüge() => Spielzüge;
    public void AddSpielZug() => Spielzüge++;
    
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
        Mine mine = spielfeld[i, j];

        SafeAddToMinesInNear(mine, i + 1, j);
        SafeAddToMinesInNear(mine, i - 1, j);
        SafeAddToMinesInNear(mine, i, j - 1);
        SafeAddToMinesInNear(mine, i, j + 1);
        SafeAddToMinesInNear(mine, i + 1, j + 1);
        SafeAddToMinesInNear(mine, i - 1, j - 1);
        SafeAddToMinesInNear(mine, i + 1, j - 1);
        SafeAddToMinesInNear(mine, i - 1, j + 1);
    }

    public void SafeAddToMinesInNear(Mine m, int i, int j)
    {
        try
        {
            m.AddOneToMinesInNear(isbomb(spielfeld[i, j]));
        }
        catch
        {
        }
    }

    public void ResetMines()
    {
        foreach (var m in spielfeld)
        {
            m.ResetMine();
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
    
    public void ResetGame()
    {
       
        gameOver = false;
        gewonnen = false;
        gameOverMessage = false;
        firstClick = true;
        isFlagMode = false;
        Alreadyclicked = false;
        Spielzüge = 0;
    }

    public void InsertMines()
    {
        int c = spielfeld.Length;
        
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
    public void Load() //wird in CameraScript aufgerufen
    {
        ResetGame();
        this.cameraScript = GameObject.Find("MainCamera").GetComponent<CameraScript>();

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
                spielfeld[zählerLänge, zählerBreite] =
                    Instantiate(myPrefab, new Vector3(x, y, 0), Quaternion.identity, gameController.transform)
                        .GetComponent<Mine>();
                spielfeld[zählerLänge, zählerBreite].SetPosition(zählerInsgesamt);

                zählerInsgesamt++;
                zählerBreite++;
            }

            zählerLänge++;
        }

        cameraScript.createCameraSettings(length_x, length_y);
    }
}
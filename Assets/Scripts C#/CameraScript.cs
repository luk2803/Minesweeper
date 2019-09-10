using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameController").GetComponent<GameController>().Load();
    }

    public void createCameraSettings(int spielfeld_length_x, int spielfeld_length_y)
    {
        this.transform.localPosition = 
            new Vector3((float)(spielfeld_length_x * 1.2 / 2), (float)(spielfeld_length_y * 1.2 / 2), (float)-1);

        this.GetComponent<Camera>().orthographicSize = 
            (spielfeld_length_x < spielfeld_length_y) ? spielfeld_length_y : spielfeld_length_x;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}

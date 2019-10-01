using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        cameraInstance =  this.GetComponent<Camera>();
        GameObject.Find("GameController").GetComponent<GameController>().Load();
    }

    private Camera cameraInstance;

    public void createCameraSettings(int spielfeld_length_x, int spielfeld_length_y)
    {
        float mineLength = 1.2f;
        this.transform.localPosition =
            new Vector3((float) (spielfeld_length_x * mineLength / 2), (float) (spielfeld_length_y * mineLength / 2)-0.6f, (float) -1);

        int longerSide =  (spielfeld_length_x < spielfeld_length_y) ? spielfeld_length_y : spielfeld_length_x;

        cameraInstance.orthographicSize = longerSide * 0.75f;


    }

    // Update is called once per frame
    void Update()
    {
        float mouseWheelDirection = Input.GetAxis("Mouse ScrollWheel");
        float faktorOfZoom = 6;
       
        if (mouseWheelDirection > 0f) //forward
        {
            if (cameraInstance.orthographicSize - mouseWheelDirection * faktorOfZoom < 0)
                return;
            cameraInstance.orthographicSize -= mouseWheelDirection * faktorOfZoom;
        }
        else if (mouseWheelDirection < 0f) //backwards
        {
            cameraInstance.orthographicSize -= mouseWheelDirection * faktorOfZoom;
        }
    }
}
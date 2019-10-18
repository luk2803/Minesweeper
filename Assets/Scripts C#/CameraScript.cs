using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 cameraPosition;
    private Camera cameraInstance;
    void Awake()
    {
        GameController controllerScript;
        cameraInstance =  this.GetComponent<Camera>();
        controllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        controllerScript.LoadCamera();
        controllerScript.InstantiateMines();
        
        cameraPosition = cameraInstance.transform.position;
    }
    public void createCameraSettings(int spielfeld_length_x, int spielfeld_length_y)
    {
        float mineLength = 1.2f;
        this.transform.localPosition =
            new Vector3((float) (spielfeld_length_x * mineLength / 2), (float) (spielfeld_length_y * mineLength / 2)-0.6f, (float) -1);

        int longerSide =  (spielfeld_length_x < spielfeld_length_y) ? spielfeld_length_y : spielfeld_length_x;

        cameraInstance.orthographicSize = longerSide * 0.75f;
        
    }
    void Update()
    {
        MouseScroll();
    }

    private void MouseScroll()
    {
        float mouseWheelDirection = Input.GetAxis("Mouse ScrollWheel");
        float faktorOfZoom = 6;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 camPos = cameraPosition;
        Vector3 righttop = cameraInstance.ViewportToWorldPoint(new Vector3(1,1,cameraInstance.nearClipPlane));

        int XAddToCameraPos = (mousePosition.x > 0.5) ? 1: -1;
        int YAddToCameraPos = (mousePosition.y > 0.5) ? 1: -1;

        Vector3 midOfCameraPostition = new Vector3(camPos.x / 2, camPos.y / 2, camPos.y);
        
        if (mouseWheelDirection > 0f) //forward
        {
            if (cameraInstance.orthographicSize - mouseWheelDirection * faktorOfZoom < 0)
                return;
            cameraInstance.orthographicSize -= mouseWheelDirection * faktorOfZoom;
            transform.position =new Vector3(camPos.x+ XAddToCameraPos, camPos.y + YAddToCameraPos, camPos.z);
        }
        else if (mouseWheelDirection < 0f) //backwards
        {
            cameraInstance.orthographicSize -= mouseWheelDirection * faktorOfZoom;
            transform.position =new Vector3(camPos.x- XAddToCameraPos, camPos.y - YAddToCameraPos, camPos.z);

        }
    }
}
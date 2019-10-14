using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 cameraPosition;
    private Camera cameraInstance;
    void Awake()
    {
        cameraInstance =  this.GetComponent<Camera>();
        GameObject.Find("GameController").GetComponent<GameController>().Load();
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

    // Update is called once per frame
    void Update()
    {
        MouseScroll();

    }

    private void MouseScroll()
    {
        float mouseWheelDirection = Input.GetAxis("Mouse ScrollWheel");
        float faktorOfZoom = 6;

        Vector3 mousePosition = Input.mousePosition.normalized;
        Vector3 camPos = cameraPosition; // 7,2 6,6 -1

        int XAddToCameraPos = (mousePosition.x > 0.5) ? 1: -1;
        int YAddToCameraPos = (mousePosition.y > 0.5) ? 1: -1;
        
        
        Debug.Log(mousePosition);
        
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
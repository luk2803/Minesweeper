using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CanvasScripts : MonoBehaviour
{
    // Start is called before the first frame update
    private GameController controllerScript;
    void Start()
    {
        controllerScript = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ToggleFlag()
    {
        controllerScript.isFlagMode = !controllerScript.isFlagMode;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

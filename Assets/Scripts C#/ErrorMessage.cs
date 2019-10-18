using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ErrorMessage : MonoBehaviour
{
    public static string NODATA = "There is some data missing"; 
    public static string INVALIDDATA = "This is an invalid data"; 
    public static string IMPOSSIBLEGAME = "This is an impossible or to hard game";

    private TextMeshProUGUI errorMessageLabel;
    public ErrorMessage(TextMeshProUGUI label)
    {
        errorMessageLabel = label;
    }
    
    public void Show(string errorMessage)
    {
        errorMessageLabel.text = errorMessage;
    }



}
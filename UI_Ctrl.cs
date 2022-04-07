using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Ctrl : MonoBehaviour {


    public void OnClickStart()
    {
        Application.LoadLevel("Play");
    }

    public void OnClickFirst()
    {
        Application.LoadLevel("Play");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }



}

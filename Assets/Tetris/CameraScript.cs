using UnityEngine;
using System.Collections;
using System;

public class CameraScript : MonoBehaviour 
{

    public TetrisMain main;

    /*bool draw = true;

    public void setToDraw()
    {
        draw = true;
    }*/
    void OnPreRender() { }

    void OnPostRender()
    {
        if (main == null) return;

      //  if (draw)
            main.Draw();
        //draw = false;
    }
}

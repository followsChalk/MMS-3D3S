using UnityEngine;
using System.Collections;

public class NextBlockCamera : MonoBehaviour 
{

    public TetrisMain main;

    void OnPostRender()
    {
        if (main == null) return;

        main.DrawNextBlock();
    }
}

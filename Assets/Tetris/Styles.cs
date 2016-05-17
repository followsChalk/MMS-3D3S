using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Styles : MonoBehaviour
{

    public static Dictionary<string, GUIStyle> styles = new Dictionary<string, GUIStyle>();

    // Use this for initialization
    void Start()
    {
        Object[] files = Resources.LoadAll("Textures/RotationButtons");

        for (int i = 0; i < files.Length; ++i)
        {
            styles[files[i].name] = new GUIStyle();
            styles[files[i].name].normal.background = Resources.Load("Textures/RotationButtons/" + files[i].name) as Texture2D;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

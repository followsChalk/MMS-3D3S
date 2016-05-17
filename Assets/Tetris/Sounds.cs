using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sounds : MonoBehaviour {

    public static Dictionary<string,AudioSource> sounds = new Dictionary<string,AudioSource>();
     
	// Use this for initialization
    void Start()
    {
        Object[] clips = Resources.LoadAll("Sounds") ;

        for (int i = 0; i < clips.Length; ++i)
        {
            sounds[clips[i].name] = gameObject.AddComponent<AudioSource>(); 
            sounds[clips[i].name].clip=    Resources.Load("Sounds/" + clips[i].name) as AudioClip;
            
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

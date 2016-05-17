using UnityEngine;
using System.Collections;

public class SimpleAutoRotation : MonoBehaviour {

    public float    speed   = 90f;
    public Vector3  axis    = Vector3.up;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(axis, speed * Time.deltaTime);
	}
}

using UnityEngine;
using System.Collections;

public class SimpleCameraController : MonoBehaviour {

    static float    rotationTime = 0f;
    static float    yAngle = 45f;
    static float    xAngle = 20;
    enum Axis { Y, LocalX };
    static Axis     axis = Axis.Y;
    static float    direction = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (rotationTime > 0)
        {
            rotationTime -= Time.deltaTime;
            Vector3 axisVec = axis==Axis.Y ? transform.InverseTransformDirection(Vector3.up) : Vector3.right;
            float angle = axis == Axis.Y ? yAngle : xAngle;
            transform.Rotate(axisVec, direction*angle * Time.deltaTime);

            if (rotationTime <= 0f) rotationTime = 0f;
        }

	}

    public static void RotateRight()
    {
        axis = Axis.Y;
        direction = 1f;
        rotationTime = 1f;
    }
    public static void RotateLeft()
    {
        axis = Axis.Y;
        direction = -1f;
        rotationTime = 1f;
    }
    public static void RotateUp()
    {
        axis = Axis.LocalX;
        direction = -1f;
        rotationTime = 1f;
    }
    public static void RotateDown()
    {
        axis = Axis.LocalX;
        direction = 1f; 
        rotationTime = 1f;
    }
}

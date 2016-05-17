using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// source: http://answers.unity3d.com/questions/600148/detect-swipe-in-four-directions-android.html

public class SwipeDetector : MonoBehaviour
{

    public float minSwipeDistY=5;

    public float minSwipeDistX=5;

    private Vector2 startPos;
    private Vector2 swipeDelta;

    public int rotationModeArea = 200;

    public enum SwipeDirection  {Up,Down,Right,Left}
   /*
    public string output = "empty";
    public void PrintSwipe(SwipeDirection dir)
    {
       output = minSwipeDistX.ToString() + minSwipeDistY.ToString() + "\n(" + (int)swipeDelta.x + "," + (int)swipeDelta.y + ")\n"+"("+startPos.x+","+startPos.y+")"+  dir.ToString();
    }

    void Start()
    {
       Swipe += PrintSwipe;
    }
    */

    public static event Action<SwipeDirection> Swipe;

    public static event Action<bool> RotationMode;

    void Update()
     {
         if (Input.GetKeyUp(KeyCode.D)) Swipe(SwipeDirection.Right);
         if (Input.GetKeyUp(KeyCode.A)) Swipe(SwipeDirection.Left);
         if (Input.GetKeyUp(KeyCode.W)) Swipe(SwipeDirection.Up);
         if (Input.GetKeyUp(KeyCode.S)) Swipe(SwipeDirection.Down);
         if (Input.GetKeyUp(KeyCode.LeftShift)) RotationMode(true);

         if (Input.GetKeyUp(KeyCode.LeftControl)) RotationMode(false);


         if (Input.touchCount > 0 )    
         { 
             List<Touch> allTouches=new List<Touch>();
             allTouches.AddRange(Input.touches);
             Touch touch = allTouches.Find(t => t.position.y > rotationModeArea);

             if (allTouches.Exists(t => t.position.y <= rotationModeArea))
                { if(RotationMode!=null) RotationMode(true);}
             else if (RotationMode!=null)RotationMode(false);

             if (allTouches.Exists(t => t.position.y > rotationModeArea))
             {
                 if(touch.phase==TouchPhase.Began)     startPos = touch.position;     
                 else if(touch.phase==TouchPhase.Ended || touch.phase==TouchPhase.Canceled)
                 { 
                         swipeDelta= touch.position-startPos;
    
                         if (Math.Abs(swipeDelta.x) >= minSwipeDistX || Math.Abs(swipeDelta.y)>=minSwipeDistY)    
                             if(Swipe!=null)
							 Swipe( Math.Abs(swipeDelta.x) > Math.Abs(swipeDelta.y)
                                    ? (swipeDelta.x>0 ? SwipeDirection.Right : SwipeDirection.Left)
                                    : (swipeDelta.y>0 ? SwipeDirection.Up : SwipeDirection.Down)
                                    );
                }
             }
         }
     }
    
    /*void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 500, 300), output);
    }*/
}
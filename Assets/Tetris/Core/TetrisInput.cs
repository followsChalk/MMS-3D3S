using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tetris
{
	static class TetrisInput
	{
        static class Keys
        {
            public static KeyCode right = KeyCode.RightArrow;
            public static KeyCode left = KeyCode.LeftArrow;
            public static KeyCode forw = KeyCode.UpArrow;
            public static KeyCode back = KeyCode.DownArrow;
            public static KeyCode rXinc = KeyCode.W;
            public static KeyCode rXdec = KeyCode.S;
            public static KeyCode rYinc = KeyCode.D;
            public static KeyCode rYdec = KeyCode.A;
            public static KeyCode rZinc = KeyCode.E;
            public static KeyCode rZdec = KeyCode.Q;
        }

        public static Vector3int GetDirectionFromInput()  { return new Vector3int(  (Input.GetKeyDown(Keys.right)?1:0) - (Input.GetKeyDown(Keys.left)?1:0),
                                                                                    0,
                                                                                    (Input.GetKeyDown(Keys.forw)?1:0) - (Input.GetKeyDown(Keys.back)?1:0)); }

        public static Vector3int GetRotationFromInput()   { 
                                                            int xInc = Input.GetKeyDown(Keys.rXinc)?1:0;
                                                            int xDec = Input.GetKeyDown(Keys.rXdec)?1:0;
                                                            int yInc = Input.GetKeyDown(Keys.rYinc)?1:0;
                                                            int yDec = Input.GetKeyDown(Keys.rYdec)?1:0;
                                                            int zInc = Input.GetKeyDown(Keys.rZinc)?1:0;
                                                            int zDec = Input.GetKeyDown(Keys.rZdec)?1:0;
                                                            
                                                            return new Vector3int(  (xInc - xDec) * (1-yInc-yDec+yInc*yDec)*(1-zInc-zDec+zInc*zDec), 
                                                                                    (yInc - yDec) * (1-xInc-xDec+xInc*xDec)*(1-zInc-zDec+zInc*zDec),
                                                                                    (zInc - zDec) * (1-xInc-xDec+xInc*xDec)*(1-yInc-yDec+yInc*yDec));       }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using State = MainMenu.State;
using Controls = MainMenu.Controls;

namespace Tetris
{
    class Tetris
    {
        public MainMenu     mainMenu;

        public Vector3int   dimensions;                 // sirina visina dubina
        public Vector3int   spawnPosition;
        public List<Block>  spawnedBlocks               = new List<Block>();
        public Block        currentBlock;
        public Block        nextBlock;

        float time = 0f;
        int frq = 1;

        //za lakse iteriranje po XZ ravnini
        private List<int> xs                            = new List<int>();
        private List<int> zs                            = new List<int>();

        public int score;
        public int level = 0;
        public float speed = 0.7f;

        public CameraScript cam1;
        public CameraScript cam2;

        public Tetris(Vector3int dimensions)            { this.dimensions = dimensions;
                                                          this.spawnPosition = new Vector3int(dimensions.X/2, dimensions.Y-2, dimensions.Z/2);          
                                                          for(int i=0;i<dimensions.X;++i)xs.Add(i);  
                                                          for(int k=0;k<dimensions.Z;++k)zs.Add(k);                                                     
                                                          Block.AddFullBlock(dimensions.X,dimensions.Z);                                         
                                                        }

        // Game 
        public void StartGame()                         { spawnedBlocks.Clear(); nextBlock=null;  SpawnNextBlock();  score=0; level=0;                  }
        
        public void SpawnNextBlock()                    { //spawnedBlocks.Add(Block.RandomBlock(spawnPosition,2));
                                                          if(nextBlock==null)nextBlock=Block.RandomStandardBlock(spawnPosition);
                                                          spawnedBlocks.Add(nextBlock);
                                                          nextBlock = Block.RandomStandardBlock(spawnPosition);
                                                          currentBlock=spawnedBlocks.Last();
                                                          frq = 1;
                                                          if (InvalidPosition(currentBlock)) mainMenu.NoResume(score);                                           
                                                                                                                                                        }
        
        public void UpdateGame()                        { time+=Time.deltaTime; if(time > 1/((float)frq*(Math.Floor(level/3.0)+1)*speed)) 
                                                            {time = 0f; this.NextStep(); SetToDraw(); }                                                 }
        private void NextStep()                         { if (CanCurrentBlockMove(Vector3int.Down)) currentBlock.MoveDown();  
                                                          else {CheckSolvedLevels(); Sounds.sounds["DropDown"].Play(); SpawnNextBlock();  }             }

        private void CheckSolvedLevels()                {  List<int> ys = currentBlock.bricks.Select(v => currentBlock.AbsPos(v).Y).Distinct().ToList<int>();
                                                           ys.RemoveAll( y => xs.Any( x => zs.Any(z => 
                                                                  spawnedBlocks.All(block=> block.bricks.All(v => 
                                                                            block.AbsPos(v) != new Vector3int(x,y,z))))));
                                                           level += ys.Count * ys.Count;
                                                           score = 100 * level;
                                                           DeleteLevels(ys);  LowerLevels(ys);                                                          }

        private void DeleteLevels(List<int> ys)         { Sounds.sounds["DeleteLevel"].Play();
                                                          spawnedBlocks.ForEach(block => block.bricks.RemoveAll(v => ys.Contains(block.AbsPos(v).Y)));  }
        private void LowerLevels(List<int> ys)          { spawnedBlocks.ForEach(block => block.bricks.ForEach(v => 
                                                              v.MoveDown( ys.Count(y => y < block.AbsPos(v).Y))));                                      }

        // Movement,Rotation
        private bool 
        CanCurrentBlockMove(Vector3int direction)       { return InvalidPosition(currentBlock.GetMoved(direction)) == false;                            }
        private bool
        CanCurrentBlockRotate(Vector3int rotation)      { return InvalidPosition(currentBlock.GetRotated(rotation)) == false;                           }

        

        // Collision
        private bool IsOutOfStage(Block block)          { return block.NotBoundedZeroAndUpper(dimensions);                                              }            
        private bool InvalidPosition(Block block)       { return        IsOutOfStage(block)
                                                                    ||  spawnedBlocks.Any(block2 =>     block2 != currentBlock
                                                                                                    &&  Block.CollisionTest(block,block2)==true);       }
        

        // Input
        public void UpdateInput()                       { 
                                                          Vector3int dir = GetDirectionFromInput();
                                                          Vector3int rot = GetRotationFromInput();
                                                          if (rot != Vector3int.Zero)
                                                          {
                                                              if (CanCurrentBlockRotate(rot))
                                                              { currentBlock.Rotate(rot); Sounds.sounds["Rotation"].Play();
                                                                SetToDraw();                                                   }
                                                              else Sounds.sounds["InvalidPosition"].Play();
                                                          }
                                                          if (dir != Vector3int.Zero)
                                                          {
                                                              if (CanCurrentBlockMove(dir)) 
                                                              { currentBlock.Move(dir);
                                                                SetToDraw();                                                    }
                                                              else Sounds.sounds["InvalidPosition"].Play();
                                                          }
                                                          ResetVars();
                                                        }
                                                                      
        
        // Draw    
        public void Draw()                              {  spawnedBlocks.ForEach(block=>block.Draw());                    
                                                          Drawing.DrawAxis(dimensions);                                              }
        public void DrawNextBlock()                     { nextBlock.GetMoved(new Vector3int(0,50,0) - spawnPosition).Draw();         }
                                                          
       
        
        static bool dirDownKey, dirUpKey,dirLeftKey,dirRightKey;

        static int  xInc,xDec,yInc,yDec,zInc,zDec;

        static float scrWidth,scrHeight,btnSize ;

        Rect dirDownRect,dirUpRect,dirLeftRect,dirRightRect,xIncRect,xDecRect,yIncRect,yDecRect,zIncRect,zDecRect,dropDownRect;

        void SetGUIVars()
        { 
              scrWidth=Screen.width;
              scrHeight = Screen.height;
            btnSize = 0.15f;

            dirDownRect = new Rect(0.775f * scrWidth, scrHeight - 0.15f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            dirUpRect = new Rect(0.775f * scrWidth, scrHeight - 0.45f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            dirLeftRect = new Rect(0.7f * scrWidth, scrHeight - 0.3f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            dirRightRect = new Rect(0.85f * scrWidth, scrHeight - 0.3f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);


            xIncRect = new Rect(0.075f * scrWidth, scrHeight - 0.45f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            xDecRect = new Rect(0.075f * scrWidth, scrHeight - 0.15f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            yIncRect = new Rect(0.0f * scrWidth, scrHeight - 0.3f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            yDecRect = new Rect(0.15f * scrWidth, scrHeight - 0.3f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            zIncRect = new Rect(0.15f * scrWidth, scrHeight - 0.6f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);
            zDecRect = new Rect(0.0f * scrWidth, scrHeight - 0.6f * scrWidth, btnSize * scrWidth, btnSize * scrWidth);

            dropDownRect = new Rect(0.35f * scrWidth, scrHeight - 0.15f * scrWidth, 0.3f * scrWidth, btnSize * scrWidth);
        
        }

        public void OnGUI()
        {
            GUI.skin = mainMenu.defaultSkin;
            SetGUIVars();

            switch (mainMenu.controls)
            {
                case Controls.Swipe:

                    GUI.Box(new Rect(0f, Screen.height - 200f, 200, 200), "", Styles.styles["rotMode"]);
                  
                    break;

                case Controls.Buttons:

                    if (GUI.Button(dirDownRect, "", Styles.styles["dirdown"])) dirDownKey = true;
                    if (GUI.Button(dirUpRect, "", Styles.styles["dirup"])) dirUpKey = true;
                    if (GUI.Button(dirLeftRect, "", Styles.styles["dirleft"])) dirLeftKey = true;
                    if (GUI.Button(dirRightRect, "", Styles.styles["dirright"])) dirRightKey = true;
                    if (GUI.Button(xIncRect, "",Styles.styles["rotXLc"]))       xInc=1; 
                    if (GUI.Button(xDecRect, "",Styles.styles["rotXRc"]))       xDec=1;
                    if (GUI.Button(yIncRect, "",Styles.styles["rotYLc"]))       yInc=1;
                    if (GUI.Button(yDecRect, "",Styles.styles["rotYRc"]))       yDec=1;
                    if (GUI.Button(zIncRect, "",Styles.styles["rotZLc"]))       zInc=1;
                    if (GUI.Button(zDecRect, "",Styles.styles["rotZRc"]))       zDec=1;
                    
                    break;
            }

            if (GUI.Button(dropDownRect, "", Styles.styles["dropdown2"])) frq = (frq * 10) % 11;

            GUI.Label(new Rect(Screen.width*(1f - 2*btnSize), 0.02f*Screen.height, btnSize * scrWidth, btnSize * scrHeight), "score:"+ score.ToString());
        }

        bool rotationMode = false;

        public void SetRotationMode(bool b)
        {
            rotationMode = b;
        }
        // pretplati se na static event iz MiniGestureRecognizera
        public void SetVarsFromSwipe(SwipeDetector.SwipeDirection dir)
        {
            if(mainMenu.controls==Controls.Swipe)
            {
                if(rotationMode)
                {
                    if (dir == SwipeDetector.SwipeDirection.Up)     xInc = 1;
                    if (dir == SwipeDetector.SwipeDirection.Down)   xDec = 1;
                    if (dir == SwipeDetector.SwipeDirection.Left)   yInc = 1;
                    if (dir == SwipeDetector.SwipeDirection.Right)  yDec = 1;
                }
                else
                {
                    if (dir == SwipeDetector.SwipeDirection.Up)     dirUpKey = true;
                    if (dir == SwipeDetector.SwipeDirection.Down)   dirDownKey = true;
                    if (dir == SwipeDetector.SwipeDirection.Left)   dirLeftKey = true;
                    if (dir == SwipeDetector.SwipeDirection.Right)  dirRightKey = true;
                }
            }
        }

        void ResetVars()
                                                          {
                                                            dirDownKey = false; dirUpKey = false;   dirLeftKey = false; dirRightKey = false;
                                                            xInc = 0;   xDec = 0;   yInc = 0;   yDec = 0;zInc = 0;zDec = 0;
                                                          } 

        public static Vector3int GetDirectionFromInput()  { return new Vector3int(  (dirRightKey?1:0) - (dirLeftKey?1:0),
                                                                                    0,
                                                                                    (dirUpKey ? 1 : 0) - (dirDownKey ? 1 : 0));
                                                          }

        public static Vector3int GetRotationFromInput()   { 
                                                            return new Vector3int(  (xInc - xDec) * (1-yInc-yDec+yInc*yDec)*(1-zInc-zDec+zInc*zDec), 
                                                                                    (yInc - yDec) * (1-xInc-xDec+xInc*xDec)*(1-zInc-zDec+zInc*zDec),
                                                                                    (zInc - zDec) * (1-xInc-xDec+xInc*xDec)*(1-yInc-yDec+yInc*yDec));       
                                                          }


        private void SetToDraw()                        {// cam1.setToDraw(); cam2.setToDraw(); Debug.Log("draw");
        }

                                                                    
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;


namespace Tetris
{
	class Block
	{
        Vector3int                  position;
        public List<Vector3int>     bricks                   = new List<Vector3int>();
        int                         color                    =0;


        public  Block(  Vector3int position,int color,
                        params Vector3int[] bricks)         { this.position=position;   
                                                              this.bricks = bricks.ToList<Vector3int>();                    
                                                              this.color=color;                                             }
        private Block(Block block)                          { this.position=block.position;
                                                              foreach(Vector3int brick in block.bricks)
                                                                  this.bricks.Add(new Vector3int(brick));
                                                              this.color = block.color;                                     }
        private Block(Vector3int position,Block block)      { this.position=position;
                                                              foreach(Vector3int brick in block.bricks)
                                                                  this.bricks.Add(new Vector3int(brick));      
                                                              this.color=block.color;                                       }
    
        // ima smisla samo za brickse
        public Vector3int 
        AbsPos(Vector3int brick)                            { return this.position + brick;                                 }

        //korisiti se kod Bounded(..) tako da izracuna relativne kordinate granica(lower,upper)
        //umjesto da racuna puno apsolutnih koordinata od bricksova
        public Vector3int 
        RelPos(Vector3int v)                                { return v - this.position ;                                    }
       
        //local rotation
        public void Rotate(Vector3int a)                    { bricks.ForEach(v=>v.Rotate(a));                               }
        public Block GetRotated(Vector3int a)               { Block ret = new Block(this); ret.Rotate(a); return ret;       }

        public void Place(Vector3int position)              { this.position = position;                                     }
        public void Move(Vector3int dPos)                   { this.position += dPos;                                        }
        
        public void MoveRight()                             { this.Move(Vector3int.Right);                                  }            
        public void MoveLeft()                              { this.Move(Vector3int.Left);                                   }   
        public void MoveUp()                                { this.Move(Vector3int.Up);                                     }     
        public void MoveDown()                              { this.Move(Vector3int.Down);                                   }   
        public void MoveForward()                           { this.Move(Vector3int.Forward);                                }   
        public void MoveBackward()                          { this.Move(Vector3int.Backward);                               }
        
        //neda mi se ostale
        public Block GetMovedDown()                         { Block ret=new Block(this);    ret.MoveDown();    return ret;  }   
        public Block GetMoved(Vector3int dir)               { Block ret=new Block(this);    ret.Move(dir);     return ret;  }   


        //unutar kvadra definiranog vrhovima (0,0,0) i upper
        public bool 
        BoundedZeroAndUpper(Vector3int upper)               { return bricks.All(v => v.BoundedHalf(
                                                                    this.RelPos(Vector3int.Zero),this.RelPos(upper)));      }
        public bool
        NotBoundedZeroAndUpper(Vector3int upper)            { return BoundedZeroAndUpper(upper)==false;                     }

        public static bool
        CollisionTest(Block b1,Block b2)                    { return b1.bricks.Any( v=> 
                                                                        b2.bricks.Any( u => b1.AbsPos(v) ==b2.AbsPos(u)));  }


        public static Block 
        RandomBlock(Vector3int position,int r)              { List<Vector3int> vectors=new List<Vector3int>();
                                                              for(int i=0;i<8*r*r*r;++i) // (2*radius)^3
                                                                    vectors.Add(
                                                                        new Vector3int( UnityEngine.Random.Range(-r,r),
                                                                                        UnityEngine.Random.Range(-r,r),
                                                                                        UnityEngine.Random.Range(-r,r)));
                                                              return new Block(position,1, vectors.ToArray());              }

        public static Block 
        RandomStandardBlock(Vector3int position)            {   return new Block(position, standardBlocks[
                                                                    UnityEngine.Random.Range(0,standardBlocks.Count)]);     }
        public static Block
        LBlock(Vector3int position,int r)                   { 
                                                                List<Vector3int> vectors=new List<Vector3int>();
                                                                for(int i=-r;i<=r;++i)
                                                                    vectors.Add(new Vector3int(0,0,i));
                                                                vectors.Add(new Vector3int(1, 0, r)); 
                                                                vectors.Add(new Vector3int(0, 1, -r+1));
                                                                return new Block(position,1, vectors.ToArray());              }



        //Draw
        public void Draw()                                  {   for(int i=0;i<this.bricks.Count;++i)
                                                                        this.AbsPos(bricks[i]).Draw(color);       }

        public override string ToString()                   { string ret=""; ret+=position+";"; 
                                                              bricks.ForEach(b=> ret+=b); return ret;                       }

        // baza standardnik blokova
        public static List<Block> standardBlocks            =new List<Block>{
                                                            //dugi
                                                            new Block( Vector3int.Zero,0,
                                                                        new Vector3int(0,0,0),  new Vector3int(1,0,0),
                                                                        new Vector3int(-1,0,0), new Vector3int(2,0,0)),
                                                            // L blok
                                                            new Block( Vector3int.Zero,1,
                                                                        new Vector3int(0,0,0),  new Vector3int(1,0,0),
                                                                        new Vector3int(-1,0,0), new Vector3int(1,0,1)),
                                                            // pseudo L blok
                                                        /*   new Block( Vector3int.Zero,2,
                                                                        new Vector3int(0,0,0),  new Vector3int(1,0,0),
                                                                        new Vector3int(-1,0,0), new Vector3int(2,0,0),
                                                                        new Vector3int(1,0,1)),
                                                           */ // T blok    
                                                            new Block( Vector3int.Zero,3,
                                                                        new Vector3int(0,0,0),  new Vector3int(1,0,0),
                                                                        new Vector3int(-1,0,0), new Vector3int(0,0,1)),
                                                            // S blok
                                                            new Block( Vector3int.Zero,4,
                                                                        new Vector3int(0,0,0),  new Vector3int(0,0,1),
                                                                        new Vector3int(-1,0,0), new Vector3int(1,0,1)),
                                                            // kocka
                                                            new Block( Vector3int.Zero,5,
                                                                        new Vector3int(0,0,0),  new Vector3int(1,0,0),
                                                                        new Vector3int(0,0,1), new Vector3int(1,0,1),
                                                                        new Vector3int(0,1,0),  new Vector3int(1,1,0),
                                                                        new Vector3int(0,1,1), new Vector3int(1,1,1)),
              
                                                            // puni
                                         /*                   new Block( Vector3int.Zero,5,
                                                                        new Vector3int(-1,0,0),  new Vector3int(0,0,0),
                                                                        new Vector3int(1,0,0), new Vector3int(-2,0,0),
                                                                        new Vector3int(-1,0,1),  new Vector3int(0,0,1),
                                                                        new Vector3int(1,0,1), new Vector3int(-2,0,1),
                                                                        new Vector3int(-1,0,-2),  new Vector3int(0,0,-2),
                                                                        new Vector3int(1,0,-2), new Vector3int(-2,0,-2),
                                                                        new Vector3int(-1,0,-1),  new Vector3int(0,0,-1),
                                                                        new Vector3int(1,0,-1), new Vector3int(-2,0,-1))
                                           */               
                                                            };

        public static void AddFullBlock(int n, int m) 
        {
            List<Vector3int> vs=new List<Vector3int>();

            for (int i = 0; i < n; ++i)
                for (int j = 0; j < m; ++j)
                    if(i!=0 || j!=0)vs.Add(new Vector3int(i - n / 2, 0, j - n / 2));

            standardBlocks.Add(new Block(Vector3int.Zero, 7, vs.ToArray()));                   
        }
	}
}
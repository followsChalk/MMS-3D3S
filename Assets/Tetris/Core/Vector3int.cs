using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tetris
{

	class Vector3int
	{
        public int X, Y, Z;

        public Vector3int(int x, int y, int z)                          { X=x;     Y=y;     Z=z;                                                }
        public Vector3int(Vector3int v)                                 { X=v.X;   Y=v.Y;   Z=v.Z;                                              }
        public void Set(int x,int y,int z)                              { X=x;     Y=y;     Z=z;                                                }
       
        public void Rotate(Vector3int a)                                {  this.Set( a.X*a.X * X      +     a.Y * Z     +     a.Z *(-Y),
                                                                                         a.X *(-Z)    + a.Y*a.Y * Y     +     a.Z * X,
                                                                                         a.X * Y      +     a.Y *(-X)   + a.Z*a.Z * Z);         }

        public void MoveDown(int dy) { this.Y -= dy; }
        //Grupa
        public static Vector3int operator+(Vector3int v,Vector3int u)   { return new Vector3int(v.X + u.X, v.Y + u.Y, v.Z + u.Z);               }
        public static Vector3int operator-(Vector3int v,Vector3int u)   { return new Vector3int(v.X - u.X, v.Y - u.Y, v.Z - u.Z);               }
        public static Vector3int operator-(Vector3int v)                { return new Vector3int(-v.X, -v.Y, -v.Z);                              }

        //Vektorski prostor
        public static Vector3int operator *(int a, Vector3int v)        { return new Vector3int(a * v.X, a * v.Y, a * v.Z);                     }
        public static Vector3int operator *(Vector3int v,int a)         { return a*v;                                                           }
        public static Vector3int operator /(Vector3int v,int a)         { return new Vector3int(v.X/a,v.Y/a,v.Z/a);                             }     
        
        //eq
        public static bool operator==(Vector3int v,Vector3int u)        { return v.X==u.X && v.Y==u.Y && v.Z==u.Z;                              }
        public static bool operator!=(Vector3int v, Vector3int u)       { return (v==u) == false;                                               }                          

        //ne-simetricni uređaj(> i >= nisu intuitivni)
        public static bool operator <(Vector3int v, Vector3int u)       { return v.X < u.X && v.Y < u.Y && v.Z < u.Z;                           }
        public static bool operator >(Vector3int v, Vector3int u)       { Debug.LogError("Use operator<"); Debug.Break();   return false;       }
        public static bool operator <=(Vector3int v, Vector3int u)      { return v.X <= u.X && v.Y <= u.Y && v.Z <= u.Z;                        }
        public static bool operator >=(Vector3int v, Vector3int u)      { Debug.LogError("Use operator<="); Debug.Break();  return false;       }

        //segment,poluotvoreni interval,interval
        public bool Bounded(Vector3int lower, Vector3int upper)         { return lower <= this && this <= upper;                                }
        public bool BoundedHalf(Vector3int lower, Vector3int upper)     { return lower <= this && this < upper;                                 }
        public bool BoundedStrictly(Vector3int lower, Vector3int upper) { return lower < this && this < upper;                                  }

        public static Vector3int Zero                                   { get{ return new Vector3int(0,0,0); } }

        public static Vector3int Right                                  { get{ return new Vector3int(1  ,   0   ,   0   );}                     }
        public static Vector3int Left                                   { get{ return new Vector3int(-1 ,   0   ,   0   );}                     }
        public static Vector3int Up                                     { get{ return new Vector3int(0  ,   1   ,   0   );}                     }
        public static Vector3int Down                                   { get{ return new Vector3int(0  ,   -1  ,   0   );}                     }
        public static Vector3int Forward                                { get{ return new Vector3int(0  ,   0   ,   1   );}                     }
        public static Vector3int Backward                               { get{ return new Vector3int(0  ,   0   ,  -1   );}                     }

        public Vector3  GetVector3()                                    { return new Vector3(X,Y,Z);                                            }
        public override string ToString()                               { return "("+this.X+","+this.Y+","+this.Z+")";                          }
        public static Vector3int FromBinary(int source)                 { return new Vector3int(source%2,(source>>1)%2,(source>>2)%2);          }
	
        //Draw
        public void Draw(int i)                                         { Drawing.DrawCube(this,i);                                             }



	}
}

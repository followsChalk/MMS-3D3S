using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tetris
{
    static class Drawing
    {
        // uglavnom cratanja sa GLom
        
        public static void DrawAxis(Vector3int dimensions)
        {
            GL.Color(Color.red); GL.Begin(GL.LINES);
            for (int i = 0; i < dimensions.X; ++i)
            {   GL.Vertex3(i, 0f, 0f); GL.Vertex3(i, 0f, dimensions.Z );    GL.Vertex3(i, 0f, dimensions.Z ); GL.Vertex3(i, dimensions.Y , dimensions.Z );    }
            GL.End(); GL.Color(Color.green); GL.Begin(GL.LINES);
            for (int i = 0; i < dimensions.Y; ++i)
            { GL.Vertex3(0f, i, dimensions.Z); GL.Vertex3(dimensions.X, i, dimensions.Z);       GL.Vertex3(dimensions.X, i, 0f); GL.Vertex3(dimensions.X, i, dimensions.Z); }
            GL.End(); GL.Color(Color.blue); GL.Begin(GL.LINES);
            for (int i = 0; i < dimensions.Z; ++i)
            { GL.Vertex3(0f, 0f, i); GL.Vertex3(dimensions.X , 0f, i);      GL.Vertex3(dimensions.X, 0f, i); GL.Vertex3(dimensions.X, dimensions.Y , i);    }
            GL.End();

            GL.Begin(GL.QUADS);
            GL.Color(new Color(1,0,0,0.2f));
            GL.Vertex3(dimensions.X,0,0); GL.Vertex3(dimensions.X,dimensions.Y,0); GL.Vertex3(dimensions.X,dimensions.Y,dimensions.Z);  GL.Vertex3(dimensions.X,0,dimensions.Z);  
            GL.End();

            GL.Begin(GL.QUADS);
            GL.Color(new Color(0,1,0,0.2f));
            GL.Vertex3(0,0,0); GL.Vertex3(dimensions.X,0,0); GL.Vertex3(dimensions.X,0,dimensions.Z); GL.Vertex3(0f,0,dimensions.Z);   
            GL.End();

            GL.Begin(GL.QUADS);
            GL.Color(new Color(0,0,1,0.2f));
            GL.Vertex3(0,0,dimensions.Z); GL.Vertex3(dimensions.X,0,dimensions.Z); GL.Vertex3(dimensions.X,dimensions.Y,dimensions.Z); GL.Vertex3(0f,dimensions.Y,dimensions.Z);   
            GL.End();

        }

        //smece
        public static void DrawGrid(Vector3int dimensions)
        {
            GL.Begin(GL.LINES);
            for (int i = 0; i < dimensions.X; ++i)
                for (int j = 0; j < dimensions.Y; ++j)
                { GL.Vertex3(i, j, 0f); GL.Vertex3(i, j, dimensions.Z - 1); }

            for (int i = 0; i < dimensions.X; ++i)
                for (int j = 0; j < dimensions.Z; ++j)
                { GL.Vertex3(i, 0f, j); GL.Vertex3(i, dimensions.Y - 1, j); }
            for (int i = 0; i < dimensions.Y; ++i)
                for (int j = 0; j < dimensions.Z; ++j)
                { GL.Vertex3(0f, i, j); GL.Vertex3(dimensions.X - 1, i, j); }

            GL.End();
        }

        private static List<Color> cubeColors = new List<Color>()
        { Color.blue,Color.cyan,Color.gray,Color.green,Color.magenta,Color.red,Color.white,Color.yellow     };


        public static Vector3int tempCenter;
        
        public static void DrawCube(Vector3int center,int index)
        {
            Vector3 tempCenter = new Vector3(center.X,center.Y,center.Z);
         
            GL.Begin(GL.QUADS); 

            GL.Color(cubeColors[index%cubeColors.Count]);

            if(cubeVsPrepared==null) PrepareCubeVs();

            for (int i = 0; i < cubeVsPrepared.Length; ++i)
                GL.Vertex(tempCenter + cubeVsPrepared[i]);
            

            GL.End();
          
     

            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            
            for (int i = 0; i < cubeLs.Length; i+=2)
            {
                GL.Vertex(tempCenter + cubeLs[i]);
                GL.Vertex(tempCenter + cubeLs[i+1]);
            }

            GL.End();
            
       }

        static Vector3 halfVec = new Vector3(0.5f, 0.5f, 0.5f);
        static float bla = 0.98f;


        static Vector3[] cubeVs = new Vector3[] 
        {
            new Vector3(0, 0, 0),  new Vector3(1, 0, 0),  new Vector3(1, 1, 0),  new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),  new Vector3(0, 0, 1),  new Vector3(0, 1, 1),  new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),  new Vector3(1, 0, 0),  new Vector3(1, 0, 1),  new Vector3(0, 0, 1),

            new Vector3(1, 1, 1),  new Vector3(0, 1, 1),  new Vector3(0, 0, 1),  new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),  new Vector3(1, 1, 0),  new Vector3(1, 0, 0),  new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),  new Vector3(0, 1, 1),  new Vector3(0, 1, 0),  new Vector3(1, 1, 0)
        };

        static Vector3[] cubeVsPrepared;
        static void PrepareCubeVs()
        { 
            cubeVsPrepared=new Vector3[24];

            for(int i=0;i<cubeVs.Length;++i)
                cubeVsPrepared[i] = halfVec + bla * (cubeVs[i] - halfVec);

        }


        static Vector3[] cubeLs = new Vector3[] 
        {
            new Vector3(0, 0, 0),  new Vector3( 1, 0, 0),  new Vector3(0, 1, 0),  new Vector3( 1, 1, 0),
            new Vector3(0, 0, 1),  new Vector3( 1, 0, 1),  new Vector3(0, 1, 1),  new Vector3( 1, 1, 1),

            new Vector3(0, 0, 0),  new Vector3( 0, 1, 0),  new Vector3(1, 0, 0),  new Vector3( 1, 1, 0),
            new Vector3(0, 0, 1),  new Vector3( 0, 1, 1),  new Vector3(1, 0, 1),  new Vector3( 1, 1, 1),

            new Vector3(0, 0, 0),  new Vector3( 0, 0, 1),  new Vector3(1, 0, 0),  new Vector3( 1, 0, 1),
            new Vector3(0, 1, 0),  new Vector3( 0, 1, 1),  new Vector3(1, 1, 0),  new Vector3( 1, 1, 1)
        };
    }
}

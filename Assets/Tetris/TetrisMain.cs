using UnityEngine;
using System.Collections;
using Tetris;
using System.Collections.Generic;
using Tetris;

using State = MainMenu.State;

// backgrounMusic source: Korobeiniki , https://vgmdaily.wordpress.com/2009/10/22/korobeiniki-aka-tetris-type-a-arr-hirokazu-tanaka/
public class TetrisMain : MonoBehaviour 
{
    public MainMenu mainMenu;
    Tetris.Tetris tetris = new Tetris.Tetris(new Vector3int(6, 18, 6));


    public Quaternion cam1InitRot;

    public void Set(MainMenu mainMenu) { this.mainMenu = mainMenu; tetris.mainMenu = mainMenu; }
    void Awake ()           {
                                CreateLineMaterial();
                                tetris.cam1 = transform.GetChild(0).GetChild(0).GetComponent<CameraScript>();
                                tetris.cam1.main = this;
                                tetris.cam2=transform.GetChild(1).GetChild(0).GetComponent<CameraScript>();
                                tetris.cam2.main = this;
                                transform.GetChild(2).GetChild(0).GetComponent<NextBlockCamera>().main = this;
                                cam1InitRot = tetris.cam1.transform.parent.localRotation;
                            }
    public void Restart()   { SwipeDetector.Swipe += tetris.SetVarsFromSwipe; 
                              SwipeDetector.RotationMode += tetris.SetRotationMode;
                              tetris.StartGame();
                              StartCoroutine(RepositionCam1());  
                              Sounds.sounds["backgroundMusic"].loop = true;
                              Sounds.sounds["backgroundMusic"].Stop(); 
                              Sounds.sounds["backgroundMusic"].Play();  
                                }

	void Update ()          { if(mainMenu.state==State.Game){ tetris.UpdateInput();   tetris.UpdateGame(); }                        }        
    

    public void Draw()      { lineMaterial.SetPass(0); 
                              tetris.Draw();                                                                                        }
    
    public void DrawNextBlock() {lineMaterial.SetPass(0);
                              tetris.DrawNextBlock();                                                                               }

    public void OnGUI()     { if(mainMenu.state==State.Game)tetris.OnGUI();                                                         }



    #region smece


    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            //lineMaterial = Resources.Load("TetrisMat") as Material;
            
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 1);
           // lineMaterial.EnableKeyword("_ZTest");
            //lineMaterial.SetInt("_ZTest",1);
        }
    }

    

    #endregion

     public IEnumerator RepositionCam1()
    {
        if (cam1InitRot != null)
        {
            float t=0f,maxT=1f;
            while(t<maxT)
            {
               // Debug.Log("F");
                tetris.cam1.transform.parent.localRotation=Quaternion.Slerp(tetris.cam1.transform.parent.localRotation, cam1InitRot, 0.4f);
                t += Time.deltaTime;
                yield return 0;
            }
        }
        yield return 0;
    }
}

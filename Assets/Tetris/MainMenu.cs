using UnityEngine;
using System.Collections;
using Tetris;

public class MainMenu : MonoBehaviour {

    GameObject tetrisMainGO;
    TetrisMain tetrisMain;
    float btnWidth = 0.8f;
    float btnHeight = 0.15f;

    public GUISkin mainMenuSkin;
    public GUISkin defaultSkin;
    
    public enum State { Menu, Pause, NoResume,Game };
    public State state = State.Menu;

    public enum Controls { Buttons, Swipe }
    public Controls controls = Controls.Swipe;


	// Use this for initialization
	void Awake () 
    {
       // PlayerPrefs.SetInt("score", 0);
        tetrisMainGO=transform.GetChild(0).gameObject;
        tetrisMain = tetrisMainGO.GetComponent<TetrisMain>();
        tetrisMain.Set(this);
        tetrisMainGO.SetActive(false);

        mainMenuSkin = Resources.Load("MainMenuSkin") as GUISkin;
        defaultSkin= Resources.Load("DefaultSkin") as GUISkin;

        SwipeDetector.Swipe += SwipeCameraRotation;
	}

    public void NoResume(int currentScore)
    {
        if (currentScore > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", currentScore);
        state = State.NoResume;
        Sounds.sounds["backgroundMusic"].Stop();
    }

    void OnGUI()
    {
        float smallBtnHeight = btnHeight *0.4f;
        Rect titleRect          = new Rect(Screen.width * (0.5f - btnWidth / 2f), (0.15f) * Screen.height, Screen.width * btnWidth, Screen.height * btnHeight);
        Rect newGameRect        = new Rect(Screen.width * (0.5f - btnWidth / 2f), (0.15f + 1 * btnHeight) * Screen.height, Screen.width * btnWidth, Screen.height * btnHeight);
        Rect ctrlRect           = new Rect(Screen.width * (0.5f - btnWidth / 2f), (0.15f + 2  * btnHeight) * Screen.height, Screen.width * btnWidth, Screen.height * btnHeight);
        Rect highScoreRect      = new Rect(Screen.width * (0.5f - btnWidth / 2f), (0.15f + 3  * btnHeight) * Screen.height, Screen.width * btnWidth, Screen.height * btnHeight);
        Rect highScoreIntRect   = new Rect(Screen.width * (0.5f - btnWidth / 2f), (0.15f + 4  * btnHeight) * Screen.height, Screen.width * btnWidth, Screen.height * btnHeight);
       
        Rect pauseResumeRect    = new Rect(Screen.width * (0.0f), (0.02f) * Screen.height, Screen.width * btnWidth / 4f, Screen.height * smallBtnHeight);
        Rect restartRect        = new Rect(Screen.width * (0.0f), (0.02f + smallBtnHeight) * Screen.height, Screen.width * btnWidth / 4f, Screen.height * smallBtnHeight);
        Rect mainMenuRect       = new Rect(Screen.width * (0.0f), (0.02f + 2 * smallBtnHeight) * Screen.height, Screen.width * btnWidth / 4f, Screen.height * smallBtnHeight);
      
        GUI.skin = defaultSkin;
        switch (state)
        {
            case State.Menu:

                GUI.skin = mainMenuSkin;

                GUI.Label(titleRect, "3D3s");
              
                if (GUI.Button(newGameRect, "New Game"))        { state =State.Game;
                                                                  tetrisMainGO.SetActive(true);
                                                                  tetrisMain.Restart();
                                                                                                                        }

                if (GUI.Button(ctrlRect,"Controls:"+controls))  controls = (Controls)(1 - (int)controls);

                GUI.Label(highScoreRect, "High Score");
              
                GUI.Label(highScoreIntRect, PlayerPrefs.GetInt("score").ToString());


                break;
    
            case State.Pause:
                if (GUI.Button(pauseResumeRect, "Resume"))      { state = State.Game; 
                                                                  Sounds.sounds["backgroundMusic"].UnPause();           }
                if (GUI.Button(restartRect, "Restart"))         { tetrisMain.Restart();             state = State.Game; }
                if (GUI.Button(mainMenuRect, "Main Menu"))      { tetrisMainGO.SetActive(false);    state = State.Menu; 
                                                                  Sounds.sounds["backgroundMusic"].Stop();              }
                
                break;

            case State.NoResume:
                
                GUI.Box(pauseResumeRect, "Resume");
                if (GUI.Button(restartRect, "Restart"))         { tetrisMain.Restart();             state = State.Game; }
                if (GUI.Button(mainMenuRect, "Main Menu"))      { tetrisMainGO.SetActive(false);    state = State.Menu;
                                                                  Sounds.sounds["backgroundMusic"].Stop();              }
                
                break;

            case State.Game:

                if (GUI.Button(pauseResumeRect, "Pause"))       { state = State.Pause; 
                                                                  Sounds.sounds["backgroundMusic"].Pause();             }

                break;
            
        }

    }

    public void SwipeCameraRotation(SwipeDetector.SwipeDirection dir)
    {
        if (state == State.NoResume || state == State.Pause)
        {
            if (dir == SwipeDetector.SwipeDirection.Right) SimpleCameraController.RotateRight();
            if (dir == SwipeDetector.SwipeDirection.Left) SimpleCameraController.RotateLeft();
            if (dir == SwipeDetector.SwipeDirection.Up) SimpleCameraController.RotateUp();
            if (dir == SwipeDetector.SwipeDirection.Down) SimpleCameraController.RotateDown();
        }
    }

  
}

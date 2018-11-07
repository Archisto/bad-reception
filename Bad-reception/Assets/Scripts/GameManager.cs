using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Statics
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    instance = Resources.Load<GameManager>("GameManager");
                }

                if (instance == null)
                {
                    Debug.LogError(
                        "Could not find GameManager object in Resources");
                }
            }

            return instance;
        }
    }
    #endregion Statics

    private const string MainMenuKey = "MainMenu";
    private const string PressStartKey = "PressStart";

    public enum State
    {
        PressStart = 0,
        MainMenu = 1,
        Play = 2,
        LevelEnd = 3
    }

    public enum TransitionPhase
    {
        None = 0,
        ResetingScene = 1,
        ExitingScene = 2,
        StartingScene = 3
    }

    #region Fields

    private UIController _ui;
    private FadeToColor _fade;
    private PlayerTaskController _taskController;

    private bool _freshGameStart;
    private bool _updateAtSceneStart;
    private string _nextSceneName;
    private List<string> _levelSceneNames;

    public State GameState { get; set; }

    public TransitionPhase SceneTransition { get; set; }

    public bool SceneChanging
    {
        get
        {
            return SceneTransition == TransitionPhase.ExitingScene ||
                SceneTransition == TransitionPhase.StartingScene;
        }
    }

    public int CurrentLevel { get; private set; }

    #endregion Fields

    #region Initialization

    // Use this for initialization
    private void Awake ()
    {
		if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Init();
        }
	}

    // Use this for initialization
    private void Init()
    {
        SceneManager.activeSceneChanged += InitScene;

        if (SceneManager.GetActiveScene().name.Equals(MainMenuKey))
        {
            GameState = State.MainMenu;
        }
        else if (SceneManager.GetActiveScene().name.Equals(PressStartKey))
        {
            GameState = State.PressStart;
        }
        else
        {
            GameState = State.Play;
        }

        InitLevels();
        _taskController = FindObjectOfType<PlayerTaskController>();
        _updateAtSceneStart = true;
    }

    private void InitLevels()
    {
        _levelSceneNames = new List<string>
        {
            "Level1",
            "Level2",
            "Level3"
        };
    }

    private void LevelStartInit()
    {
        if (GameState == State.Play)
        {
            _freshGameStart = false;
            Debug.Log("Level starts");
        }
        else
        {
            Debug.Log("Menu starts");
        }

        _updateAtSceneStart = false;
    }

    private void InitScene()
    {
        InitUI();
        InitFade();

        if (GameState == State.Play)
        {
        }

        SceneTransition = TransitionPhase.None;
        _updateAtSceneStart = true;
    }

    /// <summary>
    /// Initializes the UI.
    /// </summary>
    private void InitUI()
    {
        _ui = FindObjectOfType<UIController>();
    }

    /// <summary>
    /// Initializes fading to a color.
    /// </summary>
    private void InitFade()
    {
        _fade = FindObjectOfType<FadeToColor>();
        if (_fade != null)
        {
            _fade.Init(_ui.fadeScreen);

            if (SceneTransition == TransitionPhase.StartingScene)
            {
                _fade.StartFadeIn(true);
            }
        }
    }

    #endregion Initialization

    #region Updating

    // Update is called once per frame
    private void Update()
    {
        if (SceneTransition == TransitionPhase.ExitingScene
                && _fade.FadedOut)
        {
            LoadScene(_nextSceneName);
        }
        else if (SceneTransition == TransitionPhase.ResetingScene
                 && _fade.FadedOut)
        {
            SceneTransition = TransitionPhase.None;
            ResetScene();
        }
        else if (!SceneChanging)
        {
            DebugInput();
        }

        if (_updateAtSceneStart)
        {
            LevelStartInit();
        }
    }

    #endregion Updating

    #region Scene Management

    /// <summary>
    /// Starts reseting level with a fade-out.
    /// </summary>
    public void StartSceneReset()
    {
        SceneTransition = TransitionPhase.ResetingScene;
        _fade.StartFadeOut(false);
        Debug.Log("Restarting level");
    }

    /// <summary>
    /// Resets the current scene.
    /// </summary>
    public void ResetScene()
    {
        _fade.StartFadeIn(true);
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        StartLoadingScene(MainMenuKey);
        GameState = State.MainMenu;
    }

    /// <summary>
    /// Loads a level.
    /// </summary>
    /// <param name="levelNum"></param>
    /// <returns></returns>
    public void LoadLevel(int levelNum)
    {
        CurrentLevel = levelNum;
        Debug.Log(string.Format("Going to level {0}",
            CurrentLevel));
        string sceneName = _levelSceneNames[CurrentLevel - 1];
        StartLoadingScene(sceneName);
        GameState = State.Play;
    }

    /// <summary>
    /// Tries to load a level.
    /// </summary>
    /// <param name="levelNum"></param>
    public void TryLoadLevel(int levelNum)
    {
        if (!SceneChanging)
        {
            if (levelNum >= 1 && levelNum <= _levelSceneNames.Count)
            {
                LoadLevel(levelNum);
            }
            else
            {
                Debug.LogError(string.Format("Invalid level number ({0}).", levelNum));
            }
        }
        else
        {
            Debug.LogWarning("Scene is already changing.");
        }
    }

    /// <summary>
    /// Testing.
    /// Loads a debug level: Lauri's Colosseum.
    /// </summary>
    public void LoadDebugLevel()
    {
        LoadDebugLevel("SampleScene");
    }

    /// <summary>
    /// Testing. Loads a debug level.
    /// </summary>
    public void LoadDebugLevel(string sceneName)
    {
        CurrentLevel = 0;
        StartLoadingScene(sceneName);
        GameState = State.Play;
    }

    /// <summary>
    /// Starts loading a scene with a fade-out.
    /// </summary>
    /// <param name="sceneName">The scene's name</param>
    public void StartLoadingScene(string sceneName)
    {
        if (SceneTransition != TransitionPhase.ExitingScene)
        {
            Debug.Log("Loading scene: " + sceneName);

            if (GameState == State.Play)
            {
                // Cancel player actions
            }

            SceneTransition = TransitionPhase.ExitingScene;
            _nextSceneName = sceneName;
            _fade.StartFadeOut(false);
        }
    }

    /// <summary>
    /// Loads a scene.
    /// </summary>
    /// <param name="sceneName">The scene's name</param>
    private void LoadScene(string sceneName)
    {
        SceneTransition = TransitionPhase.StartingScene;
        SceneManager.LoadScene(sceneName);
    }

    private void InitScene(Scene prev, Scene next)
    {
        if (this != instance)
        {
            return;
        }

        InitScene();
    }

    #endregion Scene Management

    public void WinGame()
    {
        // TODO
    }

    public void LoseGame()
    {
        // TODO
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _fade.StartNextFade();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            _taskController.NextTask();
        }
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= InitScene;
    }
}

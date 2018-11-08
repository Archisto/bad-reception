using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Persistence;

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

    [SerializeField]
    private int _days = 3;

    [SerializeField]
    private float _dayLengthMinutes = 2f;

    [SerializeField]
    private int _tasksPerDay = 3;

    private SaveSystem _saveSystem;

    private bool _updateAtSceneStart;
    private string _nextSceneName;
    private List<string> _levelSceneNames;
    private int _elapsedDays;
    public float elapsedDayTime;
    public bool radioActivated;
    public bool radioDeactivated;

    public State GameState { get; set; }

    public TransitionPhase SceneTransition { get; set; }

    private int programId = 0;
    private List<int> programs;

    public bool SceneChanging
    {
        get
        {
            return SceneTransition == TransitionPhase.ExitingScene ||
                SceneTransition == TransitionPhase.StartingScene;
        }
    }

    public bool DayOver { get; private set; }

    public float DayLengthSeconds
    {
        get
        {
            return _dayLengthMinutes * 60;
        }
    }

    public PlayerTaskController TaskController { get; private set; }

    public UIController UIController { get; private set; }

    public FadeToColor Fade { get; private set; }

    #endregion Fields

    #region Initialization

    // Use this for initialization
    private void Awake ()
    {
        this.programs = new List<int>();
        
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
        InitUI();

        if (UIController.MainMenuActive)
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

        TaskController = FindObjectOfType<PlayerTaskController>();
        TaskController.Init();
        _saveSystem = new SaveSystem(new JSONPersistence(SavePath));
        LoadTasks();
        InitLevels();
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

    public void LevelStartInit()
    {
        if (GameState == State.Play)
        {
            Debug.Log("Level scene initialized");
            StartLevel();
        }
        else
        {
            Debug.Log("Menu scene initialized");
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
        UIController = FindObjectOfType<UIController>();
        if (UIController == null)
        {
            Debug.LogError("A UIController object could not be found in the scene.");
        }
    }

    /// <summary>
    /// Initializes fading to a color.
    /// </summary>
    private void InitFade()
    {
        Fade = FindObjectOfType<FadeToColor>();
        if (Fade != null)
        {
            Fade.Init(UIController.fadeScreen);

            if (SceneTransition == TransitionPhase.StartingScene)
            {
                Fade.StartFadeIn(true);
            }
        }
    }

    #endregion Initialization

    #region Updating

    // Update is called once per frame
    private void Update()
    {
        if (SceneTransition == TransitionPhase.ExitingScene
                && Fade.FadedOut)
        {
            LoadScene(_nextSceneName);
        }
        else if (SceneTransition == TransitionPhase.ResetingScene
                 && Fade.FadedOut)
        {
            SceneTransition = TransitionPhase.None;
            ResetScene();
        }
        else if (SceneTransition == TransitionPhase.None &&
                 GameState == State.Play)
        {
            UpdatePlayState();
        }

        //if (_updateAtSceneStart)
        //{
        //    LevelStartInit();
        //}
    }

    private void UpdatePlayState()
    {
        if (!radioActivated && RadioManager.Running)
        {
            Debug.Log("radio Activated");
            radioActivated = true;
        }
        else if (radioActivated && !radioDeactivated && !RadioManager.Running)
        {
            Debug.Log("radio Deactivated");
            radioDeactivated = true;
        }

        if (radioDeactivated)
        {
            elapsedDayTime = DayLengthSeconds;
        }

        if (!DayOver && (RadioManager.Running || radioDeactivated))
        {
            Debug.Log("Time runs");
            UpdateDayTime();
        }
    }

    private void UpdateDayTime()
    {
        elapsedDayTime += Time.deltaTime;
        if (elapsedDayTime >= DayLengthSeconds)
        {
            Debug.Log("DAY OVER - Tasks begin");
            DayOver = true;
            RadioManager.Running = false;
            RadioManager.allowStart = false;
            TaskController.taskPanel.Activate(true);
            TaskController.ActivateAnswerPhase(true);
        }
    }

    #endregion Updating

    #region Persistence

    /// <summary>
    /// The file path where the game is saved. On Windows, points to
    /// %userprofile%\AppData\LocalLow\<companyname>\<productname>\<savefile>.
    /// </summary>
    public string SavePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "tasks");
        }
    }

    /// <summary>
    /// Gets data from the game and stores it to a data object.
    /// </summary>
    public void SaveGame()
    {
        GameData data = new GameData();

        foreach (PlayerTask task in TaskController.tasks)
        {
            data.PlayerTasks.Add(task);
        }

        //foreach (PlayerTask task in _taskController.tasks)
        //{
        //    data.TaskQuestions.Add(task.question);
        //    data.TaskAnswers.Add(task.answers);
        //}

        //foreach (PlayerTask task in data.Tasks)
        //{
        //    Debug.Log(task.question);
        //}

        _saveSystem.Save(data);
        Debug.Log(string.Format("Game saved"));
    }

    /// <summary>
    /// Loads saved data.
    /// </summary>
    /// <returns>Loaded game data</returns>
    public GameData LoadTasks()
    {
        GameData data = _saveSystem.LoadFromResources("Tasks");
        //GameData data = _saveSystem.Load();

        if (data == null)
        {
            Debug.LogWarning("Save data not loaded.");
            return null;
        }

        List<PlayerTask> tasks = new List<PlayerTask>();
        for(int i = 0; i < 4; i++)
        {
            var d = Resources.Load<TextAsset>("news_" + i);
            var j = JsonUtility.FromJson<GameData>(d.ToString());
            for(int k = 0; k < j.PlayerTasks.Count; k++)
            {
                j.PlayerTasks[k].id = i;
                Debug.Log("task " + j.PlayerTasks[k].Question + " to id " + i);
                tasks.Add(j.PlayerTasks[k]);
                Debug.Log(j.PlayerTasks[k].id);
            }
        }
        /*
        foreach (PlayerTask task in data.PlayerTasks)
        {
            tasks.Add(task);
        }*/
        //for (int i = 0; i < data.TaskQuestions.Count; i++)
        //{
        //    PlayerTask task = new PlayerTask(data.TaskQuestions[i]);
        //    task.SetAnswers(data.TaskCorrectAnswers[i], data.TaskAnswers[i]);
        //    tasks.Add(task);
        //}
        TaskController.SetTasks(tasks);

        return data;
    }

    #endregion Persistence

    #region Scene Management

    /// <summary>
    /// Starts reseting the level with a fade-out.
    /// </summary>
    public void StartSceneReset()
    {
        SceneTransition = TransitionPhase.ResetingScene;
        Fade.StartFadeOut(false);
        Debug.Log("Restarting game");
    }

    /// <summary>
    /// Resets the current scene.
    /// </summary>
    public void ResetScene()
    {
        Fade.StartFadeIn(true);
        ResetGame();
        StartLevel();
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
    /// Testing. Loads a debug level.
    /// </summary>
    public void LoadDebugScene(string sceneName)
    {
        StartLoadingScene(sceneName);
        StartLevel();
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
            Fade.StartFadeOut(false);
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

    public void StartLevel()
    {
        radioActivated = false;
        radioDeactivated = false;
        GameState = State.Play;

        if(programs.Count == 0)
        {
            var t = new List<int>();
            t.Add(0);
            t.Add(1);
            t.Add(2);
            t.Add(3);

            this.programs = new List<int>();
            for(int i = 0; i < 4; i++)
            {
                int rnd = (int)Mathf.Floor(t.Count*Random.value);
                programs.Add(t[rnd]);
                t.RemoveAt(rnd);
            }
        }
        this.programId = programs[0];
        programs.RemoveAt(0);
        TaskController.ChooseRandomTasks(_tasksPerDay, false, this.programId);
        TaskController.StartFirstTask();
        RadioManager.allowStart = true;
        Debug.Log("Day begins");
    }

    public void EndLevel()
    {
        Debug.Log("All tasks completed");
        _elapsedDays++;
        if (_elapsedDays == _days)
        {
            Debug.Log("GAME COMPLETED");
        }
        else
        {
            ResetLevel();
            StartLevel();
        }
    }

    public void ResetLevel()
    {
        Debug.Log("Reseting day");
        DayOver = false;
        elapsedDayTime = 0f;
        TaskController.ActivateAnswerPhase(false);
    }

    public void ResetGame()
    {
        ResetLevel();
        _elapsedDays = 0;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= InitScene;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerTaskController : MonoBehaviour
{
    public int startTaskNum;
    public PlayerTaskPanel taskPanel;
    public List<PlayerTask> tasks;
    public List<PlayerTask> chosenTasks;

    
    private int _currentTaskNum;
    private bool _answerPhaseActive;

    [Header("DEBUG")]
    public int taskTotal;
    public int[] chosenTaskNumbers;

    private int _nextRandomTaskIndex;

    public PlayerTask CurrentTask
    {
        get {
            if (_currentTaskNum >= 0 && _currentTaskNum < chosenTasks.Count)
                return chosenTasks[_currentTaskNum];
            else
                return null;
        }
    }

    public void Init()
    {
        taskPanel.Init();
        _currentTaskNum = startTaskNum;
        chosenTasks = new List<PlayerTask>();
        taskPanel.Activate(false);
    }

    private void CreateDebugTasks()
    {
        tasks = new List<PlayerTask>();

        tasks.Add(new PlayerTask("What?"));
        tasks.Add(new PlayerTask("When?"));
        tasks.Add(new PlayerTask("Who?"));
        tasks.Add(new PlayerTask("How?"));

        tasks[0].SetAnswers(0,
            "This", "That");
        tasks[1].SetAnswers(0,
            "Today", "Yesterday", "Tomorrow");
        tasks[2].SetAnswers(0,
            "Me", "You", "Her", "Him");
        tasks[3].SetAnswers(0,
            "Easily", "Loudly", "Reasonably", "Smartly", "Quickly");
    }

    public void SetTasks(List<PlayerTask> tasks)
    {
        this.tasks = tasks;
        taskPanel.UpdateTask(this.tasks[_currentTaskNum]);
        taskTotal = this.tasks.Count;
    }

    public void ChooseRandomTasks(int taskCount, bool forceShuffle, int programId)
    {
        if (chosenTasks.Count == 0 || forceShuffle)
        {
            ShuffleTasks();
            _nextRandomTaskIndex = 0;
        }

        // Debug
        Debug.Log("taskCount: " + taskCount);
        chosenTaskNumbers = new int[taskCount];

        chosenTasks.Clear();
        var temp = new List<PlayerTask>();
        foreach(PlayerTask tsk in this.tasks)
        {
            Debug.Log("task qq " + tsk.question + " id " + tsk.id);
            if(tsk.id == programId)
            {
                temp.Add(tsk);
            }
        }
        Debug.Log("use program " + programId + " tasks " + temp.Count);

        for (int i = 0; i < (int)Mathf.Min(temp.Count,taskCount); i++)
        {
            var rnd =(int) Mathf.Floor( Random.value * temp.Count);
            chosenTasks.Add(temp[rnd]);
            temp.RemoveAt(rnd);
        }

        /*
        int taskIndex = _nextRandomTaskIndex;
        for (int i = 0; i < taskCount; i++)
        {
            if (taskIndex >= tasks.Count)
            {
                taskIndex = 0;
            }

            if (i < tasks.Count)
            {
                chosenTasks.Add(tasks[taskIndex]);
                chosenTaskNumbers[i] = taskIndex;
            }
            else
            {
                Debug.LogError("Cannot choose task; no unchosen tasks left.");
            }

            taskIndex++;
            _nextRandomTaskIndex = taskIndex;
        }*/
    }

    private void ShuffleTasks()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            int randInt = Random.Range(0, tasks.Count);
            PlayerTask temp = tasks[i];
            tasks[i] = tasks[randInt];
            tasks[randInt] = temp;
        }
    }

    public void SetTasks(PlayerTask[] tasks, int programId)
    {
        
        this.tasks = new List<PlayerTask>();
        foreach (PlayerTask item in tasks)
        {
            this.tasks.Add(item);
        }
    }

    public AudioClip GetAudioClip(string audioClipTag)
    {
        // TODO
        return null;
    }

    public void UpdateTaskUI()
    {
        taskPanel.UpdateTask(chosenTasks[_currentTaskNum]);
    }

    public void StartFirstTask()
    {
        if (chosenTasks == null || chosenTasks.Count == 0)
        {
            Debug.LogError("No chosen tasks.");
            return;
        }

        _currentTaskNum = 0;
        taskPanel.Activate(true);
        UpdateTaskUI();
        taskPanel.UpdateIntroTasks(chosenTasks);
    }

    public void StartNextTask()
    {
        if (chosenTasks == null || chosenTasks.Count == 0)
        {
            Debug.LogError("No chosen tasks.");
            return;
        }

        Debug.Log("Task completed");

        if (_currentTaskNum < chosenTasks.Count - 1)
        {
            _currentTaskNum++;
        }
        else
        {
            ActivateAnswerPhase(false);
            GameManager.Instance.EndLevel();
            //_currentTaskNum = 0;
        }

        UpdateTaskUI();
    }

    public void ActivateAnswerPhase(bool activate)
    {
        taskPanel.ActivateAnswerPanel(activate);
        _answerPhaseActive = activate;
    }

    public void Answer(int answerNum)
    {
        if (_answerPhaseActive)
        {
            if (CurrentTask.IsValidAnswer(answerNum))
            {
                bool correct = CurrentTask.CheckAnswer(answerNum);
                Debug.Log("Answered \"" + CurrentTask.GetAnswer(answerNum) +
                    "\": " + (correct ? "Correct!" : "Wrong"));

                if (correct)
                {
                    StartNextTask();
                }
            }
            else
            {
                Debug.LogWarning("Not an answer (" + answerNum + ")");
            }
        }
        else
        {
            Debug.LogWarning("Can't answer right now");
        }
    }
}

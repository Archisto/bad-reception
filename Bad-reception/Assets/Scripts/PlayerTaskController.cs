using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTaskController : MonoBehaviour
{
    public int startTaskNum;
    public PlayerTaskPanel taskPanel;

    private List<PlayerTask> _tasks;
    private int _currentTask;

    // Use this for initialization
    private void Start ()
    {
        taskPanel.Init();
        _currentTask = startTaskNum;
        InitTasks();
        taskPanel.UpdateQuestion(_tasks[_currentTask].question);
    }

    private void InitTasks()
    {
        _tasks = new List<PlayerTask>();
        _tasks.Add(new PlayerTask("What?"));
        _tasks.Add(new PlayerTask("When?"));
        _tasks.Add(new PlayerTask("Who?"));
        _tasks.Add(new PlayerTask("How?"));
    }

    public void NextTask()
    {
        if (_currentTask < _tasks.Count - 1)
        {
            _currentTask++;
        }
        else
        {
            _currentTask = 0;
        }

        taskPanel.UpdateQuestion(_tasks[_currentTask].question);
    }
}

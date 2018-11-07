using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTaskController : MonoBehaviour
{
    public int startTaskNum;
    public PlayerTaskPanel taskPanel;

    public List<PlayerTask> tasks;

    private int _currentTask;

    public void Init()
    {
        taskPanel.Init();
        _currentTask = startTaskNum;
        //InitTasks();
        //taskPanel.UpdateTask(tasks[_currentTask]);
    }

    private void InitTasks()
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
        taskPanel.UpdateTask(this.tasks[_currentTask]);
    }

    public void SetTasks(PlayerTask[] tasks)
    {
        this.tasks = new List<PlayerTask>();
        foreach (PlayerTask item in tasks)
        {
            this.tasks.Add(item);
        }

        taskPanel.UpdateTask(this.tasks[_currentTask]);
    }

    public void NextTask()
    {
        if (tasks == null || tasks.Count == 0)
        {
            Debug.LogError("No tasks.");
            return;
        }

        if (_currentTask < tasks.Count - 1)
        {
            _currentTask++;
        }
        else
        {
            _currentTask = 0;
        }

        taskPanel.UpdateTask(tasks[_currentTask]);
    }
}

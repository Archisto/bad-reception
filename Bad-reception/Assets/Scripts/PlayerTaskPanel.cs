using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTaskPanel : MonoBehaviour
{
    public string question;
    public GameObject answerPanel;

    private PlayerTask _task;
    private Text _questionText;
    private Text[] _answerTexts;

    public void Init()
    {
        _questionText = GetComponentInChildren<Text>();
        _answerTexts = answerPanel.GetComponentsInChildren<Text>();
        ActivateAnswerPanel(false);
    }

    public void UpdateTask(PlayerTask task)
    {
        _task = task;
        _questionText.text = task.question;
        UpdateAnswers();
    }

    private void UpdateAnswers()
    {
        for (int i = 0; i < _answerTexts.Length; i++)
        {
            if (i < _task.answers.Count)
            {
                _answerTexts[i].text = _task.answers[i];
            }
            else
            {
                _answerTexts[i].text = "";
            }

            if (i == _task.correctAnswer)
            {
                _answerTexts[i].color = Color.green;
            }
            else
            {
                _answerTexts[i].color = Color.black;
            }
        }
    }

    public void Activate(bool activate)
    {
        gameObject.SetActive(activate);
    }

    public void ActivateAnswerPanel(bool activate)
    {
        answerPanel.SetActive(activate);
    }
}

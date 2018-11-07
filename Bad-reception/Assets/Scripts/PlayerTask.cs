using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask
{
    public string question;
    public List<string> answers;
    public int correctAnswer = -1;

    public PlayerTask(string question)
    {
        this.question = question;
    }

    public void SetAnswers(int correctAnswer, params string[] answers)
    {
        this.answers = new List<string>();
        foreach (string answer in answers)
        {
            this.answers.Add(answer);
        }

        this.correctAnswer = correctAnswer;
    }

    public void SetAnswers(int correctAnswer, List<string> answers)
    {
        this.answers = answers;
        this.correctAnswer = correctAnswer;
    }

    public bool CheckAnswer(int answer)
    {
        if (correctAnswer < 0)
        {
            Debug.LogError("Correct answer has not been set.");
        }

        return answer == correctAnswer;
    }
}

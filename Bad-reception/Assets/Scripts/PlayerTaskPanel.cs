using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTaskPanel : MonoBehaviour
{
    public string question;
    private Text _text;

    public void Init()
    {
        _text = GetComponentInChildren<Text>();
    }

    public void UpdateQuestion(string question)
    {
        this.question = question;
        _text.text = question;
    }
}

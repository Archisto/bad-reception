using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour
{
    public Text resultText;

    private UIController _ui;

    public void Init(UIController ui)
    {
        _ui = ui;
    }

    public void SetResultText(int score)
    {
        resultText.text = string.Format("Your score: {0} out of {1}.",
            score, GameManager.Instance.TotalTasksToComplete);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("ReturnToMainMenu");
        SetActive(false);
        _ui.mainMenuScreen.SetActive(true);
        GameManager.Instance.ResetGame();
        GameManager.Instance.GameState = GameManager.State.MainMenu;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void Update()
    {
        if (Input.GetButtonDown("answerB") ||
            Input.GetKeyDown(KeyCode.Return))
        {
            ReturnToMainMenu();
        }
    }
}

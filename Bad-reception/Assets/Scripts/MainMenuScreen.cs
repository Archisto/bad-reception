using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void StartGame()
    {
        // TODO
        Debug.Log("StartGame");
        SetActive(false);
        GameManager.Instance.StartLevel();
        RadioManager.allowStart = true;
    }

    public void ToggleDisplayCredits()
    {
        Debug.Log("ToggleDisplayCredits");
        SetActive(!gameObject.activeSelf);
        GameManager.Instance.UIController.ToggleCreditsScreen();
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}

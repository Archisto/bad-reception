using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image fadeScreen;
    public MainMenuScreen mainMenuScreen;
    public GameEndScreen gameEndScreen;
    public GameObject creditsScreen;

    private void Start()
    {
        gameEndScreen.Init(this);
    }

    public bool MainMenuActive
    {
        get
        {
            return mainMenuScreen.gameObject.activeSelf;
        }
    }

    public void ToggleMainMenuScreen()
    {
        mainMenuScreen.SetActive(!MainMenuActive);
    }

    public void ToggleCreditsScreen()
    {
        creditsScreen.SetActive(!creditsScreen.activeSelf);
    }

    public void ActivateCreditsScreen(bool activate)
    {
        creditsScreen.SetActive(activate);
    }
}

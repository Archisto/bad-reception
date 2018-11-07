using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image fadeScreen;
    public GameObject creditsScreen;

    private void Start()
    {
        
    }

    private void Update()
    {
        
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

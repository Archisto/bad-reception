using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image fadeScreen;

    public void ActivatePauseScreen(bool activate)
    {
        Debug.Log("Game should " + (activate ? "" : "not ") + "be paused now");
    }

    private void Update()
    {
        
    }

    public void ResetUI()
    {

    }
}

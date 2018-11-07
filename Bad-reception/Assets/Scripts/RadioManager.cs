using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour {

    public float minFrequency = 148.5f;
    public float maxFrequency = 283.5f;

    private float _frequency = 148.0f;
    public float frequency
    {
        get{ return _frequency; }
        set{ _frequency = Mathf.Clamp(value, minFrequency, maxFrequency); }
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

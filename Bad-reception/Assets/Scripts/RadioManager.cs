using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour {

    public float minFrequency = 148.5f;
    public float maxFrequency = 283.5f;
    
    private float _volume = 1f;
    public float volume
    {
        get { return _volume; }
        set { _volume = Mathf.Clamp(value, 0f, 1f); updateWWValues(); }
    }

    private int _channelA = 0;
    public int channelA
    {
        get { return _channelA; }
        set { _channelA = value; updateWWValues(); }
    }

    private float _frequency = 148.0f;
    public float frequency
    {
        get{ return _frequency; }
        set{ _frequency = Mathf.Clamp(value, minFrequency, maxFrequency); updateWWValues(); }
    }

    private float _tuning = 0f;
    public float tuning
    {
        get { return _tuning; }
        set { _tuning = Mathf.Clamp(value, 0, 1f); updateWWValues(); }
    }

    private float _noise = 0f;
    public float noise
    {
        get { return _noise; }
        set { _noise = Mathf.Clamp(value, 0, 1f); updateWWValues(); }
    }

    private float _distort = 0f;
    public float distort
    {
        get { return _distort; }
        set { _distort = Mathf.Clamp(value, 0, 1f); updateWWValues(); }
    }
    
    // Use this for initialization
    void Start () {
        AkSoundEngine.PostEvent("PlayRadio", gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void updateWWValues()
    {
        AkSoundEngine.SetRTPCValue("ChannelA", _channelA);
        //AkSoundEngine.SetRTPCValue("ChannelB", _channelB);
        AkSoundEngine.SetRTPCValue("Tuning", _tuning);
        AkSoundEngine.SetRTPCValue("Noise", _noise);
        AkSoundEngine.SetRTPCValue("Distort", _distort);
        AkSoundEngine.SetRTPCValue("Volume", _tuning);
        AkSoundEngine.SetRTPCValue("Program", _tuning);
        
    }
}

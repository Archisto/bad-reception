using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour {
    
    public static float minFrequency = 148.5f;
    public static float maxFrequency = 283.5f;

    public Channels channels;
    
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
        set{ _frequency = Mathf.Clamp(value, RadioManager.minFrequency, RadioManager.maxFrequency); updateWWValues(); }
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

    private float _angle = 0f;
    public float angle
    {
        get { return _angle; }
        set { _angle = value; updateWWValues(); }
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
        //Calculate nearest channel & tuning & noise
        Channel nearest = null;
        float nearestDistance = 999f;

        foreach (Channel chnl in channels.channels)
        {
            //Create channel indicators
            float distance = Mathf.Abs(chnl.frequency - this.frequency);
            if(distance < nearestDistance)
            {
                nearest = chnl;
                nearestDistance = distance;
            }
        }
        _tuning = Mathf.Clamp( (5f-nearestDistance)/5f, 0f,1f);
        _channelA = nearest.channelId;

        var angle = smallestAngleBetween(nearest.angle, this.angle);
        this._noise = Mathf.Min(1f,Mathf.Abs(angle));

        AkSoundEngine.SetRTPCValue("ChannelA", _channelA);
        //AkSoundEngine.SetRTPCValue("ChannelB", _channelB);
        AkSoundEngine.SetRTPCValue("Tuning", _tuning);
        AkSoundEngine.SetRTPCValue("Noise", _noise);
        AkSoundEngine.SetRTPCValue("Distort", _distort);
        AkSoundEngine.SetRTPCValue("Volume", _tuning);
        AkSoundEngine.SetRTPCValue("Program", _tuning);


       // Debug.Log("Update: channel " + _channelA + ", tuning " + _tuning + " noise " + _noise);
    }

    float smallestAngleBetween(float a1, float a2)
    {
        float d = a1 - a2;
        float max = Mathf.PI * 2;
        float mp = Mathf.Floor((d - -Mathf.PI) / max);
        return d - (mp * max);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour {
    
    public static bool Running = false;
    public static bool allowStart = true;

    public static float minFrequency = 148.5f;
    public static float maxFrequency = 283.5f;

    public Channels channels;
    public GameObject distortObject;

    public GameObject switchon;
    public GameObject switchoff;

    private float _volume = 1f;
    public float volume
    {
        get { return _volume; }
        set { _volume = Mathf.Clamp(value, 0f, 1f);  }
    }

    private int _channelA = 0;
    public int channelA
    {
        get { return _channelA; }
        set { _channelA = value;  }
    }

    private float _frequency = 148.0f;
    public float frequency
    {
        get{ return _frequency; }
        set{ _frequency = Mathf.Clamp(value, RadioManager.minFrequency, RadioManager.maxFrequency); }
    }

    private float _tuning = 0f;
    public float tuning
    {
        get { return _tuning; }
        set { _tuning = Mathf.Clamp(value, 0, 1f);}
    }

    private float _noise = 0f;
    public float noise
    {
        get { return _noise; }
        set { _noise = Mathf.Clamp(value, 0, 1f);  }
    }

    private float _distort = 0f;
    public float distort
    {
        get { return _distort; }
        set { _distort = Mathf.Clamp(value, 0, 1f); }
    }

    private float _angle = 0f;
    public float angle
    {
        get { return _angle; }
        set { _angle = value;  }
    }

    public float distortTarget = 0.1f;
    public float userDistortLevel = 0.5f;

    public int program;

    // Use this for initialization
    void Start () {

        //AkSoundEngine.PostEvent("PlayRadio", gameObject);
        AkSoundEngine.PostEvent("MuteRadio", gameObject);
        //AkSoundEngine.PostEvent("StopRadio", gameObject);
    }

    private void Awake()
    {
        program = (int)Mathf.Floor(Random.value * 4f);
        distortTarget = Random.value;
        this.frequency = Random.value * (RadioManager.maxFrequency - RadioManager.minFrequency) + RadioManager.minFrequency;
    }

    // Update is called once per frame
    void Update () {
        if(!RadioManager.Running && Input.GetButtonDown("start") && allowStart)
        {
            RadioManager.Running = true;
            AkSoundEngine.PostEvent("PlayRadio", gameObject);
        }
        else if(RadioManager.Running && Input.GetButtonDown("start"))
        {
            RadioManager.Running = false;
            AkSoundEngine.PostEvent("StopRadio", gameObject);
        }

        this.switchon.SetActive(RadioManager.Running);
        this.switchoff.SetActive(!RadioManager.Running);

        distortTarget += (Random.value-0.5f)*0.6f*Time.deltaTime + Mathf.Sin(Time.time*0.6f)*0.000f;
        distortTarget = Mathf.Clamp(distortTarget, 0f, 1f);
        userDistortLevel = Mathf.Clamp(userDistortLevel, -1f, 2f);
        if(!RadioManager.Running)
        {
            distortTarget = 0.5f;
            userDistortLevel = 0.5f;
        }
        updateWWValues();
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
        _tuning = Mathf.Clamp( (10f-nearestDistance)/10f, 0f,1f);
        _tuning = getPowIn(0.8f, _tuning);
        _channelA = nearest.channelId;

        var angle = smallestAngleBetween(nearest.angle, this.angle);
        this._noise = Mathf.Min(1f,Mathf.Abs( Mathf.Sin(Time.time*0.25f)+Mathf.Sin(Time.time*0.1f)))*(1.0f-_tuning*0.3f);

        distortObject.transform.localRotation = Quaternion.Euler(0f, 0f, (userDistortLevel - distortTarget)*60f);
        _distort = Mathf.Clamp( getPowIn(2.05f,  Mathf.Abs(userDistortLevel - distortTarget)) , 0f, 1f)+0.5f;

        

        AkSoundEngine.SetRTPCValue("ChannelA", _channelA);
        //AkSoundEngine.SetRTPCValue("ChannelB", _channelB);
        AkSoundEngine.SetRTPCValue("Tuning", _tuning);
        AkSoundEngine.SetRTPCValue("Noise", _noise);
        AkSoundEngine.SetRTPCValue("Distort", _distort);
        AkSoundEngine.SetRTPCValue("Volume", _tuning);
        AkSoundEngine.SetRTPCValue("Program", program);


  //      Debug.Log("Update: channel " + _channelA + ", tuning " + _tuning + " noise " + _noise +" "+ "distort "+  _distort + " target " + distortTarget + " user " + userDistortLevel);
    }



    float getPowIn(float pow, float t)
    {
        return Mathf.Pow(t, pow);
    }


    float smallestAngleBetween(float a1, float a2)
    {
        float d = a1 - a2;
        float max = Mathf.PI * 2;
        float mp = Mathf.Floor((d - -Mathf.PI) / max);
        return d - (mp * max);
    }
}

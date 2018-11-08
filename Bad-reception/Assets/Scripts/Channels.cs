using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channels : MonoBehaviour {
    public GameObject channelIndicator;

    public bool showChannels = true;
    public List<Channel> channels;

	// Use this for initialization
	void Start () {
        this.channels = new List<Channel>();
        this.channels.Add(new Channel(0, 210, 0, "Tampereen Radio"));
        this.channels.Add(new Channel(1, 170, 70, "Tukholma"));
        this.channels.Add(new Channel(1, 240, 140, "Helsinki"));
        this.channels.Add(new Channel(1, 280, 270, "Ruotsi"));
        this.channels.Add(new Channel(1, 150, 300, "Lahti"));

        if(showChannels)
        {
            foreach(Channel chnl in channels)
            {
                //Create channel indicators
                var go = Instantiate(channelIndicator, this.transform);
                chnl.channelIndicator = go;
                chnl.updateChannelIndicatorPosition();
            }
        }
        randomizeChannels();

    }

    void randomizeChannels()
    {
        List<float> availableFrequencies = new List<float>();
        for(float i = RadioManager.minFrequency; i < RadioManager.maxFrequency; i+=20)
        {
            var rnd = (Random.value-0.5f)*5f + i;
            var f = Mathf.Clamp(rnd, RadioManager.minFrequency, RadioManager.maxFrequency);
            Debug.Log(f + " vs " + i);
            availableFrequencies.Add(f);
        }

        //Set the channel frequencies
        foreach(Channel chnl in channels)
        {
            int rnd = (int)Mathf.Floor(Random.value * availableFrequencies.Count);
            chnl.frequency = availableFrequencies[rnd];
            availableFrequencies.RemoveAt(rnd);
            Debug.Log(chnl.frequency);
        }
    }

    // Update is called once per frame
    void Update () {
		foreach(Channel chnl in channels)
        {
            chnl.randomizeChannelPosition();
            chnl.updateChannelIndicatorPosition();
        }
	}
}

/**
 * Single channel information
 */
public class Channel
{
    public GameObject channelIndicator;
    public string name;
    public int channelId;

    private float _baseFrequency;
    private float _frequency;
    public float frequency {
        get{ return _frequency; }
        set{
            this._baseFrequency = value;
            this._frequency = value;
            this.updateChannelIndicatorPosition();
        }
    }

    private float phase = 0f;

    public float angle;

    public Channel(int id, float frequency, float angle, string name)
    {
        this.frequency = frequency;
        this.name = name;
        this.channelId = id;
        this.angle = angle;
    }

    public void randomizeChannelPosition()
    {
        phase += (float)Mathf.Sin(_baseFrequency + Time.time*0.15f)*0.015f + (float)Mathf.Cos( _baseFrequency/3.0f + Time.time*0.073f)*0.007f;
        phase = Mathf.Clamp(phase, -7f, 7f);
        this._frequency =Mathf.Clamp(_baseFrequency + phase, RadioManager.minFrequency, RadioManager.maxFrequency);
    }

    public void updateChannelIndicatorPosition()
    {
        if(channelIndicator)
        {
            var position = -1.5f + (this._frequency - RadioManager.minFrequency) / (RadioManager.maxFrequency - RadioManager.minFrequency) * 3f;
            this.channelIndicator.transform.localPosition = new Vector3(position, 0f, 0.1f);
        }
    }
}
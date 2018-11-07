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
    }

    // Update is called once per frame
    void Update () {
		
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
    private float _frequency;
    public float frequency {
        get{ return _frequency; }
        set{
            this._frequency = value;
        }
    }

    public float angle;

    public Channel(int id, float frequency, float angle, string name)
    {
        this.frequency = frequency;
        this.name = name;
        this.channelId = id;
        this.angle = angle;
    }

    public void updateChannelIndicatorPosition()
    {
        if(channelIndicator)
        {
            var position = -1.5f + (this._frequency - RadioManager.minFrequency) / (RadioManager.maxFrequency - RadioManager.minFrequency) * 3f;
            this.channelIndicator.transform.localPosition = new Vector3(position, 0f, -0.1f);
        }
    }
}
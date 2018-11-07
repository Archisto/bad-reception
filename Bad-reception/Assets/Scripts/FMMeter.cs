using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMMeter : MonoBehaviour {

    public GameObject radio;
    private RadioManager rm;

    private Transform meter;

	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
        this.rm = radio.GetComponent<RadioManager>();
        this.meter = this.transform.Find("meter");
    }

    // Update is called once per frame
    void Update () {
        var position = -1.5f + (rm.frequency-rm.minFrequency) / (rm.maxFrequency - rm.minFrequency) * 3f;
        this.meter.localPosition = new Vector3(position, 0f, 0f);
	}
}

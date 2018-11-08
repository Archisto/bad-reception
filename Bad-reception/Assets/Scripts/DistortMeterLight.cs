using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortMeterLight : MonoBehaviour {

    private Light spotlight;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        spotlight = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
        spotlight.intensity += (Random.value-0.5f) * 1.35f;
        spotlight.intensity = Mathf.Clamp(spotlight.intensity, 0.2f, 5f);
	}
}

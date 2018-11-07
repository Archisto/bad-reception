using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public GameObject radio;

    private RadioManager rm;

    public float leftAngle;
    public float rightAngle;

    public GameObject leftTemp;
    public GameObject rightTemp;

    private float? previousLeftAngle;
    private float? previousRightAngle;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        this.rm = this.radio.GetComponent<RadioManager>();
    }

    // Update is called once per frame
    void Update () {
        var leftValX = Input.GetAxis("left axis x");
        var leftValY = Input.GetAxis("left axis y");
        var rightValX = Input.GetAxis("right axis x");
        var rightValY = Input.GetAxis("right axis y");
        
        var leftAngle = Mathf.Atan2(leftValY, leftValX);
        var rightAngle = Mathf.Atan2(rightValY, rightValX);

        if (Mathf.Abs(leftValX) + Mathf.Abs(leftValY) < 0.9)
        {
            //Reset previous val
            previousLeftAngle = null;
        }

        if(Mathf.Abs(rightValX) + Mathf.Abs(rightValY) < 0.9)
        {
            previousRightAngle = null;
        }

        if(previousLeftAngle != null)
        {
            var speed = Input.GetAxis("left trigger") > 0 ? 0.25f : 0.05f;
            var d = smallestAngleBetween(previousLeftAngle.Value, leftAngle) * speed;
            this.leftAngle += d * 180 / Mathf.PI;

            rm.frequency -= d*5f;
        }

        if(previousRightAngle != null)
        {
            var speed = Input.GetAxis("right trigger") > 0 ? 0.25f : 0.05f;
            var d = smallestAngleBetween(previousRightAngle.Value, rightAngle) * speed;
            this.rightAngle += d * 180 / Mathf.PI;
            rm.frequency += d;
        }

        leftTemp.transform.rotation = Quaternion.Euler(new Vector3(0,0,this.leftAngle));
        rightTemp.transform.rotation = Quaternion.Euler(new Vector3(0,0,this.rightAngle));

        previousLeftAngle = leftAngle;
        previousRightAngle = rightAngle;
    }

    float smallestAngleBetween(float a1, float a2)
    {
        float d = a1 - a2;
        float max = Mathf.PI * 2;
        float mp = Mathf.Floor((d - -Mathf.PI) / max);
        return d - (mp * max);
    }

}

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

    private float? previousMouseX1;
    private float? previousMouseX2;

    // Use this for initialization
    void Start () {
		
	}

    private void Awake()
    {
        this.rm = this.radio.GetComponent<RadioManager>();
    }

    // Update is called once per frame
    void Update () {
        //Read xbox controller.
        var leftValX = Input.GetAxis("right axis x");
        var leftValY = Input.GetAxis("right axis y");
        var rightValX = Input.GetAxis("left axis x");
        var rightValY = Input.GetAxis("left axis y");
        
        var leftAngle = Mathf.Atan2(leftValY, leftValX);
        var rightAngle = Mathf.Atan2(rightValY, rightValX);
        
        //Read mouse as backup - left thumb
        if(Input.GetMouseButtonDown(0))
        {
            previousMouseX1 = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(0))
        {
            leftValX = 1f;
            if (previousLeftAngle != null && previousMouseX1 != null)
                leftAngle = previousLeftAngle.Value + (previousMouseX1.Value- Input.mousePosition.x)/150f;
        }
        else
        {
            previousMouseX1 = null;
        }

        //Read mouse as backup - right thumb
        if (Input.GetMouseButtonDown(2))
        {
            previousMouseX2 = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(2))
        {
            leftValX = 1f;
            if (previousLeftAngle != null && previousMouseX2 != null)
                leftAngle = previousLeftAngle.Value + (previousMouseX2.Value - Input.mousePosition.x) / 150f;
        }
        else
        {
            previousMouseX2 = null;
        }

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
            var speed = Input.GetAxis("left trigger") > 0 ? 0.5f : 0.05f;
            var d = smallestAngleBetween(previousLeftAngle.Value, leftAngle) * speed;
            this.leftAngle -= d * 180 / Mathf.PI;

            rm.frequency += d*20f;
        }

        if(previousRightAngle != null)
        {
            var speed = Input.GetAxis("right trigger") > 0 ? 1.6f : 0.2f;
            var d = smallestAngleBetween(previousRightAngle.Value, rightAngle) * speed;
            this.rightAngle -= d * 180 / Mathf.PI;
            rm.userDistortLevel -= d * 0.1f;
        }
        
        leftTemp.transform.localRotation = Quaternion.Euler(new Vector3(this.leftAngle,-90f, -90f));
        rightTemp.transform.localRotation = Quaternion.Euler(new Vector3(0,0,this.rightAngle));

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

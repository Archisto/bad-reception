using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateMover : MonoBehaviour {

    public Vector3 moveVector;
    public Vector3 rotVector;

    private Vector3 origpos;
    private Quaternion origRot;
    // Use this for initialization

    private float movephase = 0f;
    private float rotphase = 0f;

	void Start () {
		
	}

    private void Awake()
    {
        origpos = this.transform.position;
        origRot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update () {

        movephase = Time.time * 0.03f;
        rotphase = Time.time * 0.015f;

        this.transform.position = origpos + moveVector * Mathf.Sin(movephase);
        this.transform.rotation = Quaternion.Euler(origRot.eulerAngles+ rotVector* Mathf.Cos(rotphase));
	}
}

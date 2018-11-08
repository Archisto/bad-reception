using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

    public GameObject minute;
    public GameObject hour;

    public float hours = 5f;
    public float minutes = 37f;
    

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (!RadioManager.Running) return;
        float addm = GameManager.Instance.elapsedDayTime * 1f ;
        float addh = addm / 60f;

        var rh = (hours + addh) / 12 * 360f;
        hour.transform.localRotation = Quaternion.Euler(0, rh,0);

        var mh = ((addm+minutes) % 60f) / 60f*360f;
        minute.transform.localRotation = Quaternion.Euler(0, mh, 0);
	}
}

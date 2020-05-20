using UnityEngine;
using System.Collections;

public class Rotate: MonoBehaviour {

	public GameObject target, cam, lightSource;
	public float speed = 5.0f;
	private Vector3 pivot,inclination;

	void Start () {
		pivot = target.transform.position;
		cam.transform.LookAt(pivot);
        lightSource.transform.LookAt(pivot);
        if (speed > 7)
            speed = 7;
	}

	void Update () {
        cam.transform.RotateAround(pivot,new Vector3(0.0f,1.0f,0.0f),30*Time.deltaTime*speed);
        lightSource.transform.RotateAround(pivot, new Vector3(0.0f, 1.0f, 0.0f), 30 * Time.deltaTime * speed);
        inclination = cam.transform.eulerAngles;
		inclination.x = 30.0f;
		cam.transform.eulerAngles = inclination;
        lightSource.transform.eulerAngles = inclination;
    }
}

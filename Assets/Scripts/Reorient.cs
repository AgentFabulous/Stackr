using UnityEngine;
using System.Collections;

public class Reorient : MonoBehaviour {

    public GameObject cam;
    private Vector3 camPos;
    public bool decrease=false,orientComplete=false;
    private float timeRedn=0.78f;
    private int lim;
    void OnEnable () {
        
        Rotate gs = cam.GetComponent<Rotate>();
        if (gs.speed == 5)
            timeRedn = 0.79f;
        else if (gs.speed == 6)
            timeRedn = 0.55f;
        else if (gs.speed == 7)
            timeRedn = 0.415f;
        lim = (int)gs.speed;
    }
    void OnDisable()
    {
        decrease = false;
    }
    void Update() {

        camPos = cam.transform.position;
        if (((int)(camPos.z)==13) && (camPos.x<0))
          if (decrease == false)
            {
                StartCoroutine(Wait());
                decrease = true;

            }
    }
    IEnumerator Wait()
    {
        Rotate gs = cam.GetComponent<Rotate>();
            for (int i = 0; i < lim; i++)
            {
                yield return new WaitForSeconds(timeRedn);
                gs.speed -= 1;
            }
        gs.speed = 0;
        orientComplete = true;
    }
}

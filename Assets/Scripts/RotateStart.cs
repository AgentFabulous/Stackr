using UnityEngine;
using System.Collections;

public class RotateStart : MonoBehaviour {

    public GameObject cam;
    public int lim;
    void OnEnable()
    {
        StartCoroutine(Wait());
    } 
    IEnumerator Wait()
    {
            Rotate gs = cam.GetComponent<Rotate>();
            for (int i = 0; i < lim; i++)
            {
                yield return new WaitForSeconds(0.5f);
                gs.speed += 1;
                if (gs.speed > lim)
                    gs.speed = lim;
             }
            
            
    }
}

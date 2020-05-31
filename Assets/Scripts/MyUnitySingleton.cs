using UnityEngine;
using System.Collections;

public class MyUnitySingleton : MonoBehaviour {

    private static MyUnitySingleton instance = null;
    void Awake()
    {
        
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }
}

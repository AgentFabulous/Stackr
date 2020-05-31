using UnityEngine;
using System.Collections;

public class UIDestroy : MonoBehaviour {
    public GameObject HeaderField,TestButton,MainCanvas;
    void OnEnable() {
        StartCoroutine(AlphaReduction());
        
	}

    IEnumerator AlphaReduction()
    {
        CanvasRenderer text = HeaderField.GetComponent<CanvasRenderer>();
        CanvasRenderer button = TestButton.GetComponent<CanvasRenderer>();
        float a;
        for (int i = 1; i <= 100; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            a = text.GetAlpha();
            text.SetAlpha(a-=0.01f);
            button.SetAlpha(a -= 0.01f);
        }
        MainCanvas.SetActive(false);

    }
	
	void Update () {
	
	}
}

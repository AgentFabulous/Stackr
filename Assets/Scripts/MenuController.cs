using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    
    void OnEnable()
    {
        LevelFade();
    }
    public void newGame()
    {
        StartCoroutine(loadLevel("Game"));
    }
    IEnumerator loadLevel(string level)
    {
        float fadeTime = GameObject.Find("ScriptHolder").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(level);
    }
    IEnumerator LevelFade()
    {
        float fadeTime = GameObject.Find("ScriptHolder").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
}

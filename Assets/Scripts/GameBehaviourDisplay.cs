using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameBehaviourDisplay : MonoBehaviour {

    //public Members
    public GameObject cam, scriptHolder, pointsField, gameOverText, lightobj;
    public GameObject MasterCube,bg;
    public int speed = 2;
    public Material CubeMat;
    //private Members
    private Vector3 startPosi, targetPosi;
    private GameObject[] cube;
    private float movementSpeed = 1.5f;
    private string side = "left";
    private int tap = 1;
    private bool move = true, gameStarted = false, isRunning = false, gameover = false, animcheck = false, test = false;
    private bool red = false, green = false, blue = false;
    private int tmp;

    //Main Functions
    void Start() {
        cube = new GameObject[5000];
        gameover = false;
        gameStarted = true;
        CreateFirstCube();
        StartCoroutine(Anim());
    }
    IEnumerator LevelFade()
    {
        float fadeTime = GameObject.Find("ScriptHolder").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
    void Update()
    {
        if (!move)
            test = false;
        if (cube[tap] != null)
        {
            if (cube[tap].GetComponent<Renderer>().material.color.r >= 2f)
            { red = true; green = false; blue = false; }
            else if (cube[tap].GetComponent<Renderer>().material.color.g >= 2f)
            { red = false; green = true; blue = false; }
            else if (cube[tap].GetComponent<Renderer>().material.color.b >= 2f)
            { red = false; green = false; blue = true; }
        }
        if (tap >= 0 && gameStarted == true && move == true)
        {
            if (!isRunning)
                StartCoroutine(Anim());
        }
        if ((((cube[tap].transform.position.x<= cube[tap-1].transform.position.x) && side=="left") ||
            ((cube[tap].transform.position.z >= cube[tap - 1].transform.position.z) && side == "right")))
           
        {
            Vector3 tempCubePos = cube[tap].transform.position;
            
            test = true;
            if (side=="left")
                tempCubePos.x=cube[tap - 1].transform.position.x;
            else
                tempCubePos.z = cube[tap - 1].transform.position.z;
            cube[tap].transform.position = tempCubePos;
            if (!gameover)
            {
                switch (side)
                {
                    case "left":
                        side = "right";
                        break;
                    case "right":
                        side = "left";
                        break;
                }
                StartCoroutine(DestroyCube(cube[tap]));
                tap++;
                CreateCube();
                Color cubecol = cube[tap].GetComponent<Renderer>().material.color, bgcol = bg.GetComponent<Image>().color;
                if (red)
                {
                    cubecol += new Color(-0.05f, 0.00f, 0.05f, 0f);
                    bgcol += new Color(-0.0125f, 0.00f, 0.0125f, 0f);
                }
                else if (green)
                {
                    cubecol += new Color(0.05f, -0.05f, 0f, 0f);
                    bgcol += new Color(0.0125f, -0.0125f, 0f, 0f);
                }
                else if (blue)
                {
                    cubecol += new Color(0f, 0.05f, -0.05f, 0f);
                    bgcol += new Color(0f, 0.0125f, -0.0125f, 0f);
                }
                cube[tap].GetComponent<Renderer>().material.color = cubecol;
                bg.GetComponent<Image>().color = bgcol;
                StartCoroutine(MoveCam());
                
            }

            else
            {
                tmp = tap;
                gameStarted=false;
            }
            
        }

        if (gameover && !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                tap++;
            if (tap==tmp+2)
                StartCoroutine(loadLevel("Main Menu"));
        }
    }

    //Assisting Functions
    IEnumerator loadLevel(string level)
    {
        float fadeTime = GameObject.Find("ScriptHolder").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(level);
    }
    void CreateCube()
    {

        if (side == "left")
        {
            float xPos = (cube[tap - 1].transform.position.x) + 4.72f;
            float yPos = cube[tap - 1].transform.position.y + 0.75f;
            float zPos = (cube[tap - 1].transform.position.z);
            startPosi = new Vector3(xPos, yPos, zPos);
            targetPosi = new Vector3(xPos - 9.44f, yPos, zPos);
            cube[tap] = Instantiate(cube[tap - 1], new Vector3(xPos, yPos, zPos), Quaternion.identity) as GameObject;
        }
        else
        {
            float xPos = (cube[tap - 1].transform.position.x);
            float yPos = cube[tap - 1].transform.position.y + 0.75f;
            float zPos = (cube[tap - 1].transform.position.z) - 4.72f;
            startPosi = new Vector3(xPos, yPos, zPos);
            targetPosi = new Vector3(xPos, yPos, zPos + 9.44f);
            cube[tap] = Instantiate(cube[tap - 1], new Vector3(xPos, yPos, zPos), Quaternion.identity) as GameObject;
        }

    }
    void CreateFirstCube()
    {
        //Creates first cube 
        int startcol = Random.Range(0, 4);
        cube[0] = MasterCube;
        bg.GetComponent<Image>().color = new Color(0.5f, 0.25f, 0.25f);
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        switch (startcol)
        {
            case 0:
            case 1:
                cube[0].GetComponent<Renderer>().material.color = go.GetComponent<Renderer>().material.color = new Color(2f, 1f, 1f);
                go.GetComponent<Renderer>().material.color += new Color(-0.05f, 0.00f, 0.05f, 0f);
                bg.GetComponent<Image>().color = new Color(0.5f, 0.25f, 0.25f);
                bg.GetComponent<Image>().color += new Color(-0.0125f, 0.00f, 0.0125f, 0f);
                red = true;
                break;
            case 2:
                cube[0].GetComponent<Renderer>().material.color = go.GetComponent<Renderer>().material.color = new Color(1f, 2f, 1f);
                go.GetComponent<Renderer>().material.color += new Color(0.05f, -0.05f, 0f, 0f);
                bg.GetComponent<Image>().color = new Color(0.25f, 0.5f, 0.25f);
                bg.GetComponent<Image>().color += new Color(0.0125f, -0.0125f, 0f, 0f);
                green = true;
                break;
            case 3:
            case 4:
                cube[0].GetComponent<Renderer>().material.color = go.GetComponent<Renderer>().material.color = new Color(1f, 1f, 2f);
                go.GetComponent<Renderer>().material.color += new Color(0f, 0.05f, -0.05f, 0f);
                bg.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.5f);
                bg.GetComponent<Image>().color += new Color(0f, 0.0125f, -0.0125f, 0f);
                blue = true;
                break;
        }
        go.transform.position = new Vector3(4.50f, -1.845f, 3.61f);
        go.transform.localScale = new Vector3(3.5f, 0.75f, 3.5f);
        cube[1] = go;
        tap = 1;
        startPosi = new Vector3(4.50f, -1.845f, 3.61f);
        targetPosi = new Vector3(-4.94f, -1.845f, 3.61f);
        gameStarted = true;
        animcheck = true;
    }
    IEnumerator Anim()
    {
        isRunning = true;
        while (move)
        {
            if (test)
            {
                move = false;
                yield return null;
                break;
            }
            if (animcheck)
            {
                yield return StartCoroutine(MoveObject(cube[tap].transform, startPosi, targetPosi, movementSpeed));
                yield return StartCoroutine(MoveObject(cube[tap].transform, targetPosi, startPosi, movementSpeed));
            }
            else
                yield return null;

        }
        isRunning = false;
        move = true;
    }
    IEnumerator MoveCam()
    {
        Vector3 temp;
        temp = cam.transform.position;
        temp.y += 0.75f;
        yield return StartCoroutine(ChangeHeight(cam.transform, cam.transform.position.y, temp.y, 0.5f));
        temp = lightobj.transform.position;
        temp.y += 0.75f;
        yield return StartCoroutine(ChangeHeight(lightobj.transform, lightobj.transform.position.y, temp.y, 0.5f));
    }
    IEnumerator ChangeHeight(Transform thisTransform, float startY, float endY, float time)
    {
        if (move == true)
        {
            float i = 0.0f;
            float rate = 1.0f / time;
            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                Vector3 temp = thisTransform.position;
                temp.y = Mathf.Lerp(startY, endY, i);
                thisTransform.position = temp;
                yield return null;
                if (test)
                {
                    move = false;
                    break;

                }
            }
        }
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        if (move == true)
        {
            float i = 0.0f;
            float rate = 1.0f / time;
            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                thisTransform.position = Vector3.Lerp(startPos, endPos, i);
                yield return null;
                if (test)
                {
                    move = false;
                    break;
                }
            }
        }
    }
    IEnumerator DestroyCube(GameObject obj)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(obj);
        
    }
}

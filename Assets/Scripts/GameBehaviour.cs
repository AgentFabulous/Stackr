using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameBehaviour : MonoBehaviour {

    //public Members
    public GameObject cam, scriptHolder, pointsField, gameOverText, lightobj;
    public GameObject MasterCube;
    public float accuracyLevel = 0.5f;
    public int speed = 2;
    public Material CubeMat;
    public bool paused = false;
    public GameObject bg;
    //private Members
    private bool spinning = false;
//    private float diff = 0;
    private Vector3 startPosi, targetPosi;
    private GameObject[] cube;
    private float movementSpeed = 2.0f, backupSpeed=0f;
    private string side = "left";
    private int tap = 1, slo=0,accu=0;
    private bool move = true, gameStarted = false, isRunning = false, gameover = false, animcheck = false, backupComplete=false;
    private bool nospin = false, accuracy = false, slowmotion = false;
    private bool red = false, green = false, blue = false;
    private int tmp;
    
    //Main Functions
   public void MenuControls(string option)
    {
        if (option == "paused")
            paused = !paused;
        if (option == "nospin")
            nospin = true;
        if (option == "accuracy")
            accuracy = true;
        if (option == "slowmotion")
            slowmotion = true;

    }
    bool UIInteractionCheck()
    {
            if (Application.isEditor)
                return EventSystem.current.IsPointerOverGameObject();
            else
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
    void Start() {
        cube = new GameObject[5000];
        gameover = false;
        gameStarted = true;
        CreateFirstCube();
        StartCoroutine(Anim());
        Text gotxt = gameOverText.GetComponent<Text>();
        gotxt.CrossFadeAlpha(0f, 0f, true);
        StartCoroutine(LevelFade());
    }
    IEnumerator LevelFade()
    {
        float fadeTime = GameObject.Find("ScriptHolder").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
    void Update()
    {
        if (nospin && spinning)
            Stopspin();
        Rotate scriptSpin = cam.GetComponent<Rotate>();
        RotateStart scriptStart = cam.GetComponent<RotateStart>();
        if (spinning && gameover && scriptSpin.speed == scriptStart.lim) 
            Stopspin();
        if (paused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;

        if (cube[tap] != null)
        {
            if (cube[tap].GetComponent<Renderer>().material.color.r >= 2f)
            { red = true; green = false; blue = false; }
            else if (cube[tap].GetComponent<Renderer>().material.color.g >= 2f)
            { red = false; green = true; blue = false; }
            else if (cube[tap].GetComponent<Renderer>().material.color.b >= 2f)
            { red = false; green = false; blue = true; }
        }
        int tmp=tap-1;
        if (tmp >= 10 && tmp < 15)
            movementSpeed = 1.5f;
        else if (tmp >= 15 && tmp < 25)
            movementSpeed = 1.3f;
        else if (tmp >= 25 && tmp < 30)
            Startspin();
        else if (tmp >= 30 && tmp < 45)
        { Stopspin(); movementSpeed = 1.0f; }
        else if (tmp >= 45 && tmp < 50)
            Startspin();
        else if (tmp >= 50 && tmp < 70)
            Stopspin();
        else if (tmp>70)
            movementSpeed = 0.65f;
        if (tap > 70 && tap % 10 == 0 && !gameover)
            Startspin();
        else if (tap > 80 && tap % 7 == 0)
            Stopspin();

        if (slowmotion)
        {
            if (!backupComplete)
            { backupSpeed = movementSpeed; backupComplete = true; }
            movementSpeed = 2f;
            if (Input.GetMouseButtonDown(0) && !paused && !UIInteractionCheck() && gameStarted == true && animcheck)
                slo++;
            if (slo == 5)
            {
                slowmotion = backupComplete = false;
                movementSpeed = backupSpeed;
                slo = 0;
            }
        }
        if (tap >= 0 && gameStarted == true && move == true)
        {
            if (!isRunning)
                StartCoroutine(Anim());
        }
        if (Input.GetMouseButtonDown(0) && !paused && !UIInteractionCheck() && gameStarted == true && animcheck)
        {

            if (accu == 5)
            { accuracy = false; accu = 0; }
            if (accuracy)
            { 
                Vector3 currentPos = cube[tap].transform.position;
                if (side == "left")
                {
                    if (Mathf.Abs(currentPos.x - cube[tap - 1].transform.position.x) < accuracyLevel)
                    {
                        print("Accurate left");
                        currentPos.x = cube[tap - 1].transform.position.x;
                        cube[tap].transform.position = currentPos;
                        accu++;
                    }
                    else
                        ReduceCube();
                }
                else
                {
                    if (Mathf.Abs(currentPos.z - cube[tap - 1].transform.position.z) < accuracyLevel)
                    {
                        print("Accurate right");
                        currentPos.z = cube[tap - 1].transform.position.z;
                        cube[tap].transform.position = currentPos;
                        accu++;
                    }
                    else
                        ReduceCube();
                }
            }
            else
                ReduceCube();
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
                Text textfield = pointsField.GetComponent<Text>();
                textfield.text = tap.ToString();
                tap++;
                CreateCube();
                Color cubecol= cube[tap].GetComponent<Renderer>().material.color, bgcol= bg.GetComponent<Image>().color;
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
                if (tap > 4)
                    StartCoroutine(MoveCam());
                
            }

            else
            {
                Text gotxt = gameOverText.GetComponent<Text>();
                gotxt.CrossFadeAlpha(1f, 1f, false);
                tmp = tap;
                gameStarted=false;
            }

        }

        if (gameover && !gameStarted)
        {
            if (Input.GetMouseButtonDown(0) && !UIInteractionCheck() && !paused)
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
    void ReduceCube()
    {
        //reduces cube size
        float TempPos = 0f;
        float xPos = cube[tap - 1].transform.position.x;
        float zPos = cube[tap - 1].transform.position.z;
        float xScale = cube[tap - 1].transform.localScale.x;
        float zScale = cube[tap - 1].transform.localScale.z;
        Vector3 Pos = cube[tap].transform.position;
        Vector3 Scale = cube[tap].transform.localScale;
        //For Left Start
        if (side == "left")
        {
            if (Mathf.Abs((Pos.x) - (xPos)) > xScale)
            {
                gameover = true;
                gameStarted = false;
                cube[tap].AddComponent<Rigidbody>();
                Destroy(cube[tap], 10);
            }
            else
            {
                float difference = xPos - cube[tap].transform.position.x;
                Pos.x += (difference / 2);
                if (difference > 0)
                {
                    Scale.x = xScale - difference;
                    TempPos = (Pos.x - (Scale.x / 2) - (difference / 2));
                }
                else if (difference < 0)
                {
                    Scale.x = xScale + difference;
                    TempPos = (Pos.x + (Scale.x / 2) - (difference / 2));
                }
                cube[tap].transform.localScale = Scale;
                cube[tap].transform.position = Pos;
                GameObject Brokencube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Brokencube.transform.localScale = new Vector3(difference, Scale.y, Scale.z);
                Brokencube.transform.localPosition = new Vector3(TempPos, Pos.y, Pos.z);
                Brokencube.AddComponent<Rigidbody>();
                Brokencube.GetComponent<Renderer>().material = CubeMat;
                Brokencube.GetComponent<Renderer>().material.color = cube[tap].GetComponent<Renderer>().material.color;
                StartCoroutine(DestroyBroken(Brokencube));
            }
        }
        else if (side == "right")
        {
            if (Mathf.Abs(Pos.z - zPos) > zScale)
            {
                gameover = true;
                gameStarted = false;
                cube[tap].AddComponent<Rigidbody>();
            }
            else
            {
                float difference = zPos - cube[tap].transform.position.z;
                Pos.z += (difference / 2);
                if (difference > 0)
                {
                    Scale.z = zScale - difference;
                    TempPos = (Pos.z - (Scale.z / 2) - (difference / 2));
                }
                else if (difference < 0)
                {
                    Scale.z = zScale + difference;
                    TempPos = (Pos.z + (Scale.z / 2) - (difference / 2));
                }
                cube[tap].transform.localScale = Scale;
                cube[tap].transform.position = Pos;
                GameObject Brokencube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Brokencube.transform.localScale = new Vector3(Scale.x, Scale.y, difference);
                Brokencube.transform.localPosition = new Vector3(Pos.x, Pos.y, TempPos);
                Brokencube.AddComponent<Rigidbody>();
                Brokencube.GetComponent<Renderer>().material = CubeMat;
                Brokencube.GetComponent<Renderer>().material.color =cube[tap].GetComponent<Renderer>().material.color;
                StartCoroutine(DestroyBroken(Brokencube));
            }
        }
    }
    void Startspin()
    {
        if (!nospin && !spinning)
        {
            //Starts Spinning the camera
            Reorient stop = cam.GetComponent<Reorient>();
            RotateStart start = cam.GetComponent<RotateStart>();
            start.enabled = true;
            stop.enabled = false;
            spinning = true;
        }
    }
    void Stopspin()
    {
        if (spinning)
        {
            //Stops Spinning the camera
            Rotate rot = cam.GetComponent<Rotate>();
            if (rot.speed!= 5)
                StartCoroutine(RotateWait(3.5f));
            else
                StartCoroutine(RotateWait(0f));
        }
    }
    IEnumerator RotateWait(float time)
    {
        yield return new WaitForSeconds(time);
        RotateStart start = cam.GetComponent<RotateStart>();
        Reorient stop = cam.GetComponent<Reorient>();
        stop.enabled = true;
        start.enabled = false;
        spinning = false;
    }
    IEnumerator Anim()
    {
        isRunning = true;
        while (move)
        {
            if (Input.GetMouseButtonDown(0) && !UIInteractionCheck() && !paused)
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
                if (Input.GetMouseButtonDown(0) && !UIInteractionCheck() && !paused)
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
                if (Input.GetMouseButtonDown(0) && !UIInteractionCheck() && !paused)
                {
                    move = false;
                    break;
                }
            }
        }
    }
    IEnumerator DestroyBroken(GameObject obj)
    {
        yield return new WaitForSeconds(2.0f);
        while (obj.GetComponent<Renderer>().material.color.a>-2f)
        {
            obj.GetComponent<Renderer>().material.color-=new Color (0f,0f,0f,0.01f);
            yield return new WaitForSeconds(0.0025f);
        }
        Destroy(obj);
    }
  
}

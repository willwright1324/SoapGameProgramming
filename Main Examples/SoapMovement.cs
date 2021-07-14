using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoapMovement : MonoBehaviour {

    public bool grounded, pause, canLatch, canJump, jumping, boost, canBoost, oneBubbleAdder, increaseScore, decreaseScore, increaseBigPoints, escapeOpen;
    public bool canSuds = true;
    bool riding;
    GameObject camEmpty, respawn, bubbleM, bub1, bub2, bub3, jumpTrigger, jPos1, jPos2, moveDir, turnDir, bubbleTrigger;
    Vector3 vec;
    public int points, bigPoints, displayScore, jumpCount, turnVal;
    public int rotSpeed = 200;
    public float boostForce = 3.5f;
    public int bubbleCount = 3;
    int movementClamp;
    int boostClamp = 200;
    public float facingHor, facingVert, camFacingHor;
    public float speed = 0.5f;
    public float jumpForce = 7;
    public float boostDecay = 1.02f;
    public float moveDecay = 1.05f;
    float jumpHeight = 3;
    Rigidbody rb;
    public Vector3 vel;
    public float floatForce;
    Text score, controlsText, totalScore, totalTime, exitText, lvl1Score, lvl1Time, lvl2Score, lvl2Time, lvl3Score, lvl3Time, lvl4Score, lvl4Time, bigPointsCounter, bigPointsText, gameEndText;
    GameObject choiceMenu, gameEnd, pauseMenu, scoresMenu, blackScreen, bigPointsDisplay;
    Light displayLight;
    public string choice;
    public int buildIndex, time;
    TimerSystem timer;
    CameraMouseMovement cam;
    StartCamera sCam;
    public Canvas canvas;
    int yesInt;
    
    public Quaternion startRot;
    public List<Button> buttons;
    MeshRenderer door;
    
    public bool canBeHurt = true;
    public Behaviour halo;

    void Start() {
        camEmpty = GameObject.Find("CameraEmpty");
        respawn = GameObject.Find("Respawn");
        respawn.transform.position = transform.position;
        startRot = transform.rotation;
        jumpTrigger = GameObject.Find("JumpTrigger");
        jPos1 = GameObject.Find("JPos1");
        jPos2 = GameObject.Find("JPos2");
        moveDir = GameObject.Find("MovementDirection");
        turnDir = GameObject.Find("TurnDirection");
        rb = GetComponent<Rigidbody>();
        bubbleTrigger = GameObject.Find("BubbleTrigger");
        bubbleTrigger.SetActive(false);
        bubbleM = Resources.Load("universal_Prefabs/Suds") as GameObject;
        bub1 = GameObject.Find("Bubble1");
        bub2 = GameObject.Find("Bubble2");
        bub3 = GameObject.Find("Bubble3");
        score = GameObject.Find("ScoreText").GetComponent<Text>();
        points = 0;
        score.text = "Score: " + points; 
        displayScore = points;
        
        halo = (Behaviour)gameObject.GetComponent("Halo");
        
        controlsText = GameObject.Find("ControlsText").GetComponent<Text>();
        totalScore = GameObject.Find("TotalScoreText").GetComponent<Text>();
        totalTime = GameObject.Find("TotalTimeText").GetComponent<Text>();
        exitText = GameObject.Find("ExitText").GetComponent<Text>();
        lvl1Score = GameObject.Find("Level1Score").GetComponent<Text>();
        lvl2Score = GameObject.Find("Level2Score").GetComponent<Text>();
        lvl1Time = GameObject.Find("Level1Time").GetComponent<Text>();
        lvl2Time = GameObject.Find("Level2Time").GetComponent<Text>();
        
        GameObject[] btn = GameObject.FindGameObjectsWithTag("Button");
        for(int i = 0; i < btn.Length; i++) {
            buttons.Add(btn[i].GetComponent<Button>());
        }
       for(int i=0; i<buttons.Count; i++){
            if(buttons[i].name == "ResumeButton") {buttons[i].onClick.AddListener(NoStayInGame);} 
            if(buttons[i].name == "MainMenuButton") {buttons[i].onClick.AddListener(delegate {AskFirst("menu"); });}
            if(buttons[i].name == "RepeatLevelButton") {buttons[i].onClick.AddListener(delegate {AskFirst("restart"); });}
            if(buttons[i].name == "YesButton") {buttons[i].onClick.AddListener(YesToQuestion);}
            if(buttons[i].name == "NoButton") {buttons[i].onClick.AddListener(NoToQuestion);}
            if(buttons[i].name == "EndMainMenuButton") {buttons[i].onClick.AddListener(ReturnToMenu);}
            if(buttons[i].name == "EndRepeatLevelButton") {buttons[i].onClick.AddListener(RestartLevel);}
        }
        
        scoresMenu = GameObject.Find("ScoresMenu");
        choiceMenu = GameObject.Find("ExitMenu");
        gameEnd = GameObject.Find("GameEnd");
        pauseMenu = GameObject.Find("PauseMenu");
        blackScreen = GameObject.Find("BlackScreen");
        gameEndText = GameObject.Find("EndText").GetComponent<Text>();
        pauseMenu.SetActive(false);
        gameEnd.SetActive(false);
        choiceMenu.SetActive(false);
        scoresMenu.SetActive(false);
        blackScreen.SetActive(false);
              
        canvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
        canvas.enabled = false;
        
        bigPointsCounter = GameObject.Find("BigPointsCounter").GetComponent<Text>();
        bigPointsDisplay = GameObject.Find("BigPointsDisplay");
        bigPointsText = bigPointsDisplay.GetComponent<Text>();
        displayLight = bigPointsDisplay.GetComponent<Light>();

        buildIndex = SceneManager.GetActiveScene().buildIndex;
        
        timer = GameObject.Find("TimerText").GetComponent<TimerSystem>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraMouseMovement>();
        sCam = GameObject.Find("StartCamera").GetComponent<StartCamera>();
        bigPointsCounter.text = "Big Points Collected:\n0 / " + sCam.points.Length;
        bigPointsText.text = "0 / " + sCam.points.Length;
        
        controlsText.enabled = false;
    }

    void Update() {
        vel = rb.velocity;
        //Determines if player is upside-down then limits velocity
        facingVert = Vector3.Dot(transform.up, Vector3.down);
        if (grounded)
            movementClamp = facingVert < 0 ? 16 : 22;

        //Determines horizontal rotation of player and camera
        facingHor = Vector3.Dot(-turnDir.transform.right, Vector3.right);
        camFacingHor = Vector3.Dot(-moveDir.transform.right, Vector3.right);

        //Enables jumping        
        if (sCam.done == true) {
            if (Input.GetKeyDown("space") && canJump && grounded) {
                jumping = true;
                canJump = false;
            }
            if (Input.GetKeyUp("space")) {
                canJump = false;
                jumping = false;
                jumpCount = 0;
            }
        }

        //Places suds
        if (sCam.done == true) {
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && grounded && canJump && canSuds && bubbleCount > 0) {
                bubbleTrigger.SetActive(true);
                canBoost = false;
                bubbleCount--;
                UpdateBubble();
                GameObject bubbleC = Instantiate(bubbleM, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z)) as GameObject;
            }
        }
    }
    void FixedUpdate() {
        //Clamped velocity when moving, faster if on suds side, decayed after boosting
        if (!(boost || pause))
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -movementClamp, movementClamp), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -movementClamp, movementClamp));
        else {
            if (rb.velocity.x > movementClamp || rb.velocity.x < -movementClamp || rb.velocity.z > movementClamp || rb.velocity.z < -movementClamp) {
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -boostClamp, boostClamp), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -boostClamp, boostClamp));
                rb.velocity /= boostDecay;
            }
            else {
                boost = false;
                canSuds = true;
            }
        }

        //Enables movement and rotation
        if (!pause && sCam.done == true) {
            if (!riding) {
                if (Input.GetKey("a")) rb.AddForce(-moveDir.transform.right   * speed, ForceMode.VelocityChange);
                if (Input.GetKey("d")) rb.AddForce(moveDir.transform.right    * speed, ForceMode.VelocityChange);
                if (Input.GetKey("w")) rb.AddForce(moveDir.transform.forward  * speed, ForceMode.VelocityChange);
                if (Input.GetKey("s")) rb.AddForce(-moveDir.transform.forward * speed, ForceMode.VelocityChange);
            }

            //Rotates player left and right horizontally
            if (Input.GetKey("q")) transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
            if (Input.GetKey("e")) transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

            //Flips player with LMB
            if (!grounded && Input.GetKey(KeyCode.Mouse0))
                transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);

            //Restricts jumping
            if (jumping) {
                if (jumpCount < jumpHeight) {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    jumpCount++;
                }
                else {
                    jumpCount = 0;
                    jumping = false;
                }
            }
        }

        if (oneBubbleAdder) {
            int timer = 0;
            if (timer == 0) {
                bubbleCount++;
                UpdateBubble();
                timer++;
            }
            oneBubbleAdder = false;
        }

        //Keeps track of score
        if (increaseScore) {
            points += 200;
            StartCoroutine(ScoreUpdaterInc());
            increaseScore = false;
        }
        
        if (increaseBigPoints) {
            bigPoints++;
            points += 200;
            bigPointsCounter.text = "Big Points Collected:\n" + bigPoints + " / " + sCam.points.Length;
            bigPointsText.text = bigPoints + " / " + sCam.points.Length;
            if (buildIndex != 1 && bigPoints >= sCam.points.Length) { 
                displayLight.color = Color.green;
                escapeOpen = true;
                bigPointsCounter.text = "Big Points Collected:\n" + bigPoints + " / " + sCam.points.Length + "\nEscape through the door!!";
            }
            StartCoroutine(ScoreUpdaterInc());
            increaseBigPoints = false;
            
            if(buildIndex == 1 && bigPoints >= sCam.points.Length) {
                EndGame();
            }
        }

        if (decreaseScore && !canBeHurt) {
            points -= 100;
            StartCoroutine(ScoreUpdaterDec()); 
            StartCoroutine(Cooldown());
            if(points <= 0) {
                GameOver();
            }
            decreaseScore = false;
        }

        //Exits the game (build form)
        if (Input.GetKeyDown(KeyCode.Escape)) {  
            if(!pause) {
                PauseGame();
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    private void LateUpdate() {
        moveDir.transform.eulerAngles = new Vector3(0, camEmpty.transform.eulerAngles.y, 0);
        turnDir.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        //Adjusts JumpTrigger
        Vector3 jPos1B = jPos1.transform.position;
        Vector3 jPos2B = jPos2.transform.position;
        float jTemp1;
        float jTemp2;
        if (facingHor > -0.5f && facingHor < 0.5f) {
            jTemp1 = jPos1B.z;
            jTemp2 = jPos2B.z;
        }
        else {
            jTemp1 = jPos1B.x;
            jTemp2 = jPos2B.x;
        }
        jumpTrigger.transform.position = new Vector3(transform.position.x, (jPos1B.y < jPos2B.y ? jPos1B.y : jPos2B.y) - 0.5f, transform.position.z);

        float jtScale = Mathf.Abs(jTemp1 - jTemp2);
        jumpTrigger.transform.localScale = new Vector3(Mathf.Clamp(jtScale, 0.8f, 5), 0.5f, 2.2f);

        //Toggles controls text on/off
        if (Input.GetKeyDown("i")) {
            if (controlsText.enabled) {
                controlsText.enabled = false;
            }
            else {
                controlsText.enabled = true;
            }
        }
    }
    private void OnCollisionStay(Collision collision) {
        grounded = true;
        if (collision.gameObject.tag == "Ridable")
            riding = true;
    }
    private void OnCollisionExit(Collision collision) {
        grounded = false;
        canJump = false;
        if (collision.gameObject.tag == "Ridable")
            riding = false;
    }
    private void OnTriggerEnter(Collider other) {
        //Sets temporary invincibility
        if (other.gameObject.tag == "Enemy") {
            if(canBeHurt && !pause) {
                decreaseScore = true;
                canBeHurt = false;
            }
        }
        
        //Aquires points for score and unlocks exit
        if (other.gameObject.tag == "Points") {
            Destroy(other.gameObject);
            increaseScore = true;
        }
        
        if (other.gameObject.tag == "BigPoints") {
            Destroy(other.gameObject);
            increaseBigPoints = true;
        }

        if ((other.gameObject.tag == "End" && escapeOpen)  || (buildIndex == 1 && bigPoints >= sCam.points.Length)) {
            EndGame();
        }
    }
    private void OnTriggerStay(Collider other) {
        //Player floats more in water
        if (other.gameObject.tag == "Water") {
            rb.AddForce(Vector3.up * floatForce);
            canSuds = false;  
        }

        if (other.gameObject.tag == "Suds" && !boost) {
            canSuds = false;
            //Boosts the player in direction they are moving
            if ((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && canBoost && Vector3.Distance(transform.position, other.transform.position) < 2) {
                boost = true;
                rb.AddForce(vel * boostForce, ForceMode.Impulse);
            }
            //Absorbs suds
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse1)) {
                Collider suds = other.GetComponent<Collider>();
                suds.enabled = false;
                oneBubbleAdder = true;
                canBoost = true;
                canSuds = true;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Water")
            canSuds = true;
    }

    private IEnumerator ScoreUpdaterInc() {
        while (true) {
            if (displayScore < points) {
                displayScore++;
                score.text = "Score: " + displayScore;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ScoreUpdaterDec() {
        while (true) {
            if (displayScore > points) {
                displayScore--;
                score.text = "Score: " + displayScore;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator Cooldown() {
        halo.enabled = true;
        yield return new WaitForSeconds(3f);
        canBeHurt = true;
        halo.enabled = false;
    }

    //Updates Bubble GUI
    void UpdateBubble() {
        switch (bubbleCount) {
            case 3:
                bub3.SetActive(true);
                bub2.SetActive(true);
                bub1.SetActive(true);
                break;
            case 2:
                bub3.SetActive(true);
                bub2.SetActive(true);
                bub1.SetActive(false);
                break;
            case 1:
                bub3.SetActive(true);
                bub2.SetActive(false);
                bub1.SetActive(false);
                break;
            case 0:
                bub3.SetActive(false);
                bub2.SetActive(false);
                bub1.SetActive(false);
                break;
        }
    }

    public void CheckHighScores(Text tS, Text tT) {
        int highScore = int.Parse(tS.text);
        String[] parts = tT.text.Split(" "[0]);
        int highTime = int.Parse(parts[0]);
        if (highScore < points && highTime > time) {
            tS.text = points.ToString();
            tT.text = time.ToString() + " seconds";
        }
    }

    public void PauseGame() {
        pause = true;
        vec = rb.velocity;
        rb.isKinematic = true; 
        time = timer.counter;
        timer.enabled = false;
        sCam.enabled = false;
        cam.enabled = false;
    }

    public void UnpauseGame() {
        pause = false;
        rb.isKinematic = false; 
        rb.velocity = vec;  
        timer.enabled = true;
        sCam.enabled = true;
        timer.counter = time;
        cam.enabled = true;
    }
        
    public void EndGame() {
        PauseGame();
        gameEnd.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameEndText.text = "You Won!!!";
        totalScore.text = "Total Score: " + points;
        totalTime.text = "Total Time: " + time + " seconds";

        switch (buildIndex) {
            case 1:
                CheckHighScores(lvl1Score, lvl1Time);
                break;
            case 2:
                CheckHighScores(lvl2Score, lvl2Time);
                break;
        }
    }
    
    public void GameOver() {
        PauseGame();
        gameEnd.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameEndText.text = "Game Over";
        totalScore.text = "";
        totalTime.text = "";
    }

    public void AskFirst(string choice) {
        choiceMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (choice == "menu") {
            yesInt = 0;
            exitText.text = "Exit to main menu? \n (all progress will be lost)";
        }
        if (choice == "restart") {
            yesInt = 1;
            exitText.text = "Restart the current level? \n (all progress will be lost)";
        }
    }

    public void NoToQuestion() {
        choiceMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void YesToQuestion() {
        if (yesInt == 0) {
            canvas.enabled = true;
            SceneManager.LoadScene(0);
        }
        if (yesInt == 1) {
            canvas.enabled = true;
            blackScreen.SetActive(true);
            pauseMenu.SetActive(true);
            gameEnd.SetActive(true);
            choiceMenu.SetActive(true);
            scoresMenu.SetActive(true);
            SceneManager.LoadScene(buildIndex);
        }
    }

    public void NoStayInGame() {
        UnpauseGame();
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMenu() {
        canvas.enabled = true;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel() {
        canvas.enabled = true;
        blackScreen.SetActive(true);
        pauseMenu.SetActive(true);
        gameEnd.SetActive(true);
        choiceMenu.SetActive(true);
        scoresMenu.SetActive(true);
        SceneManager.LoadScene(buildIndex);
    }
}

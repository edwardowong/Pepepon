using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Rigidbody2D unitRB;
    public GameObject hitMarker;
    public GameObject offhitMarker;
    public GameObject feverMarker;
    public GameObject aNote;
    public GameObject dNote;
    public GameObject wNote;
    public AudioSource aNoteAudio;
    public AudioSource dNoteAudio;
    public AudioSource wNoteAudio;
    public AudioSource runningAudio;
    public AudioSource attackAudio;
    public AudioSource defendAudio;
    public AudioSource missAudio;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI healthCount;
    public TextMesh damageTextPrefab;
    public Weapon weapon;

    private AudioSource bgm;
    private Animator player;

    private float movespeed = 2f;
    private int minDmg, maxDmg;

    private int streak = 0;
    private int bestStreak = 0;
    private string comboString = "";
    private int comboCount = 0;
    private bool successfulCommand;
    private int currHealth = 100;
    public bool tutorial = true;
    private bool firstLoad = true;
    private bool gamePaused = false;
    private float nextBeatTime = 0.0f;
    public float beatTimer;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        bgm = GameObject.Find("AudioController").GetComponent<AudioSource>();
        string playerCharacter = PlayerPrefs.GetString("Character");                                    // Change character based on selection
        if (playerCharacter == "Mike" || !PlayerPrefs.HasKey("Character"))
        {
            player = unitRB.gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();
            foreach (GameObject objects in GameObject.FindGameObjectsWithTag("Crewmate"))
                Destroy(objects);

        }
        else if (playerCharacter == "Crewmate")
        {
            player = unitRB.gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>();
            foreach (GameObject objects in GameObject.FindGameObjectsWithTag("Mike"))
                Destroy(objects);
        }

        if (tutorial)                                                                                   // Tutorial screen at the beginning
        {
            gamePaused = true;
            TimerController.instance.PauseTimer();
            bgm.Pause();            
        }
        else
        {
            bgm.Play();
            TimerController.instance.BeginTimer();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)                                                                             // Resume game or exit tutorial
            {
                TimerController.instance.ResumeTimer();
                pauseMenuUI.SetActive(false);
                gamePaused = false;
                bgm.Play();
            }
            else
            {                                                                                           // Pause game

                TimerController.instance.PauseTimer();
                pauseMenuUI.SetActive(true);
                gamePaused = true;
                bgm.Pause();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) && gamePaused && tutorial == false)                        // Restart level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (Input.GetKeyDown(KeyCode.D) && gamePaused && tutorial == false)                        // Quit to menu
        {
            SceneManager.LoadScene("Menu");
        }

        if (currHealth <= 0)
        {
            TimerController.instance.PauseTimer();
            deathMenuUI.SetActive(true);
            gamePaused = true;
            if (Input.GetKeyDown(KeyCode.A))                                                            // Restart level
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKeyDown(KeyCode.D))                                                       // Quit to menu
            {
                SceneManager.LoadScene("Menu");
            }
        }

        if (!gamePaused)
        {
            if (tutorial == true)
            {
                tutorial = false;
                GameObject.FindWithTag("Tutorial").SetActive(false);
            }

            nextBeatTime += Time.deltaTime;
            healthCount.text = currHealth.ToString();
            if (nextBeatTime >= 0.696f && firstLoad == true)                                            //  Delay code to time beat with music
            {
                firstLoad = false;
                nextBeatTime = 0;
            }

            if (comboCount == 4 && successfulCommand == true)                                           // Redundancy to make the first beat easier to hit
            {
                successfulCommand = false;
                comboCount = 0;
                player.SetBool("isMove", false);
                player.SetBool("isAttack", false);
                player.SetBool("isDefend", false);
            }

            if (nextBeatTime >= beatTimer - 0.35f && successfulCommand == false)                        // Note input timing
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    comboString += "A";
                    aNote.transform.position = new Vector3(175, 600 + Random.Range(0, 300), 0);
                    aNote.GetComponent<Image>().enabled = true;
                    aNoteAudio.Play();

                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    comboString += "D";
                    dNote.transform.position = new Vector3(175, 600 + Random.Range(0, 300), 0);
                    dNote.GetComponent<Image>().enabled = true;
                    dNoteAudio.Play();

                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    comboString += "W";
                    wNote.transform.position = new Vector3(175, 600 + Random.Range(0, 300), 0);
                    wNote.GetComponent<Image>().enabled = true;
                    wNoteAudio.Play();
                }

                if ((comboString == "AA" && comboCount == 2) || (comboString == "AAA" && comboCount == 3) || (comboString == "AAAD" && comboCount == 4))    // move forward
                {
                    if (comboString == "AAAD" && comboCount == 4)
                    {
                        streak++;
                        comboString = "";
                        comboCount = 0;
                        player.SetBool("isMove", true);
                        successfulCommand = true;
                        runningAudio.Play();
                    }


                }
                else if ((comboString == "DD" && comboCount == 2) || (comboString == "DDA" && comboCount == 3) || (comboString == "DDAD" && comboCount == 4))   // attack
                {
                    if (comboString == "DDAD" && comboCount == 4)
                    {
                        streak++;
                        comboString = "";
                        comboCount = 0;
                        player.SetBool("isAttack", true);
                        weapon.Shoot();
                        successfulCommand = true;
                        attackAudio.Play();
                    }
                }
                else if ((comboString == "WW" && comboCount == 2) || (comboString == "WWA" && comboCount == 3) || (comboString == "WWAD" && comboCount == 4))   // defend
                {
                    if (comboString == "WWAD" && comboCount == 4)
                    {
                        streak++;
                        comboString = "";
                        comboCount = 0;
                        player.SetBool("isDefend", true);
                        if (currHealth <= 70)
                        {
                            currHealth += 30;
                        }
                        else if (currHealth <= 100)
                        {
                            currHealth = 100;
                        }
                        successfulCommand = true;
                        defendAudio.Play();
                    }
                }
                else if (comboString == "AD" || comboString == "AW" || comboString == "DA" || comboString == "DW" || comboString == "WA" || comboString == "WD"
                    || comboString == "AAD" || comboString == "AAW" || comboString == "DDD" || comboString == "DDW" || comboString == "WWW" || comboString == "WWD"
                    || comboString == "WWAW" || comboString == "WWAA" || comboString == "AADW" || comboString == "AADD" || comboString == "DDAW" || comboString == "DDAA"
                    || comboString.Length >= 5 || comboCount >= 5 || comboString.Length < comboCount)
                {                                                                                                                                               // Wrong input
                    BeatError();
                }

            }
            else if (nextBeatTime < beatTimer - 0.35f && successfulCommand == false)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.A))  // Mistimed note
                {
                    BeatError();
                }
            }

            if (streak >= bestStreak)
            {                                                                                                   // Highest streak count
                bestStreak = streak;
                streakText.text = "Best Streak: " + bestStreak;
            }

            if (player.GetBool("isMove"))                                                                       // Move units forward if move is true
            {
                unitRB.MovePosition(unitRB.position + new Vector2(movespeed * Time.deltaTime, 0));
            }

            if (nextBeatTime >= (beatTimer - 0.25f) && nextBeatTime <= beatTimer)                               // Beat marker timing                                               
            {
                if (streak >= 1 && successfulCommand == true)                                                   // Show offhit marker on beat
                {
                    if (comboCount == 4)                                                                        // Reset variables after offhit markers complete their sequence
                    {
                        successfulCommand = false;
                        comboCount = 0;
                        player.SetBool("isMove", false);
                        player.SetBool("isAttack", false);
                        player.SetBool("isDefend", false);
                    }
                    else
                    {
                        offhitMarker.GetComponent<Image>().enabled = true;
                    }
                }
                else if (streak >= 4 && successfulCommand == false)                                             // Show fever marker on beat if streaking
                    feverMarker.GetComponent<Image>().enabled = true;
                else if (streak >= 0 && successfulCommand == false)                                             // Show hit marker on beat if not streaking
                    hitMarker.GetComponent<Image>().enabled = true;
            }
            else if (nextBeatTime > beatTimer)                                                                  // Reset beat timer and turn off marker
            {
                nextBeatTime = 0.0f;
                if (comboString != "" || successfulCommand == true)                                             // Good input so increase combo count
                {
                    comboCount++;
                }
                if (streak >= 1 && comboString == "" && successfulCommand == false)                             // Broke streak
                {
                    BeatError();
                }
                feverMarker.GetComponent<Image>().enabled = false;
                hitMarker.GetComponent<Image>().enabled = false;
                offhitMarker.GetComponent<Image>().enabled = false;
                aNote.GetComponent<Image>().enabled = false;
                wNote.GetComponent<Image>().enabled = false;
                dNote.GetComponent<Image>().enabled = false;

                //Debug.Log(comboString);
                //Debug.Log(comboCount);
            }
        }
    }

    private void BeatError()
    {
        comboString = "";
        comboCount = 0;
        streak = 0;
        missAudio.Play();
    }

    public void TakeDamage()
    {
        int damageDealt = Random.Range(2, 5);
        currHealth -= damageDealt;
        TextMesh textObject = Instantiate(damageTextPrefab, unitRB.transform.position + new Vector3(-1, 3, 0), Quaternion.identity);
        textObject.text = damageDealt.ToString();
        Object.Destroy(textObject.gameObject, 0.5f);
    }
}

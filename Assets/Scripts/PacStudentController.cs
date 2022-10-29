using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class PacStudentController : MonoBehaviour
{
    private LoadManager loadManager;
    private KeyCode currentInput;
    private KeyCode lastInput;
    private Tweener tweener;
    private GameObject[] obstacleSprites;
    private GameObject[] pelletSprites;
    private List<GameObject> pelletSpritesList = new List<GameObject>();
    private GameObject[] powerPelletSprites;
    private List<GameObject> powerPelletSpritesList = new List<GameObject>();
    private Vector3 futurePos;
    private Animator pacStudentAnim;
    [SerializeField] private Animator[] enemyAnims;
    private AudioSource movementAudio;
    private AudioSource pelletAudio;
    private AudioSource wallAudio;
    private AudioSource pacDeathAudio;
    private AudioSource enemyDeathAudio;
    private Boolean wallAudioPlayed = false;
    private GameObject dustParticles;
    private GameObject collisionParticles;
    private GameObject deathParticles;
    private GameUIControl HUD;
    private BoxCollider2D boxCollider;
    private CherryController CherryController;
    [SerializeField] private MusicManager music;
    private Boolean gameOver = true;

    // Start is called before the first frame update
    void Start()
    {
        loadManager = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadManager>();
        tweener = gameObject.GetComponent<Tweener>();
        obstacleSprites = GameObject.FindGameObjectsWithTag("obstacle");
        pelletSprites = GameObject.FindGameObjectsWithTag("InitialLayout");
        foreach (GameObject pelletObj in pelletSprites)
        {
            pelletSpritesList.Add(pelletObj);
        }
        powerPelletSprites = GameObject.FindGameObjectsWithTag("PowerPellet");
        foreach (GameObject pelletObj in powerPelletSprites)
        {
            powerPelletSpritesList.Add(pelletObj);
        }
        futurePos = transform.position;
        pacStudentAnim = gameObject.GetComponent<Animator>();
        movementAudio = GameObject.FindGameObjectWithTag("Movement").GetComponent<AudioSource>();
        pelletAudio = GameObject.FindGameObjectWithTag("Pellet").GetComponent<AudioSource>();
        wallAudio = GameObject.FindGameObjectWithTag("WallCollision").GetComponent<AudioSource>();
        pacDeathAudio = GameObject.FindGameObjectWithTag("Death").GetComponent<AudioSource>();
        enemyDeathAudio = GameObject.FindGameObjectWithTag("EnemyDeath").GetComponent<AudioSource>();
        dustParticles = GameObject.FindGameObjectWithTag("Particle");
        dustParticles.SetActive(false);
        collisionParticles = GameObject.FindGameObjectWithTag("WallParticle");
        collisionParticles.SetActive(false);
        deathParticles = GameObject.FindGameObjectWithTag("DeathParticle");
        deathParticles.SetActive(false);
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUIControl>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        CherryController = GameObject.FindGameObjectWithTag("CherrySpawner").GetComponent<CherryController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pacStudentAnim.GetBool("Dead") && !HUD.timerStart && HUD.timerGoing)
        {
            getInput();
            teleport();
            pelletCollision();
            powerPelletCollision();
        }
        if (!tweener.TweenExists(transform))
        {
            StartCoroutine(removeParticles());
            if (lastInput != KeyCode.None)
            {
                setFuturePos(lastInput);
                if (walkable())
                {
                    currentInput = lastInput;
                    tweener.AddTween(transform, transform.position, futurePos, 0.3f);
                    rotateAnim(lastInput);
                    playAudio();
                    wallAudioPlayed = false;
                } else
                {
                    setFuturePos(currentInput);
                    if (walkable())
                    {
                        tweener.AddTween(transform, transform.position, futurePos, 0.3f);
                        rotateAnim(currentInput);
                        playAudio();
                    } else
                    {
                        StartCoroutine(wallCollision());
                    }
                }
            }
        } else
        {
            dustParticles.SetActive(true);
        }
        setFuturePos(currentInput);
        dustParticles.transform.position = gameObject.transform.position;
        collisionParticles.transform.position = futurePos;
        checkGameOver();
    }

    private void playAudio()
    {
        Boolean pellet = false;
        foreach (GameObject sprite in pelletSpritesList)
        {
            if (sprite.transform.position == futurePos)
            {
                pellet = true;
            }
        }
        if (pellet)
        {
            pelletAudio.Play();
        } else
        {
            movementAudio.Play();
        }
    }

    private void rotateAnim(KeyCode input)
    {
        pacStudentAnim.enabled = true;
        if (input == KeyCode.W)
        {
            pacStudentAnim.SetBool("BoolUp", true);
            pacStudentAnim.SetBool("BoolDown", false);
            pacStudentAnim.SetBool("BoolLeft", false);
            pacStudentAnim.SetBool("BoolRight", false);
        }
        if (input == KeyCode.S)
        {
            pacStudentAnim.SetBool("BoolUp", false);
            pacStudentAnim.SetBool("BoolDown", true);
            pacStudentAnim.SetBool("BoolLeft", false);
            pacStudentAnim.SetBool("BoolRight", false);
        }
        if (input == KeyCode.A)
        {
            pacStudentAnim.SetBool("BoolUp", false);
            pacStudentAnim.SetBool("BoolDown", false);
            pacStudentAnim.SetBool("BoolLeft", true);
            pacStudentAnim.SetBool("BoolRight", false);
        }
        if (input == KeyCode.D)
        {
            pacStudentAnim.SetBool("BoolUp", false);
            pacStudentAnim.SetBool("BoolDown", false);
            pacStudentAnim.SetBool("BoolLeft", false);
            pacStudentAnim.SetBool("BoolRight", true);
        }
    }

    private void setFuturePos(KeyCode input)
    {
        if (input == KeyCode.W)
        {
            futurePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        if (input == KeyCode.S)
        {
            futurePos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        if (input == KeyCode.A)
        {
            futurePos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        if (input == KeyCode.D)
        {
            futurePos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
    }

    private void getInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = KeyCode.W;

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = KeyCode.S;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = KeyCode.D;
        }
    }

    private bool walkable()
    {
        foreach (GameObject sprite in obstacleSprites)
        {
            if (sprite.transform.position == futurePos)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator removeParticles()
    {
        Vector3 currentPos = transform.position;
        yield return new WaitForSeconds(0.2f);
        if (transform.position == currentPos)
        {
            dustParticles.SetActive(false);
        }
    }

    IEnumerator wallCollision()
    {
        if (!wallAudioPlayed)
        {
            wallAudio.Play();
            collisionParticles.SetActive(true);
            pacStudentAnim.enabled = false;
            wallAudioPlayed = true;
            yield return new WaitForSeconds(2.0f);
            collisionParticles.SetActive(false);
        }
    }

    private void teleport()
    {
        Vector3 leftTeleporter = new Vector3(-9.5f, -0.5f, 0.0f);
        Vector3 rightTeleporter = new Vector3(17.5f, -0.5f, 0.0f);
        if (gameObject.transform.position.x <= -9.5f)
        {
            gameObject.transform.position = rightTeleporter;
            return;
        }
        if (gameObject.transform.position.x >= 17.5f)
        {
            gameObject.transform.position = leftTeleporter;
            return;
        }
    }

    private void pelletCollision()
    {
        GameObject deletePellet = null;
        foreach (GameObject pellet in pelletSpritesList)
        {
            if (pellet.transform.position == gameObject.transform.position)
            {
                deletePellet = pellet;
                GameObject.Destroy(pellet);
                HUD.addScore(10);
            }
        }
        if (deletePellet != null)
        {
            pelletSpritesList.Remove(deletePellet);
        }
    }

    private void powerPelletCollision()
    {
        GameObject deletePellet = null;
        foreach (GameObject pellet in powerPelletSpritesList)
        {
            if (pellet.transform.position == gameObject.transform.position)
            {
                deletePellet = pellet;
                GameObject.Destroy(pellet);
                foreach (Animator anim in enemyAnims)
                {
                    anim.SetBool("Scared", true);
                    Invoke("resetScaredAnimState", 9.9f);
                }
                music.playScared();
                HUD.vulnerableCountdown();
            }
        }
        if (deletePellet != null)
        {
            powerPelletSpritesList.Remove(deletePellet);
        }
    }

    private void resetScaredAnimState()
    {
        foreach (Animator anim in enemyAnims)
        {
            anim.SetBool("Scared", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Animator EnemyAnim = null;
        if (otherCollider.tag == "Cherry")
        {
            CherryController.GetComponent<Tweener>().removeTweens();
            Destroy(otherCollider.gameObject);
            HUD.addScore(100);
        } else
        {
            EnemyAnim = otherCollider.GetComponent<Animator>();
        }
        if (otherCollider.tag == "Enemy" && !EnemyAnim.GetBool("Scared"))
        {
            StartCoroutine(playerDeath());
            tweener.removeTweens();
            lastInput = KeyCode.None;
            currentInput = KeyCode.None;
            HUD.removeLife();
            deathParticles.transform.position = otherCollider.transform.position;
        }
        if (otherCollider.tag == "Enemy" && EnemyAnim.GetBool("Scared"))
        {
            EnemyAnim.SetBool("Dead", true);
            HUD.addScore(300);
            enemyDeathAudio.Play();
            GhostController ghost = otherCollider.gameObject.GetComponent<GhostController>();
            ghost.moveToSpawn();
        }
    }

    IEnumerator playerDeath()
    {
        deathParticles.SetActive(true);
        pacStudentAnim.SetBool("Dead", true);
        pacDeathAudio.Play();
        Vector3 pos = gameObject.transform.position;
        float posX = (float)Math.Round(pos.x * 2) / 2;
        float posY = (float)Math.Round(pos.y * 2) / 2;
        Vector3 nearestGrid = new Vector3((posX), posY, pos.z);
        gameObject.transform.position = nearestGrid;
        yield return new WaitForSeconds(1.0f);
        gameObject.transform.position = new Vector3(-8.5f, 12.5f, 0.0f);
        pacStudentAnim.SetBool("Dead", false);
        yield return new WaitForSeconds(1.0f);
        deathParticles.SetActive(false);
    }

    private IEnumerator gameover()
    {
        if (gameOver)
        {
            gameOver = false;
            StartCoroutine(HUD.displayGameover());
            HUD.timerGoing = false;
            tweener.removeTweens();
            lastInput = KeyCode.None;
            currentInput = KeyCode.None;
            //remember to stop enemey movement too ^^^
            if (HUD.getScore() > PlayerPrefs.GetInt("HighScore") || (HUD.getScore() == PlayerPrefs.GetInt("HighScore") && HUD.getTime() < PlayerPrefs.GetFloat("Time")))
            {
                PlayerPrefs.SetInt("HighScore", HUD.getScore());
                PlayerPrefs.SetFloat("Time", HUD.getTime());
                PlayerPrefs.Save();
            }
            yield return new WaitForSeconds(3.0f);
            loadManager.loadStartScreen();
        }
    }

    private void checkGameOver()
    {
        if (pelletSpritesList.Count == 0 || HUD.noLivesLeft())
        {
            StartCoroutine(gameover());
        }
    }
}

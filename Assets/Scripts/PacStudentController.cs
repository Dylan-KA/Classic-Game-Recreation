using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class PacStudentController : MonoBehaviour
{
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
    private AudioSource deathAudio;
    private Boolean wallAudioPlayed = false;
    private GameObject dustParticles;
    private GameObject collisionParticles;
    private GameObject deathParticles;
    private UIControl HUD;
    private BoxCollider2D boxCollider;
    private CherryController CherryController;
    [SerializeField] private MusicManager music;

    // Start is called before the first frame update
    void Start()
    {
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
        deathAudio = GameObject.FindGameObjectWithTag("Death").GetComponent<AudioSource>();
        dustParticles = GameObject.FindGameObjectWithTag("Particle");
        dustParticles.SetActive(false);
        collisionParticles = GameObject.FindGameObjectWithTag("WallParticle");
        collisionParticles.SetActive(false);
        deathParticles = GameObject.FindGameObjectWithTag("DeathParticle");
        deathParticles.SetActive(false);
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<UIControl>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        CherryController = GameObject.FindGameObjectWithTag("CherrySpawner").GetComponent<CherryController>();
    }

    // Update is called once per frame
    void Update()
    {
        teleport();
        pelletCollision();
        powerPelletCollision();
        getInput();
        if (!tweener.TweenExists(transform)) //if not lerping
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
                    Invoke("resetScaredAnimState", 1.0f);
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
        if (otherCollider.tag == "Cherry")
        {
            CherryController.GetComponent<Tweener>().removeTweens();
            Destroy(otherCollider.gameObject);
            HUD.addScore(100);
        }
        if (otherCollider.tag == "Enemy") //and state is walking
        {
            StartCoroutine(playerDeath());
            tweener.removeTweens();
            //maybe snap to closest grid position
            lastInput = KeyCode.None;
            currentInput = KeyCode.None;
            HUD.removeLife();
            deathParticles.transform.position = otherCollider.transform.position;
        }
    }

    IEnumerator playerDeath()
    {
        deathParticles.SetActive(true);
        pacStudentAnim.SetBool("Dead", true);
        deathAudio.Play();
        gameObject.transform.position = new Vector3(-8.5f, 12.5f, 0.0f);
        yield return new WaitForSeconds(2.0f);
        pacStudentAnim.SetBool("Dead", false);
        deathParticles.SetActive(false);
    }
}

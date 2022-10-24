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
    private GameObject[] powerPelletSprites;
    private Vector3 futurePos;
    private Animator pacStudentAnim;
    private AudioSource movementAudio;
    private AudioSource pelletAudio;
    private GameObject particles;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        obstacleSprites = GameObject.FindGameObjectsWithTag("obstacle");
        pelletSprites = GameObject.FindGameObjectsWithTag("InitialLayout");
        powerPelletSprites = GameObject.FindGameObjectsWithTag("PowerPellet");
        futurePos = transform.position;
        pacStudentAnim = gameObject.GetComponent<Animator>();
        movementAudio = GameObject.FindGameObjectWithTag("Movement").GetComponent<AudioSource>();
        pelletAudio = GameObject.FindGameObjectWithTag("Pellet").GetComponent<AudioSource>();
        particles = GameObject.FindGameObjectWithTag("Particle");
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
                } else
                {
                    setFuturePos(currentInput);
                    if (walkable())
                    {
                        tweener.AddTween(transform, transform.position, futurePos, 0.3f);
                        rotateAnim(currentInput);
                        playAudio();
                    }
                }
            }
        } else
        {
            particles.SetActive(true);
            Debug.Log("true");
        }
    }

    private void playAudio()
    {
        Boolean pellet = false;
        foreach (GameObject sprite in pelletSprites)
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
            particles.SetActive(false);
        }
    }
}

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
    private Vector3 futurePos;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        obstacleSprites = GameObject.FindGameObjectsWithTag("obstacle");
        futurePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        if (!tweener.TweenExists(transform)) //if not lerping
        {
            if (lastInput != KeyCode.None)
            {
                if (walkable())
                {
                    setFuturePos();
                    tweener.AddTween(transform, transform.position, futurePos, 0.3f);
                } else
                {
                    //check currentInput and try to move
                }
            }
        }
    }

    private void setFuturePos()
    {
        if (lastInput == KeyCode.W)
        {
            futurePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        if (lastInput == KeyCode.S)
        {
            futurePos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        if (lastInput == KeyCode.A)
        {
            futurePos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        if (lastInput == KeyCode.D)
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
        return true;
    }
}

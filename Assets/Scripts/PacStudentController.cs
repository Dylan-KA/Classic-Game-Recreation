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
    private GameObject[] levelSprites;
    private Vector3 futurePos;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        levelSprites = GameObject.FindGameObjectsWithTag("InitialLayout");
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
                    tweener.AddTween(transform, transform.position, futurePos, 0.5f);
                } else
                {
                    //check currentInput and try to move
                }
            }
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

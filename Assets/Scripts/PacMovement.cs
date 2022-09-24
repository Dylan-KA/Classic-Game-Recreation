using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMovement : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    [SerializeField] private Animator animator;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
        pos1 = new Vector3(-3.5f, 8.5f, 0.0f);
        pos2 = new Vector3(-8.5f, 8.5f, 0.0f);
        pos3 = new Vector3(-8.5f, 12.5f, 0.0f);
        pos4 = new Vector3(-3.5f, 12.5f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pos1)
        {
            animator.SetTrigger("TriggerLeft");
            animator.ResetTrigger("TriggerDown");
            if (tweener.AddTween(item.transform, item.transform.position, pos2, 1.5f))
            {
                //Debug.Log("At pos1 going to pos2");
            }
        }
        if (transform.position == pos2)
        {
            animator.SetTrigger("TriggerUp");
            animator.ResetTrigger("TriggerLeft");
            if (tweener.AddTween(item.transform, item.transform.position, pos3, 1.5f))
            {
                //Debug.Log("At pos2 going to pos3");
            }
        }
        if (transform.position == pos3)
        {
            animator.SetTrigger("TriggerRight");
            animator.ResetTrigger("TriggerUp");
            if (tweener.AddTween(item.transform, item.transform.position, pos4, 1.5f))
            {
                //Debug.Log("At pos3 going to pos4");
            }
        }
        if (transform.position == pos4)
        {
            animator.SetTrigger("TriggerDown");
            animator.ResetTrigger("TriggerRight");
            if (tweener.AddTween(item.transform, item.transform.position, pos1, 1.5f))
            {
                //Debug.Log("At pos4 going to pos1");
            }
        }
        
    }
}

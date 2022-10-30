using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();
    private float timeFraction;
    private float elapsedTime;
    private Vector3 endPosVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTweens != null)
        {
            List<Tween> deleteList = new List<Tween>();
            foreach (Tween tween in activeTweens)
            {
                elapsedTime += Time.deltaTime;
                float distance = Vector3.Distance(tween.Target.position, tween.EndPos);
                timeFraction = elapsedTime / tween.Duration;
                if (distance > 0.1f)
                {
                    tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, timeFraction);
                }
                if (distance <= 0.1f)
                {
                    tween.Target.position = tween.EndPos;
                    elapsedTime = 0;
                    timeFraction = 0;
                    deleteList.Add(tween);
                }
            }
            foreach (Tween tween in deleteList)
            {
                activeTweens.Remove(tween);
            }
        }

    }

    public Boolean AddTween(Transform targetObject,Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            Tween activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            endPosVec = endPos;
            activeTweens.Add(activeTween);
            return true;
        } else
        {
            return false;
        }
    }

    public void removeTweens()
    {
        activeTweens.Clear();
    }

    public Boolean TweenExists(Transform target)
    {
        if (activeTweens != null)
        {
            foreach (Tween tween in activeTweens)
            {
                if (tween.Target == target)
                {
                    return true;
                }
            }
        }   
        return false;
    }

    public Vector3 getEndPos()
    {
        return endPosVec;
    }
}

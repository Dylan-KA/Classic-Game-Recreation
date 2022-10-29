using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GhostController : MonoBehaviour
{
    private float distanceToPlayer;
    private float futureDistanceToPlayer;
    private int enemyNumber = 0;
    private Vector3 previousPos;
    private List<Vector3> potentialDirections = new List<Vector3>();
    private List<Vector3> validDirections = new List<Vector3>();
    private Vector3 spawnPos;
    private GameObject player;
    private GameObject[] obstacleSprites;
    private GameObject[] enemies;
    private Tweener tweener;
    private Transform enemyTrans;
    private GameUIControl HUD;
    private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyTrans = gameObject.transform;
        obstacleSprites = GameObject.FindGameObjectsWithTag("obstacle");
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUIControl>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        initialisePotentialDirections();
        spawnPos = gameObject.transform.position;
        if (gameObject.name == "Enemy Blue")
        {
            enemyNumber = 1;
        }
        if (gameObject.name == "Enemy Orange")
        {
            enemyNumber = 2;
        }
        if (gameObject.name == "Enemy Purple")
        {
            enemyNumber = 3;
        }
        if (gameObject.name == "Enemy Green")
        {
            enemyNumber = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUD.timerStart)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (!enemyAnimator.GetBool("Scared"))
            {
                switch (enemyNumber)
                {
                    case 1:
                        enemy1Movement();
                        break;
                    case 2:
                        enemy2Movement();
                        break;
                    case 3:
                        enemy3Movement();
                        break;
                    case 4:
                        enemy4Movement();
                        break;
                }
            } else
            {
                enemy1Movement();
            }
        }

    }   

    private void enemy1Movement()
    {
        if (!tweener.TweenExists(transform))
        {
            calculateFuturePositions();
            validDirections.Clear();
            foreach (Vector3 pos in potentialDirections)
            {
                if (walkable(pos) && pos != previousPos)
                {
                    validDirections.Add(pos);
                }
            }
            System.Random random = new System.Random();
            int index = random.Next(validDirections.Count);
            if (validDirections.Count == 0)
            {
                validDirections.Add(previousPos);
            }
            futureDistanceToPlayer = Vector3.Distance(player.transform.position, validDirections[index]);
            if (futureDistanceToPlayer < distanceToPlayer)
            {
                Vector3 temp = validDirections[index];
                validDirections.Remove(validDirections[index]);
                index = random.Next(validDirections.Count);
                if (validDirections.Count == 0)
                {
                    validDirections.Add(temp);
                }
                futureDistanceToPlayer = Vector3.Distance(player.transform.position, validDirections[index]);
            }
            tweener.AddTween(enemyTrans, enemyTrans.position, validDirections[index], 0.3f);
            previousPos = enemyTrans.position;
        }
    }

    private void enemy2Movement()
    {
        if (!tweener.TweenExists(transform))
        {
            calculateFuturePositions();
            validDirections.Clear();
            foreach (Vector3 pos in potentialDirections)
            {
                if (walkable(pos) && pos != previousPos)
                {
                    validDirections.Add(pos);
                }
            }
            System.Random random = new System.Random();
            int index = random.Next(validDirections.Count);
            if (validDirections.Count == 0)
            {
                validDirections.Add(previousPos);
            }
            futureDistanceToPlayer = Vector3.Distance(player.transform.position, validDirections[index]);
            if (futureDistanceToPlayer > distanceToPlayer)
            {
                Vector3 temp = validDirections[index];
                validDirections.Remove(validDirections[index]);
                index = random.Next(validDirections.Count);
                if (validDirections.Count == 0)
                {
                    validDirections.Add(temp);
                }
                futureDistanceToPlayer = Vector3.Distance(player.transform.position, validDirections[index]);
            }
            tweener.AddTween(enemyTrans, enemyTrans.position, validDirections[index], 0.3f);
            previousPos = enemyTrans.position;
        }
    }

    private void enemy3Movement()
    {
        if (!tweener.TweenExists(transform))
        {
            calculateFuturePositions();
            validDirections.Clear();
            foreach (Vector3 pos in potentialDirections)
            {
                if (walkable(pos) && pos != previousPos)
                {
                    validDirections.Add(pos);
                }
            }
            System.Random random = new System.Random();
            int index = random.Next(validDirections.Count);
            if (validDirections.Count == 0)
            {
                validDirections.Add(previousPos);
            }
            futureDistanceToPlayer = Vector3.Distance(player.transform.position, validDirections[index]);
            tweener.AddTween(enemyTrans, enemyTrans.position, validDirections[index], 0.3f);
            previousPos = enemyTrans.position;
        }
    }

    private void enemy4Movement()
    {

    }

    public void leaveSpawn()
    {

    }

    public void moveToSpawn()
    {
        float distance = Vector3.Distance(enemyTrans.position, spawnPos);
        float finalSpeed = (distance / 10.0f);
        tweener.removeTweens();
        Vector3 pos = gameObject.transform.position;
        float posX = (float)Math.Round(pos.x * 2) / 2;
        float posY = (float)Math.Round(pos.y * 2) / 2;
        Vector3 nearestGrid = new Vector3((posX), posY, pos.z);
        gameObject.transform.position = nearestGrid;
        tweener.AddTween(enemyTrans, enemyTrans.position, spawnPos, finalSpeed);
    }

    private bool walkable(Vector3 futurePos)
    {
        Vector3 teleportLeft = new Vector3(-4.5f, -0.5f, 0.0f);
        Vector3 teleportRight = new Vector3(12.5f, -0.5f, 0.0f);
        foreach (GameObject sprite in obstacleSprites)
        {
            if (sprite.transform.position == futurePos)
            {
                return false;
            }
        }
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position == futurePos)
            {
                return false;
            }
        }
        if (futurePos == teleportLeft || futurePos == teleportRight)
        {
            return false;
        }
        return true;
    }

    private void calculateFuturePositions()
    {
        potentialDirections[0] = new Vector3(enemyTrans.position.x, enemyTrans.position.y + 1, enemyTrans.position.z); //Up
        potentialDirections[1] = new Vector3(enemyTrans.position.x, enemyTrans.position.y - 1, enemyTrans.position.z); //Down
        potentialDirections[2] = new Vector3(enemyTrans.position.x - 1, enemyTrans.position.y, enemyTrans.position.z); //Left
        potentialDirections[3] = new Vector3(enemyTrans.position.x + 1, enemyTrans.position.y, enemyTrans.position.z); //Right
    }

    private void initialisePotentialDirections()
    {
        potentialDirections.Add(Vector3.zero);
        potentialDirections.Add(Vector3.zero);
        potentialDirections.Add(Vector3.zero);
        potentialDirections.Add(Vector3.zero);
    }
}

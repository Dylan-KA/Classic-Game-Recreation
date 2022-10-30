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
    private Boolean inSpawn = true;
    private String cornerDestination = "BottomRight";
    private float cornerDistance;
    private float futureCornerDistance;
    private Vector3[] corners = new Vector3[4];

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
        corners[0] = new Vector3(16.5f, -13.5f, 0.0f); //Bottom Right
        corners[1] = new Vector3(-8.5f, -13.5f, 0.0f); //Bottom Left
        corners[2] = new Vector3(-8.5f, 12.5f, 0.0f); //Top Left
        corners[3] = new Vector3(16.5f, 12.5f, 0.0f); //Top Right
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUD.timerStart && HUD.timerGoing)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);

            if (!enemyAnimator.GetBool("Scared"))
            {
                switch (enemyNumber)
                {
                    case 1:
                        if (!inSpawn)
                        {
                            enemy1Movement();
                        } else
                        {
                            enemy1LeaveSpawn();
                        }
                        break;
                    case 2:
                        if (!inSpawn)
                        {
                            enemy2Movement();
                        }
                        else
                        {
                            enemy2LeaveSpawn();
                        }
                        break;
                    case 3:
                        if (!inSpawn)
                        {
                            enemy3Movement();
                        }
                        else
                        {
                            enemy3LeaveSpawn();
                        }
                        break;
                    case 4:
                        if (!inSpawn)
                        {
                            enemy4Movement();
                        }
                        else
                        {
                            enemy4LeaveSpawn();
                        }
                        break;
                }
            } else
            {
                if (HUD.vulnTimerGoing() && !enemyAnimator.GetBool("Dead"))
                {
                    enemy1Movement();
                }
            }
            movementAnimations();
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
        if (!tweener.TweenExists(transform))
        {
            calculateCornerDistance();
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
            calculateFutureCornerDistance(validDirections[index]);
            if (futureCornerDistance > cornerDistance)
            {
                Vector3 temp = validDirections[index];
                validDirections.Remove(validDirections[index]);
                index = random.Next(validDirections.Count);
                if (validDirections.Count == 0)
                {
                    validDirections.Add(temp);
                }
                calculateFutureCornerDistance(validDirections[index]);
            }
            if (!enemy4EdgeCases())
            {
                tweener.AddTween(enemyTrans, enemyTrans.position, validDirections[index], 0.3f);
            }
            previousPos = enemyTrans.position;
            if (enemyTrans.position == corners[0])
            {
                previousPos = Vector3.zero;
                cornerDestination = "BottomLeft";
            }
            if (enemyTrans.position == corners[1])
            {
                previousPos = Vector3.zero;
                cornerDestination = "TopLeft";
            }
            if (enemyTrans.position == corners[2])
            {
                previousPos = Vector3.zero;
                cornerDestination = "TopRight";
            }
            if (enemyTrans.position == corners[3])
            {
                previousPos = Vector3.zero;
                cornerDestination = "BottomRight";
            }
        }
    }

    private Boolean enemy4EdgeCases()
    {
        if (enemyTrans.position == new Vector3(5.5f, 8.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[0], 0.3f);
            return true;
        }
        if (enemyTrans.position == new Vector3(-3.5f, 5.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[2], 0.3f);
            return true;
        }
        if (enemyTrans.position == new Vector3(11.5f, 5.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
            return true;
        }
        if (enemyTrans.position == new Vector3(2.5f, -9.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
            return true;
        }
        return false;
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
        inSpawn = true;
    }

    private bool walkable(Vector3 futurePos)
    {
        Vector3 teleportLeft = new Vector3(-4.5f, -0.5f, 0.0f);
        Vector3 teleportRight = new Vector3(12.5f, -0.5f, 0.0f);
        Vector3 spawn1 = new Vector3(3.5f, 1.5f, 0.0f);
        Vector3 spawn2 = new Vector3(4.5f, 1.5f, 0.0f);
        Vector3 spawn3 = new Vector3(3.5f, -2.5f, 0.0f);
        Vector3 spawn4 = new Vector3(4.5f, -2.5f, 0.0f);
        foreach (GameObject sprite in obstacleSprites)
        {
            if (sprite.transform.position == futurePos)
            {
                return false;
            }
        }
        if (futurePos == teleportLeft || futurePos == teleportRight)
        {
            return false;
        }
        if (futurePos == spawn1 || futurePos == spawn2 || futurePos == spawn3 || futurePos == spawn4)
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

    private void enemy1LeaveSpawn()
    {
        calculateFuturePositions();
        if (enemyTrans.position == spawnPos)
        {
            enemyAnimator.SetBool("Scared", false);
            enemyAnimator.SetBool("Dead", false);
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[3], 0.3f);
        }
        if (enemyTrans.position == new Vector3(3.5f, 0.5f, 0.0f) )
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[3], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, 0.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -0.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -1.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -2.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -3.5f, 0.0f))
        {
            inSpawn = false;
        }
    }

    private void enemy2LeaveSpawn()
    {
        calculateFuturePositions();
        if (enemyTrans.position == spawnPos)
        {
            enemyAnimator.SetBool("Scared", false);
            enemyAnimator.SetBool("Dead", false);
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[2], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, 0.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[0], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, 1.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[0], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, 2.5f, 0.0f))
        {
            inSpawn = false;
        }
    }

    private void enemy3LeaveSpawn()
    {
        calculateFuturePositions();
        if (enemyTrans.position == spawnPos)
        {
            enemyAnimator.SetBool("Scared", false);
            enemyAnimator.SetBool("Dead", false);
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[3], 0.3f);
        }
        if (enemyTrans.position == new Vector3(3.5f, -1.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(3.5f, -2.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(3.5f, -3.5f, 0.0f))
        {
            inSpawn = false;
        }
    }

    private void enemy4LeaveSpawn()
    {
        calculateFuturePositions();
        if (enemyTrans.position == spawnPos)
        {
            enemyAnimator.SetBool("Scared", false);
            enemyAnimator.SetBool("Dead", false);
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[2], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -1.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -2.5f, 0.0f))
        {
            tweener.AddTween(enemyTrans, enemyTrans.position, potentialDirections[1], 0.3f);
        }
        if (enemyTrans.position == new Vector3(4.5f, -3.5f, 0.0f))
        {
            inSpawn = false;
        }
    }

    private void calculateCornerDistance()
    {
        if (cornerDestination == "BottomRight")
        {
            cornerDistance = Vector3.Distance(enemyTrans.position, corners[0]);
        }
        if (cornerDestination == "BottomLeft")
        {
            cornerDistance = Vector3.Distance(enemyTrans.position, corners[1]);
        }
        if (cornerDestination == "TopLeft")
        {
            cornerDistance = Vector3.Distance(enemyTrans.position, corners[2]);
        }
        if (cornerDestination == "TopRight")
        {
            cornerDistance = Vector3.Distance(enemyTrans.position, corners[3]);
        }
    }

    private void calculateFutureCornerDistance(Vector3 futurePos)
    {
        if (cornerDestination == "BottomRight")
        {
            futureCornerDistance = Vector3.Distance(futurePos, corners[0]);
        }
        if (cornerDestination == "BottomLeft")
        {
            futureCornerDistance = Vector3.Distance(futurePos, corners[1]);
        }
        if (cornerDestination == "TopLeft")
        {
            futureCornerDistance = Vector3.Distance(futurePos, corners[2]);
        }
        if (cornerDestination == "TopRight")
        {
            futureCornerDistance = Vector3.Distance(futurePos, corners[3]);
        }
    }

    private void movementAnimations()
    {
        if (tweener.TweenExists(gameObject.transform))
        {
            if (tweener.getEndPos() == potentialDirections[0]) //Up
            {
                enemyAnimator.SetBool("Up", true);
                enemyAnimator.SetBool("Down", false);
                enemyAnimator.SetBool("Left", false);
                enemyAnimator.SetBool("Right", false);
            }
            if (tweener.getEndPos() == potentialDirections[1]) //Down
            {
                enemyAnimator.SetBool("Up", false);
                enemyAnimator.SetBool("Down", true);
                enemyAnimator.SetBool("Left", false);
                enemyAnimator.SetBool("Right", false);
            }
            if (tweener.getEndPos() == potentialDirections[2]) //Left
            {
                enemyAnimator.SetBool("Up", false);
                enemyAnimator.SetBool("Down", false);
                enemyAnimator.SetBool("Left", true);
                enemyAnimator.SetBool("Right", false);
            }
            if (tweener.getEndPos() == potentialDirections[3]) //Right
            {
                enemyAnimator.SetBool("Up", false);
                enemyAnimator.SetBool("Down", false);
                enemyAnimator.SetBool("Left", false);
                enemyAnimator.SetBool("Right", true);
            }
        }
    }
}

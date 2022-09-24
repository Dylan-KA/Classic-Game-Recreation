using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.FilePathAttribute;

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    GameObject[] manualLevelLayout;
    [SerializeField] private GameObject[] spritePrefabs;
    Vector3 startPos = new Vector3(-9.5f, 13.5f, 0.0f);
    Vector3 currentPos = new Vector3(-9.5f, 13.5f, 0.0f);
    GameObject currentSprite;
    String currentQuadrant;
    int outerInt = 0;
    int innerInt = 0;
    int outerSize;
    int innerSize;
    float previousRotation = 0.0f;

    void Start()
    {
        // Delete existing Level layout.
        manualLevelLayout = GameObject.FindGameObjectsWithTag("InitialLayout");
        foreach (GameObject obj in manualLevelLayout)
        {
            Destroy(obj);
        }
        topLeftQuad();
        topRightQuad();
        bottomLeftQuad();
        bottomRightQuad();
    }

    private void topLeftQuad()
    {
        currentQuadrant = "TopLeft";
        outerSize = levelMap.GetUpperBound(0);
        innerSize = levelMap.GetUpperBound(1);
        for (int outer = 0; outer <= outerSize; outer++)
        {
            outerInt = outer;
            for (int inner = 0; inner <= innerSize; inner++)
            {
                currentPos.y = startPos.y - outer;
                currentPos.x = startPos.x + inner;
                createAndMove(outer, inner);
            }
        }
    }
    private void topRightQuad()
    {
        currentQuadrant = "TopRight";
        outerSize = levelMap.GetUpperBound(0);
        innerSize = levelMap.GetUpperBound(1);
        startPos.x += outerSize;
        for (int outer = 0; outer <= outerSize; outer++)
        {
            outerInt = outer;
            for (int inner = innerSize; inner >= 0; --inner) //loop backwards through inner loop
            {
                currentPos.y = startPos.y - outer;
                currentPos.x = startPos.x + (innerSize - inner);
                createAndMove(outer, inner);
            }
        }
    }
    private void bottomLeftQuad()
    {
        currentQuadrant = "BottomLeft";
        outerSize = levelMap.GetUpperBound(0);
        innerSize = levelMap.GetUpperBound(1);
        startPos.x -= outerSize;
        startPos.y -= (outerSize + 1);
        for (int outer = 0; outer <= outerSize; outer++)
        {
            outerInt = outer;
            for (int inner = 0; inner <= innerSize; inner++)
            {
                currentPos.y = startPos.y - (outerSize - outer);
                currentPos.x = startPos.x + inner;
                createAndMove(outer, inner);
            }
        }
    }
    private void bottomRightQuad()
    {
        currentQuadrant = "BottomRight";
    }

    private void createAndMove(int outer, int inner)
    {
        innerInt = inner;
        int sprite = levelMap[outer, inner];
        if (sprite != 0)
        {
            currentSprite = (GameObject)Instantiate(spritePrefabs[sprite], currentPos, spritePrefabs[sprite].transform.rotation);
            rotate(outer, inner);
        }
    }

    private void rotate(int outer, int inner)
    {
        Vector3 rotation = new Vector3(0, 0, 0);
        Quaternion quaternion = Quaternion.Euler(rotation);
        if (levelMap[outer, inner] == 2) //if current piece is outside wall
        {
            if ((spriteToLeft() == 1 || spriteToLeft() == 2) || (spriteToRight() == 1 || spriteToRight() == 2))
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
        }
        if (levelMap[outer, inner] == 1) //if current piece is outside corner
        {
            if (spriteToLeft() == 2 && spriteBelow() == 2)
            {
                rotation = new Vector3(0, 0, 270);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 2 && spriteAbove() == 2)
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteAbove() == 2 && spriteToRight() == 2)
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToRight() == 2 && spriteBelow() == 2)
            {
                rotation = new Vector3(0, 0, 0);
                quaternion = Quaternion.Euler(rotation);
            }
        }
        if (levelMap[outer, inner] == 4) //if current piece is inner wall
        {
            if ((spriteToLeft() == 3 || spriteToLeft() == 4) && (spriteToRight() == 3 || spriteToRight() == 4))
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if ((spriteToLeft() == 4 || spriteToRight() == 4) && (spriteAbove() != 4 && spriteBelow() != 4))
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
        }
        if (levelMap[outer, inner] == 3) //if current piece is inner corner
        {
            if ((spriteToLeft() == 4 || spriteToLeft() == 3) && (spriteBelow() == 4 || spriteBelow() == 3))
            {
                rotation = new Vector3(0, 0, 270);
                quaternion = Quaternion.Euler(rotation);
            }
            if ((spriteToLeft() == 4 || spriteToLeft() == 3) && (spriteAbove() == 4 || spriteAbove() == 3))
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if ((spriteAbove() == 4 || spriteAbove() == 3) && (spriteToRight() == 4 || spriteToRight() == 3))
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if ((spriteBelow() == 4 || spriteBelow() == 3) && (spriteToRight() == 4 || spriteToRight() == 3))
            {
                rotation = new Vector3(0, 0, 0);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 4 && spriteAbove() == 4 && spriteToRight() == 4)
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if ((spriteToLeft() != 4 && spriteToLeft() != 3) && spriteAbove() == 4 && spriteToRight() != 4)
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 4 && spriteAbove() == 4 && spriteBelow() == 4 && spriteToRight() != 4)
            {
                rotation = new Vector3(0, 0, -90);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 0 && spriteToRight() == 5 && spriteAbove() == 4 && spriteBelow() == 5)
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 0 && spriteToRight() == 0 && spriteAbove() == 4 && spriteBelow() == 0 && currentQuadrant == "TopRight")
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 4 && spriteToRight() == 4 && spriteAbove() == 4 && spriteBelow() == 3 && currentQuadrant == "TopRight")
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 4 && spriteToRight() == 4 && spriteAbove() == 3 && spriteBelow() == 4 && currentQuadrant == "TopRight")
            {
                rotation = new Vector3(0, 0, 270);
                quaternion = Quaternion.Euler(rotation);
            }
        }
        if (levelMap[outer, inner] == 7) //if current piece is T juntion
        {
            if (spriteBelow() == 0 && spriteAbove() != 0 && (spriteToLeft() == 2 || spriteToRight() == 2))
            {
                rotation = new Vector3(0, 0, 180);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToLeft() == 0 && spriteToRight() != 0 && (spriteAbove() == 2 || spriteBelow() == 2))
            {
                rotation = new Vector3(0, 0, 90);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteToRight() == 0 && spriteToLeft() != 0 && (spriteAbove() == 2 || spriteBelow() == 2))
            {
                rotation = new Vector3(0, 0, 270);
                quaternion = Quaternion.Euler(rotation);
            }
            if (spriteAbove() == 0 && spriteBelow() != 0 && (spriteToLeft() == 2 || spriteToRight() == 2))
            {
                rotation = new Vector3(0, 0, 0);
                quaternion = Quaternion.Euler(rotation);
            }
        }
            Debug.Log(
            currentQuadrant +
            " outer: " + outer + " inner: " + inner + " currentPos " + currentPos +
            " SpriteToLeft: " + spriteToLeft() + " SpriteToRight: " + spriteToRight() +
            " SpriteAbove: " + spriteAbove() + " SpriteBelow: " + spriteBelow() +
            " outerSize: " + outerSize + " innerSize: " + innerSize);
        currentSprite.transform.rotation = quaternion;
        previousRotation = rotation.z;
    }

    private int spriteToLeft()
    {
        if (currentQuadrant == "TopLeft" || currentQuadrant == "BottomLeft")
        {
            if (innerInt != 0)
            {
                return levelMap[outerInt, innerInt - 1];
            }
        }
        if (currentQuadrant == "TopRight" || currentQuadrant == "BottomRight")
        {
            if (innerInt != innerSize)
            {
                return levelMap[outerInt, innerInt + 1];
            }
        }
        return 0;
    }
    private int spriteToRight()
    {       
       if (currentQuadrant == "TopRight" || currentQuadrant == "BottomRight")
       {
           if (innerInt != 0)
           {
               return levelMap[outerInt, innerInt - 1];
           }
       }
        if (currentQuadrant == "TopLeft" || currentQuadrant == "BottomLeft")
        {
            if (innerInt != innerSize)
            {
                return levelMap[outerInt, innerInt + 1];
            }
        }
        return 0;
    }
    private int spriteAbove()
    {
        if (currentQuadrant == "TopLeft" || currentQuadrant == "TopRight")
        {
            if (outerInt != 0)
            {
                return levelMap[outerInt - 1, innerInt];
            }
        }
        if (currentQuadrant == "BottomLeft" || currentQuadrant == "BottomRight")
        {
            if (outerInt != outerSize)
            {
                return levelMap[outerInt + 1, innerInt];
            }
        }
        return 0;
    }
    private int spriteBelow()
    {
        if (currentQuadrant == "TopLeft" || currentQuadrant == "TopRight")
        {
            if (outerInt != outerSize)
            {
                return levelMap[outerInt + 1, innerInt];
            }
        }
        if (currentQuadrant == "BottomLeft" || currentQuadrant == "BottomRight")
        {
            if (outerInt != 0)
            {
                return levelMap[outerInt - 1, innerInt];
            }
        }
        return 0;
    }
}

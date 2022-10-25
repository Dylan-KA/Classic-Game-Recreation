using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherry;
    GameObject currentCherry;
    private Tweener tweener;
    private Vector3 spawnPosition;
    private Vector3 endPosition;
    private float x = 0;
    private float y = 0;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        InvokeRepeating("instantiate", 0.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCherry != null && currentCherry.transform.position == endPosition)
        {
            GameObject.Destroy(currentCherry);
        }
    }

    private void instantiate()
    {
        float startAxis = Random.Range(0.0f, 1.0f);
        if (startAxis >= 0.5f)
        {
            float vertical = Random.Range(0.0f, 1.0f);
            if (vertical >= 0.5f)
            {   //start at top with random x
                y = Camera.main.scaledPixelHeight + 16.0f;
                x = Camera.main.scaledPixelWidth;
                x += Random.Range(Camera.main.scaledPixelWidth * -1, 0.0f);
                endPosition = new Vector3(x, y - (Camera.main.scaledPixelHeight + 32.0f), 0.0f);
            }
            else
            {   //start at bottom with random x
                y = Camera.main.scaledPixelHeight - 16.0f;
                y -= Camera.main.scaledPixelHeight;
                x = Camera.main.scaledPixelWidth;
                x += Random.Range(Camera.main.scaledPixelWidth * -1, 0.0f);
                endPosition = new Vector3(x, y + (Camera.main.scaledPixelHeight + 32.0f), 0.0f);
            }
        } else {
            float horizontal = Random.Range(0.0f, 1.0f);
            if (horizontal >= 0.5f)
            {   //start at right with random y
                x = Camera.main.scaledPixelWidth + 16.0f;
                y = Camera.main.scaledPixelHeight;
                y += Random.Range(Camera.main.scaledPixelHeight * -1, 0.0f);
                endPosition = new Vector3(x - (Camera.main.scaledPixelWidth + 32.0f), y, 0.0f);
            }
            else
            {   //start at left with random y
                x = Camera.main.scaledPixelWidth - 16.0f;
                x -= Camera.main.scaledPixelWidth;
                y = Camera.main.scaledPixelHeight;
                y += Random.Range(Camera.main.scaledPixelHeight * -1, 0.0f);
                endPosition = new Vector3(x + (Camera.main.scaledPixelWidth + 32.0f), y, 0.0f);
            }
        }
        spawnPosition = new Vector3(x, y, 0.0f);
        spawnPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
        spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0.0f);
        endPosition = Camera.main.ScreenToWorldPoint(endPosition);
        endPosition = new Vector3(endPosition.x, endPosition.y, 0.0f);
        currentCherry = (GameObject)Instantiate(cherry, spawnPosition, Quaternion.identity);
        tweener.AddTween(currentCherry.transform, currentCherry.transform.position, endPosition, 5.0f);
    }
}

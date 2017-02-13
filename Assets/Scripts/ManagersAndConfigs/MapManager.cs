using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static MapManager instance;

    public GameObject brickPrefab;
    public GameObject destructableBrickPrefab;
    public GameObject spikeBrickPrefab;
    public GameObject reflectBrickPrefab;
    public GameObject gravityBrickPrefab;

    public int width;
    public int height;
    public float brickChance;
    public float destructableBrickChance;
    public float spikeBrickChance;
    public float reflectBrickChance;
    public float blackholeBrickChance;
    public float spawnClearRange;
    public Bounds bounds;

    private float brickSize = 0;
    private List<Brick> bricks = new List<Brick>();
    

    private void Awake()
    {
        instance = this;
        brickSize = brickPrefab.GetComponent<BoxCollider2D>().size.x;
    }
    

    public void CreateLevel()
    {
        float chanceToUse = GameStateManager.instance.currentMode.useCustomBrickChance ? GameStateManager.instance.currentMode.customBrickChance : brickChance;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1)
                    CreateBrick(x, y);
                else if (y == 0 || y == height - 1)
                    CreateBrick(x, y);
                else if (UnityEngine.Random.value < chanceToUse)
                    CreateBrick(x, y, true);
            }
        }

        Vector3 camPos = Camera.main.transform.parent.position;
        float centerX = width * brickSize / 2f;
        float centerY = height * brickSize / 2f;
        camPos = new Vector3(centerX, centerY, camPos.z);
        Camera.main.transform.parent.position = camPos;
        Camera.main.orthographicSize = height * brickSize / 2f;
        bounds = new Bounds(new Vector3(centerX, centerY), new Vector3(width * brickSize, height * brickSize, 4f));
    }

    public Vector3 GetWorldPosition(int mapX, int mapY)
    {
        return new Vector3(mapX * brickSize + brickSize / 2f, mapY * brickSize + brickSize / 2f);
    }

    void CreateBrick(int x, int y, bool useSpecial = false)
    {
        GameObject brickObj;

        if (useSpecial && (UnityEngine.Random.value < spikeBrickChance || GameStateManager.instance.currentMode.useOnlySpikes))
            brickObj = (GameObject)Instantiate(spikeBrickPrefab, new Vector3(x * brickSize + brickSize / 2f, y * brickSize + brickSize / 2f), Quaternion.identity);
        else if (useSpecial && UnityEngine.Random.value < reflectBrickChance)
            brickObj = (GameObject)Instantiate(reflectBrickPrefab, new Vector3(x * brickSize + brickSize / 2f, y * brickSize + brickSize / 2f), Quaternion.identity);
        else if (useSpecial && UnityEngine.Random.value < blackholeBrickChance)
            brickObj = (GameObject)Instantiate(gravityBrickPrefab, new Vector3(x * brickSize + brickSize / 2f, y * brickSize + brickSize / 2f), Quaternion.identity);
        else if (UnityEngine.Random.value < destructableBrickChance || GameStateManager.instance.currentMode.onlyDestructable)
            brickObj = (GameObject)Instantiate(destructableBrickPrefab, new Vector3(x * brickSize + brickSize / 2f, y * brickSize + brickSize / 2f), Quaternion.identity);
        else
            brickObj = (GameObject)Instantiate(brickPrefab, new Vector3(x * brickSize + brickSize / 2f, y * brickSize + brickSize / 2f), Quaternion.identity);

        brickObj.GetComponent<Brick>().Init(x, y);
    }

    public void RegisterBrick(Brick brick)
    {
        bricks.Add(brick);
    }

    public void UnregisterBrick(Brick brick)
    {
        bricks.Remove(brick);
    }

    public void Cleanup()
    {
        List<Brick> toDestroy = new List<Brick>(bricks);

        for (int i = 0; i < toDestroy.Count; i++)
            Destroy(toDestroy[i].gameObject);

        bricks = new List<Brick>();
    }
}

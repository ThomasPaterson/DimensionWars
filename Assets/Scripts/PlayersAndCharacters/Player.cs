using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerNum;
    public int startDimension;
    public int wins;
    public Vector2 startPosition;
    public Vector3 indicatorOffset;


    public void SpawnPlayer()
    {
        Vector3 pos = GetSpawnPosition();
        GameObject charObj = (GameObject)Instantiate(PlayerManager.instance.characterPrefab, pos, Quaternion.identity);
        charObj.GetComponent<Character>().Init(this);
        GameObject alert = (GameObject)Instantiate(EffectsConfig.instance.playerAlertPrefab, pos + indicatorOffset, Quaternion.identity);
        alert.GetComponent<TextMesh>().color = DimensionConfig.instance.dimensions[playerNum].color;
        alert.GetComponent<TextMesh>().text = "Player " + (playerNum + 1).ToString();
        alert.GetComponent<Renderer>().sortingLayerName = "Middle";

        ClearBricks(pos);
    }

    Vector3 GetSpawnPosition()
    {
        if (startDimension == 0)
            return MapManager.instance.GetWorldPosition(MapManager.instance.width / 2, 2);
        else if (startDimension == 1)
            return MapManager.instance.GetWorldPosition(MapManager.instance.width-2, MapManager.instance.height / 2);
        else if (startDimension == 2)
            return MapManager.instance.GetWorldPosition(MapManager.instance.width / 2, MapManager.instance.height - 2);
        else 
            return MapManager.instance.GetWorldPosition(2, MapManager.instance.height / 2);
    }

    void ClearBricks(Vector3 position)
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(position, MapManager.instance.spawnClearRange, LayerConfig.instance.terrainLayers);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.GetComponent<Brick>() != null)
                colls[i].gameObject.GetComponent<Brick>().ClearSpawn();
        }
    }
    
}

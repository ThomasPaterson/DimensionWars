using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsConfig : MonoBehaviour
{

    public static EffectsConfig instance;

    public GameObject bulletDeathPrefab;
    public GameObject characterDeathPrefab;
    public GameObject blockBreakPrefab;
    public GameObject blockShakePrefab;
    public GameObject dimensionalShiftPrefab;
    public GameObject bulletFirePrefab;
    public GameObject playerDeathStompPrefab;
    public GameObject playerHardLandingPrefab;
    public GameObject playerAlertPrefab;

    private void Awake()
    {
        instance = this;
    }
   
    public void HandleBullet(Vector3 position, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(bulletDeathPrefab, position, bulletDeathPrefab.transform.rotation);
        particleObj.GetComponent<ParticleSystem>().startColor = DimensionConfig.instance.dimensions[dimension].color;
    }

    public void FireBullet(Vector3 position, Vector3 direction, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(bulletFirePrefab, position, Quaternion.FromToRotation(Vector3.up, direction.normalized));
        particleObj.GetComponent<ParticleSystem>().startColor = DimensionConfig.instance.dimensions[dimension].color;
    }

    public void HandleCharacterDeath(Vector3 position, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(characterDeathPrefab, position, characterDeathPrefab.transform.rotation);

        foreach (ParticleSystem system in particleObj.GetComponentsInChildren<ParticleSystem>())
            system.startColor = DimensionConfig.instance.dimensions[dimension].color;
    }

    public void HandleBlockBreak(Vector3 position)
    {

        GameObject particleObj = (GameObject)Instantiate(blockBreakPrefab, position, blockBreakPrefab.transform.rotation);
    }

    public void HandleBlockShake(Vector3 position)
    {

        GameObject particleObj = (GameObject)Instantiate(blockShakePrefab, position, blockShakePrefab.transform.rotation);
    }

    public void HandleDimensionalShift(Vector3 position, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(dimensionalShiftPrefab, position, dimensionalShiftPrefab.transform.rotation);

        foreach (ParticleSystem system in particleObj.GetComponentsInChildren<ParticleSystem>())
            system.startColor = DimensionConfig.instance.dimensions[dimension].color;
    }

    public void HandlePlayerStomp(Vector3 position, Vector3 direction, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(playerDeathStompPrefab, position, Quaternion.FromToRotation(Vector3.up, (-1 * direction).normalized));
        particleObj.GetComponent<ParticleSystem>().startColor = DimensionConfig.instance.dimensions[dimension].color;
    }

    public void HandleHardLanding(Vector3 position, Vector3 direction, int dimension)
    {
        GameObject particleObj = (GameObject)Instantiate(playerHardLandingPrefab, position, Quaternion.FromToRotation(Vector3.up, (-1 * direction).normalized));
        //particleObj.GetComponent<ParticleSystem>().startColor = DimensionConfig.instance.dimensions[dimension].color;
    }
}

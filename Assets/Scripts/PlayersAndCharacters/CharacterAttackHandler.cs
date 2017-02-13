using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackHandler : MonoBehaviour
{
    public Transform leftGunTransform;
    public Transform rightGunTransform;
    public Transform centerTransform;
    public float reloadSpeed;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public Transform gunTransform;
    public int clip;
    public float clipRechargeRate;
    public float accuracy;
    public float timeUntilReloadAutomatically;
    public bool reloading { get; private set; }

    private Character character;
    private bool firing;
    private float fireDistance;
    public float shotsLeft { get; private set; }
    private float timeSinceLastFired;

    private void Awake()
    {
        this.character = GetComponent<Character>();
        fireDistance = Vector3.Distance(leftGunTransform.position, centerTransform.position);
        shotsLeft = (float)clip;
    }

    private void Update()
    {
        if (!GameStateManager.instance.playing || GameStateManager.instance.currentMode.noFiring)
            return;

        if (shotsLeft < clip && reloading)
        {
            shotsLeft += Time.deltaTime * clipRechargeRate;

            if (shotsLeft >= clip)
            {
                shotsLeft = (float)clip;
                reloading = false;
            }
        }else if (shotsLeft < clip)
        {
            timeSinceLastFired += Time.deltaTime;

            if (timeSinceLastFired >= timeUntilReloadAutomatically)
            {
                timeSinceLastFired = 0f;
                shotsLeft = (float)clip;
            }
        }

        float x = Input.GetAxis("FireLeft" + character.controllingPlayer.playerNum.ToString());
        float y = Input.GetAxis("FireRight" + character.controllingPlayer.playerNum.ToString());

        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
        {
            if (!firing && !reloading)
                StartCoroutine(Fire(new Vector3(x, -y).normalized, centerTransform.position));

            gunTransform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(x, -y).normalized);
        }
        
    }



    IEnumerator Fire(Vector3 direction, Vector3 centerPos)
    {
        firing = true;

        timeSinceLastFired = 0f;
        shotsLeft -= 1f;

        if (shotsLeft < 1f)
            reloading = true;

        direction = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-accuracy, accuracy)) * direction;
        GameObject bulletObj = Instantiate(bulletPrefab, centerPos + direction * fireDistance, Quaternion.identity);
        bulletObj.GetComponent<Bullet>().Init(character.currentDimension, character, direction * bulletSpeed);
        gunTransform.GetComponent<Bounce>().StartBounce();

        yield return new WaitForSeconds(reloadSpeed);

        firing = false;

    }

}

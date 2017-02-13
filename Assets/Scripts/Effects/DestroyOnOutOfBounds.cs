using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnOutOfBounds : MonoBehaviour
{
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void Update ()
    {
        if (!MapManager.instance.bounds.Contains(_transform.position))
        {
            if (GetComponent<Bullet>() != null)
                GetComponent<Bullet>().DestroyBullet();
            else if (GetComponent<Character>() != null)
                GetComponent<Character>().HandleDeath();
            else
                Destroy(gameObject);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{

    public float timeToLive;

	void Update ()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0f)
            Destroy(gameObject);
	}
}

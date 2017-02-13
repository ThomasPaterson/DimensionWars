using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool indestructable;
    public int health;
    public Sprite[] destructionStages;
    public bool killsPlayer;
    public bool killsBullet = true;

    private SpriteRenderer rend;
    private Shake shake;
    private int x;
    private int y;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    private void Awake()
    {
        MapManager.instance.RegisterBrick(this);
        shake = GetComponentInChildren<Shake>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        MapManager.instance.UnregisterBrick(this);
    }

    public void TakeHit(Bullet bullet)
    {
        TakeHit();
    }

    public void TakeHit(Character character)
    {
        TakeHit();
    }

    public void TakeHit()
    {

        bool destroyed = false;
      

        if (!indestructable)
        {
            health -= GameStateManager.instance.currentMode.instantDestruction ? 1000 : 1;

            if (health <= 0)
            {
                destroyed = true;
                HandleDeath();
            }
            else
                rend.sprite = destructionStages[health - 1];
        }

        if (shake != null && !destroyed)
        {
            EffectsConfig.instance.HandleBlockShake(transform.position);
            shake.AddShake();
        }
    }

    public void ClearSpawn()
    {
        if (x != 0 && x != MapManager.instance.width - 1 && y != 0 && y != MapManager.instance.height - 1)
            Destroy(gameObject);
    }

    void HandleDeath()
    {
        EffectsConfig.instance.HandleBlockBreak(transform.position);
        Destroy(gameObject);
    }
}

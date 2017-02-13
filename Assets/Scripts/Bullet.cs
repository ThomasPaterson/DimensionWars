using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SpriteRenderer rend;
    public Character owner;
    public bool useGravity;
    public bool useSwitch;
    public bool useFade;
    public bool inheritVelocity;
    public float gravityMod;
    public float timeUntilFade;
    public float fadeDrag;
    public float velocityCutoff;
    public Sprite[] playerSprites;

    private Vector3 gravity;
    private Rigidbody2D rigid;
    private int currentDimension;
    private float timePassed;

	public void Init(int dimension, Character owner, Vector2 velocity)
    {
        rend.sprite = playerSprites[owner.controllingPlayer.playerNum];
        rend.transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity.normalized);
        this.owner = owner;
        this.rigid = GetComponent<Rigidbody2D>();
        UpdateDimension(dimension);

        if (inheritVelocity)
            rigid.velocity = velocity + owner.rigid.velocity;
        else
            rigid.velocity = velocity * GameStateManager.instance.currentMode.bulletSpeed;

        EffectsConfig.instance.FireBullet(transform.position, new Vector3(velocity.x, velocity.y), dimension);

    }

    private void Awake()
    {
        CharacterManager.instance.RegisterBullet(this);
        gameObject.AddComponent<DestroyOnOutOfBounds>();
    }

    private void FixedUpdate()
    {
        timePassed += Time.deltaTime;

        if (!GameStateManager.instance.currentMode.stopBulletFade && timePassed >= timeUntilFade)
            rigid.drag = fadeDrag;

        if (GameStateManager.instance.currentMode.useBulletGravity)
            rigid.AddForce(gravity, ForceMode2D.Force);

        if (!GameStateManager.instance.currentMode.stopBulletFade && rigid.velocity.magnitude <= velocityCutoff)
            DestroyBullet();

        if (useSwitch && owner != null && owner.currentDimension != currentDimension)
            UpdateDimension(owner.currentDimension);
    }

    private void OnDestroy()
    {
        CharacterManager.instance.UnregisterBullet(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.collider.gameObject.GetComponent<Character>();
        Brick brick = collision.collider.gameObject.GetComponent<Brick>();
        
        if (brick != null)
        {
            brick.TakeHit(this);

            if (brick.killsBullet)
                DestroyBullet();
            else
                RefreshBullet();
        }
        else if (character == null)
            DestroyBullet();
        else if (character != null && character != owner)
        {
            collision.collider.gameObject.GetComponent<Character>().HitByBullet(this);
            DestroyBullet();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Brick>() != null && !collision.gameObject.GetComponent<Brick>().killsBullet)
            RefreshBullet();
    }

    public void UpdateDimension(int dimension)
    {
        currentDimension = dimension;
        rend.color = DimensionConfig.instance.dimensions[dimension].color;
        gameObject.layer = LayerMask.NameToLayer(DimensionConfig.instance.dimensions[dimension].layerName);
        gravity = DimensionConfig.instance.dimensions[dimension].gravityDirection.normalized * (DimensionConfig.instance.gravity * gravityMod);
    }

    public void DestroyBullet()
    {
        EffectsConfig.instance.HandleBullet(transform.position, currentDimension);
        Destroy(gameObject);
    }

    void RefreshBullet()
    {
        rend.transform.rotation = Quaternion.FromToRotation(Vector3.right, rigid.velocity.normalized);
        timePassed = -timeUntilFade;
        rigid.drag = 0f;
    }


}

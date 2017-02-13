using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector2 gravityVector;
    public float speed;
    public float aerialSpeed;
    public float groundDrag;
    public float aerialDrag;
    public int currentDimension;
    public SpriteRenderer rend;
    public SpriteRenderer gunRend;
    public Sprite[] playerSprites;
    public Transform feet;
    public float rotationSpeed;
    public bool ignoreAerial = true;
    public float groundCheckDistance = 0.1f;
    public float groundTrail;
    public float groundCheckWidth = 0.9f;
    public GameObject directionArrow;
    public float lastFrameVelocity;


    public Rigidbody2D rigid { get; private set; }
    public Collider2D coll { get; private set; }
    public Player controllingPlayer { get; private set; }


    public bool onGround;
    private Vector3 targetRotation;
    public float timePassedInDimension;
    private TrailRenderer trail;
    private float trailTime;
    private CharacterSprites characterSprites;
    private CharacterAttackHandler attackHandler;

    private void Awake()
    {
        attackHandler = GetComponent<CharacterAttackHandler>();
        rigid = GetComponentInChildren<Rigidbody2D>();
        coll = GetComponentInChildren<Collider2D>();
        targetRotation = transform.rotation.eulerAngles;
        CharacterManager.instance.RegisterCharacter(this);
        gameObject.AddComponent<DestroyOnOutOfBounds>();
        trail = GetComponentInChildren<TrailRenderer>();
        trailTime = trail.time;
        
        
    }

    private void OnDestroy()
    {
        if (CharacterManager.instance != null)
            CharacterManager.instance.UnregisterCharacter(this);
    }

    public void Init(Player controller)
    {
        this.controllingPlayer = controller;
        characterSprites = CharacterManager.instance.characterSprites[controllingPlayer.playerNum];
        UpdateDimension(controller.startDimension);
    }

    private void Update()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotationSpeed);

        if (!GameStateManager.instance.playing)
            return;

        gunRend.enabled = !attackHandler.reloading && !GameStateManager.instance.currentMode.noFiring;
        timePassedInDimension += Time.deltaTime;
        // float x = Input.GetAxis("Horizontal" + player.faction.ToString());
        // float y = Input.GetAxis("Vertical" + player.faction.ToString());
        onGround = CheckOnGround();

        if (onGround || !GameStateManager.instance.currentMode.stopMidAirJump)
        {
            if (Input.GetButtonDown("Dimension0_" + controllingPlayer.playerNum.ToString()))
                UpdateDimension(0);
            else if (Input.GetButtonDown("Dimension1_" + controllingPlayer.playerNum.ToString()))
                UpdateDimension(1);
            else if (Input.GetButtonDown("Dimension2_" + controllingPlayer.playerNum.ToString()))
                UpdateDimension(2);
            else if (Input.GetButtonDown("Dimension3_" + controllingPlayer.playerNum.ToString()))
                UpdateDimension(3);

            onGround = CheckOnGround();
        }

        trail.time = onGround ? trailTime * groundTrail : trailTime;

        float x = Input.GetAxis("Horizontal" + controllingPlayer.playerNum.ToString());
        float y = Input.GetAxis("Vertical" + controllingPlayer.playerNum.ToString());

        float speedToUse = onGround ? speed : aerialSpeed;
        rigid.drag = onGround ? groundDrag : aerialDrag;

        // if (onGround)
        // {
        if (gravityVector.x == 0f)
            rigid.velocity = rigid.velocity + new Vector2(x * speedToUse * Time.deltaTime, 0f);
        else
            rigid.velocity = rigid.velocity + new Vector2(0f, y * speedToUse * Time.deltaTime);
        // }
        // else
        //    rigid.velocity = rigid.velocity + new Vector2(x * speedToUse * Time.deltaTime, y * speedToUse * Time.deltaTime);

        if (GameStateManager.instance.currentMode.killOnNotSwitch)
        {
            if (timePassedInDimension >= CharacterManager.instance.maxDimensionalShiftTime)
            {
                HandleDeath();
            }
            else
            {
                if (timePassedInDimension > CharacterManager.instance.timeToShowMax)
                {
                    float percent = (timePassedInDimension - CharacterManager.instance.timeToShowMax) / (CharacterManager.instance.maxDimensionalShiftTime - CharacterManager.instance.timeToShowMax);
                    rend.GetComponent<Shake>().shakeModifier = percent;
                    rend.GetComponent<Shake>().AddShake();
                }
            }
        }
   
    }



    private void FixedUpdate()
    {
        lastFrameVelocity = rigid.velocity.magnitude;
        rigid.AddForce(gravityVector.normalized * DimensionConfig.instance.gravity, ForceMode2D.Force);
        //rigid.velocity = rigid.velocity + gravityVector.normalized * DimensionConfig.instance.gravity * Time.deltaTime;
    }

    void UpdateDimension(int dimension)
    {
        bool jump = currentDimension == dimension && onGround;

        if (!jump)
            timePassedInDimension = 0f;

        currentDimension = dimension;
        gravityVector = DimensionConfig.instance.dimensions[currentDimension].gravityDirection;
        targetRotation = DimensionConfig.instance.dimensions[currentDimension].eulerAngles;
        //rend.color = DimensionConfig.instance.dimensions[currentDimension].color;
        gameObject.layer = LayerMask.NameToLayer(DimensionConfig.instance.dimensions[currentDimension].layerName);
        trail.startColor = DimensionConfig.instance.dimensions[currentDimension].color;
        trail.endColor = DimensionConfig.instance.dimensions[currentDimension].color;
        rend.sprite = characterSprites.dimensionSprites[dimension];
        gunRend.sprite = characterSprites.gunSprites[dimension];
        EffectsConfig.instance.HandleDimensionalShift(transform.position, dimension);
        directionArrow.gameObject.GetComponent<PulseThenDisable>().Pulse();
        directionArrow.GetComponent<SpriteRenderer>().color = DimensionConfig.instance.dimensions[currentDimension].color;

        if (jump)
            rigid.AddForce(gravityVector * CharacterManager.instance.jumpForce, ForceMode2D.Impulse);
    }

    bool CheckOnGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(feet.transform.position, coll.bounds.extents.x * groundCheckWidth, gravityVector, groundCheckDistance, LayerConfig.instance.terrainLayers);
        return hit.collider != null;
    }


    public void HitByBullet(Bullet bullet)
    {
        HandleDeath();
    }

    public void HitByCharacter(Character attacker)
    {
        HandleDeath();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Character>() != null)
        {
            if (collision.relativeVelocity.magnitude > CharacterManager.instance.minSpeedForKill && lastFrameVelocity > CharacterManager.instance.minSpeedForKill)
            {
                Vector3 directionTowardsFeet = (feet.transform.position - transform.position).normalized;
                Vector3 directionTowardsColl = (collision.collider.transform.position - transform.position).normalized;

                if (Vector3.Dot(directionTowardsFeet, directionTowardsColl) > CharacterManager.instance.dotForFootHit)
                {
                    collision.gameObject.GetComponent<Character>().HitByCharacter(this);
                    EffectsConfig.instance.HandlePlayerStomp(feet.transform.position, directionTowardsFeet, currentDimension);
                }
            }
        }
        else if (collision.gameObject.GetComponent<Brick>() != null)
        {
            if (collision.gameObject.GetComponent<Brick>().killsPlayer)
                HandleDeath();
            else
            {
                if (collision.relativeVelocity.magnitude > CharacterManager.instance.minSpeedForKill)
                {
                    Vector3 directionTowardsFeet = (feet.transform.position - transform.position).normalized;
                    Vector3 directionTowardsColl = (collision.collider.transform.position - transform.position).normalized;

                    if (Vector3.Dot(directionTowardsFeet, directionTowardsColl) > CharacterManager.instance.dotForFootHit && Vector3.Dot(directionTowardsFeet, collision.relativeVelocity.normalized) > CharacterManager.instance.dotForFootHit)
                    {
                        collision.gameObject.GetComponent<Brick>().TakeHit(this);
                        EffectsConfig.instance.HandleHardLanding(feet.transform.position, directionTowardsFeet, currentDimension);
                    }
                }
            }
           
        }
    }

    public void HandleDeath()
    {
        EffectsConfig.instance.HandleCharacterDeath(transform.position, currentDimension);
        Destroy(gameObject);
        Camera.main.GetComponent<Shake>().AddShake();
    }

}

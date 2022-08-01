using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private enum State
    {
        NORMAL,
        ATTACKING,
    }
    private State state;

    //player parameters
    public float moveSpeed = 5f;
    public float attackOffset = 3f;
    public float attackRange = 4f;
    public int lightAttackDamage = 10;
    public float skillRange = 10f;

    public float attackDelay = .7f;
    public bool isAttacking = false;
    public static Player instance;

    public int maxHP;
    public int currentHP;
    public int maxHack;
    public int currentHack = 0;

    public AbilitySO AOEStun;
    private bool canUseAOEStun = true;
    public AbilitySO AOEDamage;
    private bool canUseAOEDamage = true;
    public AbilitySO invisibility;
    private bool canUseInvisibility = true;
    public AbilitySO assasination;
    private bool canUseAssasination = true;

    private bool isInvisible = false;
    private float invisibilityTimer;
    private float invisibilityDuration;


    // sys parameters
    public Rigidbody2D rb;
    public GameObject CircleAOETwoX;
    public HudController hudController;
    private bool isSkillInUse = false;
    private bool AOEAlreadyInstantiated = false;
    GameObject AOEIndicator;
    Vector2 movement;
    Enemy targetEnemy;
    KeyCode usedSkillKey;

    //temp paramaters
    private float timer;
    
    //Animator parameters
    public Animator anim;
    private static readonly int Running = Animator.StringToHash("Running");
    
    //Particle Parameters
    public ParticleSystem particleSystem;

    private AudioSource source;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHP = maxHP;
        rb = gameObject.GetComponent<Rigidbody2D>();
        state = State.NORMAL;
        anim = GetComponent<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        hudController = FindObjectOfType<HudController>();
        hudController.SetSliders(currentHP, maxHP, currentHack, maxHack);
    }
    void Update()
    {//Input here
        if (isInvisible)
        {
            if(Time.time - invisibilityTimer >= invisibilityDuration)
            {
                invisibility.UseSkill();
                isInvisible = false;
            }
        }
        if (!isSkillInUse)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!canUseAOEStun)
                    return;
                usedSkillKey = KeyCode.Q;
                isSkillInUse = true;
                canUseAOEStun = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!canUseAOEDamage)
                    return;
                usedSkillKey = KeyCode.E;
                isSkillInUse = true;
                canUseAOEDamage = false;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (!canUseInvisibility)
                    return;
                usedSkillKey = KeyCode.R;
                isSkillInUse = true;
                canUseInvisibility = false;

            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (!canUseAssasination)
                    return;
                usedSkillKey = KeyCode.F;
                isSkillInUse = true;
                canUseAssasination = false;
            }
        }

        else
        {
            switch (usedSkillKey)
            {
                case KeyCode.Q:
                    if (!AOEAlreadyInstantiated)
                        AOEInstantiate();

                    AOEFollowMouse(skillRange);
                    if (Input.GetMouseButtonDown(1))
                    {
                        ExecuteSkill("Q");
                    }
                        
                    break;
                case KeyCode.E:
                    if(!AOEAlreadyInstantiated)
                        AOEInstantiate();

                    AOEFollowMouse(skillRange);
                    if (Input.GetMouseButtonDown(1))
                    {
                        ExecuteSkill("E");
                    }
                    break;
                case KeyCode.R:
                    ExecuteSkill("R");
                    break;
                case KeyCode.F:
                    ExecuteSkill("F");
                    break;
                default:
                    break;
            }
            AOEFollowMouse(skillRange);
            
        }

        GetMovementInput();
        if (movement.magnitude > 0.1f)
        {
            anim.SetBool(Running, true);
            particleSystem.Play();
        }
        else if(movement.magnitude < 0.1f)
        {
            anim.SetBool(Running, false);
        }
        HandleAttack();
        
    }
    private void FixedUpdate()
    {//Movement here
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    private void GetMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x > .1f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement.x < -.1f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        /*if (movement.x > 0)
            anim.SetBool("Right", true);
        else if(movement.x < 0)
            anim.SetBool("Right", false);*/
    }

    private void AOEInstantiate()
    {
        AOEIndicator = Instantiate(CircleAOETwoX, Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.rotation);
        AOEAlreadyInstantiated = true;
    }
    private void AOEFollowMouse(float skillRange)
    {
        if (AOEIndicator == null)
            return;
        /*if (Vector3.Distance(gameObject.transform.position, Input.mousePosition) >= skillRange)
            return;*/
        AOEIndicator.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
    }
    private void ExecuteSkill(string key)
    {
        if(key == "Q")
        {
            AOEStun.UseSkill();
            Destroy(AOEIndicator);
            AOEAlreadyInstantiated = false;
        }
        else if(key == "E")
        {
            AOEDamage.UseSkill();
            Destroy(AOEIndicator);
            AOEAlreadyInstantiated = false;
        }
        else if(key == "R")
        {
            invisibility.UseSkill();
        }
        else if(key == "F")
        {
            assasination.UseSkill();
        }
        isSkillInUse = false;
    }
    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            //StartCoroutine(BladeSound());
            
            isAttacking = true;
            particleSystem.Play();
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //get mouse position
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
            
            Vector3 attackDirection = (mousePosition - transform.position).normalized; // get attack direction
            Vector3 attackPosition = transform.position + attackDirection * attackOffset;
            
            targetEnemy = Enemy.GetClosestEnemy(attackPosition, attackRange); //Pick enemy through its script
            
            if(targetEnemy != null)
            {//we have a valid enemy
                targetEnemy.Damage(lightAttackDamage);
                targetEnemy.GetComponent<Rigidbody2D>().AddForce(transform.forward*100f);
            }

            state = State.ATTACKING;
            timer = Time.time;
        }
    }

    /*IEnumerator BladeSound()
    {
        yield return new WaitForSeconds(.1f);
        //source.Play();
    }*/
    
    public void Damage(int dmg)
    {
        currentHP -= dmg;
        if(currentHP <= 0)
        {
            Die();
        }
        CombatManager.PlayerTookDamage();
        hudController.SetHealthBarValue(currentHP);
    }
    public GameObject GetAOEIndicator()
    {
        return AOEIndicator;
    }
    public void Die()
    {
        SceneManager.LoadScene("Aybars Level Generator");
    }
    public void SetCanUseSkills(string skill)
    {
        switch (skill)
        {
            case "AOEStun":
                canUseAOEStun = true;
                break;
            case "AOEDamage":
                canUseAOEDamage = true;
                break;
            case "invisibility":
                canUseInvisibility = true;
                break;
            case "assasination":
                canUseAssasination = true;
                break;
        }
    }
    public void SetInvisibility(bool newBool)
    {
        if (newBool)
            invisibilityTimer = Time.time;
        isInvisible = newBool;
    }
    public void SetInvisibilityDuration(float duration)
    {
        invisibilityDuration = duration;
    }
    public void SetLightAttackDamage(int newDamage)
    {
        lightAttackDamage = newDamage;
    }
    public bool GetIsInvisible()
    {
        return isInvisible;
    }
    public float GetStunSkillCooldown()
    {
        return AOEStun.GetCooldown();
    }
    public float GetDamageSkillCooldown()
    {
        return AOEDamage.GetCooldown();
    }
    public float GetInvisibilitySkillCooldown()
    {
        return invisibility.GetCooldown();
    }
    public float GetAssasinationSkillCooldown()
    {
        return assasination.GetCooldown();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<DamageDealer>() != null)
        {
            DamageDealer projectile = collision.gameObject.GetComponent<DamageDealer>();
            Damage(projectile.GetDamage());
            Destroy(collision.gameObject);
        }
    }
}

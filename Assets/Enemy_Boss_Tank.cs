using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Tank : MonoBehaviour
{
    public static Player player;
    public Enemy thisEnemy;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    public float moveSpeed;
    public float agroRange = 20f;
    public float minMoveRange = 1.5f;
    public float attackRange = 2f;

    private float roamingTimer = -10f;
    private float roamingDuration = 5f;

    private float timeBetweenAttacks = 1f;
    private float attackTimer = 0f;
    public int attackDamage;
    private bool isFirstAttack = true;
    private bool wasOutOfRange = true;

    private Animator anim;
    private bool firstPunch = false;
    
    private void Start()
    {
        thisEnemy = gameObject.transform.GetComponent<Enemy>();
        if (player == null)
            player = FindObjectOfType<Player>();
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();

        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (thisEnemy.GetIsStunned() || player.GetIsInvisible())
        {
            return;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < agroRange)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > minMoveRange)
            {//yürüme animasyonu#1
                MoveTo(player.transform.position);
                wasOutOfRange = true;
                anim.SetBool("Running", true);
            }
            else
            {
                HandleAttack();
            }
        }
        else
        {
            HandleRoaming();
        }
        
        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        
    }

    private void HandleRoaming()
    {
        if (Time.time - roamingTimer > roamingDuration)
        {
            roamingTimer = Time.time;
            roamPosition = GetRoamingPosition();
        }
        else
        {//yürüme animasyonu#2
            MoveTo(roamPosition);
            anim.SetBool("Running", true);
        }
    }
    private void HandleAttack()
    {
        if (isFirstAttack || wasOutOfRange)
        {
            anim.SetBool("Running", false);
            attackTimer = Time.time;
            isFirstAttack = false;
            wasOutOfRange = false;
        }
        if(Time.time - attackTimer >= timeBetweenAttacks)
        {
            anim.SetBool("Running", false);
            if(Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {//attack animasyonu buraya gelecek muhtemelen
                anim.SetTrigger("AttackLeft");
                player.Damage(attackDamage);
                attackTimer = Time.time;
            }
        }
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f), 0f).normalized;
        return startingPosition + randomDirection * Random.Range(10f, 70f);
    }

    private void MoveTo(Vector3 targetPosition)
    {
        anim.SetBool("Running", true);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCop : MonoBehaviour
{
    public DamageDealer projectile;
    public static Player player;
    public Enemy thisEnemy;

    public float attackRange = 5f;
    public float moveSpeed = 3.5f;
    private int attackingHand = -1;

    public Transform firstMuzzleLocation;
    public Transform secondMuzzleLocation;
    private bool firstMuzzleShot = false;

    private float timeBetweenShots = 0.9f;
    private float initialShotTime = 0f;

    private AudioSource audioSource;
    private AudioClip gunShotClip;

    private Animator anim;
    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
        thisEnemy = FindObjectOfType<Enemy>();

        audioSource = GetComponent<AudioSource>();
        gunShotClip = Resources.Load("SystemCops_FireSound") as AudioClip;
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        
        if (thisEnemy.GetIsStunned() || player.GetIsInvisible())
        {
            return;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            if (initialShotTime == 0)
                initialShotTime = Time.time;
            CountDownAndShoot();
        }
        else
        {
            MoveTo(player.transform.position);
            anim.SetBool("Running", true);
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
    private void CountDownAndShoot()
    {
        if (Time.time - initialShotTime >= timeBetweenShots)
        {
            
            Debug.Log("here2");
            Fire();
            initialShotTime = Time.time;
        }
    }
    private void Fire()
    {
        if (!firstMuzzleShot)
        {
            anim.SetBool("Running", false);
            anim.SetTrigger("AttackRight");
            DamageDealer enemyRangedProjectile = Instantiate(projectile, firstMuzzleLocation.position, Quaternion.identity);
            enemyRangedProjectile.SetTarget(player.transform.position);
            // audioSource.PlayOneShot(gunShotClip);
            firstMuzzleShot = true;
        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetTrigger("AttackLeft");
            DamageDealer enemyRangedProjectile = Instantiate(projectile, secondMuzzleLocation.position, Quaternion.identity);
            enemyRangedProjectile.SetTarget(player.transform.position);
            // audioSource.PlayOneShot(gunShotClip);
            firstMuzzleShot = false;
        }
        
    }
    private void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        anim.SetBool("Running", true);
        
    }
}

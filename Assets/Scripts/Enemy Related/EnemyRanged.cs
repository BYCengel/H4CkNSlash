using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    //public static Player player;
    public Enemy thisEnemy;
    public DamageDealer projectile;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    public float moveSpeed;
    public float agroRange = 10f;
    public float minMoveRange = 5f;
    public float attackRange = 6f;

    private float roamingTimer = -10f;
    private float roamingDuration = 5f;

    private float timeBetweenShots = 1.5f;
    public float initialShotTime = 0f;
    public int attackDamage;

    private Animator anim;
    [SerializeField] private GameObject player;
    private Player playerScript;

    private void Start()
    {
        player = PlayerManager.instance.player;
        playerScript = player.GetComponent<Player>();
        thisEnemy = gameObject.transform.GetComponent<Enemy>();
        /*if (player == null)
            player = FindObjectOfType<Player>();*/
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();

        initialShotTime = 0;

        anim = GetComponentInChildren<Animator>();
    }
    private void FixedUpdate()
    {
        if (thisEnemy.GetIsStunned() || playerScript.GetIsInvisible())
        {
            return;
        }
        if(Vector2.Distance(player.transform.position, transform.position) < attackRange)
        {
            if (initialShotTime == 0)
                initialShotTime = Time.time;
            CountDownAndShoot();
        }else if(Vector2.Distance(player.transform.position, transform.position) < agroRange)
        {
            MoveTo(player.transform.position);
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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
        DamageDealer enemyRangedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        //enemyRangedProjectile.SetTarget(player.transform.position);
    }
    
    
}

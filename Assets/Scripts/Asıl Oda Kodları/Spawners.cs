using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Spawners : MonoBehaviour
{
    public GameObject[] spawner;
    public GameObject[] doors;
    GameObject levelManager;
    private int roomEnemyCount = 0;
    private bool inFight = false;
    private int currentLevel;

    private AudioSource source;
    private AudioClip doorSound;

    private void Start()
    {
        levelManager = LevelManager.instance.level;
        currentLevel = levelManager.GetComponent<LevelGeneration>().level;

        source = PlayerManager.instance.player.GetComponent<AudioSource>();
        doorSound = Resources.Load("DoorLaserSound") as AudioClip;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if(levelManager.GetComponent<LevelGeneration>().firstRoom == false && inFight == false && currentLevel%5 == 0)
            {
                source.PlayOneShot(doorSound);
                //Debug.Log("BOSS ODA");
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }

                inFight = true;
                SpawnBoss();
            }
            //Debug.Log("FirstRoom " + levelManager.GetComponent<LevelGeneration>().firstRoom);
            else if (levelManager.GetComponent<LevelGeneration>().firstRoom == false && inFight == false)
            {
                source.PlayOneShot(doorSound);
                //Debug.Log("BAÞKA ODA");
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }

                inFight = true;
                SpawnEnemy();
            }
            else if (levelManager.GetComponent<LevelGeneration>().firstRoom)
            {
                
                //Debug.Log("ÝLK ODA");
                levelManager.GetComponent<LevelGeneration>().firstRoom = false;
                Destroy(gameObject);
            }
            
        }
    }

    void SpawnEnemy()
    {
        //Debug.Log("SPAWNENEMY " + transform.parent.name);
        int enemyNumber = Random.Range(3, 6);
        roomEnemyCount = enemyNumber;
        for (int i = 0; i < enemyNumber; i++)
        {
            int randSpawner = Random.Range(0, spawner.Length - 1);
            int randomEnemy = Random.Range(0, 2);
            Object.Instantiate(LevelManager.instance.enemy[randomEnemy], spawner[randSpawner].transform.position, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        int randSpawner = 0;
        int randomEnemy = 0;
        int enemyNumber = Random.Range(2, 4);
        roomEnemyCount = enemyNumber;
        for (int i = 0; i < enemyNumber; i++)
        {
            randSpawner = Random.Range(0, spawner.Length - 1);
            randomEnemy = Random.Range(0, 1);
            Object.Instantiate(LevelManager.instance.enemy[randomEnemy], spawner[randSpawner].transform.position, Quaternion.identity);
        }
        Object.Instantiate(LevelManager.instance.boss, spawner[randSpawner].transform.position, Quaternion.identity);
    }

    void SpawnCop()
    {
        int randSpawner = Random.Range(0, spawner.Length - 1);
        Instantiate(LevelManager.instance.police, spawner[randSpawner].transform.position, Quaternion.identity);
        roomEnemyCount += 1;
    }
    
    private void FixedUpdate()
    {
        if (inFight && levelManager.GetComponent<LevelGeneration>().copCanSpawn)
        {
            SpawnCop();
            levelManager.GetComponent<LevelGeneration>().copCanSpawn = false;
        }
        
        if (levelManager.GetComponent<LevelGeneration>().roomKill >= roomEnemyCount && inFight)
        {
            levelManager.GetComponent<LevelGeneration>().roomKill = 0;
            levelManager.GetComponent<LevelGeneration>().levelRoomCount += 1;
            inFight = false;
            Destroy(gameObject);
        }

    }

    /*void EndFight()
    {
        Destroy(gameObject);
    }*/
    
}
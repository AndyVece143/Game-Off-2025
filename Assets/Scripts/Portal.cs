using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameManager manager;
    [SerializeField] private float respawnTimer;
    private float respawnTimerMax;
    public Enemy enemy;
    private float enemiesSpawned;
    public bool coolDown;

    [SerializeField] private float coolDownTimer;
    private float coolDownTimerMax;

    public List<Enemy> enemyList = new List<Enemy>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnTimerMax = respawnTimer;
        coolDownTimerMax = coolDownTimer;
}

    // Update is called once per frame
    void Update()
    {
        if (manager.inBattle == true)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0 && !coolDown)
            {
                SpawnEnemy();
                respawnTimer = respawnTimerMax;
            }
            if (enemiesSpawned >= 5)
            {
                coolDown = true;
                enemiesSpawned = 0;
            }
            if (coolDown)
            {
                coolDownTimer -= Time.deltaTime;
                if (coolDownTimer <= 0)
                {
                    coolDown = false;
                    coolDownTimer = coolDownTimerMax;
                }
            }
        }

        if (manager.inBattle == false && enemyList.Count > 0)
        {
            try
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].Death();
                    enemyList.RemoveAt(i);
                }
            }
            catch { }
 
        }

    }

    void SpawnEnemy()
    {
        Enemy newEnemy = Instantiate(enemy, gameObject.transform);
        enemyList.Add(newEnemy);
        enemiesSpawned++;
    }
}

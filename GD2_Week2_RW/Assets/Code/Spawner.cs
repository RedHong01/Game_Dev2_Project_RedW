using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 引入 SceneManager

public class Spawner : MonoBehaviour
{
    public Turn[] turns;
    public Enemy enemy;
    public string nextSceneName;  // 新的场景名称

    Turn currentTurn;
    int currentTurnNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    void Start()
    {
        NextTurn();
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentTurn.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        // 减少活着的敌人数量
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            NextTurn();  // 当所有敌人都被杀死时进入下一回合
        }
    }

    void NextTurn()
    {
        currentTurnNumber++;

        // 检查是否已经完成所有回合
        if (currentTurnNumber - 1 < turns.Length)
        {
            currentTurn = turns[currentTurnNumber - 1];

            enemiesRemainingToSpawn = currentTurn.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
        else
        {
            // 当所有回合结束时，跳转到新的场景
            SceneManager.LoadScene(nextSceneName);
        }
    }

    [System.Serializable]
    public class Turn
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}

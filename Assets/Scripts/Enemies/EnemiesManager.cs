using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<Enemy> enemies = new();

    [SerializeField] float decreaseAmount;

    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    float timer;

    [SerializeField] Transform topCorner;
    [SerializeField] Transform bottomCorner;

    [SerializeField] Transform bulletsParent;



    [System.Serializable]
    public struct EnemyStruct
    {
        public string name;
        public GameObject enemyGO;
    }

    [SerializeField] List<EnemyStruct> enemyList;

    public static EnemiesManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        minSpawnTime = Mathf.Clamp(minSpawnTime - decreaseAmount * Time.deltaTime, .05f, 100f);
        maxSpawnTime = Mathf.Clamp(maxSpawnTime - decreaseAmount * Time.deltaTime, .2f, 100f);

        if (timer < 0f)
        {
            timer = Random.Range(minSpawnTime, maxSpawnTime);

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        EnemyStruct enemyToSpawn = enemyList[Random.Range(0, enemyList.Count)];
        GameObject enemy = Instantiate(enemyToSpawn.enemyGO, transform);
        enemy.transform.position = Vector2.Lerp(topCorner.position, bottomCorner.position, Random.Range(0f, 1f));

        enemy.GetComponent<Enemy>().Init(this, bulletsParent);
        AddEnemy(enemy.GetComponent<Enemy>());
    }

    public Enemy GetClosestEnemy(Vector2 pos)
    {
        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies) 
        {
            if (enemy != null && (closestEnemy == null || Vector2.Distance(enemy.transform.position, pos) < Vector2.Distance(closestEnemy.transform.position, pos)))
            {
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}

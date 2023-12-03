using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected bool canMove = true;

    protected Vector2 movementDir = Vector2.left;

    [SerializeField] protected float speed;

    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected float life;

    

    protected ShipPart target;

    protected PartsManager partsManager;

    protected EnemiesManager enemiesManager;

    public Transform bulletsParent;

    

    [System.Serializable]
    public struct EnemyLoot
    {
        public GameObject lootGO;
        public float lootChance;
    }

    [Header("Loot")]

    [SerializeField] List<EnemyLoot> lootTable = new();

    [SerializeField] int minLootAmount;
    [SerializeField] int maxLootAmount;

    [SerializeField] float lootSpeed;

    protected void Start()
    {
        partsManager = PartsManager.instance;


        target = partsManager.GetClosestNonDestroyPart(transform.position);
    }

    public void Init(EnemiesManager em, Transform bulletsP)
    {
        enemiesManager = em;
        bulletsParent = bulletsP;
    }

    protected void Update()
    {
        target = partsManager.GetClosestNonDestroyPart(transform.position);

        if (canMove && target != null)
        {
            movementDir = target.transform.position - transform.position;

            rb.AddForce(movementDir * speed * Time.deltaTime);
        }
            
    }

    public bool TakeDamage(float damage)
    {
        if(life <= 0)
            return false;

        life -= damage;

        if (life <= 0)
            Kill();
        return true;
    }

    private void Kill()
    {
        SpawnLoot();
        Destroy(gameObject);
    }

    private void SpawnLoot()
    {
        int lootAmount = Random.Range(minLootAmount, maxLootAmount + 1);

        int looted = 0;

        while (looted < lootAmount)
        {
            int random = Random.Range(0, lootTable.Count);
            float chance = Random.Range(0f, 100f);

            if (chance < lootTable[random].lootChance)
            {
                looted++;
                GameObject go = Instantiate(lootTable[random].lootGO);
                go.transform.position = transform.position;
                go.GetComponent<Rigidbody2D>().AddForce(new(Random.Range(-1f, 1f) * lootSpeed, Random.Range(-1f, 1f) * lootSpeed), ForceMode2D.Impulse);
            }
        }
    }
}

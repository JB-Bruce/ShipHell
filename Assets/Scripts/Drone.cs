using UnityEngine;

public class Drone : MonoBehaviour
{
    public bool available { get; private set; } = true;

    [SerializeField] AnimationCurve speed;
    [SerializeField] float speedMultiplier;

    Loot target = null;
    float distToTarget;

    [SerializeField] float distToPickup;
    bool pickedup = false;

    GameManager gm;


    private void Start()
    {
        gm = GameManager.instance;
    }


    public void SetTarget(Loot loot)
    {
        target = loot;
        available = false;
        pickedup = false;
        target.isTargeted = true;

        distToTarget = Vector2.Distance(transform.position, target.transform.position);

        Vector2 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        if(target != null)
        {
            if(pickedup)
            {
                float distBaseLeft = Vector2.Distance(transform.position, transform.parent.position);
                transform.position = Vector2.MoveTowards(transform.position, transform.parent.position, speed.Evaluate(distBaseLeft / distToTarget) * speedMultiplier * Time.deltaTime);

                if(Vector2.Distance(transform.position, transform.parent.position) < distToPickup)
                {
                    gm.AddRessources(target.lootType);
                    Destroy(target.gameObject);
                    target = null;
                    available = true;
                    transform.localPosition = Vector2.zero;
                    pickedup = false;

                    transform.rotation = Quaternion.Euler(0, 0, -90f);
                }

                return;
            }

            float distLeft = Vector2.Distance(transform.position, target.transform.position);

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed.Evaluate(distLeft / distToTarget) * speedMultiplier * Time.deltaTime);

            if(distLeft <= distToPickup)
            {
                target.transform.parent = transform;
                target.transform.localPosition = Vector2.zero;
                target.GetComponent<Collider2D>().enabled = false;
                pickedup = true;
                distToTarget = Vector2.Distance(transform.position, transform.parent.position);

                Vector2 dir = -transform.localPosition;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}

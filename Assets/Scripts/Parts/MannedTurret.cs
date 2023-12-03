using UnityEngine;

public class MannedTurret : Turret
{
    [SerializeField] bool semiAuto;
    
    Camera cam;

    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    private new void Update()
    {
        if (isBroken) return;

        base.Update();

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        canonPart.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && (shootTimer <= 0 || semiAuto))
        {
            shootTimer = bulletsPerSeconds;

            Shoot(dir.normalized, angle);
        }
    }
}

using UnityEngine;

public class Armor : ShipPart
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color brokeColor;

    [SerializeField] SpriteRenderer sp;

    protected override void Start()
    {
        base.Start();

        sp.color = baseColor;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        sp.color = Color.Lerp(baseColor, brokeColor, 1 - (partLife / baseLife));
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        sp.color = Color.Lerp(baseColor, brokeColor, 1 - (partLife / baseLife));
    }
}

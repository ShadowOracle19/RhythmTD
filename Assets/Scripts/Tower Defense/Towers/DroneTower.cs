using UnityEngine;

public class DroneTower : Tower
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void CreateBullet(int damage, Vector3 position)
    {
        base.CreateBullet(damage, position);
    }
}

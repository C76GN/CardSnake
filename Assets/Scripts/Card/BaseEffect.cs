using UnityEngine;

public class BaseEffect : BaseCardEffect
{
    [Header("攻击卡牌配置")]
    public GameObject bulletPrefab; // 子弹预制体
    public float damage = 10f; // 子弹伤害
    public float range = 5f; // 攻击范围
    public Transform firePoint; // 发射子弹的位置

    public override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);

        // 查找范围内的敌人
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, range);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy")) // 假设敌人标签为 "Enemy"
            {
                // 发射子弹
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Initialize(enemy.transform, damage);
            }
        }
    }
}

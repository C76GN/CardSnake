using UnityEngine;

public class BaseEffect : BaseCardEffect
{
    [Header("������������")]
    public GameObject bulletPrefab; // �ӵ�Ԥ����
    public float damage = 10f; // �ӵ��˺�
    public float range = 5f; // ������Χ
    public Transform firePoint; // �����ӵ���λ��

    public override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);

        // ���ҷ�Χ�ڵĵ���
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, range);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy")) // ������˱�ǩΪ "Enemy"
            {
                // �����ӵ�
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Initialize(enemy.transform, damage);
            }
        }
    }
}

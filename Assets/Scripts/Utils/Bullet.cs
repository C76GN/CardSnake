using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float damage;
    public float speed = 10f;

    public void Initialize(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 如果目标消失，销毁子弹
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // 对敌人造成伤害
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class HealingEffect : BaseCardEffect
{
    [Header("������������")]
    public float recoveryBoost = 5f; // �����Ļָ�����

    public override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);

        // ��������Ļָ�����
        SnakeHealthManager healthManager = target.GetComponent<SnakeHealthManager>();
        if (healthManager != null)
        {
            healthManager.config.recoveryRate += recoveryBoost;
            Debug.Log($"HealingEffect �Ѽ���ָ�������� {recoveryBoost}��");
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        base.RemoveEffect(target);

        // �ָ�����Ļָ�����
        SnakeHealthManager healthManager = target.GetComponent<SnakeHealthManager>();
        if (healthManager != null)
        {
            healthManager.config.recoveryRate -= recoveryBoost;
            Debug.Log($"HealingEffect ���Ƴ����ָ����ʻָ�������");
        }
    }
}

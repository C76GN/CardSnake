using UnityEngine;

public class HealingEffect : BaseCardEffect
{
    [Header("辅助卡牌配置")]
    public float recoveryBoost = 5f; // 提升的恢复速率

    public override void ApplyEffect(GameObject target)
    {
        base.ApplyEffect(target);

        // 增加蛇体的恢复速率
        SnakeHealthManager healthManager = target.GetComponent<SnakeHealthManager>();
        if (healthManager != null)
        {
            healthManager.config.recoveryRate += recoveryBoost;
            Debug.Log($"HealingEffect 已激活，恢复速率提高 {recoveryBoost}！");
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        base.RemoveEffect(target);

        // 恢复蛇体的恢复速率
        SnakeHealthManager healthManager = target.GetComponent<SnakeHealthManager>();
        if (healthManager != null)
        {
            healthManager.config.recoveryRate -= recoveryBoost;
            Debug.Log($"HealingEffect 已移除，恢复速率恢复正常。");
        }
    }
}

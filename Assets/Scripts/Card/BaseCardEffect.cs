using UnityEngine;

public abstract class BaseCardEffect : MonoBehaviour, ICardEffect
{
    [Header("卡牌效果基础配置")]
    public string effectName; // 效果名称
    public string description; // 效果描述
    public float duration; // 效果持续时间（秒）

    private float startTime; // 效果开始时间

    // 应用效果时记录开始时间
    public virtual void ApplyEffect(GameObject target)
    {
        startTime = Time.time;
        Debug.Log($"效果 {effectName} 已应用，描述：{description}");
    }

    // 移除效果
    public virtual void RemoveEffect(GameObject target)
    {
        Debug.Log($"效果 {effectName} 已移除。");
    }

    // 检查效果是否已过期
    public bool IsExpired()
    {
        return Time.time - startTime >= duration;
    }
}

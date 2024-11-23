using System.Collections.Generic;
using UnityEngine;
using static ListExtensions;

// 负责管理蛇身体各部位的健康状态，包括受伤和恢复
public class SnakeHealthManager : MonoBehaviour
{
    // 身体部位健康值列表
    private List<float> bodyPartHealth = new List<float>();

    // 每个身体部位上一次受伤的时间戳
    private List<float> lastDamageTime = new List<float>();

    // 每个身体部位的损坏状态（是否已经损坏）
    private List<bool> bodyPartDamaged = new List<bool>();

    // 配置文件，包含健康管理所需的参数
    public SnakeConfig config;

    /// <summary>
    /// 初始化身体部位健康状态
    /// </summary>
    /// <param name="bodyPartCount">身体部位的数量</param>
    public void Initialize(int bodyPartCount)
    {
        if (config == null)
        {
            Debug.LogError("未设置 SnakeConfig，无法初始化健康管理器！");
            return;
        }

        if (bodyPartCount <= 0)
        {
            if (bodyPartHealth.Count > 0)
            {
                Debug.LogWarning("身体部位数量为 0，健康状态被清空！");
            }
            bodyPartHealth.Clear();
            lastDamageTime.Clear();
            bodyPartDamaged.Clear();
            return;
        }

        // 调整健康相关列表的大小
        bodyPartHealth.Resize(bodyPartCount, config.initialHealth);
        lastDamageTime.Resize(bodyPartCount, 0f);
        bodyPartDamaged.Resize(bodyPartCount, false);

        Debug.Log($"健康状态已初始化，身体部位总数: {bodyPartCount}, 健康列表长度: {bodyPartHealth.Count}");
    }

    /// <summary>
    /// 触发健康变化事件，通知外部模块更新状态
    /// </summary>
    /// <param name="index">发生变化的身体部位索引</param>
    private void NotifyHealthChanged(int index)
    {
        EventManager.TriggerEvent("OnHealthChanged", new { Index = index, Health = bodyPartHealth[index] });
    }

    /// <summary>
    /// 获取当前身体部位总数
    /// </summary>
    public int PartCount => bodyPartHealth.Count;

    /// <summary>
    /// 为指定的身体部位造成伤害
    /// </summary>
    /// <param name="index">身体部位的索引</param>
    /// <param name="damage">造成的伤害值</param>
    public void TakeDamage(int index, float damage)
    {
        if (index < 0 || index >= bodyPartHealth.Count) return;

        // 如果该部位已经损坏，则不再处理伤害
        if (bodyPartDamaged[index]) return;

        // 减少健康值并记录受伤时间
        bodyPartHealth[index] -= damage;
        lastDamageTime[index] = Time.time;

        // 通知健康状态变化
        NotifyHealthChanged(index);

        // 如果健康值降到 0 或以下，将该部位标记为损坏
        if (bodyPartHealth[index] <= 0)
        {
            bodyPartHealth[index] = 0;
            bodyPartDamaged[index] = true;
            EventManager.TriggerEvent("OnBodyPartDamaged", index);
        }

        Debug.Log($"身体部位 {index} 受到 {damage} 点伤害，剩余健康值: {bodyPartHealth[index]}");
    }

    /// <summary>
    /// 更新所有身体部位的健康状态，用于处理恢复机制
    /// </summary>
    /// <param name="deltaTime">时间增量，用于计算恢复量</param>
    public void UpdateHealth(float deltaTime)
    {
        for (int i = 0; i < bodyPartHealth.Count; i++)
        {
            // 恢复阈值为初始健康值的百分比
            float actualRecoveryThreshold = config.initialHealth * config.recoveryThreshold;

            // 如果健康值低于恢复阈值并且超过了恢复延迟时间，则进行恢复
            if (bodyPartHealth[i] < actualRecoveryThreshold &&
                Time.time - lastDamageTime[i] >= config.recoveryDelay)
            {
                bodyPartHealth[i] += config.recoveryRate * deltaTime;

                // 确保健康值不会超过恢复阈值
                if (bodyPartHealth[i] > actualRecoveryThreshold)
                {
                    bodyPartHealth[i] = actualRecoveryThreshold;
                }
            }
        }
    }

    /// <summary>
    /// 获取指定身体部位的当前健康值
    /// </summary>
    /// <param name="index">身体部位的索引</param>
    /// <returns>健康值</returns>
    public float GetHealth(int index)
    {
        if (index < 0 || index >= bodyPartHealth.Count)
        {
            Debug.LogError($"无效的身体部位索引: {index}。健康列表长度为: {bodyPartHealth.Count}");
            return 0f;
        }

        return bodyPartHealth[index];
    }
}
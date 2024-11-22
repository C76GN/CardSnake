using System.Collections.Generic; // 引入使用列表所需的命名空间
using UnityEngine;

// 管理蛇的健康状态，处理蛇身体部位的损伤和恢复
public class SnakeHealthManager : MonoBehaviour
{
    // 存储每个身体部分的健康值
    private List<float> bodyPartHealth = new List<float>();

    // 存储每个身体部分最近一次受到伤害的时间
    private List<float> lastDamageTime = new List<float>();

    // 标记每个身体部分是否已经损坏
    private List<bool> bodyPartDamaged = new List<bool>();

    // 配置文件，用于设置恢复速率和延迟等参数
    public SnakeConfig config;

    // 初始化身体部位的健康状态
    public void Initialize(int bodyPartCount)
    {
        if (config == null)
        {
            Debug.LogError("SnakeConfig 未正确传递到 SnakeHealthManager！");
            return;
        }

        if (bodyPartCount <= 0)
        {
            // 如果身体段数量为 0，清空健康列表，并避免重复打印警告
            if (bodyPartHealth.Count > 0)
            {
                Debug.LogWarning("身体段数量为 0，无法初始化健康列表！");
            }
            bodyPartHealth.Clear();
            lastDamageTime.Clear();
            bodyPartDamaged.Clear();
            return;
        }

        // 调整列表大小
        AdjustListSize(bodyPartHealth, bodyPartCount, config.initialHealth);
        AdjustListSize(lastDamageTime, bodyPartCount, 0f);
        AdjustListSize(bodyPartDamaged, bodyPartCount, false);

        Debug.Log($"健康列表已初始化，当前身体段数量: {bodyPartCount}, 健康列表长度: {bodyPartHealth.Count}");
    }

    public int PartCount
    {
        get
        {
            // 返回健康列表的数量
            return bodyPartHealth.Count;
        }
    }

    // 调整列表大小的通用方法
    private void AdjustListSize<T>(List<T> list, int targetCount, T defaultValue)
    {
        // 如果列表当前长度不足，则添加默认值
        while (list.Count < targetCount)
        {
            list.Add(defaultValue);
        }

        // 如果列表当前长度超出目标，则移除多余部分
        while (list.Count > targetCount)
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    // 处理对指定身体部分造成的伤害
    public void TakeDamage(int index, float damage)
    {
        // 检查索引是否有效，防止访问越界
        if (index < 0 || index >= bodyPartHealth.Count) return;

        // 如果该身体部分已经损坏，则不再处理伤害
        if (bodyPartDamaged[index]) return;

        // 减少指定身体部分的健康值
        bodyPartHealth[index] -= damage;

        // 记录当前时间为该身体部分的最后受伤时间
        lastDamageTime[index] = Time.time;

        // 如果健康值降至 0 或以下，将该部分标记为损坏
        if (bodyPartHealth[index] <= 0)
        {
            bodyPartHealth[index] = 0;        // 确保健康值不会低于 0
            bodyPartDamaged[index] = true;    // 标记为损坏
        }

        Debug.Log($"身体段 {index} 受到 {damage} 点伤害，当前健康值: {bodyPartHealth[index]}");
    }

    // 更新身体部位的健康状态（用于恢复机制）
    public void UpdateHealth(float deltaTime)
    {
        // 遍历所有身体部分，检查是否需要恢复健康
        for (int i = 0; i < bodyPartHealth.Count; i++)
        {
            // 计算实际恢复阈值
            float actualRecoveryThreshold = config.initialHealth * config.recoveryThreshold;

            // 检查是否满足恢复条件（低于恢复阈值，并且经过了恢复延迟时间）
            if (bodyPartHealth[i] < actualRecoveryThreshold &&
                Time.time - lastDamageTime[i] >= config.recoveryDelay)
            {
                // 按照恢复速率增加健康值
                bodyPartHealth[i] += config.recoveryRate * deltaTime;

                // 确保健康值不会超过恢复阈值
                if (bodyPartHealth[i] > actualRecoveryThreshold)
                {
                    bodyPartHealth[i] = actualRecoveryThreshold;
                }
            }
        }
    }

    // 获取指定身体段的健康值
    public float GetHealth(int index)
    {
        if (index < 0 || index >= bodyPartHealth.Count)
        {
            Debug.LogError($"无效的健康索引: {index}。当前健康列表长度: {bodyPartHealth.Count}");
            return 0f;
        }

        Debug.Log($"获取身体段 {index} 的健康值: {bodyPartHealth[index]}");
        return bodyPartHealth[index];
    }
}

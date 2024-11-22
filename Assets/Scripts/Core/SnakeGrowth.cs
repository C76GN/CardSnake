using System.Collections.Generic; // 引入集合库，用于管理身体部件列表
using UnityEngine;

// 管理蛇的生长、身体段添加和健康状态
public class SnakeGrowth : MonoBehaviour
{
    // 蛇的配置参数，例如速度、身体段间距等
    public SnakeConfig config;

    // 存放蛇身体段的父对象
    public Transform snakeBodyParent;

    // 使用对象池管理身体段的生成和回收
    public SnakeObjectPool bodySegmentPool;

    // 存储蛇的身体段，包括头部
    private List<Transform> bodyParts = new List<Transform>();

    // 管理蛇各个身体段的健康状态
    private SnakeHealthManager healthManager;

    // 用于更新身体段的频率控制
    private float lastUpdateTime = 0f;

    void Start()
    {
        // 确保配置已设置
        if (config == null)
        {
            Debug.LogError("SnakeConfig 未设置！");
            return;
        }

        // 初始化健康管理器
        healthManager = GetComponent<SnakeHealthManager>();
        if (healthManager == null)
        {
            healthManager = gameObject.AddComponent<SnakeHealthManager>();
        }
        healthManager.config = config;

        // 将蛇头添加到身体段列表中
        Transform snakeHead = transform.Find("SnakeHead");
        if (snakeHead == null)
        {
            Debug.LogError("未找到 SnakeHead 对象！");
            return;
        }
        bodyParts.Add(snakeHead); // 必须先添加蛇头

        // 初始化健康管理器
        healthManager.Initialize(bodyParts.Count);
    }


    public int HealthPartCount
    {
        get
        {
            // 检查 healthManager 是否为空，防止空引用错误
            if (healthManager == null)
            {
                Debug.LogError("HealthManager 未初始化！");
                return 0;
            }

            // 返回健康列表的数量
            return healthManager.PartCount;
        }
    }

    // 增加新的身体段
    public void AddBodySegment(int count = 1)
    {
        // 检查蛇的配置是否存在
        if (config == null)
        {
            Debug.LogError("SnakeConfig 未设置！");
            return;
        }

        // 检查蛇的身体段预制件是否设置
        if (config.bodyPrefab == null)
        {
            Debug.LogError("SnakeConfig 中的 bodyPrefab 未设置！");
            return;
        }

        // 检查对象池是否已设置
        if (bodySegmentPool == null)
        {
            Debug.LogError("bodySegmentPool 未设置！");
            return;
        }

        // 检查是否超过最大身体段数量限制
        if (bodyParts.Count + count > config.maxSegments)
        {
            Debug.LogWarning("已达到最大身体段数量，无法再增加！");
            return;
        }

        // 循环添加指定数量的身体段
        for (int i = 0; i < count; i++)
        {
            // 获取当前最后一个身体段的位置，用于确定新段的生成位置
            Transform lastBodyPart = bodyParts[bodyParts.Count - 1];
            Vector3 spawnPosition = lastBodyPart.position - lastBodyPart.forward * config.segmentDistance;

            // 从对象池中获取一个新的身体段
            GameObject newBodyPart = bodySegmentPool.GetObject(spawnPosition, Quaternion.identity);
            if (newBodyPart == null)
            {
                Debug.LogWarning("对象池耗尽，动态创建新的身体段！");
                newBodyPart = Instantiate(config.bodyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // 如果是从对象池中获取的，需要重置位置和朝向
                newBodyPart.transform.position = spawnPosition;
                newBodyPart.transform.rotation = Quaternion.identity;
            }

            // 将新身体段设置为蛇身体父对象的子对象
            newBodyPart.transform.SetParent(snakeBodyParent);

            // 确保新身体段有 SnakeBodySegment 组件
            var bodySegment = newBodyPart.GetComponent<SnakeBodySegment>();
            if (bodySegment == null)
            {
                bodySegment = newBodyPart.AddComponent<SnakeBodySegment>();
            }

            // 设置身体段的目标和距离
            bodySegment.target = lastBodyPart;
            bodySegment.distance = config.segmentDistance;

            // 将新身体段添加到身体段列表中
            bodyParts.Add(newBodyPart.transform);

            Debug.Log($"生成身体段: {newBodyPart.name}, 位置: {spawnPosition}, 目标: {lastBodyPart.name}");
        }

        // **同步健康状态**
        healthManager.Initialize(bodyParts.Count);
    }

    // 重置蛇的状态，将所有身体段返回到对象池中
    public void ResetSnake()
    {
        if (bodyParts.Count == 0 || bodyParts[0] == null)
        {
            Debug.LogError("SnakeHead 丢失，无法重置蛇的状态！");
            return;
        }

        // 保留蛇头引用
        Transform snakeHead = bodyParts[0];

        // 将其他身体段返回对象池
        for (int i = 1; i < bodyParts.Count; i++)
        {
            bodySegmentPool.ReturnObject(bodyParts[i].gameObject);
        }

        // 清空列表，仅保留蛇头
        bodyParts.Clear();
        bodyParts.Add(snakeHead);

        // 重置健康状态
        healthManager.Initialize(bodyParts.Count);
    }

    // 对特定身体段造成伤害
    public void TakeDamage(int segmentIndex, float damage)
    {
        healthManager.TakeDamage(segmentIndex, damage);
    }

    // 在固定更新帧中更新健康状态和身体段位置
    void FixedUpdate()
    {
        healthManager.UpdateHealth(Time.fixedDeltaTime); // 更新健康恢复状态
        UpdateBodySegments(); // 更新身体段位置
    }

    // 更新每个身体段的位置，使其跟随前一个身体段
    private void UpdateBodySegments()
    {
        if (Time.time - lastUpdateTime < config.segmentUpdateFrequency)
            return;

        lastUpdateTime = Time.time;

        for (int i = 1; i < bodyParts.Count; i++)
        {
            var segment = bodyParts[i].GetComponent<SnakeBodySegment>();
            if (segment != null)
            {
                segment.UpdateSegment(config.followSmoothness);
            }
        }
    }

    public int BodyPartCount => bodyParts.Count;

    public float GetBodySegmentHealth(int index)
    {
        if (index < 0 || index >= bodyParts.Count)
        {
            Debug.LogError($"无效的身体段索引: {index}");
            return 0f;
        }
        return healthManager.GetHealth(index); // 调用健康管理器获取血量
    }

}

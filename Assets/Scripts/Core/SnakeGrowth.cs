using System.Collections.Generic;
using UnityEngine;

// 负责管理蛇的身体段生成、回收、以及与健康状态的联动逻辑
public class SnakeGrowth : MonoBehaviour
{
    // 蛇的配置文件，包含速度、身体段间距等参数
    public SnakeConfig config;

    // 蛇身体段的父节点，用于组织层级
    public Transform snakeBodyParent;

    // 身体段对象池，用于优化生成与回收
    public SnakeObjectPool bodySegmentPool;

    // 存储蛇的所有身体段（包括蛇头）的列表
    private List<Transform> bodyParts = new List<Transform>();

    // 健康管理器，用于管理每个身体段的健康状态
    private SnakeHealthManager healthManager;

    // 控制身体段更新频率的时间戳
    private float lastUpdateTime = 0f;

    /// <summary>
    /// 初始化蛇的身体段和健康管理器
    /// </summary>
    void Start()
    {
        if (config == null)
        {
            Debug.LogError("SnakeConfig 配置文件未设置，无法初始化蛇的生长管理器！");
            return;
        }

        // 获取或创建健康管理器组件
        healthManager = GetComponent<SnakeHealthManager>();
        if (healthManager == null)
        {
            healthManager = gameObject.AddComponent<SnakeHealthManager>();
        }
        healthManager.config = config;

        // 初始化蛇头并加入身体段列表
        Transform snakeHead = transform.Find("SnakeHead");
        if (snakeHead == null)
        {
            Debug.LogError("未找到 SnakeHead 对象，无法初始化！");
            return;
        }
        bodyParts.Add(snakeHead);

        // 初始化健康状态
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// 获取当前健康管理器中身体段的数量
    /// </summary>
    public int HealthPartCount
    {
        get
        {
            if (healthManager == null)
            {
                Debug.LogError("健康管理器未初始化！");
                return 0;
            }
            return healthManager.PartCount;
        }
    }

    /// <summary>
    /// 添加新的身体段
    /// </summary>
    /// <param name="count">需要添加的身体段数量</param>
    public void AddBodySegment(int count = 1)
    {
        if (config == null || config.bodyPrefab == null)
        {
            Debug.LogError("配置文件或身体段预制体未正确设置！");
            return;
        }

        if (bodySegmentPool == null)
        {
            Debug.LogError("身体段对象池未设置！");
            return;
        }

        if (bodyParts.Count + count > config.maxSegments)
        {
            Debug.LogWarning("已达到最大身体段数量限制，无法继续添加！");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // 确定新身体段的生成位置
            Transform lastBodyPart = bodyParts[bodyParts.Count - 1];
            Vector3 spawnPosition = lastBodyPart.position - lastBodyPart.forward * config.segmentDistance;

            // 尝试从对象池中获取身体段，若失败则动态生成
            GameObject newBodyPart = bodySegmentPool.GetObject(spawnPosition, Quaternion.identity);
            if (newBodyPart == null)
            {
                Debug.LogWarning("对象池已耗尽，动态创建新的身体段！");
                newBodyPart = Instantiate(config.bodyPrefab, spawnPosition, Quaternion.identity);
            }

            // 设置身体段的层级和位置
            newBodyPart.transform.SetParent(snakeBodyParent);
            newBodyPart.transform.position = spawnPosition;
            newBodyPart.transform.rotation = Quaternion.identity;

            // 确保身体段具备 SnakeBodySegment 组件
            var bodySegment = newBodyPart.GetComponent<SnakeBodySegment>();
            if (bodySegment == null)
            {
                bodySegment = newBodyPart.AddComponent<SnakeBodySegment>();
            }

            // 配置身体段的跟随逻辑
            bodySegment.target = lastBodyPart;
            bodySegment.distance = config.segmentDistance;

            // 将新身体段加入列表
            bodyParts.Add(newBodyPart.transform);
            Debug.Log($"新增身体段: {newBodyPart.name}，位置: {spawnPosition}，目标: {lastBodyPart.name}");
        }

        // 更新健康管理器状态
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// 重置蛇的状态，将除蛇头外的身体段返回对象池
    /// </summary>
    public void ResetSnake()
    {
        if (bodyParts.Count == 0 || bodyParts[0] == null)
        {
            Debug.LogError("蛇头未找到，无法重置！");
            return;
        }

        // 保留蛇头，清空其他身体段
        Transform snakeHead = bodyParts[0];
        for (int i = 1; i < bodyParts.Count; i++)
        {
            bodySegmentPool.ReturnObject(bodyParts[i].gameObject);
        }
        bodyParts.Clear();
        bodyParts.Add(snakeHead);

        // 重置健康状态
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// 对指定的身体段造成伤害
    /// </summary>
    /// <param name="segmentIndex">身体段索引</param>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(int segmentIndex, float damage)
    {
        healthManager.TakeDamage(segmentIndex, damage);
    }

    /// <summary>
    /// 固定更新中处理身体段位置更新和健康恢复
    /// </summary>
    void FixedUpdate()
    {
        healthManager.UpdateHealth(Time.fixedDeltaTime);
        UpdateBodySegments();
    }

    /// <summary>
    /// 更新所有身体段的位置，使其跟随前一个身体段
    /// </summary>
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

    /// <summary>
    /// 获取指定身体段的健康值
    /// </summary>
    /// <param name="index">身体段索引</param>
    /// <returns>健康值</returns>
    public float GetBodySegmentHealth(int index)
    {
        if (index < 0 || index >= bodyParts.Count)
        {
            Debug.LogError($"无效的身体段索引: {index}");
            return 0f;
        }
        return healthManager.GetHealth(index);
    }

    /// <summary>
    /// 获取当前身体段的总数量
    /// </summary>
    public int BodyPartCount => bodyParts.Count;
}
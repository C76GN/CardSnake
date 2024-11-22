using UnityEngine;

[CreateAssetMenu(fileName = "SnakeConfig", menuName = "Snake/Config")]
public class SnakeConfig : ScriptableObject
{
    public float moveSpeed = 5f; // 蛇头移动速度
    public float rotationSpeed = 8f; // 蛇头旋转速度
    public float segmentDistance = 1f; // 身体段之间的距离
    public GameObject bodyPrefab; // 身体段预制体
    public int maxSegments = 10; // 最大身体段数量，包括蛇头
    public float initialHealth = 100f; // 每个身体段的初始血量

    // 新增配置
    public float followSmoothness = 10f; // 身体段跟随的平滑系数
    public float segmentUpdateFrequency = 0.02f; // 身体段更新的频率（秒）
    // 恢复机制相关
    public float recoveryRate = 10f;          // 恢复速率（每秒恢复的血量）
    public float recoveryDelay = 5f;          // 恢复延迟时间（秒）
    public float recoveryThreshold = 0.5f;     // 恢复的最大血量阈值
}

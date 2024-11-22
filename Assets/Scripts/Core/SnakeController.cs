using UnityEngine;

// 确保该组件附加了 Rigidbody 组件
[RequireComponent(typeof(Rigidbody))]
public class SnakeController : MonoBehaviour
{
    // 蛇的配置参数，例如移动速度、旋转速度等
    public SnakeConfig config;

    // 用于处理物理模拟的 Rigidbody 组件
    private Rigidbody rb;

    // 存储输入方向
    private Vector3 inputDirection;

    // 当前速度，用于平滑移动
    private Vector3 currentVelocity;

    // 旋转的最小速度阈值
    private float minSpeedThreshold = 0.1f;

    // 引用蛇的生长管理器
    private SnakeGrowth snakeGrowth;

    void Start()
    {
        // 获取 Rigidbody 组件
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 未找到！");
            enabled = false; // 禁用脚本，防止后续错误
            return;
        }

        // 设置 Rigidbody 参数，防止蛇受重力影响并限制某些方向的运动和旋转
        rb.useGravity = false; // 禁用重力
        rb.isKinematic = true; // 设置为 Kinematic（不受物理力影响）
        rb.constraints = RigidbodyConstraints.FreezePositionY | // 限制垂直方向移动
                         RigidbodyConstraints.FreezeRotationX | // 限制 X 轴旋转
                         RigidbodyConstraints.FreezeRotationZ;  // 限制 Z 轴旋转

        // 检查是否设置了蛇的配置文件
        if (config == null)
        {
            Debug.LogError("SnakeConfig 未设置！");
            enabled = false;
            return;
        }

        // 获取蛇的生长管理器
        snakeGrowth = transform.parent.GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("SnakeGrowth 未找到！");
            enabled = false;
        }
    }

    void Update()
    {
        // 获取玩家输入（水平和垂直方向）
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical).normalized; // 标准化输入向量
    }

    void FixedUpdate()
    {
        // 处理移动和旋转
        HandleMovement();
        HandleRotation();
    }

    // 处理蛇的移动
    private void HandleMovement()
    {
        if (inputDirection.magnitude > 0) // 检查是否有输入
        {
            // 平滑插值计算当前速度，使蛇逐渐加速到目标速度
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                inputDirection * config.moveSpeed, // 目标速度由配置中的移动速度确定
                Time.fixedDeltaTime * 10f // 平滑系数，影响加速/减速的速度
            );
        }
        else
        {
            // 如果没有输入，逐渐减速到静止状态
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero, // 目标速度为零
                Time.fixedDeltaTime * 10f
            );
        }

        // 使用 Rigidbody 移动蛇的位置
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    // 处理蛇的旋转
    private void HandleRotation()
    {
        // 仅当速度超过阈值时才进行旋转
        if (currentVelocity.sqrMagnitude > minSpeedThreshold)
        {
            // 计算目标旋转方向
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);

            // 平滑插值旋转到目标方向
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,           // 当前旋转
                targetRotation,        // 目标旋转
                Time.fixedDeltaTime * config.rotationSpeed // 旋转速度由配置确定
            ));
        }
    }
}

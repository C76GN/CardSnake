using UnityEngine;

// 控制蛇头的移动和旋转行为，依赖 Rigidbody 组件进行物理模拟
[RequireComponent(typeof(Rigidbody))]
public class SnakeController : MonoBehaviour
{
    // 蛇的参数配置文件
    public SnakeConfig config;

    // 用于移动蛇头的物理组件
    private Rigidbody rb;

    // 当前的输入方向
    private Vector3 inputDirection;

    // 蛇头的当前速度
    private Vector3 currentVelocity;

    // 最小速度阈值，低于该值时蛇头不进行旋转
    private float minSpeedThreshold = 0.1f;

    // 引用蛇身体的生长管理器，用于处理与身体段的交互
    private SnakeGrowth snakeGrowth;

    /// <summary>
    /// 初始化控制器，检查必要的依赖是否存在
    /// </summary>
    void Start()
    {
        InitializeController();
    }

    /// <summary>
    /// 处理控制器的初始化，包括 Rigidbody 和相关配置的检查
    /// </summary>
    private void InitializeController()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("未找到 Rigidbody 组件！");
            enabled = false;
            return;
        }

        // 设置 Rigidbody 的参数，确保物理模拟符合需求
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        if (config == null)
        {
            Debug.LogError("未设置 SnakeConfig 配置文件！");
            enabled = false;
            return;
        }

        snakeGrowth = transform.parent.GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("未找到 SnakeGrowth 组件！");
            enabled = false;
        }
    }

    /// <summary>
    /// 更新输入方向
    /// </summary>
    void Update()
    {
        ProcessInput();
    }

    /// <summary>
    /// 获取玩家输入并计算移动方向
    /// </summary>
    private void ProcessInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    /// <summary>
    /// 处理物理更新，包括移动和旋转
    /// </summary>
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// 根据输入方向处理蛇头的移动
    /// </summary>
    private void HandleMovement()
    {
        if (inputDirection.magnitude > 0)
        {
            // 逐渐加速到目标速度
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                inputDirection * config.moveSpeed,
                Time.fixedDeltaTime * 10f
            );
        }
        else
        {
            // 平滑减速到静止状态
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero,
                Time.fixedDeltaTime * 10f
            );
        }

        // 使用 Rigidbody 移动蛇头
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 根据当前速度处理蛇头的旋转方向
    /// </summary>
    private void HandleRotation()
    {
        if (currentVelocity.sqrMagnitude > minSpeedThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                Time.fixedDeltaTime * config.rotationSpeed
            ));
        }
    }
}

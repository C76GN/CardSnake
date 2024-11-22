using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标设置")]
    public Transform target; // 要跟踪的目标（蛇头）

    [Header("视角偏移设置")]
    public Vector3 baseOffset = new Vector3(0, 10, -10); // 摄像机的基础偏移量
    public float followSpeed = 5f; // 跟随的平滑速度

    [Header("缩放设置")]
    public float zoomSpeed = 2f; // 缩放速度
    public float minZoomFactor = 0.5f; // 最小缩放比例
    public float maxZoomFactor = 2f; // 最大缩放比例
    public float defaultZoomFactor = 1f; // 默认缩放比例

    // 内部变量
    private Vector3 currentOffset; // 当前偏移量
    private float zoomFactor; // 当前缩放因子
    private Coroutine currentZoomCoroutine; // 当前的缩放协程

    void Start()
    {
        // 初始化缩放因子和偏移量
        zoomFactor = defaultZoomFactor;
        currentOffset = baseOffset * zoomFactor;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("摄像机目标未设置！");
            return;
        }

        // 调整视角
        AdjustView();

        // 计算摄像机的目标位置
        Vector3 desiredPosition = target.position + currentOffset;

        // 平滑移动摄像机到目标位置
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 始终朝向目标
        transform.LookAt(target);
    }

    private void AdjustView()
    {
        // 根据当前缩放因子调整偏移量
        currentOffset = baseOffset * zoomFactor;
    }

    /// <summary>
    /// 设置缩放因子
    /// </summary>
    /// <param name="newZoomFactor">新的缩放因子</param>
    /// <param name="smoothTransition">是否启用平滑过渡</param>
    public void SetZoomFactor(float newZoomFactor, bool smoothTransition = true)
    {
        // 限制缩放因子在最小和最大范围内
        newZoomFactor = Mathf.Clamp(newZoomFactor, minZoomFactor, maxZoomFactor);

        if (smoothTransition)
        {
            if (currentZoomCoroutine != null)
            {
                StopCoroutine(currentZoomCoroutine);
            }
            currentZoomCoroutine = StartCoroutine(SmoothZoomTransition(newZoomFactor));
        }
        else
        {
            zoomFactor = newZoomFactor;
            AdjustView();
        }
    }

    /// <summary>
    /// 平滑调整缩放因子的协程
    /// </summary>
    /// <param name="targetZoomFactor">目标缩放因子</param>
    /// <returns>协程</returns>
    private System.Collections.IEnumerator SmoothZoomTransition(float targetZoomFactor)
    {
        while (Mathf.Abs(zoomFactor - targetZoomFactor) > 0.01f)
        {
            zoomFactor = Mathf.Lerp(zoomFactor, targetZoomFactor, zoomSpeed * Time.deltaTime);
            AdjustView();
            yield return null;
        }
        zoomFactor = targetZoomFactor;
    }
}

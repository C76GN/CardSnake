using UnityEngine;

// 单个身体段的行为逻辑
public class SnakeBodySegment : MonoBehaviour
{
    public Transform target; // 跟随的目标段
    public float distance;   // 与目标段之间的距离

    // 更新身体段的位置和朝向
    public void UpdateSegment(float smoothness)
    {
        if (target == null) return;

        // 计算目标位置
        Vector3 targetPosition = target.position - target.forward * distance;

        // 平滑移动到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);

        // 朝向目标段
        transform.LookAt(target);
    }
}

using UnityEngine;

// ��������ε���Ϊ�߼�
public class SnakeBodySegment : MonoBehaviour
{
    public Transform target; // �����Ŀ���
    public float distance;   // ��Ŀ���֮��ľ���

    // ��������ε�λ�úͳ���
    public void UpdateSegment(float smoothness)
    {
        if (target == null) return;

        // ����Ŀ��λ��
        Vector3 targetPosition = target.position - target.forward * distance;

        // ƽ���ƶ���Ŀ��λ��
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);

        // ����Ŀ���
        transform.LookAt(target);
    }
}

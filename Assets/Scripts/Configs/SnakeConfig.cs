using UnityEngine;

[CreateAssetMenu(fileName = "SnakeConfig", menuName = "Snake/Config")]
public class SnakeConfig : ScriptableObject
{
    public float moveSpeed = 5f; // ��ͷ�ƶ��ٶ�
    public float rotationSpeed = 8f; // ��ͷ��ת�ٶ�
    public float segmentDistance = 1f; // �����֮��ľ���
    public GameObject bodyPrefab; // �����Ԥ����
    public int maxSegments = 10; // ��������������������ͷ
    public float initialHealth = 100f; // ÿ������εĳ�ʼѪ��

    // ��������
    public float followSmoothness = 10f; // ����θ����ƽ��ϵ��
    public float segmentUpdateFrequency = 0.02f; // ����θ��µ�Ƶ�ʣ��룩
    // �ָ��������
    public float recoveryRate = 10f;          // �ָ����ʣ�ÿ��ָ���Ѫ����
    public float recoveryDelay = 5f;          // �ָ��ӳ�ʱ�䣨�룩
    public float recoveryThreshold = 0.5f;     // �ָ������Ѫ����ֵ
}

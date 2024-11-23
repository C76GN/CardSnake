using System.Collections.Generic;
using UnityEngine;

// ��������ߵ���������ɡ����ա��Լ��뽡��״̬�������߼�
public class SnakeGrowth : MonoBehaviour
{
    // �ߵ������ļ��������ٶȡ�����μ��Ȳ���
    public SnakeConfig config;

    // ������εĸ��ڵ㣬������֯�㼶
    public Transform snakeBodyParent;

    // ����ζ���أ������Ż����������
    public SnakeObjectPool bodySegmentPool;

    // �洢�ߵ���������Σ�������ͷ�����б�
    private List<Transform> bodyParts = new List<Transform>();

    // ���������������ڹ���ÿ������εĽ���״̬
    private SnakeHealthManager healthManager;

    // ��������θ���Ƶ�ʵ�ʱ���
    private float lastUpdateTime = 0f;

    /// <summary>
    /// ��ʼ���ߵ�����κͽ���������
    /// </summary>
    void Start()
    {
        if (config == null)
        {
            Debug.LogError("SnakeConfig �����ļ�δ���ã��޷���ʼ���ߵ�������������");
            return;
        }

        // ��ȡ�򴴽��������������
        healthManager = GetComponent<SnakeHealthManager>();
        if (healthManager == null)
        {
            healthManager = gameObject.AddComponent<SnakeHealthManager>();
        }
        healthManager.config = config;

        // ��ʼ����ͷ������������б�
        Transform snakeHead = transform.Find("SnakeHead");
        if (snakeHead == null)
        {
            Debug.LogError("δ�ҵ� SnakeHead �����޷���ʼ����");
            return;
        }
        bodyParts.Add(snakeHead);

        // ��ʼ������״̬
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// ��ȡ��ǰ����������������ε�����
    /// </summary>
    public int HealthPartCount
    {
        get
        {
            if (healthManager == null)
            {
                Debug.LogError("����������δ��ʼ����");
                return 0;
            }
            return healthManager.PartCount;
        }
    }

    /// <summary>
    /// ����µ������
    /// </summary>
    /// <param name="count">��Ҫ��ӵ����������</param>
    public void AddBodySegment(int count = 1)
    {
        if (config == null || config.bodyPrefab == null)
        {
            Debug.LogError("�����ļ��������Ԥ����δ��ȷ���ã�");
            return;
        }

        if (bodySegmentPool == null)
        {
            Debug.LogError("����ζ����δ���ã�");
            return;
        }

        if (bodyParts.Count + count > config.maxSegments)
        {
            Debug.LogWarning("�Ѵﵽ���������������ƣ��޷�������ӣ�");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // ȷ��������ε�����λ��
            Transform lastBodyPart = bodyParts[bodyParts.Count - 1];
            Vector3 spawnPosition = lastBodyPart.position - lastBodyPart.forward * config.segmentDistance;

            // ���ԴӶ�����л�ȡ����Σ���ʧ����̬����
            GameObject newBodyPart = bodySegmentPool.GetObject(spawnPosition, Quaternion.identity);
            if (newBodyPart == null)
            {
                Debug.LogWarning("������Ѻľ�����̬�����µ�����Σ�");
                newBodyPart = Instantiate(config.bodyPrefab, spawnPosition, Quaternion.identity);
            }

            // ��������εĲ㼶��λ��
            newBodyPart.transform.SetParent(snakeBodyParent);
            newBodyPart.transform.position = spawnPosition;
            newBodyPart.transform.rotation = Quaternion.identity;

            // ȷ������ξ߱� SnakeBodySegment ���
            var bodySegment = newBodyPart.GetComponent<SnakeBodySegment>();
            if (bodySegment == null)
            {
                bodySegment = newBodyPart.AddComponent<SnakeBodySegment>();
            }

            // ��������εĸ����߼�
            bodySegment.target = lastBodyPart;
            bodySegment.distance = config.segmentDistance;

            // ��������μ����б�
            bodyParts.Add(newBodyPart.transform);
            Debug.Log($"���������: {newBodyPart.name}��λ��: {spawnPosition}��Ŀ��: {lastBodyPart.name}");
        }

        // ���½���������״̬
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// �����ߵ�״̬��������ͷ�������η��ض����
    /// </summary>
    public void ResetSnake()
    {
        if (bodyParts.Count == 0 || bodyParts[0] == null)
        {
            Debug.LogError("��ͷδ�ҵ����޷����ã�");
            return;
        }

        // ������ͷ��������������
        Transform snakeHead = bodyParts[0];
        for (int i = 1; i < bodyParts.Count; i++)
        {
            bodySegmentPool.ReturnObject(bodyParts[i].gameObject);
        }
        bodyParts.Clear();
        bodyParts.Add(snakeHead);

        // ���ý���״̬
        healthManager.Initialize(bodyParts.Count);
    }

    /// <summary>
    /// ��ָ�������������˺�
    /// </summary>
    /// <param name="segmentIndex">���������</param>
    /// <param name="damage">�˺�ֵ</param>
    public void TakeDamage(int segmentIndex, float damage)
    {
        healthManager.TakeDamage(segmentIndex, damage);
    }

    /// <summary>
    /// �̶������д��������λ�ø��ºͽ����ָ�
    /// </summary>
    void FixedUpdate()
    {
        healthManager.UpdateHealth(Time.fixedDeltaTime);
        UpdateBodySegments();
    }

    /// <summary>
    /// ������������ε�λ�ã�ʹ�����ǰһ�������
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
    /// ��ȡָ������εĽ���ֵ
    /// </summary>
    /// <param name="index">���������</param>
    /// <returns>����ֵ</returns>
    public float GetBodySegmentHealth(int index)
    {
        if (index < 0 || index >= bodyParts.Count)
        {
            Debug.LogError($"��Ч�����������: {index}");
            return 0f;
        }
        return healthManager.GetHealth(index);
    }

    /// <summary>
    /// ��ȡ��ǰ����ε�������
    /// </summary>
    public int BodyPartCount => bodyParts.Count;
}
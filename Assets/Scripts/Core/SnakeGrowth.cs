using System.Collections.Generic; // ���뼯�Ͽ⣬���ڹ������岿���б�
using UnityEngine;

// �����ߵ��������������Ӻͽ���״̬
public class SnakeGrowth : MonoBehaviour
{
    // �ߵ����ò����������ٶȡ�����μ���
    public SnakeConfig config;

    // ���������εĸ�����
    public Transform snakeBodyParent;

    // ʹ�ö���ع�������ε����ɺͻ���
    public SnakeObjectPool bodySegmentPool;

    // �洢�ߵ�����Σ�����ͷ��
    private List<Transform> bodyParts = new List<Transform>();

    // �����߸�������εĽ���״̬
    private SnakeHealthManager healthManager;

    // ���ڸ�������ε�Ƶ�ʿ���
    private float lastUpdateTime = 0f;

    void Start()
    {
        // ȷ������������
        if (config == null)
        {
            Debug.LogError("SnakeConfig δ���ã�");
            return;
        }

        // ��ʼ������������
        healthManager = GetComponent<SnakeHealthManager>();
        if (healthManager == null)
        {
            healthManager = gameObject.AddComponent<SnakeHealthManager>();
        }
        healthManager.config = config;

        // ����ͷ��ӵ�������б���
        Transform snakeHead = transform.Find("SnakeHead");
        if (snakeHead == null)
        {
            Debug.LogError("δ�ҵ� SnakeHead ����");
            return;
        }
        bodyParts.Add(snakeHead); // �����������ͷ

        // ��ʼ������������
        healthManager.Initialize(bodyParts.Count);
    }


    public int HealthPartCount
    {
        get
        {
            // ��� healthManager �Ƿ�Ϊ�գ���ֹ�����ô���
            if (healthManager == null)
            {
                Debug.LogError("HealthManager δ��ʼ����");
                return 0;
            }

            // ���ؽ����б������
            return healthManager.PartCount;
        }
    }

    // �����µ������
    public void AddBodySegment(int count = 1)
    {
        // ����ߵ������Ƿ����
        if (config == null)
        {
            Debug.LogError("SnakeConfig δ���ã�");
            return;
        }

        // ����ߵ������Ԥ�Ƽ��Ƿ�����
        if (config.bodyPrefab == null)
        {
            Debug.LogError("SnakeConfig �е� bodyPrefab δ���ã�");
            return;
        }

        // ��������Ƿ�������
        if (bodySegmentPool == null)
        {
            Debug.LogError("bodySegmentPool δ���ã�");
            return;
        }

        // ����Ƿ񳬹�����������������
        if (bodyParts.Count + count > config.maxSegments)
        {
            Debug.LogWarning("�Ѵﵽ���������������޷������ӣ�");
            return;
        }

        // ѭ�����ָ�������������
        for (int i = 0; i < count; i++)
        {
            // ��ȡ��ǰ���һ������ε�λ�ã�����ȷ���¶ε�����λ��
            Transform lastBodyPart = bodyParts[bodyParts.Count - 1];
            Vector3 spawnPosition = lastBodyPart.position - lastBodyPart.forward * config.segmentDistance;

            // �Ӷ�����л�ȡһ���µ������
            GameObject newBodyPart = bodySegmentPool.GetObject(spawnPosition, Quaternion.identity);
            if (newBodyPart == null)
            {
                Debug.LogWarning("����غľ�����̬�����µ�����Σ�");
                newBodyPart = Instantiate(config.bodyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // ����ǴӶ�����л�ȡ�ģ���Ҫ����λ�úͳ���
                newBodyPart.transform.position = spawnPosition;
                newBodyPart.transform.rotation = Quaternion.identity;
            }

            // �������������Ϊ�����常������Ӷ���
            newBodyPart.transform.SetParent(snakeBodyParent);

            // ȷ����������� SnakeBodySegment ���
            var bodySegment = newBodyPart.GetComponent<SnakeBodySegment>();
            if (bodySegment == null)
            {
                bodySegment = newBodyPart.AddComponent<SnakeBodySegment>();
            }

            // ��������ε�Ŀ��;���
            bodySegment.target = lastBodyPart;
            bodySegment.distance = config.segmentDistance;

            // �����������ӵ�������б���
            bodyParts.Add(newBodyPart.transform);

            Debug.Log($"���������: {newBodyPart.name}, λ��: {spawnPosition}, Ŀ��: {lastBodyPart.name}");
        }

        // **ͬ������״̬**
        healthManager.Initialize(bodyParts.Count);
    }

    // �����ߵ�״̬������������η��ص��������
    public void ResetSnake()
    {
        if (bodyParts.Count == 0 || bodyParts[0] == null)
        {
            Debug.LogError("SnakeHead ��ʧ���޷������ߵ�״̬��");
            return;
        }

        // ������ͷ����
        Transform snakeHead = bodyParts[0];

        // ����������η��ض����
        for (int i = 1; i < bodyParts.Count; i++)
        {
            bodySegmentPool.ReturnObject(bodyParts[i].gameObject);
        }

        // ����б���������ͷ
        bodyParts.Clear();
        bodyParts.Add(snakeHead);

        // ���ý���״̬
        healthManager.Initialize(bodyParts.Count);
    }

    // ���ض����������˺�
    public void TakeDamage(int segmentIndex, float damage)
    {
        healthManager.TakeDamage(segmentIndex, damage);
    }

    // �ڹ̶�����֡�и��½���״̬�������λ��
    void FixedUpdate()
    {
        healthManager.UpdateHealth(Time.fixedDeltaTime); // ���½����ָ�״̬
        UpdateBodySegments(); // ���������λ��
    }

    // ����ÿ������ε�λ�ã�ʹ�����ǰһ�������
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
            Debug.LogError($"��Ч�����������: {index}");
            return 0f;
        }
        return healthManager.GetHealth(index); // ���ý�����������ȡѪ��
    }

}

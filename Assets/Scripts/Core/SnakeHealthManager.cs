using System.Collections.Generic; // ����ʹ���б�����������ռ�
using UnityEngine;

// �����ߵĽ���״̬�����������岿λ�����˺ͻָ�
public class SnakeHealthManager : MonoBehaviour
{
    // �洢ÿ�����岿�ֵĽ���ֵ
    private List<float> bodyPartHealth = new List<float>();

    // �洢ÿ�����岿�����һ���ܵ��˺���ʱ��
    private List<float> lastDamageTime = new List<float>();

    // ���ÿ�����岿���Ƿ��Ѿ���
    private List<bool> bodyPartDamaged = new List<bool>();

    // �����ļ����������ûָ����ʺ��ӳٵȲ���
    public SnakeConfig config;

    // ��ʼ�����岿λ�Ľ���״̬
    public void Initialize(int bodyPartCount)
    {
        if (config == null)
        {
            Debug.LogError("SnakeConfig δ��ȷ���ݵ� SnakeHealthManager��");
            return;
        }

        if (bodyPartCount <= 0)
        {
            // ������������Ϊ 0����ս����б��������ظ���ӡ����
            if (bodyPartHealth.Count > 0)
            {
                Debug.LogWarning("���������Ϊ 0���޷���ʼ�������б�");
            }
            bodyPartHealth.Clear();
            lastDamageTime.Clear();
            bodyPartDamaged.Clear();
            return;
        }

        // �����б��С
        AdjustListSize(bodyPartHealth, bodyPartCount, config.initialHealth);
        AdjustListSize(lastDamageTime, bodyPartCount, 0f);
        AdjustListSize(bodyPartDamaged, bodyPartCount, false);

        Debug.Log($"�����б��ѳ�ʼ������ǰ���������: {bodyPartCount}, �����б���: {bodyPartHealth.Count}");
    }

    public int PartCount
    {
        get
        {
            // ���ؽ����б������
            return bodyPartHealth.Count;
        }
    }

    // �����б��С��ͨ�÷���
    private void AdjustListSize<T>(List<T> list, int targetCount, T defaultValue)
    {
        // ����б�ǰ���Ȳ��㣬�����Ĭ��ֵ
        while (list.Count < targetCount)
        {
            list.Add(defaultValue);
        }

        // ����б�ǰ���ȳ���Ŀ�꣬���Ƴ����ಿ��
        while (list.Count > targetCount)
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    // �����ָ�����岿����ɵ��˺�
    public void TakeDamage(int index, float damage)
    {
        // ��������Ƿ���Ч����ֹ����Խ��
        if (index < 0 || index >= bodyPartHealth.Count) return;

        // ��������岿���Ѿ��𻵣����ٴ����˺�
        if (bodyPartDamaged[index]) return;

        // ����ָ�����岿�ֵĽ���ֵ
        bodyPartHealth[index] -= damage;

        // ��¼��ǰʱ��Ϊ�����岿�ֵ��������ʱ��
        lastDamageTime[index] = Time.time;

        // �������ֵ���� 0 �����£����ò��ֱ��Ϊ��
        if (bodyPartHealth[index] <= 0)
        {
            bodyPartHealth[index] = 0;        // ȷ������ֵ������� 0
            bodyPartDamaged[index] = true;    // ���Ϊ��
        }

        Debug.Log($"����� {index} �ܵ� {damage} ���˺�����ǰ����ֵ: {bodyPartHealth[index]}");
    }

    // �������岿λ�Ľ���״̬�����ڻָ����ƣ�
    public void UpdateHealth(float deltaTime)
    {
        // �����������岿�֣�����Ƿ���Ҫ�ָ�����
        for (int i = 0; i < bodyPartHealth.Count; i++)
        {
            // ����ʵ�ʻָ���ֵ
            float actualRecoveryThreshold = config.initialHealth * config.recoveryThreshold;

            // ����Ƿ�����ָ����������ڻָ���ֵ�����Ҿ����˻ָ��ӳ�ʱ�䣩
            if (bodyPartHealth[i] < actualRecoveryThreshold &&
                Time.time - lastDamageTime[i] >= config.recoveryDelay)
            {
                // ���ջָ��������ӽ���ֵ
                bodyPartHealth[i] += config.recoveryRate * deltaTime;

                // ȷ������ֵ���ᳬ���ָ���ֵ
                if (bodyPartHealth[i] > actualRecoveryThreshold)
                {
                    bodyPartHealth[i] = actualRecoveryThreshold;
                }
            }
        }
    }

    // ��ȡָ������εĽ���ֵ
    public float GetHealth(int index)
    {
        if (index < 0 || index >= bodyPartHealth.Count)
        {
            Debug.LogError($"��Ч�Ľ�������: {index}����ǰ�����б���: {bodyPartHealth.Count}");
            return 0f;
        }

        Debug.Log($"��ȡ����� {index} �Ľ���ֵ: {bodyPartHealth[index]}");
        return bodyPartHealth[index];
    }
}

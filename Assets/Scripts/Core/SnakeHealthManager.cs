using System.Collections.Generic;
using UnityEngine;
using static ListExtensions;

// ����������������λ�Ľ���״̬���������˺ͻָ�
public class SnakeHealthManager : MonoBehaviour
{
    // ���岿λ����ֵ�б�
    private List<float> bodyPartHealth = new List<float>();

    // ÿ�����岿λ��һ�����˵�ʱ���
    private List<float> lastDamageTime = new List<float>();

    // ÿ�����岿λ����״̬���Ƿ��Ѿ��𻵣�
    private List<bool> bodyPartDamaged = new List<bool>();

    // �����ļ�������������������Ĳ���
    public SnakeConfig config;

    /// <summary>
    /// ��ʼ�����岿λ����״̬
    /// </summary>
    /// <param name="bodyPartCount">���岿λ������</param>
    public void Initialize(int bodyPartCount)
    {
        if (config == null)
        {
            Debug.LogError("δ���� SnakeConfig���޷���ʼ��������������");
            return;
        }

        if (bodyPartCount <= 0)
        {
            if (bodyPartHealth.Count > 0)
            {
                Debug.LogWarning("���岿λ����Ϊ 0������״̬����գ�");
            }
            bodyPartHealth.Clear();
            lastDamageTime.Clear();
            bodyPartDamaged.Clear();
            return;
        }

        // ������������б�Ĵ�С
        bodyPartHealth.Resize(bodyPartCount, config.initialHealth);
        lastDamageTime.Resize(bodyPartCount, 0f);
        bodyPartDamaged.Resize(bodyPartCount, false);

        Debug.Log($"����״̬�ѳ�ʼ�������岿λ����: {bodyPartCount}, �����б���: {bodyPartHealth.Count}");
    }

    /// <summary>
    /// ���������仯�¼���֪ͨ�ⲿģ�����״̬
    /// </summary>
    /// <param name="index">�����仯�����岿λ����</param>
    private void NotifyHealthChanged(int index)
    {
        EventManager.TriggerEvent("OnHealthChanged", new { Index = index, Health = bodyPartHealth[index] });
    }

    /// <summary>
    /// ��ȡ��ǰ���岿λ����
    /// </summary>
    public int PartCount => bodyPartHealth.Count;

    /// <summary>
    /// Ϊָ�������岿λ����˺�
    /// </summary>
    /// <param name="index">���岿λ������</param>
    /// <param name="damage">��ɵ��˺�ֵ</param>
    public void TakeDamage(int index, float damage)
    {
        if (index < 0 || index >= bodyPartHealth.Count) return;

        // ����ò�λ�Ѿ��𻵣����ٴ����˺�
        if (bodyPartDamaged[index]) return;

        // ���ٽ���ֵ����¼����ʱ��
        bodyPartHealth[index] -= damage;
        lastDamageTime[index] = Time.time;

        // ֪ͨ����״̬�仯
        NotifyHealthChanged(index);

        // �������ֵ���� 0 �����£����ò�λ���Ϊ��
        if (bodyPartHealth[index] <= 0)
        {
            bodyPartHealth[index] = 0;
            bodyPartDamaged[index] = true;
            EventManager.TriggerEvent("OnBodyPartDamaged", index);
        }

        Debug.Log($"���岿λ {index} �ܵ� {damage} ���˺���ʣ�ཡ��ֵ: {bodyPartHealth[index]}");
    }

    /// <summary>
    /// �����������岿λ�Ľ���״̬�����ڴ���ָ�����
    /// </summary>
    /// <param name="deltaTime">ʱ�����������ڼ���ָ���</param>
    public void UpdateHealth(float deltaTime)
    {
        for (int i = 0; i < bodyPartHealth.Count; i++)
        {
            // �ָ���ֵΪ��ʼ����ֵ�İٷֱ�
            float actualRecoveryThreshold = config.initialHealth * config.recoveryThreshold;

            // �������ֵ���ڻָ���ֵ���ҳ����˻ָ��ӳ�ʱ�䣬����лָ�
            if (bodyPartHealth[i] < actualRecoveryThreshold &&
                Time.time - lastDamageTime[i] >= config.recoveryDelay)
            {
                bodyPartHealth[i] += config.recoveryRate * deltaTime;

                // ȷ������ֵ���ᳬ���ָ���ֵ
                if (bodyPartHealth[i] > actualRecoveryThreshold)
                {
                    bodyPartHealth[i] = actualRecoveryThreshold;
                }
            }
        }
    }

    /// <summary>
    /// ��ȡָ�����岿λ�ĵ�ǰ����ֵ
    /// </summary>
    /// <param name="index">���岿λ������</param>
    /// <returns>����ֵ</returns>
    public float GetHealth(int index)
    {
        if (index < 0 || index >= bodyPartHealth.Count)
        {
            Debug.LogError($"��Ч�����岿λ����: {index}�������б���Ϊ: {bodyPartHealth.Count}");
            return 0f;
        }

        return bodyPartHealth[index];
    }
}
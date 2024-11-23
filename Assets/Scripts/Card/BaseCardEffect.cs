using UnityEngine;

public abstract class BaseCardEffect : MonoBehaviour, ICardEffect
{
    [Header("����Ч����������")]
    public string effectName; // Ч������
    public string description; // Ч������
    public float duration; // Ч������ʱ�䣨�룩

    private float startTime; // Ч����ʼʱ��

    // Ӧ��Ч��ʱ��¼��ʼʱ��
    public virtual void ApplyEffect(GameObject target)
    {
        startTime = Time.time;
        Debug.Log($"Ч�� {effectName} ��Ӧ�ã�������{description}");
    }

    // �Ƴ�Ч��
    public virtual void RemoveEffect(GameObject target)
    {
        Debug.Log($"Ч�� {effectName} ���Ƴ���");
    }

    // ���Ч���Ƿ��ѹ���
    public bool IsExpired()
    {
        return Time.time - startTime >= duration;
    }
}

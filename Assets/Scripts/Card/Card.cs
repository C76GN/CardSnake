using UnityEngine;

public class Card
{
    public string cardName;       // ��������
    public int level;             // ���Ƶȼ�
    public string effect;         // ����Ч������
    public float cooldownTime;    // ��ȴʱ��
    public float lastUsedTime;    // �ϴ�ʹ��ʱ��

    // ���캯��
    public Card(string name, int level, string effect, float cooldown)
    {
        this.cardName = name;
        this.level = level;
        this.effect = effect;
        this.cooldownTime = cooldown;
        this.lastUsedTime = -cooldown; // ��ʼ��Ϊ��ȴ���
    }

    // ��鿨���Ƿ����ʹ��
    public bool CanUse()
    {
        return Time.time - lastUsedTime >= cooldownTime;
    }

    // Ӧ�ÿ���Ч��
    public void Use()
    {
        if (CanUse())
        {
            Debug.Log($"{cardName} ���Ʊ�ʹ�ã�");
            lastUsedTime = Time.time;
            // TODO: ִ�п���Ч��
        }
        else
        {
            Debug.LogWarning($"{cardName} ����������ȴ�У�");
        }
    }
}

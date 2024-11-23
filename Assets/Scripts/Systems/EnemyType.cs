using UnityEngine;

[System.Serializable]
public class EnemyType
{
    [Header("���˻�������")]
    public string enemyName;              // ��������
    public GameObject prefab;             // ����Ԥ����
    public string rarity;                 // ϡ�жȣ�Unusual, Rare, Epic �ȣ�

    [Header("��������")]
    public int baseHealth = 100;          // ��ʼѪ��
    public int baseDamage = 10;           // ��ʼ������
    public float baseMoveSpeed = 2f;      // ��ʼ�ƶ��ٶ�
    public Vector3 baseScale = Vector3.one; // ��ʼ���ʹ�С

    [Header("������������")]
    public float healthGrowthRate = 0.1f; // ÿ��Ѫ�������ʣ�10%��
    public float damageGrowthRate = 0.05f; // ÿ�������������ʣ�5%��
    public float speedGrowthRate = 0.02f;  // ÿ���ƶ��ٶ������ʣ�2%��
    public float scaleGrowthRate = 0.1f;   // ÿ�����������ʣ�10%��

    [Header("��������")]
    public int minCardDrop = 1;           // ���ٵ���Ŀ�������
    public int maxCardDrop = 3;           // ������Ŀ�������
    public string[] possibleCardRarities; // �ɵ��俨�Ƶ�ϡ�жȷ�Χ�����ڵ�ǰ���˵ȼ���
}

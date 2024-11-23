using UnityEngine;

[System.Serializable]
public class EnemyType
{
    [Header("敌人基础配置")]
    public string enemyName;              // 敌人名称
    public GameObject prefab;             // 敌人预制体
    public string rarity;                 // 稀有度（Unusual, Rare, Epic 等）

    [Header("基础属性")]
    public int baseHealth = 100;          // 初始血量
    public int baseDamage = 10;           // 初始攻击力
    public float baseMoveSpeed = 2f;      // 初始移动速度
    public Vector3 baseScale = Vector3.one; // 初始体型大小

    [Header("属性增长规则")]
    public float healthGrowthRate = 0.1f; // 每级血量增长率（10%）
    public float damageGrowthRate = 0.05f; // 每级攻击力增长率（5%）
    public float speedGrowthRate = 0.02f;  // 每级移动速度增长率（2%）
    public float scaleGrowthRate = 0.1f;   // 每级体型增长率（10%）

    [Header("掉落配置")]
    public int minCardDrop = 1;           // 最少掉落的卡牌数量
    public int maxCardDrop = 3;           // 最多掉落的卡牌数量
    public string[] possibleCardRarities; // 可掉落卡牌的稀有度范围（低于当前敌人等级）
}

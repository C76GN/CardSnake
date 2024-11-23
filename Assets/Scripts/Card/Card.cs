using UnityEngine;

public class Card
{
    public string cardName;       // 卡牌名称
    public int level;             // 卡牌等级
    public string effect;         // 卡牌效果描述
    public float cooldownTime;    // 冷却时间
    public float lastUsedTime;    // 上次使用时间

    // 构造函数
    public Card(string name, int level, string effect, float cooldown)
    {
        this.cardName = name;
        this.level = level;
        this.effect = effect;
        this.cooldownTime = cooldown;
        this.lastUsedTime = -cooldown; // 初始化为冷却完成
    }

    // 检查卡牌是否可以使用
    public bool CanUse()
    {
        return Time.time - lastUsedTime >= cooldownTime;
    }

    // 应用卡牌效果
    public void Use()
    {
        if (CanUse())
        {
            Debug.Log($"{cardName} 卡牌被使用！");
            lastUsedTime = Time.time;
            // TODO: 执行卡牌效果
        }
        else
        {
            Debug.LogWarning($"{cardName} 卡牌仍在冷却中！");
        }
    }
}

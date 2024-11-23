using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // 卡牌稀有度定义
    private Dictionary<string, Color> rarityColors = new Dictionary<string, Color>
    {
        { "Common", Color.green },
        { "Unusual", Color.yellow },
        { "Rare", Color.blue },
        { "Epic", new Color(0.5f, 0, 0.5f) },   // 紫色
        { "Legendary", Color.red },
        { "Mythic", Color.cyan },
        { "Ultra", new Color(1, 0.2f, 0.8f) },  // 粉色
        { "Super", new Color(0.5f, 1, 0.3f) }   // 荧光绿
    };

    // 动态调整合成概率参数
    public float baseSuccessRate = 0.8f; // 合成的初始成功率（80%）
    public float rateDecayPerLevel = 0.1f; // 每级别下降的成功率（10%）

    // 用于存储所有类型的卡牌配置
    public List<CardType> availableCardTypes = new List<CardType>();

    // 卡牌类型结构
    [System.Serializable]
    public class CardType
    {
        public string cardName; // 卡牌名称
        public string effect;   // 卡牌效果描述
        public float cooldown;  // 卡牌冷却时间
    }

    // 存储玩家的所有卡牌
    public List<Card> playerCards = new List<Card>();

    // 生成卡牌
    public Card GenerateCard(string cardName, int level)
    {
        // 根据卡牌名称查找配置
        CardType cardType = availableCardTypes.Find(card => card.cardName == cardName);

        if (cardType == null)
        {
            Debug.LogWarning($"卡牌类型 {cardName} 不存在！");
            return null;
        }

        // 创建新卡牌实例
        Card newCard = new Card(cardType.cardName, level, cardType.effect, cardType.cooldown);
        playerCards.Add(newCard);

        Debug.Log($"生成了一张卡牌: {newCard.cardName}, 等级: {newCard.level}, 效果: {newCard.effect}");
        return newCard;
    }

    // 合成卡牌
    public Card CombineCards(List<Card> cards)
    {
        if (cards.Count < 5)
        {
            Debug.LogWarning("需要至少 5 张同类型且同等级的卡牌才能合成！");
            return null;
        }

        // 确保所有卡牌类型和等级相同
        string targetName = cards[0].cardName;
        int targetLevel = cards[0].level;
        if (cards.Exists(card => card.cardName != targetName || card.level != targetLevel))
        {
            Debug.LogWarning("所有卡牌必须是同类型且同等级！");
            return null;
        }

        // 计算合成成功率
        float successRate = baseSuccessRate - (targetLevel * rateDecayPerLevel);
        successRate = Mathf.Clamp(successRate, 0.1f, 1.0f); // 确保在 10%~100% 之间

        if (Random.value <= successRate)
        {
            // 合成成功，生成更高一级卡牌
            Card newCard = GenerateCard(targetName, targetLevel + 1);
            Debug.Log($"合成成功！生成新卡牌: {newCard.cardName}, 等级: {newCard.level}");
            return newCard;
        }
        else
        {
            // 合成失败，随机销毁 1~4 张卡牌
            int destroyCount = Random.Range(1, 5);
            for (int i = 0; i < destroyCount && cards.Count > 0; i++)
            {
                Card destroyedCard = cards[Random.Range(0, cards.Count)];
                playerCards.Remove(destroyedCard);
                cards.Remove(destroyedCard);
                Debug.LogWarning($"合成失败！摧毁了卡牌: {destroyedCard.cardName}, 等级: {destroyedCard.level}");
            }
            return null;
        }
    }

    // 获取卡牌的颜色
    public Color GetCardColor(string rarity)
    {
        if (rarityColors.TryGetValue(rarity, out Color color))
        {
            return color;
        }
        return Color.white; // 未知稀有度返回白色
    }
}

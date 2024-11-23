using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // ����ϡ�жȶ���
    private Dictionary<string, Color> rarityColors = new Dictionary<string, Color>
    {
        { "Common", Color.green },
        { "Unusual", Color.yellow },
        { "Rare", Color.blue },
        { "Epic", new Color(0.5f, 0, 0.5f) },   // ��ɫ
        { "Legendary", Color.red },
        { "Mythic", Color.cyan },
        { "Ultra", new Color(1, 0.2f, 0.8f) },  // ��ɫ
        { "Super", new Color(0.5f, 1, 0.3f) }   // ӫ����
    };

    // ��̬�����ϳɸ��ʲ���
    public float baseSuccessRate = 0.8f; // �ϳɵĳ�ʼ�ɹ��ʣ�80%��
    public float rateDecayPerLevel = 0.1f; // ÿ�����½��ĳɹ��ʣ�10%��

    // ���ڴ洢�������͵Ŀ�������
    public List<CardType> availableCardTypes = new List<CardType>();

    // �������ͽṹ
    [System.Serializable]
    public class CardType
    {
        public string cardName; // ��������
        public string effect;   // ����Ч������
        public float cooldown;  // ������ȴʱ��
    }

    // �洢��ҵ����п���
    public List<Card> playerCards = new List<Card>();

    // ���ɿ���
    public Card GenerateCard(string cardName, int level)
    {
        // ���ݿ������Ʋ�������
        CardType cardType = availableCardTypes.Find(card => card.cardName == cardName);

        if (cardType == null)
        {
            Debug.LogWarning($"�������� {cardName} �����ڣ�");
            return null;
        }

        // �����¿���ʵ��
        Card newCard = new Card(cardType.cardName, level, cardType.effect, cardType.cooldown);
        playerCards.Add(newCard);

        Debug.Log($"������һ�ſ���: {newCard.cardName}, �ȼ�: {newCard.level}, Ч��: {newCard.effect}");
        return newCard;
    }

    // �ϳɿ���
    public Card CombineCards(List<Card> cards)
    {
        if (cards.Count < 5)
        {
            Debug.LogWarning("��Ҫ���� 5 ��ͬ������ͬ�ȼ��Ŀ��Ʋ��ܺϳɣ�");
            return null;
        }

        // ȷ�����п������ͺ͵ȼ���ͬ
        string targetName = cards[0].cardName;
        int targetLevel = cards[0].level;
        if (cards.Exists(card => card.cardName != targetName || card.level != targetLevel))
        {
            Debug.LogWarning("���п��Ʊ�����ͬ������ͬ�ȼ���");
            return null;
        }

        // ����ϳɳɹ���
        float successRate = baseSuccessRate - (targetLevel * rateDecayPerLevel);
        successRate = Mathf.Clamp(successRate, 0.1f, 1.0f); // ȷ���� 10%~100% ֮��

        if (Random.value <= successRate)
        {
            // �ϳɳɹ������ɸ���һ������
            Card newCard = GenerateCard(targetName, targetLevel + 1);
            Debug.Log($"�ϳɳɹ��������¿���: {newCard.cardName}, �ȼ�: {newCard.level}");
            return newCard;
        }
        else
        {
            // �ϳ�ʧ�ܣ�������� 1~4 �ſ���
            int destroyCount = Random.Range(1, 5);
            for (int i = 0; i < destroyCount && cards.Count > 0; i++)
            {
                Card destroyedCard = cards[Random.Range(0, cards.Count)];
                playerCards.Remove(destroyedCard);
                cards.Remove(destroyedCard);
                Debug.LogWarning($"�ϳ�ʧ�ܣ��ݻ��˿���: {destroyedCard.cardName}, �ȼ�: {destroyedCard.level}");
            }
            return null;
        }
    }

    // ��ȡ���Ƶ���ɫ
    public Color GetCardColor(string rarity)
    {
        if (rarityColors.TryGetValue(rarity, out Color color))
        {
            return color;
        }
        return Color.white; // δ֪ϡ�жȷ��ذ�ɫ
    }
}

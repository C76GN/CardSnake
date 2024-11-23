using UnityEngine;

public interface ICardEffect
{
    void ApplyEffect(GameObject target); // 应用效果到目标对象
    void RemoveEffect(GameObject target); // 移除效果
}

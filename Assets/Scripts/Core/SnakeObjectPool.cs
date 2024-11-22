using System.Collections.Generic;
using UnityEngine;

public class SnakeObjectPool : MonoBehaviour
{
    private Queue<GameObject> pool = new Queue<GameObject>(); // 对象池队列
    public GameObject prefab; // 预制体，用于创建新的对象

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true); // 激活对象
            return obj;
        }

        // 如果对象池为空，动态创建新对象
        return Instantiate(prefab, position, rotation);
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // 禁用对象

        // 重置位置和旋转，避免下次复用时出现问题
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        pool.Enqueue(obj); // 返回到对象池
    }
}

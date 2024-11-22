using System.Collections.Generic;
using UnityEngine;

public class SnakeObjectPool : MonoBehaviour
{
    private Queue<GameObject> pool = new Queue<GameObject>(); // ����ض���
    public GameObject prefab; // Ԥ���壬���ڴ����µĶ���

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true); // �������
            return obj;
        }

        // ��������Ϊ�գ���̬�����¶���
        return Instantiate(prefab, position, rotation);
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // ���ö���

        // ����λ�ú���ת�������´θ���ʱ��������
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        pool.Enqueue(obj); // ���ص������
    }
}

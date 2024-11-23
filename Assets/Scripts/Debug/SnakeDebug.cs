using System.Collections;
using UnityEngine;

public class SnakeDebug : MonoBehaviour
{
    private SnakeGrowth snakeGrowth;

    void Start()
    {
        snakeGrowth = GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("SnakeGrowth δ�ҵ���");
        }
    }

    void OnEnable()
    {
        EventManager.Subscribe("OnBodyPartDamaged", HandleBodyPartDamaged);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("OnBodyPartDamaged", HandleBodyPartDamaged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("���������");
            snakeGrowth.AddBodySegment(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("�Ե�һ���������� 10 ���˺�");
            snakeGrowth.TakeDamage(1, 10f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("�����ߵ�״̬");
            snakeGrowth.ResetSnake();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("��ʼ��ӡ��������ε�Ѫ��");
            StartCoroutine(PrintBodySegmentHealthCoroutine());
        }
    }

    private void HandleBodyPartDamaged(object index)
    {
        Debug.Log($"����� {index} ���𻵣�");
    }

    private IEnumerator PrintBodySegmentHealthCoroutine()
    {
        while (true)
        {
            if (snakeGrowth.BodyPartCount == 0 || snakeGrowth.HealthPartCount == 0)
            {
                Debug.LogWarning("�����б���δ��ʼ�������������Ϊ 0��");
            }
            else
            {
                float[] healthArray = new float[snakeGrowth.BodyPartCount];
                for (int i = 0; i < snakeGrowth.BodyPartCount; i++)
                {
                    healthArray[i] = snakeGrowth.GetBodySegmentHealth(i);
                }

                Debug.Log($"����εĵ�ǰѪ��: {string.Join(", ", healthArray)}");
            }

            yield return new WaitForSeconds(2f);
        }
    }
}

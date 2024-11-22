using System.Collections;
using UnityEngine;

public class SnakeDebug : MonoBehaviour
{
    private SnakeGrowth snakeGrowth;

    void Start()
    {
        // ��ȡ SnakeGrowth ���
        snakeGrowth = GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("SnakeGrowth δ�ҵ���");
        }
    }

    void Update()
    {
        // �������������
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("���������");
            snakeGrowth.AddBodySegment(1);
        }

        // ���ԶԵ�һ�����������˺�
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("�Ե�һ���������� 10 ���˺�");
            snakeGrowth.TakeDamage(1, 10f);
        }

        // ���������ߵ�״̬
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("�����ߵ�״̬");
            snakeGrowth.ResetSnake();
        }

        // ���Զ�̬��չ�����
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("���Զ���ض�̬��չ");
            for (int i = 0; i < 15; i++) // ����Ĭ���������Դ�������ض�̬��չ
            {
                snakeGrowth.AddBodySegment(1);
            }
        }

        // ���Խ����ָ�
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("ģ�⽡���ָ�");
            snakeGrowth.TakeDamage(1, 60f); // �Ե�һ���������ɽϴ��˺�
        }

        // ���������ƽ������
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("��������θ����߼�");
            snakeGrowth.AddBodySegment(5); // ���Ӷ�������
        }

        // ��������ε���״̬
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("��������ε���״̬");
            snakeGrowth.TakeDamage(2, 110f); // ���ڶ�������ε�Ѫ������ 0
        }

        // ��ӡÿ������ε�Ѫ��
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("��ʼ��ӡ��������ε�Ѫ��");
            StartCoroutine(PrintBodySegmentHealthCoroutine());
        }
    }

    // ��ӡÿ������ε�Ѫ��
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

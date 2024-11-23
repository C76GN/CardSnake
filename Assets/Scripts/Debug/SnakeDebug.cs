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
            Debug.LogError("SnakeGrowth 未找到！");
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
            Debug.Log("增加身体段");
            snakeGrowth.AddBodySegment(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("对第一个身体段造成 10 点伤害");
            snakeGrowth.TakeDamage(1, 10f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("重置蛇的状态");
            snakeGrowth.ResetSnake();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("开始打印所有身体段的血量");
            StartCoroutine(PrintBodySegmentHealthCoroutine());
        }
    }

    private void HandleBodyPartDamaged(object index)
    {
        Debug.Log($"身体段 {index} 已损坏！");
    }

    private IEnumerator PrintBodySegmentHealthCoroutine()
    {
        while (true)
        {
            if (snakeGrowth.BodyPartCount == 0 || snakeGrowth.HealthPartCount == 0)
            {
                Debug.LogWarning("健康列表尚未初始化或身体段数量为 0！");
            }
            else
            {
                float[] healthArray = new float[snakeGrowth.BodyPartCount];
                for (int i = 0; i < snakeGrowth.BodyPartCount; i++)
                {
                    healthArray[i] = snakeGrowth.GetBodySegmentHealth(i);
                }

                Debug.Log($"身体段的当前血量: {string.Join(", ", healthArray)}");
            }

            yield return new WaitForSeconds(2f);
        }
    }
}

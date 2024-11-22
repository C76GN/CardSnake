using System.Collections;
using UnityEngine;

public class SnakeDebug : MonoBehaviour
{
    private SnakeGrowth snakeGrowth;

    void Start()
    {
        // 获取 SnakeGrowth 组件
        snakeGrowth = GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("SnakeGrowth 未找到！");
        }
    }

    void Update()
    {
        // 测试增加身体段
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("增加身体段");
            snakeGrowth.AddBodySegment(1);
        }

        // 测试对第一个身体段造成伤害
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("对第一个身体段造成 10 点伤害");
            snakeGrowth.TakeDamage(1, 10f);
        }

        // 测试重置蛇的状态
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("重置蛇的状态");
            snakeGrowth.ResetSnake();
        }

        // 测试动态扩展对象池
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("测试对象池动态扩展");
            for (int i = 0; i < 15; i++) // 超过默认最大段数以触发对象池动态扩展
            {
                snakeGrowth.AddBodySegment(1);
            }
        }

        // 测试健康恢复
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("模拟健康恢复");
            snakeGrowth.TakeDamage(1, 60f); // 对第一个身体段造成较大伤害
        }

        // 测试身体段平滑跟随
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("测试身体段跟随逻辑");
            snakeGrowth.AddBodySegment(5); // 增加多个身体段
        }

        // 测试身体段的损坏状态
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("测试身体段的损坏状态");
            snakeGrowth.TakeDamage(2, 110f); // 将第二个身体段的血量降至 0
        }

        // 打印每个身体段的血量
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("开始打印所有身体段的血量");
            StartCoroutine(PrintBodySegmentHealthCoroutine());
        }
    }

    // 打印每个身体段的血量
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

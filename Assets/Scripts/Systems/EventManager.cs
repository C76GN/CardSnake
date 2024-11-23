using System;
using System.Collections.Generic;

/// <summary>
/// 事件管理器，用于模块之间的事件订阅、取消订阅和事件触发
/// </summary>
public static class EventManager
{
    // 存储事件名称与其对应的监听器的字典
    private static Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

    /// <summary>
    /// 订阅指定事件
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="listener">订阅该事件的回调函数</param>
    public static void Subscribe(string eventName, Action<object> listener)
    {
        // 如果事件尚未注册，则添加一个新的事件条目
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            // 如果事件已存在，则将新的监听器添加到事件的回调列表中
            eventDictionary[eventName] += listener;
        }
    }

    /// <summary>
    /// 取消订阅指定事件
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="listener">需要取消的回调函数</param>
    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        // 检查事件是否存在于字典中
        if (eventDictionary.ContainsKey(eventName))
        {
            // 从事件的回调列表中移除指定的监听器
            eventDictionary[eventName] -= listener;

            // 如果该事件没有监听器，移除该事件条目
            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 触发指定事件，并向监听器传递参数
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="parameter">传递给监听器的参数</param>
    public static void TriggerEvent(string eventName, object parameter)
    {
        // 检查事件是否存在，并调用对应的回调函数
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(parameter);
        }
    }
}

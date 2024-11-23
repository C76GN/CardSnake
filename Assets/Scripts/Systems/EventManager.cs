using System;
using System.Collections.Generic;

/// <summary>
/// �¼�������������ģ��֮����¼����ġ�ȡ�����ĺ��¼�����
/// </summary>
public static class EventManager
{
    // �洢�¼����������Ӧ�ļ��������ֵ�
    private static Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

    /// <summary>
    /// ����ָ���¼�
    /// </summary>
    /// <param name="eventName">�¼�����</param>
    /// <param name="listener">���ĸ��¼��Ļص�����</param>
    public static void Subscribe(string eventName, Action<object> listener)
    {
        // ����¼���δע�ᣬ�����һ���µ��¼���Ŀ
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            // ����¼��Ѵ��ڣ����µļ�������ӵ��¼��Ļص��б���
            eventDictionary[eventName] += listener;
        }
    }

    /// <summary>
    /// ȡ������ָ���¼�
    /// </summary>
    /// <param name="eventName">�¼�����</param>
    /// <param name="listener">��Ҫȡ���Ļص�����</param>
    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        // ����¼��Ƿ�������ֵ���
        if (eventDictionary.ContainsKey(eventName))
        {
            // ���¼��Ļص��б����Ƴ�ָ���ļ�����
            eventDictionary[eventName] -= listener;

            // ������¼�û�м��������Ƴ����¼���Ŀ
            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// ����ָ���¼���������������ݲ���
    /// </summary>
    /// <param name="eventName">�¼�����</param>
    /// <param name="parameter">���ݸ��������Ĳ���</param>
    public static void TriggerEvent(string eventName, object parameter)
    {
        // ����¼��Ƿ���ڣ������ö�Ӧ�Ļص�����
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(parameter);
        }
    }
}

using System.Collections.Generic;

/// <summary>
/// �ṩ�� List ����չ���������ڵ����б�Ĵ�С
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// �����б�Ĵ�С��Ŀ��������
    /// �����ǰ�б�Ĵ�СС��Ŀ������������ָ����Ĭ��ֵ���������Ԫ�ء�
    /// �����ǰ�б�Ĵ�С����Ŀ�����������Ƴ������Ԫ�ء�
    /// </summary>
    /// <typeparam name="T">�б���Ԫ�ص�����</typeparam>
    /// <param name="list">��Ҫ������С���б�</param>
    /// <param name="targetCount">Ŀ���б�Ĵ�С</param>
    /// <param name="defaultValue">�������б��Сʱ�������������Ԫ�ص�Ĭ��ֵ</param>
    public static void Resize<T>(this List<T> list, int targetCount, T defaultValue)
    {
        // ���б��СС��Ŀ������ʱ�����Ĭ��ֵ������б�
        while (list.Count < targetCount)
        {
            list.Add(defaultValue);
        }

        // ���б��С����Ŀ������ʱ���Ƴ������Ԫ��
        while (list.Count > targetCount)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}

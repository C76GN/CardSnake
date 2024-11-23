using System.Collections.Generic;

/// <summary>
/// 提供对 List 的扩展方法，用于调整列表的大小
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// 调整列表的大小到目标数量。
    /// 如果当前列表的大小小于目标数量，则用指定的默认值填充新增的元素。
    /// 如果当前列表的大小大于目标数量，则移除多余的元素。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型</typeparam>
    /// <param name="list">需要调整大小的列表</param>
    /// <param name="targetCount">目标列表的大小</param>
    /// <param name="defaultValue">当增加列表大小时，用于填充新增元素的默认值</param>
    public static void Resize<T>(this List<T> list, int targetCount, T defaultValue)
    {
        // 当列表大小小于目标数量时，添加默认值以填充列表
        while (list.Count < targetCount)
        {
            list.Add(defaultValue);
        }

        // 当列表大小大于目标数量时，移除多余的元素
        while (list.Count > targetCount)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}

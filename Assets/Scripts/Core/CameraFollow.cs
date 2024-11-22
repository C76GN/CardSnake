using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("����Ŀ������")]
    public Transform target; // Ҫ���ٵ�Ŀ�꣨��ͷ��

    [Header("�ӽ�ƫ������")]
    public Vector3 baseOffset = new Vector3(0, 10, -10); // ������Ļ���ƫ����
    public float followSpeed = 5f; // �����ƽ���ٶ�

    [Header("��������")]
    public float zoomSpeed = 2f; // �����ٶ�
    public float minZoomFactor = 0.5f; // ��С���ű���
    public float maxZoomFactor = 2f; // ������ű���
    public float defaultZoomFactor = 1f; // Ĭ�����ű���

    // �ڲ�����
    private Vector3 currentOffset; // ��ǰƫ����
    private float zoomFactor; // ��ǰ��������
    private Coroutine currentZoomCoroutine; // ��ǰ������Э��

    void Start()
    {
        // ��ʼ���������Ӻ�ƫ����
        zoomFactor = defaultZoomFactor;
        currentOffset = baseOffset * zoomFactor;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("�����Ŀ��δ���ã�");
            return;
        }

        // �����ӽ�
        AdjustView();

        // �����������Ŀ��λ��
        Vector3 desiredPosition = target.position + currentOffset;

        // ƽ���ƶ��������Ŀ��λ��
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // ʼ�ճ���Ŀ��
        transform.LookAt(target);
    }

    private void AdjustView()
    {
        // ���ݵ�ǰ�������ӵ���ƫ����
        currentOffset = baseOffset * zoomFactor;
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="newZoomFactor">�µ���������</param>
    /// <param name="smoothTransition">�Ƿ�����ƽ������</param>
    public void SetZoomFactor(float newZoomFactor, bool smoothTransition = true)
    {
        // ����������������С�����Χ��
        newZoomFactor = Mathf.Clamp(newZoomFactor, minZoomFactor, maxZoomFactor);

        if (smoothTransition)
        {
            if (currentZoomCoroutine != null)
            {
                StopCoroutine(currentZoomCoroutine);
            }
            currentZoomCoroutine = StartCoroutine(SmoothZoomTransition(newZoomFactor));
        }
        else
        {
            zoomFactor = newZoomFactor;
            AdjustView();
        }
    }

    /// <summary>
    /// ƽ�������������ӵ�Э��
    /// </summary>
    /// <param name="targetZoomFactor">Ŀ����������</param>
    /// <returns>Э��</returns>
    private System.Collections.IEnumerator SmoothZoomTransition(float targetZoomFactor)
    {
        while (Mathf.Abs(zoomFactor - targetZoomFactor) > 0.01f)
        {
            zoomFactor = Mathf.Lerp(zoomFactor, targetZoomFactor, zoomSpeed * Time.deltaTime);
            AdjustView();
            yield return null;
        }
        zoomFactor = targetZoomFactor;
    }
}

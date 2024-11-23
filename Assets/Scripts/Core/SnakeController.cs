using UnityEngine;

// ������ͷ���ƶ�����ת��Ϊ������ Rigidbody �����������ģ��
[RequireComponent(typeof(Rigidbody))]
public class SnakeController : MonoBehaviour
{
    // �ߵĲ��������ļ�
    public SnakeConfig config;

    // �����ƶ���ͷ���������
    private Rigidbody rb;

    // ��ǰ�����뷽��
    private Vector3 inputDirection;

    // ��ͷ�ĵ�ǰ�ٶ�
    private Vector3 currentVelocity;

    // ��С�ٶ���ֵ�����ڸ�ֵʱ��ͷ��������ת
    private float minSpeedThreshold = 0.1f;

    // ��������������������������ڴ���������εĽ���
    private SnakeGrowth snakeGrowth;

    /// <summary>
    /// ��ʼ��������������Ҫ�������Ƿ����
    /// </summary>
    void Start()
    {
        InitializeController();
    }

    /// <summary>
    /// ����������ĳ�ʼ�������� Rigidbody ��������õļ��
    /// </summary>
    private void InitializeController()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("δ�ҵ� Rigidbody �����");
            enabled = false;
            return;
        }

        // ���� Rigidbody �Ĳ�����ȷ������ģ���������
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        if (config == null)
        {
            Debug.LogError("δ���� SnakeConfig �����ļ���");
            enabled = false;
            return;
        }

        snakeGrowth = transform.parent.GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("δ�ҵ� SnakeGrowth �����");
            enabled = false;
        }
    }

    /// <summary>
    /// �������뷽��
    /// </summary>
    void Update()
    {
        ProcessInput();
    }

    /// <summary>
    /// ��ȡ������벢�����ƶ�����
    /// </summary>
    private void ProcessInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    /// <summary>
    /// ����������£������ƶ�����ת
    /// </summary>
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// �������뷽������ͷ���ƶ�
    /// </summary>
    private void HandleMovement()
    {
        if (inputDirection.magnitude > 0)
        {
            // �𽥼��ٵ�Ŀ���ٶ�
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                inputDirection * config.moveSpeed,
                Time.fixedDeltaTime * 10f
            );
        }
        else
        {
            // ƽ�����ٵ���ֹ״̬
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero,
                Time.fixedDeltaTime * 10f
            );
        }

        // ʹ�� Rigidbody �ƶ���ͷ
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// ���ݵ�ǰ�ٶȴ�����ͷ����ת����
    /// </summary>
    private void HandleRotation()
    {
        if (currentVelocity.sqrMagnitude > minSpeedThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                Time.fixedDeltaTime * config.rotationSpeed
            ));
        }
    }
}

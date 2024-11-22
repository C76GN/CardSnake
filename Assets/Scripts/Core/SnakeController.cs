using UnityEngine;

// ȷ������������� Rigidbody ���
[RequireComponent(typeof(Rigidbody))]
public class SnakeController : MonoBehaviour
{
    // �ߵ����ò����������ƶ��ٶȡ���ת�ٶȵ�
    public SnakeConfig config;

    // ���ڴ�������ģ��� Rigidbody ���
    private Rigidbody rb;

    // �洢���뷽��
    private Vector3 inputDirection;

    // ��ǰ�ٶȣ�����ƽ���ƶ�
    private Vector3 currentVelocity;

    // ��ת����С�ٶ���ֵ
    private float minSpeedThreshold = 0.1f;

    // �����ߵ�����������
    private SnakeGrowth snakeGrowth;

    void Start()
    {
        // ��ȡ Rigidbody ���
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody δ�ҵ���");
            enabled = false; // ���ýű�����ֹ��������
            return;
        }

        // ���� Rigidbody ��������ֹ��������Ӱ�첢����ĳЩ������˶�����ת
        rb.useGravity = false; // ��������
        rb.isKinematic = true; // ����Ϊ Kinematic������������Ӱ�죩
        rb.constraints = RigidbodyConstraints.FreezePositionY | // ���ƴ�ֱ�����ƶ�
                         RigidbodyConstraints.FreezeRotationX | // ���� X ����ת
                         RigidbodyConstraints.FreezeRotationZ;  // ���� Z ����ת

        // ����Ƿ��������ߵ������ļ�
        if (config == null)
        {
            Debug.LogError("SnakeConfig δ���ã�");
            enabled = false;
            return;
        }

        // ��ȡ�ߵ�����������
        snakeGrowth = transform.parent.GetComponent<SnakeGrowth>();
        if (snakeGrowth == null)
        {
            Debug.LogError("SnakeGrowth δ�ҵ���");
            enabled = false;
        }
    }

    void Update()
    {
        // ��ȡ������루ˮƽ�ʹ�ֱ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical).normalized; // ��׼����������
    }

    void FixedUpdate()
    {
        // �����ƶ�����ת
        HandleMovement();
        HandleRotation();
    }

    // �����ߵ��ƶ�
    private void HandleMovement()
    {
        if (inputDirection.magnitude > 0) // ����Ƿ�������
        {
            // ƽ����ֵ���㵱ǰ�ٶȣ�ʹ���𽥼��ٵ�Ŀ���ٶ�
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                inputDirection * config.moveSpeed, // Ŀ���ٶ��������е��ƶ��ٶ�ȷ��
                Time.fixedDeltaTime * 10f // ƽ��ϵ����Ӱ�����/���ٵ��ٶ�
            );
        }
        else
        {
            // ���û�����룬�𽥼��ٵ���ֹ״̬
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero, // Ŀ���ٶ�Ϊ��
                Time.fixedDeltaTime * 10f
            );
        }

        // ʹ�� Rigidbody �ƶ��ߵ�λ��
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    // �����ߵ���ת
    private void HandleRotation()
    {
        // �����ٶȳ�����ֵʱ�Ž�����ת
        if (currentVelocity.sqrMagnitude > minSpeedThreshold)
        {
            // ����Ŀ����ת����
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);

            // ƽ����ֵ��ת��Ŀ�귽��
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,           // ��ǰ��ת
                targetRotation,        // Ŀ����ת
                Time.fixedDeltaTime * config.rotationSpeed // ��ת�ٶ�������ȷ��
            ));
        }
    }
}

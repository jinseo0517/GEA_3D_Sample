using Cinemachine; // �ó׸ӽ� ī�޶� ��� ���
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // ���� �̵� �ӵ�
    private float currentSpeed;

    // �̵� �ӵ� ��������
    private float stopSpeed = 0f;     // ���� ���� �ӵ�
    private float walkSpeed = 5f;     // �ȱ� �ӵ�
    private float runSpeed = 12f;     // �޸��� �ӵ�

    // ���� �Ŀ� �� �߷� ��
    private float jumpPower = 10f;
    public float gravity = -9.81f;

    // TPS ī�޶� �� ȸ�� �ӵ�
    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 10f;

    // ī�޶��� POV ������Ʈ (Yaw �� ��� ���� ���)
    private CinemachinePOV pov;

    // ĳ���� ��Ʈ�ѷ� �� �ӵ� ����
    private CharacterController controller;
    private Vector3 velocity;

    // �ٴڿ� ��� �ִ��� ����
    public bool isGrounded;

    // ���� �޸��� �������� ����
    public bool isRunning;

    // ī�޶� ��� Ȯ�ο� ��ũ��Ʈ ����
    public CinemachineSwitcher cinemachineSwitcher;

    public int maxHP = 100;
    private int currentHP;

    public Slider hpSlider;

    void Start()
    {
        // ĳ���� ��Ʈ�ѷ��� POV ������Ʈ �ʱ�ȭ
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();

        currentHP = maxHP;
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pov.m_HorizontalAxis.Value = transform.eulerAngles.y;
            pov.m_VerticalAxis.Value = 0f;
        }

        // FreeLook ��尡 �ƴ� ���� �̵� �ӵ� ����
        if (cinemachineSwitcher.usingFreeLook == false)
        {
            // ���� ����Ʈ Ű�� ������ �޸��� ���
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
                isRunning = true;
            }
            else
            {
                currentSpeed = walkSpeed;
                isRunning = false;
            }
        }

        // �޸��� ������ �� ī�޶� FOV Ȯ�� (�ӵ��� ����)
        if (isRunning)
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, 65f, Time.deltaTime * 5f);
        }
        else
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, 40f, Time.deltaTime * 5f);
        }

        // �ٴ� üũ �� ���� �ӵ� �ʱ�ȭ
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Ű���� �Է°� ��������
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // ī�޶� ���� ���� ���
        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // �Է� ���⿡ ���� �̵� ���� ���
        Vector3 move = (camForward * z + camRight * x).normalized;

        // �̵� ó��
        controller.Move(move * currentSpeed * Time.deltaTime);

        // FreeLook ����� �� �̵� �� ���� ����
        if (cinemachineSwitcher.usingFreeLook == true)
        {
            currentSpeed = stopSpeed;
            jumpPower = 0f;
        }
        else
        {
            currentSpeed = walkSpeed;
            jumpPower = 5f;
        }

        // ī�޶� Yaw �� �������� �÷��̾� ȸ�� ó��
        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        // ���� �Է� ó��
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ���� ���� �̵� ó��
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
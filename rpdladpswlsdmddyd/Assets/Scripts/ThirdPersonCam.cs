using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Referencess")]
    public Transform orientation;   // �÷��̾��� ���� ������ �Ǵ� Transform
    public Transform player;        // �÷��̾��� ��ġ�� �����ϴ� Transform
    public Transform playerOBJ;     // ���� ȸ����ų �÷��̾� ������Ʈ
    public Rigidbody rb;            // �÷��̾��� Rigidbody (���� �ڵ忡���� ������ ����)

    public float rotationSpeed;     // �÷��̾� ȸ�� �ӵ�

    private void Start()
    {
        // ���� ���� �� ���콺 Ŀ���� ��װ� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ī�޶� �������� �÷��̾� ���� ���� ��� (Y�� ����)
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

        // orientation ������Ʈ�� �÷��̾� �������� ����
        orientation.forward = viewDir.normalized;

        // Ű���� �Է°� ��������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �Է� ���� ���� ��� (ī�޶� ����)
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // �Է� ������ ���� ���, �÷��̾� ������Ʈ�� �ش� �������� �ε巴�� ȸ��
        if (inputDir != Vector3.zero)
            playerOBJ.forward = Vector3.Slerp(playerOBJ.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}
using UnityEngine;           // Unity �⺻ ��� ���
using Cinemachine;          // Cinemachine ī�޶� ��� ���

public class CinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;   // TPS�� �ó׸ӽ� ī�޶�
    public CinemachineFreeLook freeLookCam;       // FreeLook�� �ó׸ӽ� ī�޶�
    public bool usingFreeLook = false;            // ���� FreeLook ������� ����

    // ���� ���� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ���

        // �⺻ ī�޶� �켱���� ����: TPS ī�޶� Ȱ��ȭ��
        virtualCam.Priority = 10;
        freeLookCam.Priority = 0;
    }

    // �� �����Ӹ��� ȣ��Ǵ� ������Ʈ �Լ�
    void Update()
    {

        // ESC Ű�� ������ Ŀ�� ���/���� ���
        if (Input.GetKeyDown(KeyCode.Escape)) { ToggleCursor(); }

        // Ŀ���� ��� ���°� �ƴϸ� ī�޶� ��ȯ�� ����
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // ���콺 ������ ��ư Ŭ�� �� ī�޶� ��� ��ȯ
        if (Input.GetMouseButtonDown(1))
        {
            usingFreeLook = !usingFreeLook; // FreeLook ���� ���

            if (usingFreeLook)
            {
                // FreeLook ī�޶� Ȱ��ȭ
                freeLookCam.Priority = 20;
                virtualCam.Priority = 0;
            }
            else
            {
                // TPS ī�޶� Ȱ��ȭ
                virtualCam.Priority = 20;
                freeLookCam.Priority = 0;
            }
        }
    }

    // Ŀ�� ���/������ ��ȯ�ϴ� �Լ�
    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Ŀ�� ���� �� ǥ��
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Ŀ�� ��� �� ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
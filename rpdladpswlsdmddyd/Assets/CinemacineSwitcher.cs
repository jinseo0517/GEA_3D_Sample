using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemacineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;  //�⺻ TPSī�޶�
    public CinemachineFreeLook freeLookCam;
    public bool usingFreeLook = false;

    // Start is called before the first frame update
    void Start()
    {
        //������ Virtual Camera Ȱ��ȭ
        VirtualCam.Priority = 10;
        freeLookCam.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))    //��Ŭ��
        {
            usingFreeLook = !usingFreeLook;
            if (usingFreeLook)
            {
                freeLookCam.Priority = 20;  // FreeLook Ȱ��ȭ
                VirtualCam.Priority = 0;
            }
            else
            {
                VirtualCam.Priority = 20;   // Virtual Camera Ȱ��ȭ
                freeLookCam.Priority = 0;
            }
        }
    }
}

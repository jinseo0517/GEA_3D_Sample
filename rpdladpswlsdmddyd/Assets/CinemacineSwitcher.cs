using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemacineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;  //기본 TPS카메라
    public CinemachineFreeLook freeLookCam;
    public bool usingFreeLook = false;

    // Start is called before the first frame update
    void Start()
    {
        //시작은 Virtual Camera 활성화
        VirtualCam.Priority = 10;
        freeLookCam.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))    //우클릭
        {
            usingFreeLook = !usingFreeLook;
            if (usingFreeLook)
            {
                freeLookCam.Priority = 20;  // FreeLook 활성화
                VirtualCam.Priority = 0;
            }
            else
            {
                VirtualCam.Priority = 20;   // Virtual Camera 활성화
                freeLookCam.Priority = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public WeaponSO[] weapon;
    public WeaponSO currentWeapon;
    public Transform firePoint;         //�߻���ġ (�ѱ�)
    Camera cam;

    void Start()
    {
        cam = Camera.main;      //���� ī�޶� ��������
        currentWeapon = weapon[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //��Ŭ�� �߻�
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentWeapon != null && currentWeapon == weapon[0])
            {
                currentWeapon = weapon[1];
            }
            else if (currentWeapon != null && currentWeapon == weapon[1])
            {
                currentWeapon = weapon[0];

            }
        }
    }
    void Shoot()
    {
        //ȭ�鿡 ���콺 -> ����(Ray)���
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;   //���⺤��

        // Projectile����
        GameObject proj = Instantiate(currentWeapon.weaponPrefab, firePoint.position, Quaternion.LookRotation(direction));
    }
}

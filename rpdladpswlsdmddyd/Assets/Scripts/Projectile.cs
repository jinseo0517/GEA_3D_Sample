using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 20f;       //�̵� �ӵ�
    public float lifeTime = 2f;     //�����ð� (��)

    void Start()
    {
        // ���� �ð� �� �ڵ� ���� (�޸� ����)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ������ forward ����(��)���� �̵�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDAmage(1);    //ü�� 1����
            }
            Destroy(gameObject);        //�Ѿ�����
        }
    }
}

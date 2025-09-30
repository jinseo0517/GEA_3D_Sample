using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // �÷��̾�� �� ���ط�
    public int damage = 2;

    // ����ü�� �̵� �ӵ�
    public float speed = 8f;

    // ����ü�� ������ �ִ� �ð� (��)
    public float lifeTime = 3f;

    // ����ü�� �̵� ����
    private Vector3 moveDir;

    // ����ü�� ������ �����ϴ� �Լ�
    public void SetDirection(Vector3 dir)
    {
        moveDir = dir.normalized; // ���� ���͸� ����ȭ�Ͽ� ����
    }

    // ������Ʈ�� ������ �� ȣ���
    void Start()
    {
        Destroy(gameObject, lifeTime); // ���� �ð��� ������ ������Ʈ ����
    }

    // �� �����Ӹ��� ȣ���
    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime; // ���⿡ ���� ��ġ �̵�
    }

    // �ٸ� �ݶ��̴��� �浹���� �� ȣ���
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            // PlayerController ������Ʈ�� ������
            PlayerController pc = other.GetComponent<PlayerController>();

            // ������Ʈ�� �����ϸ� ���ظ� ����
            if (pc != null) pc.TakeDamage(damage);

            // ����ü ����
            Destroy(gameObject);
        }
    }
}
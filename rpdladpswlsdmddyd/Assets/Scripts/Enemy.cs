using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, RunAway }
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;        //�̵��ӵ�
    public float traceRange = 15f;      //�������۰Ÿ�
    public float attackRange = 6f;      //���ݽ��� �Ÿ�
    public float attackCooldown = 1.5f;

    public GameObject projectileprefab;     //����ü ������
    public Transform firePoint;             //�߻���ġ

    private Transform player;
    private float lastAttackTime;
    public int maxHP = 5;
    private int currentHP;

    public Slider hpSlider;

    //public int currentHP = 5;      //�� ü��

    //private Transform player;           //�÷��̾� ������

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        

        void TracePlayer()
        {
            //�÷��̾������ ���� ���ϱ�
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.LookAt(player.position);
        }

        void AttackPlayer()
        {
            //���� ��ٿ�� �߻�
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                ShootProjectile();
            }
        }

        void ShootProjectile()
        {
            if (projectileprefab != null && firePoint != null)
            {
                transform.LookAt(player.position);
                GameObject proj = Instantiate(projectileprefab, firePoint.position, firePoint.rotation);
                EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
                if (ep != null)
                {
                    Vector3 dir = (player.position - firePoint.position).normalized;
                    ep.SetDirection(dir);
                }
            }
        }

        float hpRatio = (float)currentHP / maxHP; // ���� ü�� ���� ���

        switch (state)
        {
            case EnemyState.Idle:
                if (hpRatio <= 0.2f) // ü���� 20% �����̸� ����
                    state = EnemyState.RunAway;
                else if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (hpRatio <= 0.2f) // ���� �߿��� ü���� ������ ����
                    state = EnemyState.RunAway;
                else if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (hpRatio <= 0.2f) // ���� �߿��� ü���� ������ ����
                    state = EnemyState.RunAway;
                else if (dist > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                break;

            case EnemyState.RunAway:
                RunAwayFromPlayer(); // ���� ����
                if (dist > traceRange + 5f) // ���� �Ÿ� �̻� �־����� Idle
                    state = EnemyState.Idle;
                break;
        }

        //Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        //transform.LookAt(lookPos);
    }

    public void TakeDamage( int damage )
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;
        //Debug.Log($"{currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    void RunAwayFromPlayer()
    {
        // �÷��̾�� �ݴ� �������� �̵�
        Vector3 dir = (transform.position - player.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + dir); // ���� ���� �ٶ󺸱�
    }

   
}

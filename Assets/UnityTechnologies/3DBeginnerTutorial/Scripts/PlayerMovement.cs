using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    Animator m_Animator; 
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity; // ȸ�� ����

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>(); // Animator ������Ʈ�� ���� ���۷��� ����
                                               // ĳ���Ͱ� �Ȱ� �ִ��� ���θ� �˸� �� ����
        m_Rigidbody = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ�� ���� ���۷��� ����
                                                 // ĳ���Ϳ� �̵��� ȸ�� ����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical); // x��: horizontal, z��: vertical
        m_Movement.Normalize(); // �̵� ������ ũ�� ����ȭ

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // ������ 0�� �ƴϸ� ��
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f); // �������� 0�� �ƴϸ� ��
        bool isWalking = hasHorizontalInput || hasVerticalInput; // ĳ���Ͱ� �Ȱ� �ִ��� ����

        m_Animator.SetBool("Iswalking", isWalking); // isWalking �ִϸ����� �Ķ���� ����

        // desiredForward��� Vector3 ���� ����
        // ���� ȸ����, ��ǥ ȸ����, ������ ��ȭ, ũ���� ��ȭ
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        // LookRotation �޼���: �Ķ���� �������� �ٶ󺸴� ȸ�� ����
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove() // ��Ʈ ����� �����Ͽ� �̵��� ȸ���� ���������� ���� ����
    {
        // ĳ������ �̵� ����
        // Rigidbody ������Ʈ�� ���� ���۷����� ����Ͽ� MovePosition ȣ��
        // ĳ������ �� ��ġ�� Rigidbody�� ���� ��ġ
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        // ĳ������ ȸ�� ����
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}

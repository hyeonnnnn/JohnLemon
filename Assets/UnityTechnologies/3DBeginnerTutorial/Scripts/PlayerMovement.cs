using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    Animator m_Animator; 
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity; // 회전 변수

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>(); // Animator 컴포넌트에 대한 레퍼런스 설정
                                               // 캐릭터가 걷고 있는지 여부를 알릴 수 있음
        m_Rigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트에 대한 레퍼런스 설정
                                                 // 캐릭터에 이동과 회전 적용
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical); // x축: horizontal, z축: vertical
        m_Movement.Normalize(); // 이동 벡터의 크기 정규화

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // 수평값이 0이 아니면 참
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f); // 수직값이 0이 아니면 참
        bool isWalking = hasHorizontalInput || hasVerticalInput; // 캐릭터가 걷고 있는지 여부

        m_Animator.SetBool("Iswalking", isWalking); // isWalking 애니메이터 파라미터 설정

        // desiredForward라는 Vector3 변수 생성
        // 현재 회전값, 목표 회전값, 각도의 변화, 크기의 변화
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        // LookRotation 메서드: 파라미터 방향으로 바라보는 회전 생성
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove() // 루트 모션을 적용하여 이동과 회전을 개별적으로 적용 가능
    {
        // 캐릭터의 이동 설정
        // Rigidbody 컴포넌트에 대한 레퍼런스를 사용하여 MovePosition 호출
        // 캐릭터의 새 위치는 Rigidbody의 현재 위치
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        // 캐릭터의 회전 설정
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}

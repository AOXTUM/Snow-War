using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{

    public Transform targetTR;
    public float dist = 10.0f;
    public float height = 6.0f;
    public float dampTrace = 20.0f;

    public float TurnSpeed = 5f; // 카메라 회전 속도을 위한 변수.
    public float Maxdist = 15.0f;  //카메라와 주인공 사이의 거리 최대 사이즈
    public float Mindist = 5.0f; //카메라와 주인공 사이의 거리 최소 사이즈
    //카메라 자신의 트랜스폼 변수
    private Transform tr;

    public float power_Shake    = 2f;   // 카메라 충격 정도
    public float duration_Shake = 0.5f; // 카메라 충돌 지속시간


    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>(); // 카메라 자신의 트랜스폼 컴포넌트를 tr에 할당
    }

    void LateUpdate()
    {

        // 카메라의 위치를 추적 대상의 dist 변수만큼 뒤쪽으로 배치하고
        // height 변수만큼 위로 올림
        tr.position = Vector3.Lerp(tr.position, targetTR.position
                                    - (targetTR.forward * dist)
                                    + (Vector3.up * height),
                                    (Time.deltaTime * dampTrace));

        
        tr.LookAt(targetTR.position);

        //마우스 휠값을 입력받아서 화면 줌 여기서는 dist 카메라와 대상의 거리에 영향.
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            dist = dist + (-Input.GetAxis("Mouse ScrollWheel")) * TurnSpeed;
            //height = height + Input.GetAxis("Mouse ScrollWheel") * TurnSpeed;

            if (dist <= Mindist)
            {
                dist = Mindist;
            }

        }
        //마우스 휠값을 입력받아서 화면 아웃 여기서는 dist 카메라와 대상의 거리에 영향.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            dist = dist + (-Input.GetAxis("Mouse ScrollWheel")) * TurnSpeed;
            //height = height + Input.GetAxis("Mouse ScrollWheel") * TurnSpeed;

            if (dist >= Maxdist)
            {
                dist = Maxdist;
            }
        }

    }


    float shakeTimer;
    float shakeAmount;

    void Update()
    {
        if(shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);

            shakeTimer -= Time.deltaTime;

        }



    }

    // 카메라 흔들기 함수
    public void ShakeCamera(float shakePwr, float shakeDur)
    {
        shakeAmount = shakePwr;
        shakeTimer = shakeDur;

    }
    // 카메라 흔들기 함수를 외부에서 호출할때 사용하는 함수
    void CallShake()
    {
        ShakeCamera(power_Shake, duration_Shake);
    }









}

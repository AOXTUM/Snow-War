using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;


public class TestFire : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;
    public float bullet_Time = 0.6f;
    float now_Time;
    Stopwatch swatch;
    bool once = true;


    // Use this for initialization
    void Start()
    {
        swatch = new Stopwatch();   // 스탑워치 초기화
    }

    // Update is called once per frame
    void Update()
    {

            if (once)                // 최초 발사에서는 시간을 재지 않는다
            {

                Fire();
                swatch.Start();
                once = false;
            }
            else if (!once)
            {   // 'bullet_Time'의 시간 간격만큼 총알을 발사할 수 있다
                now_Time = (float)swatch.ElapsedMilliseconds * 0.001f;

                if (now_Time >= bullet_Time)
                {
                    Fire();
                    swatch.Reset();
                    swatch.Start();
                }
            }


    }

    // 총알 발사
    void Fire()
    {

        CreateBullet();

    }
    // 총알 생성
    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }





}
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class SnowFire_Ctrl : MonoBehaviour {

    public GameObject snowBall;
    public Transform firePos;
    public float snowBall_Time = 2.0f;
    float now_Time;
    Stopwatch swatch;
    bool once = true;



    // Use this for initialization
    void Start () {

        swatch = new Stopwatch();   // 스탑워치 초기화

    }
	
	// Update is called once per frame
	void Update () {

        


    }

    void CallFire()
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

            if (now_Time >= snowBall_Time)
            {
                Fire();
                swatch.Reset();
                swatch.Start();
            }
        }

    }



    void Fire()
    {
        //yield return new WaitForSeconds(0.5f);

        StartCoroutine(this.CreateBullet());
        this.gameObject.SendMessage("CallFireMotion");

    }
    // 총알 생성
    IEnumerator CreateBullet()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(snowBall, firePos.position, firePos.rotation);
    }




}

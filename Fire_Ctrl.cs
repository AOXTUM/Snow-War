using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class Fire_Ctrl : MonoBehaviour {


    public GameObject bullet;
    public Transform firePos;
    public float bullet_Time = 0.6f;
    public float snd_Time = 1.776f;
    float now_Time;
    float now_Time2;
    Stopwatch swatch;
    Stopwatch swatch2;
    bool once = true;
    bool once2 = true;

    public Text bulletNum;          // 남은 총알 UI
    public Text notice;             // 알림창 UI
    int nowBullet;
    public int maxBullet = 25;


	// Use this for initialization
	void Start () {
        swatch = new Stopwatch();   // 스탑워치 초기화
        swatch2 = new Stopwatch();
        Notice_Clear();             // 알림창 비우기
        nowBullet = maxBullet;      // 사용가능 총알 개수 초기화
        Refresh_BulletNum();        // 설정한 총알 개수로 UI재구성 하기
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0)) {

            if (nowBullet > 0)           // 총알이 있을 때
            {
                
                if (once)                // 최초 발사에서는 시간을 재지 않는다
                {
                    
                    Fire();
                    nowBullet--;
                    Refresh_BulletNum(); // 남은 총알 UI 새로고침
                    swatch.Start();
                    once = false;
                }
                else if (!once)
                {   // 'bullet_Time'의 시간 간격만큼 총알을 발사할 수 있다
                    now_Time = (float)swatch.ElapsedMilliseconds * 0.001f;
                    


                    if (now_Time >= bullet_Time)
                    {
                        Fire();
                        nowBullet--;
                        Refresh_BulletNum();
                        swatch.Reset();
                        swatch.Start();
                    }
                }

            }
            else    // 총알이 없을 때
            {
                if (once2)
                {
                    this.gameObject.SendMessage("PlaySnd_LackBullet");
                    swatch2.Start();
                    once2 = false;
                }
                else
                {
                    now_Time2 = (float)swatch2.ElapsedMilliseconds * 0.001f;
                    if (now_Time2 >= snd_Time)
                    {
                        this.gameObject.SendMessage("PlaySnd_LackBullet");
                        swatch2.Reset();
                        swatch2.Start();

                    }

                }

               
                Notice_NotEnoughBullet();   // 총알 없음을 알리는 UI 띄우기
            }




        }



	}

    // 총알 발사
    void Fire()
    {
        this.gameObject.SendMessage("PlaySnd_FireIcicle");
        CreateBullet();
        
    }
    // 총알 생성
    void CreateBullet()
    {

        Instantiate(bullet, firePos.position, firePos.rotation);
    }
    // 총알 재장전
    void Reload_Bullet()
    {
        nowBullet = maxBullet;
        Refresh_BulletNum();    // 장전 후 남은 총알 UI 새로고침
        Notice_Clear();         // 알림창 지우기
    }
    // 남은 총알UI 새로고침
    void Refresh_BulletNum()
    {
        bulletNum.text = string.Format("{0}/{1}", nowBullet, maxBullet);
    }
    // 알림창에 총알없음 띄우기
    void Notice_NotEnoughBullet()
    {
        notice.text = "총알이 부족합니다";
    }
    // 알림창 비우기
    void Notice_Clear()
    {

        notice.text = "";

    }

}

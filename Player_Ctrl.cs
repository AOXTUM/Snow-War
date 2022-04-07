using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;


[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip run;
    public AnimationClip walkSide;
    public AnimationClip defeat;
    public AnimationClip damaged;
}



public class Player_Ctrl : MonoBehaviour
{

    #region 각종 변수 모음

    private float h = 0.0f;
    private float v = 0.0f;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;
    private Transform tr;
    int runPoint;

    //public Animation _animation;
    //Animator animator;

    bool inIce = false;
    GameObject icicle;          // 고드름 오브젝트


    bool isActive = true;       // 고드름 오브젝트 활성화 여부


    Stopwatch swatch;
    bool checkOnce = true;
    public float activeTime_Icicle = 2.0f;      // 고드름 리젠 시간
    float nowTime;                              // 고드름 비활성화 된 이후 시간
    public Camera mainCamera;                   // 메인카메라


    public Anim anim;
    public Animation _animation;

    bool isDamaged = false;

    public Image imgHP;
    int hp = 100;
    int fHP;
    public int bulletDamage = 10;

    bool isDeath = false;

    public AudioClip damage_Snd;
    public AudioClip lackBullet_Snd;
    public AudioClip death_Snd;
    public AudioClip icicle_Snd;
    public AudioClip getIcicle_Snd;
    public AudioClip snowBall_Snd;
    public AudioClip attackEnemy_Snd;
    public AudioClip enemyDeath_Snd;


    AudioSource source;

    bool once = true;

    public Text _result;
    public GameObject btn_First;
    public GameObject btn_Exit;
    #endregion


    // Use this for initialization
    void Start()
    {

        tr = GetComponent<Transform>();                 // 플레이어 좌표 변수 값 할당

        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();

        swatch = new Stopwatch();                       // 스탑워치 할당

        fHP = hp;

        source = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!isDeath)
        {

            MakeControl_FPS();      // FPS의 컨트롤을 세팅해주는 함수

            if (v >= 0.1f || v <= -0.1f)
            {

                if (Input.GetKey("left shift")) // 'shift'를 누른채 움직인다면
                    _animation.CrossFade(anim.run.name, 0.3f);
                else
                    _animation.CrossFade(anim.walk.name, 0.3f);
            }
            else if (h >= 0.1f || h <= -0.1f)
            {
                _animation.CrossFade(anim.walkSide.name, 0.3f);
            }
            else if (isDamaged)
            {
                _animation.CrossFade(anim.damaged.name, 0.3f);

            }
            else
            {

                _animation.CrossFade(anim.idle.name, 0.3f);
            }

        }


        if (hp <= 0)
        {
            _animation.CrossFade(anim.defeat.name, 0.3f);
            StartCoroutine(StopAnimation());
        }




        if (Input.GetKey("right shift")) 
        {
            _animation.CrossFade(anim.defeat.name, 0.3f);
        }

        CheckInIceRange();      // 고드름이 범위 내에 있는지 체크, Space키로 획득
        CheckIcicleRegen();     // 고드름 리젠 관리




    }
    
    // FPS방식의 키보드 및 마우스 컨트롤 입력 관리 함수
    private void MakeControl_FPS()
    {

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

            if (Input.GetKey("left shift")) // 'shift'를 누른채 움직인다면
            {
                runPoint = 2;               // 이동속도 2배 증가
            }
            else
                runPoint = 1;               // 이동속도 복구

            tr.Translate((moveSpeed * runPoint) * moveDir * Time.deltaTime, Space.Self); // Translate(속도 * (이동방향 * 변위값) * Time.deltaTime, 기준 좌표)        
            tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
    }

    // 고드름의 리젠을 관리하는 함수
    private void CheckIcicleRegen()
    {
        if (!isActive)
        {
            if (checkOnce)      //고드름이 꺼져 있을때 한번
            {
                swatch.Start();
                checkOnce = false;
            }
            else if (!checkOnce) // 고드름이 꺼져 있지만 그 이후
            {
                nowTime = (float)swatch.ElapsedMilliseconds * 0.001f;
                if (nowTime >= activeTime_Icicle)  // 고드름 리젠시간이 지나게 된다면
                {
                    checkOnce = true;           // 스탑워치 체크 변수 초기화
                    swatch.Reset();             // 스탑워치 초기화
                    icicle.SetActive(true);     // 고드름 오브젝트 활성화
                    icicle = null;              // 범위를 벗어나면 고드름 변수 초기화
                    isActive = true;            // 오브젝트 활성화 체크 변수 초기화

                }
            }

        }
    }

    // 고드름 범위 내에 있는지 확인하고 'Space'키를 입력받는 함수
    private void CheckInIceRange()
    {
        if (inIce)
        {
            if (Input.GetKeyDown("space"))  // IceRange 안에서 'space바'를 
            {                               // 누른다면
                source.PlayOneShot(getIcicle_Snd, 0.9f);
                icicle.SetActive(false);
                isActive = false;
                gameObject.SendMessage("Reload_Bullet");    // 총알 재장전 함수 호출
            }
        }
    }


    // 충돌체 체크
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "IceRange") // 범위가 벗어나는 곳에 또 다른 
        {                                       // 태그 달린 충돌체 배치해두기
            inIce = true;
            icicle = other.gameObject;          // 범위 안에 들어가게 되면 해당 고드름의 오브젝트 할당받기
            

        }
        else if (other.gameObject.tag == "IceOutRng")
        {
            inIce = false;
        }
        else if(other.gameObject.tag == "Bullet")
        {

            Destroy(other.gameObject);
            mainCamera.SendMessage("CallShake");

            isDamaged = true;
            StartCoroutine(StopDamaged());

            hp -= bulletDamage;
            if(hp <= 0)
            {
                isDeath = true;
                if (once)
                {
                    source.PlayOneShot(death_Snd, 0.9f);
                }
                once = false;
            }
            imgHP.fillAmount = (float)hp / (float)fHP;

        }


    }

    void SnowAttacked()
    {
        if(hp >= 10)
            source.PlayOneShot(damage_Snd, 0.9f);
        source.PlayOneShot(snowBall_Snd, 0.9f);

        mainCamera.SendMessage("CallShake");

        isDamaged = true;
        StartCoroutine(StopDamaged());

        hp -= bulletDamage;
        if (hp <= 0)
        {
            btn_First.SetActive(true);
            btn_Exit.SetActive(true);

            _result.text = "DEFEAT";
            isDeath = true;
            if (once)
            {
                source.PlayOneShot(death_Snd, 0.9f);
            }
            once = false;
        }
        imgHP.fillAmount = (float)hp / (float)fHP;
    }

   
    IEnumerator StopDamaged()
    {
        yield return new WaitForSeconds(0.4f);
        isDamaged = false;

    }
    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(1.1f);
        _animation.Stop();
    }



    // 디버그창 하나의 함수로 알림 관리하기
    void DebugLog(string msg)
    {
        Debug.Log(msg);
    }

    void PlaySnd_LackBullet()
    {
        source.PlayOneShot(lackBullet_Snd, 0.9f);
    }
    void PlaySnd_FireIcicle()
    {
        source.PlayOneShot(icicle_Snd);
    }
    void PlaySnd_AttackEnemy()
    {
        source.PlayOneShot(attackEnemy_Snd, 0.3f);
    }
    void PlaySnd_DieEnemy()
    {
        source.PlayOneShot(enemyDeath_Snd, 0.9f);
    }

}

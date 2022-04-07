using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[System.Serializable]
public class AnimE
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip attack;
    public AnimationClip defeat;
    public AnimationClip damaged;
}



public class Enemy_AI : MonoBehaviour {


    Transform enemyTr;
    Transform playerTr;
    NavMeshAgent nvAgent;

    public float traceDist = 20;
    bool isClose = false;

    public Transform[] movePos;
    int moveNum = 0;


    int hp = 100;
    int bullet_Damage = 10;     // 고드름 데미지


    public AnimE anim;
    public Animation _animation;
    bool isWalk = false;
    bool isIdle = false;
    bool isFire = false;
    bool stopWalk = false;
    bool isDamaged = false;
    bool isDeath = false;

    public GameObject player;

    bool once = true;
    public Text result;


    public GameObject btn_First;
    public GameObject btn_Exit;



    // Use this for initialization
    void Start () {



        enemyTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();


        StartCoroutine(this.CheckPlayerDistance());
        StartCoroutine(this.EnemyAction());


        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();


    }

    // Update is called once per frame
    void Update () {

  
        if (isWalk)
        {
            _animation.CrossFade(anim.walk.name, 0.3f);
        }
        else if (isIdle)
        {
            _animation.CrossFade(anim.idle.name, 0.3f);
        }
        else if (isFire)
        {
            _animation.CrossFade(anim.attack.name, 0.3f);
        }
        //else if (isDamaged)
        //{
        //    _animation.CrossFade(anim.damaged.name, 0.3f);
        //}
        // 
        else if(isDeath)
        {
            _animation.CrossFade(anim.defeat.name, 0.3f);
            nvAgent.Stop();
            StartCoroutine(this.StopAnimation());
        }


    }

    void CallFireMotion()
    {
        isIdle = false;
        isWalk = false;
        isFire = true;
        stopWalk = true;
        nvAgent.Stop();
        StartCoroutine(this.ResumeNvAgent());

    }

    IEnumerator ResumeNvAgent()
    {
        yield return new WaitForSeconds(1.0f);
        nvAgent.Resume();
        isFire = false;
        stopWalk = false;
    }



    IEnumerator CheckPlayerDistance()
    {
        
            float dist;

            while (true)
            {
                yield return new WaitForSeconds(0.2f);

                dist = Vector3.Distance(playerTr.position, enemyTr.position);

                if (dist <= traceDist && isDamaged != true)
                {
                    isClose = true;
                    if (dist <= 13 && isDeath != true)
                        this.gameObject.SendMessage("CallFire");

                }
                else
                    isClose = false;


            }
    }

    IEnumerator EnemyAction()
    {

            while (true)
            {

                if (isClose && isDeath != true)
                {
                    nvAgent.destination = playerTr.position;
                    if (!stopWalk)
                        isWalk = true;

                }
                else if (!isClose && isDeath != true)
                {

                    if (moveNum > 3)
                        moveNum = 0;

                    nvAgent.destination = movePos[moveNum++].position;


                    StartCoroutine(this.CheckMyTR());
                    StartCoroutine(EnemyAction2());
                    yield return new WaitForSeconds(5.0f);

                }

                yield return null;
            }

    }

    IEnumerator EnemyAction2()
    {
        while(true)
        {
            if (isClose && isDeath != true)
                nvAgent.destination = playerTr.position;


            yield return null;
        }

 

    }

    IEnumerator CheckMyTR()
    {

            float _dist;

            while (true)
            {
                _dist = Vector3.Distance(enemyTr.position, movePos[moveNum - 1].position);

                if (isClose != true && isFire != true)
                {
                    if (0.0f <= _dist && _dist <= 2.5f)
                    {
                        isWalk = false;
                        isIdle = true;
                    }
                    else
                    {
                        isIdle = false;
                        isWalk = true;
                    }

                }



                yield return new WaitForSeconds(0.2f);

            }


    }




    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Bullet")
        {
            hp -= bullet_Damage;
            Destroy(other.gameObject);
            player.SendMessage("PlaySnd_AttackEnemy");

            if (hp <= 0)
            {

                btn_First.SetActive(true);
                btn_Exit.SetActive(true);



                isWalk = false;
                stopWalk = true;
                nvAgent.Stop();
                isDeath = true;

                if (once)
                    player.SendMessage("PlaySnd_DieEnemy");
                once = false;


                result.text = "WIN!";
                return;
            }


            //isWalk = false;
            //stopWalk = true;
            //isDamaged = true;
            //nvAgent.Stop();
            //StartCoroutine(this.ResumeNvAgent2());
            
        }

    }

    IEnumerator ResumeNvAgent2()
    {
        yield return new WaitForSeconds(0.5f);
        nvAgent.Resume();
        stopWalk = false;
        isDamaged = false;
    }

    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(1.3f);
        _animation.Stop();
    }



}

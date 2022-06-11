using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    
    // 스파인 애니메이션을 위한것
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    // 애니메이션에 대한 Enum
    public enum AnimState
    {
        Idle, Run, Jump, Attack
    }

    // 현재 애니메이션 처리가 무엇인지에 대한 변수
    private AnimState _AnimState;

    // 현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    private string CurrentAnimation;
    
    // 캐릭터 움직임
    private Rigidbody2D rig;
    private float xx;

    // 캐릭터가 좌,우 어디로 움직이는지 체크
    private int dir = 0;

    // 캐릭터가 땅에 닿아있는지
    private bool isgrounded = true;
    // 현재 점프를 하려는 상태인지 체크
    private bool isjump = false;

    //private float velocity = 5f;
    private float jumpvelocity = 6f;  // 점프 속도
    private AudioSource jumpsound;

    private float jumpPower = 0f; // 스페이스바 누르는 시간
    private float maxPower = 1f;  // 스페이스바 누르는 시간 최대 1초

    // 포탈 타이머
    private float timer = 0f;

    // 몬스터 충돌 이펙트
    public GameObject exp;

    //public List<SkeletonAnimation> PIXEL_list = new List<SkeletonAnimation>();
    public SkeletonAnimation character;

    GameEndManager gameendscript;  // 게임종료 스크립트 참조

    // 스폰 위치
    //public GameObject spawnPoint;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        jumpsound = GetComponent<AudioSource>();

        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        this.gameObject.transform.position = new Vector2(loadData.posX, loadData.posY);

        Debug.Log("life Num : " + loadData.life);

        if (loadData.weapon != "null")
        {
            Debug.Log(loadData.weapon);
            character.Skeleton.SetAttachment("Weapon", loadData.weapon); // 무기 설정
        }

        gameendscript = GameObject.Find("GameManager").GetComponent<GameEndManager>();
    }
    private void Start()
    {
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        if (loadData.win == true)
        {
            Debug.Log(loadData.monster);
            Destroy(GameObject.Find(loadData.monster));

            SaveData a = new SaveData(loadData.name, loadData.life, loadData.weapon, loadData.posX, loadData.posY, null, loadData.atk, false);
            SaveSystem.Save(a, "save_001");
        }
    }
    private void Update()
    {
        // 캐릭터가 땅에 닿아있고 점프키를 누르지 않았을 때 작동
        if (isgrounded == true)
        {
            if (Input.GetKey(KeyCode.Space))  // 스페이스키 누르는 시간 저장
            {
                isjump = true;  // 현재 점프하려는 상태
                // 점프키 누르는 동안 캐릭터가 움직이지 않게 프리즈
                rig.constraints = RigidbodyConstraints2D.FreezeAll;

                jumpPower += Time.deltaTime;
                Debug.Log("s: " + jumpPower);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isjump = false;  // 점프를 했으므로 점프하려는 상태 false

                // 점프 사운드
                jumpsound.Play();
                // 프리즈 해제하고 회전은 다시 프리즈
                rig.constraints = RigidbodyConstraints2D.None;
                rig.constraints = RigidbodyConstraints2D.FreezeRotation;
                
                Debug.Log("r: " + jumpPower);

                // 점프파워가 맥스파워를 초과 시 맥스파워로 초기화
                if (jumpPower > maxPower) jumpPower = maxPower;

                // 대각선으로 점프
                //rig.AddForce(new Vector2(4 * dir, jumpvelocity * jumpPower), ForceMode2D.Impulse);
                rig.AddForce(Vector2.up * jumpvelocity * jumpPower, ForceMode2D.Impulse);
                //rig.AddForce(Vector2.right * dir * velocity * 50f, ForceMode2D.Impulse);
                
                _AnimState = AnimState.Jump;

                jumpPower = 0;
            }
            if (isjump == false && isgrounded == true) // 이동 할 때
            {
                xx = Input.GetAxisRaw("Horizontal");

                if (xx == 0f)
                {
                    _AnimState = AnimState.Idle;
                }
                else
                {
                    _AnimState = AnimState.Run;

                    //skeletonAnimation.skeleton.ScaleX = Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
                    transform.localScale = new Vector2(xx, 1); // 캐릭터 방향
                }
            }

            SetCurrentAnimation(_AnimState);
        }

    }
    
    private void FixedUpdate()
    { 
        if(isgrounded == true && isjump == false)
            rig.velocity = new Vector2(xx * 125 * Time.deltaTime, rig.velocity.y);
    }

    // 캐릭터가 땅에 닿아있는지 = 점프 상태인지 체크
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isgrounded = true;
        }
        else if(other.gameObject.tag == "Monster" && gameendscript.GameOver == false)
        {
            //exp.Play();
            SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
            // 현재 위치 저장 & 충돌한 몬스터의 이름 저장
            SaveData character = new SaveData(loadData.name, loadData.life, loadData.weapon, this.transform.position.x, this.transform.position.y, other.gameObject.name, loadData.atk, loadData.win);
            SaveSystem.Save(character, "save_001");

            Instantiate(exp,new Vector2(transform.position.x, transform.position.y-0.5f), transform.rotation);
            Invoke(nameof(GoBattle), 0.5f);
        }
        else if (other.gameObject.tag == "BossMonster" && gameendscript.GameOver == false)
        {
            //exp.Play();
            SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
            // 현재 위치 저장 & 충돌한 몬스터의 이름 저장
            SaveData character = new SaveData(loadData.name, loadData.life, loadData.weapon, this.transform.position.x, this.transform.position.y, other.gameObject.name, loadData.atk, loadData.win);
            SaveSystem.Save(character, "save_001");

            Instantiate(exp, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Invoke(nameof(GoBossBattle), 0.5f);
        }

    }
    void GoBattle()
    {
        SceneManager.LoadScene("Battle1");
    }
    void GoBossBattle()
    {
        SceneManager.LoadScene("BossBattle1");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isgrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _AnimState = AnimState.Jump;
            isgrounded = false;
            rig.constraints = RigidbodyConstraints2D.None;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {

            timer += Time.deltaTime;
            Debug.Log("포탈 충돌 중 : " + timer);

        }
        else if (other.CompareTag("EndPoint"))
        {
            Debug.Log("마지막 지점 도착");
            SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
            // 캐릭터 A클리어 했으므로 캐릭터 B로 데이터 수정
            SaveData character = new SaveData("B", loadData.life, loadData.weapon, this.transform.position.x, this.transform.position.y, other.gameObject.name, loadData.atk, loadData.win);
            SaveSystem.Save(character, "save_001");

            SceneManager.LoadScene("Title");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            timer += Time.deltaTime;
            Debug.Log("포탈 충돌 중 : " + timer);
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene("JumpMap2");
            }
            if (timer >= 3f)
            {
                SceneManager.LoadScene("JumpMap2");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            timer = 0;
        }
    }

    private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        // 동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if (animClip.name.Equals(CurrentAnimation))
            return;

        // 해당 애니메이션으로 변경
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        // 현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        // 짧게 작성한다면 이렇게
        _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    }
}

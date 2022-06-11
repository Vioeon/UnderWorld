using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyBattle : MonoBehaviour
{
    //***** 턴제 게임 *******
    // 적 AI 스크립트 필요
    // 랜덤하게 약공, 강공

    public Image hpbar;
    public float maxHp;
    public float EnemyHp;  // 나의 체력

    public float Enemyatk;  // 공격력

    // 랜덤하게 약공 or 강공
    private int Atk;

    public SkeletonAnimation Monster;  // 몬스터

    public GameObject skillUI;  // 스킬 UI

    PlayerBattle playerscript;  // 몬스터의 스크립트 참조

    public Text gamePaner;  // 게임 패널
    string t; // 게임패널 텍스트

    public GameObject DamageText;
    private float damage;
    public Transform hpPos;  // 데미지텍스트 생성 위치

    private void Awake()
    {
        // 무기 장착
        Monster.Skeleton.SetAttachment("Weapon", "Black_Knight_Weapon");
        if (this.gameObject.CompareTag("Enemy"))
        {
            // 몬스터 체력 설정
            maxHp = 50f;
            EnemyHp = maxHp;
            // 몬스터 공격력 설정
            Enemyatk = 10f;
        }
        else if (this.gameObject.CompareTag("BossMonster"))
        {
            // 몬스터 체력 설정
            maxHp = 100f;
            EnemyHp = maxHp;
            // 몬스터 공격력 설정
            Enemyatk = 20f;
        }
        

        playerscript = GameObject.FindWithTag("Player").GetComponent<PlayerBattle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 체력바
        hpbar.fillAmount = EnemyHp / maxHp;
    }


    public void attack()
    {
        // 랜덤하게 약공 or 강공 중 선택
        Atk = Random.Range(1, 4);

        if (playerscript.battleEnd == false)
            Invoke("EnemyAttack", 2f);
        else
            return;
    }
    private void EnemyAttack()
    {
        switch (Atk)
        {
            case 1:  // 강공
                // 스킬 1 - 기본 공격
                // 애니메이션 & 이펙트 연출
                playerscript.skill1.SetActive(false);
                Monster.AnimationState.SetAnimation(0, "Attack_1", false);
                t = "몬스터의 '강 공격'!!!";
                Debug.Log("몬스터의 '강 공격'!!!");
                break;
            case 2:  // 약공

                Monster.AnimationState.SetAnimation(0, "Attack_1", false);
                // 이펙트
                t = "몬스터의 '약 공격'!!!";
                Debug.Log("몬스터의 '약 공격'!!!");
                break;
            case 3:  // 약공

                Monster.AnimationState.SetAnimation(0, "Attack_1", false);
                // 이펙트
                t = "몬스터의 '약 공격'!!!";
                Debug.Log("몬스터의 '약 공격'!!!");
                break;
        }
        gamePaner.text = t;  // 패널 텍스트 설정

        playerscript.monsterAtk(Atk);

    }
    // 플레이어가 선택한 공격에 따라 몬스터의 Hp 일정량 감소
    public void playerAtk(int pick)
    {
        switch (pick)
        {
            case 1:  // 플레이어 일반공격
                damage = playerscript.myatk;
                EnemyHp -= damage;
                break;
            case 2:  // 플레이어 스킬 1
                damage = playerscript.myatk * 1.2f;
                EnemyHp -= damage;
                break;
            case 3:  // 플레이어 스킬 2
                damage = playerscript.myatk * 1.5f;
                EnemyHp -= damage;
                break;
            case 4:  // 플레이어 스킬 3
                damage = playerscript.myatk * 2.0f;
                EnemyHp -= damage;
                break;
        }
        hit();
        //Invoke("hit", 0.2f);
        //takeDamage(damage);  // 데미지 텍스트

    }
    public void BattleWin()
    {
        playerscript.character.AnimationState.SetAnimation(0, "Win_1", false);
    }
    public void BattleEnd()
    {
        // 전투의 승패 저장
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData(loadData.name, loadData.life, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, true);
        SaveSystem.Save(a, "save_001");

        gamePaner.text = "전투에서 승리하였습니다!";  // 플레이어 승리 메시지 출력
        // 플레이어 승리 애니메이션
        // 플레이어 승리 이펙트
        // 플레이어 아이템 획득

        Invoke("transScene", 3f);
    }
    public void transScene()
    {
        SceneManager.LoadScene("JumpMap2");
    }
    public void OnskillUI()
    {
        skillUI.SetActive(true);
        gamePaner.text = "공격을 선택하세요.";
    }
    public void hit()
    {
        takeDamage(damage);
    }
    public void takeDamage(float damage)
    {
        GameObject hpText = Instantiate(DamageText);
        hpText.transform.position = hpPos.position;
        hpText.GetComponent<DamageText>().damage = damage;

        Monster.AnimationState.SetAnimation(0, "Hit", false);


        if (EnemyHp <= 0)  // 플레이어 승리
        {
            // 전투 종료
            playerscript.battleEnd = true;

            Invoke("BattleWin", 1f);
            // playerscript.character.AnimationState.SetAnimation(0, "Win_1", false);
            Monster.AnimationState.SetAnimation(0, "Die_1", false);
            // 몬스터 Die 애니메이션
            // 몬스터 Die 이펙트
            Invoke("BattleEnd", 2f);
        }
    }
}

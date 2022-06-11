using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBattle : MonoBehaviour
{
    //***** 턴제 게임 *******
    // 적 AI 스크립트 필요
    // 랜덤하게 약공, 강공

    // 내가 공격을 선택하면 1초뒤 바로 전투 시작
    // 내가먼저 공격하고 상대가 공격

    public Image hpbar;
    public float maxHp;
    public float currentHp;  // 나의 체력

    public float myatk;  // 공격력
    private int atkNum;  // 선택한 공격
    private bool selectAtk;  // 공격을 선택했는지 여부

    public bool battleEnd = false;  // 전투 종료 체크

    public SkeletonAnimation character;  // 플레이어 캐릭터

    public GameObject skillUI;  // 스킬 UI

    // 애니메이션
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;

    EnemyBattle enemyscript;  // 몬스터의 스크립트 참조

    public Text gamePaner;  // 게임 패널
    string t; // 게임패널 텍스트

    public GameObject DamageText;
    private float damage;
    public Transform hpPos;  // 데미지텍스트 생성위치

    private void Awake()
    {
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        loadData.weapon = "Bone_Knight_Weapon";  // 임시 무기 설정
        //무기 장착
        Debug.Log(loadData.weapon);
        character.Skeleton.SetAttachment("Weapon", loadData.weapon);

        // 체력 설정
        maxHp = 100f;
        currentHp = maxHp;
        // 공격력 설정
        myatk = loadData.atk;

        if(SceneManager.GetActiveScene().name == "Battle1")
            enemyscript = GameObject.FindWithTag("Enemy").GetComponent<EnemyBattle>();
        else if(SceneManager.GetActiveScene().name == "BossBattle1")
            enemyscript = GameObject.FindWithTag("BossMonster").GetComponent<EnemyBattle>();
    }
    void Start()
    {
        skillUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(atkNum);
        // 체력바
        hpbar.fillAmount = currentHp / maxHp;

        if (selectAtk == true)
        {
            // 플레이어의 스킬 선택 변수 초기화
            selectAtk = false;

            // 플레이어 공격 2초뒤에
            Attack();

            // 플레이어 공격 후 몬스터가 죽으면 리턴
            if (battleEnd == false)
                enemyscript.attack();  // EnemyBattle 스크립트의 적 공격 실시
            else
                return;
        }
    }


    private void Attack()
    {
        switch (atkNum)
        {
            case 1:
                // 스킬 1 - 기본 공격
                // 애니메이션 & 이펙트 연출

                //skill1.SetActive(true);
                character.AnimationState.SetAnimation(0, "Attack_1", false);
                t = "플레이어의 '일반공격'!!!";
                Debug.Log("플레이어의 '일반공격'!!!");

                break;
            case 2:
                // 스킬 2

                skill2.SetActive(true);
                character.AnimationState.SetAnimation(0, "Attack_3", false);
                t = "플레이어의 '스킬 1 공격'!!!";
                Debug.Log("플레이어의 '스킬 1 공격'!!!");
                break;
            case 3:
                // 스킬 3

                skill3.SetActive(true);
                character.AnimationState.SetAnimation(0, "Attack_5", false);
                t = "플레이어의 '스킬 2 공격'!!!";
                Debug.Log("플레이어의 '스킬 2 공격'!!!");
                break;
            case 4:
                // 스킬 4

                skill4.SetActive(true);
                character.AnimationState.SetAnimation(0, "Attack_7", false);
                t = "플레이어의 '스킬 3 공격'!!!";
                Debug.Log("플레이어의 '스킬 3 공격'!!!");
                break;
        }
        gamePaner.text = t;  // 패널 텍스트 설정
        Invoke("effectfalse", 2f);
        enemyscript.playerAtk(atkNum);
    }
    public void effectfalse()
    {
        skill1.SetActive(false);
        skill2.SetActive(false);
        skill3.SetActive(false);
        skill4.SetActive(false);
    }
    // 몬스터의 랜덤한 공격에 따라 플레이어의 Hp 감소
    public void monsterAtk(int pick)
    {
        switch (pick)
        {
            case 1:  // 몬스터 강 공격
                damage = enemyscript.Enemyatk * 1.5f;
                currentHp -= damage;
                break;
            case 2:  // 몬스터 약 공격
                damage = enemyscript.Enemyatk;
                currentHp -= damage;
                break;
            case 3:  // 몬스터 약 공격
                damage = enemyscript.Enemyatk;
                currentHp -= damage;
                break;
        }
        hit();
        //Invoke("hit", 0.2f);
        //takeDamage(damage);  // 데미지 텍스트
        
        // 전투 종료가 아니면 UI생성하여 다시 전투 시작
        if(battleEnd == false)
            Invoke("OnskillUI", 2f);
    }
    public void BattleWin()
    {
        enemyscript.Monster.AnimationState.SetAnimation(0, "Win_1", false);
    }
    public void BattleEnd()
    {
        // 전투의 승패 저장 및 라이프 -1 감소
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData(loadData.name, loadData.life - 1, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, false);
        SaveSystem.Save(a, "save_001");

        gamePaner.text = "전투에서 패배하였습니다!";  // 전투 패배 메시지 출력
        // 패배 이펙트

        Invoke("transScene", 3f);
    }
    public void transScene()
    {
        SceneManager.LoadScene("JumpMap2");
    }

    public void skill_1()
    {
        atkNum = 1;
        selectAtk = true;
        skillUI.SetActive(false);
    }
    public void skill_2()
    {
        atkNum = 2;
        selectAtk = true;
        skillUI.SetActive(false);
    }
    public void skill_3()
    {
        atkNum = 3;
        selectAtk = true;
        skillUI.SetActive(false);
    }
    public void skill_4()
    {
        atkNum = 4;
        selectAtk = true;
        skillUI.SetActive(false);
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

        character.AnimationState.SetAnimation(0, "Hit", false);


        if (currentHp <= 0)  // 전투 패배
        {
            // 전투 종료
            battleEnd = true;

            Invoke("BattleWin", 1f);
            //enemyscript.Monster.AnimationState.SetAnimation(0, "Win_1", false);
            character.AnimationState.SetAnimation(0, "Die_2", false);
            // 플레이어 Die 애니메이션
            // 플레이어 Die 이펙트
            Invoke("BattleEnd", 2f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{
    //***** 턴제 게임 *******
    // 적 AI 스크립트 필요
    // 랜덤하게 약공, 강공

    // 내가 공격을 선택하면 1초뒤 바로 전투 시작
    // 내가먼저 공격하고 상대가 공격

    public int myHp;  // 나의 체력
    private float myatk;  // 공격력
    private int atkNum;  // 선택한 공격
    private bool selectAtk;  // 공격을 선택했는지 여부

    private bool turn;  // 전투 턴

    public SkeletonAnimation character;  // 플레이어 캐릭터

    public GameObject skillUI;  // 스킬 UI

    EnemyBattle enemyscript;  // 몬스터의 스크립트 참조

    public Text gamePaner;  // 게임 패널
    string t; // 게임패널 텍스트

    private void Awake()
    {
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        loadData.weapon = "Bone_Knight_Weapon";
        //무기 장착
        Debug.Log(loadData.weapon);
        character.Skeleton.SetAttachment("Weapon", loadData.weapon);


        // 체력 설정
        myHp = 100;
        // 공격력 설정
        myatk = loadData.atk;

        enemyscript = GameObject.FindWithTag("Enemy").GetComponent<EnemyBattle>();
    }
    void Start()
    {
        skillUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(atkNum);

        if (selectAtk == true)
        {
            // 플레이어의 스킬 선택 변수 초기화
            selectAtk = false;

            // 플레이어 공격 2초뒤에
            Attack();
            // EnemyBattle 스크립트의 적 공격 실시
            enemyscript.attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 적의 공격에 닿으면 Hp 감소
        if(other.collider.tag == "EnemyAtk")
        {
            Debug.Log("플레이어가 적의 공격에 맞음");
        }
    }

    private void Attack()
    {
        switch (atkNum)
        {
            case 1:
                // 스킬 1 - 기본 공격
                // 애니메이션 & 이펙트 연출

                t = "플레이어의 '일반공격'!!!";
                Debug.Log("플레이어의 '일반공격'!!!");
                break;
            case 2:
                // 스킬 2

                t = "플레이어의 '스킬 1 공격'!!!";
                Debug.Log("플레이어의 '스킬 1 공격'!!!");
                break;
            case 3:
                // 스킬 3

                t = "플레이어의 '스킬 2 공격'!!!";
                Debug.Log("플레이어의 '스킬 2 공격'!!!");
                break;
            case 4:
                // 스킬 4

                t = "플레이어의 '스킬 3 공격'!!!";
                Debug.Log("플레이어의 '스킬 3 공격'!!!");
                break;
        }
        gamePaner.text = t;
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
}
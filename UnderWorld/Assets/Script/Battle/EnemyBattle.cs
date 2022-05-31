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

    public int EnemyHp;  // 나의 체력
    private float Enemyatk;  // 공격력

    // 랜덤하게 약공 or 강공
    private int Atk;

    public SkeletonAnimation Monster;  // 몬스터

    public GameObject skillUI;  // 스킬 UI

    public Text gamePaner;  // 게임 패널
    string t; // 게임패널 텍스트

    private void Awake()
    {
        // 무기 장착
        Monster.Skeleton.SetAttachment("Weapon", "Black_Knight_Weapon");

        // 체력 설정
        EnemyHp = 50;
        // 공격력 설정
        Enemyatk = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 플레이어의 공격에 닿으면 Hp 감소
        if (other.collider.tag == "PlayerAtk")
        {
            Debug.Log("적이 플레이어의 공격에 맞음");
        }
    }

    public void attack()
    {
        // 랜덤하게 약공 or 강공 중 선택
        Atk = Random.Range(-1, 2);
        
        Invoke("Test", 2f);
    }
    public void Test()
    {
        switch (Atk)
        {
            case -1:  // 강공
                // 스킬 1 - 기본 공격
                // 애니메이션 & 이펙트 연출

                t = "몬스터의 '강 공격'!!!";
                Debug.Log("몬스터의 '강 공격'!!!");
                break;
            case 0:  // 약공

                t = "몬스터의 '약 공격'!!!";
                Debug.Log("몬스터의 '약 공격'!!!");
                break;
            case 1:  // 약공

                t = "몬스터의 '약 공격'!!!";
                Debug.Log("몬스터의 '약 공격'!!!");
                break;
        }
        gamePaner.text = t;

        // 플레이어와 몬스터의 Hp 확인 후 결판이 안났으면 UI 활성화하고 다시 전투 시작

        // 몬스터 공격 2초뒤에 UI 다시 생성
        //Invoke("OnskillUI", 2f);
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData(loadData.name, loadData.life, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, true);

        SaveSystem.Save(a, "save_001");
        SceneManager.LoadScene("JumpMap1");
    }
    public void OnskillUI()
    {
        skillUI.SetActive(true);
        gamePaner.text = "공격을 선택하세요.";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;  // 다른 class에서도 호출가능

    [SerializeField] Menu[] menus;  // SerializedField를 사용하면 우리는 public처럼 쓸 수 있지만  public이 아니여서 외부에서는 못만짐.

    private void Awake()
    {
        Instance = this;
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData("A", 5, "null", 0, -2, null, 15, false);
        SaveSystem.Save(a, "save_001");

    }
    private void Start()
    {
        OpenMenu("title");
    }
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)  // string을 받아서 해당이름을 가진 메뉴를 여는 스크립트
            {
                //OpenMenu(menus[i]);
                menus[i].Open();  // 오픈 메뉴(스트링)에 있는 for문이 오픈 메뉴(메뉴)에도 똑같이 있어서 중복을 피하고자 코드 수정.  
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    /*
    public void PlayMenu()
    {
        GameObject.Find("TitleMenu").SetActive(false);
        GameObject.Find("PlayMenu").SetActive(true);
        
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData(loadData.name, loadData.life, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, loadData.win);
        if (loadData.name == "A")
        {
            GameObject.Find("HardModeButton").SetActive(false);
        }
        else if (loadData.name == "B")  // 캐릭터 A를 클리어 후 B가 열리면 하드모드 버튼 활성화
            GameObject.Find("HardModeButton").SetActive(true);

    }
    public void NewGameMenu()
    {
        GameObject.Find("TitleMenu").SetActive(false);
        GameObject.Find("NewGameMenu").SetActive(true);
    }*/
    public void NomalModeButton()
    {
        // 로딩화면 넣기
        Instance.OpenMenu("loading");
        SceneManager.LoadScene("StartVillage");
    }
    public void HardModeButton()
    {
        // 로딩화면 넣기
        Instance.OpenMenu("loading");
        SceneManager.LoadScene("StartVillage");
    }
    public void NewGameButton()
    {
        // 캐릭터 A 로 데이터 수정
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
        SaveData a = new SaveData("A", loadData.life, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, loadData.win);
        SaveSystem.Save(a, "save_001");

        // 로딩화면 넣기
        Instance.OpenMenu("loading");
        SceneManager.LoadScene("StartVillage");  // 캐릭터 A로 게임 시작
    }
    /*
    public void BackButton()
    {
        GameObject.Find("TitleMenu").SetActive(true);
        GameObject.Find("NewGameMenu").SetActive(false);
    }*/
}

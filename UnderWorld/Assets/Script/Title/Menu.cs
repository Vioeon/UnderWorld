using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);  // 특정 메뉴 켜지기

        if(menuName == "playmenu")
        {
            SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드
            SaveData a = new SaveData(loadData.name, loadData.life, loadData.weapon, loadData.posX, loadData.posY, loadData.monster, loadData.atk, loadData.win);

            if (loadData.name == "B")  // 캐릭터 A를 클리어 후 B가 열리면 하드모드 버튼 활성화
                GameObject.Find("HardModeButton").SetActive(true);
        }
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}

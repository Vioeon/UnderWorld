using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //public static MapManager Instance;//Room Manager 스크립트를 메서드로 사용하기 위해 선언

    //public GameObject prefab;
    public GameObject prefeb;

    void Awake()
    {
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        GameObject player = Instantiate(prefeb);
        player.transform.position = new Vector2(loadData.posX, loadData.posY);
    }

    // Start is called before the first frame update
    void Start()
    {
        //SaveData loadData = SaveSystem.Load("save_001");

        // 캐릭터 생성 및 위치 설정
        //GameObject player = Instantiate(prefab);
        //player.transform.position = new Vector2(loadData.posX, loadData.posY);

        //SaveData character = new SaveData("A", 5, null, 0, 0, null, 10);
        //SaveSystem.Save(character, "save_001");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


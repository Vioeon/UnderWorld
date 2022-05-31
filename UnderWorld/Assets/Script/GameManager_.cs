using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_ : MonoBehaviour
{
    void Awake()
    {
        // 게임 정보 초기화          (name, life, weapon, posX, posY, monster, atk)
        SaveData character = new SaveData("A", 5, "null", 0, -2, null, 10, false) ;
        SaveSystem.Save(character, "save_001");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

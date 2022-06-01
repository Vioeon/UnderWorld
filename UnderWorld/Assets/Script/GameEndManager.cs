using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public GameObject EndUI;  // 게임오버 UI

    public bool GameOver = false;

    public GameObject player;  // 플레이어 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        SaveData loadData = SaveSystem.Load("save_001"); // 데이터 로드

        // 목숨 소진 시 게임 오버
        if(loadData.life == 0)
        {
            Debug.Log("모든 목숨 소진으로 게임 오버!");
            GameOver = true;

            // 플레이어 못움직이게 고정
            player.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            // 게임오버 UI
            EndUI.gameObject.SetActive(true);

            // 태초마을로 이동
            Invoke("goStart", 5f);
        }
    }
    public void goStart()
    {
        SceneManager.LoadScene("StartVillage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

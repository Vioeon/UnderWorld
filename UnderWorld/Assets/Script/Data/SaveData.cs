using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public SaveData(string _name, int _life, string _weapon, float _posX, float _posY, string _monster, float _atk, bool _win)
	{
		name = _name;
		life = _life;
		weapon = _weapon;
		posX = _posX;
		posY = _posY;
		monster = _monster;
		atk = _atk;
		win = _win;
	}

	public string name;
	public int life;
	public string weapon;
	public float posX;
	public float posY;
	public string monster;
	public float atk;
	public bool win;

}

/*
데이터 저장
SaveData character = new SaveData("왼손잡이 개발자", 30, 100f);
SaveSystem.Save(character, "save_001");

데이터 로드
SaveData loadData = SaveSystem.Load("save_001");
Debug.Log(string.Format("LoadData Result => name : {0}, age : {1}, power : {2}", loadData.name, loadData.age, loadData.power));
*/
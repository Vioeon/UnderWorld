﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [HideInInspector]
    public Dropdown weapon_dropdown;
    [HideInInspector]
    public Dropdown animation_dropdown;
    [HideInInspector]
    public Button levelup_button;
    [HideInInspector]
    public Toggle loop_toggle;
    [HideInInspector]
    public Toggle wing_toggle;
    [HideInInspector]
    public Dropdown emoji_dropdown;


    [HideInInspector]
    public List<SkeletonAnimation> PIXEL_list = new List<SkeletonAnimation>();
    [HideInInspector]
    public List<GameObject> Level_Object;
    [HideInInspector]
    public List<Transform> Shadow_Transfrom;

    private int level_count = 0;

    public struct UI_index
    {
        public int ani_index;
        public int weapon_index;

    }
    public UI_index[] UI_Index = new UI_index[3];

    public enum weapon
    {
        None,
        Amber_Knight_Weapon,
        Black_Knight_Weapon,
        Blue_Knight_Weapon,
        Bone_Knight_Weapon,
        Elf_Knight_Weapon,
        Gold_Knight_Weapon,
        Holy_Knight_Weapon,
        Paladin_Knight_Weapon,
        Red_Knight_Weapon,
        Vengeance_Knight_Weapon

    }
    public enum Animation
    {
        Idle_1,
        Idle_2,
        Idle_3,
        Attack_1,
        Attack_2,
        Attack_3,
        Attack_4,
        Attack_5,
        Attack_6,
        Attack_7,
        Attack_8,
        Attack_9,
        Attack_10,
        Die_1,
        Die_2,
        Hit,
        Stun,
        Run_NoHand,
        Run_Weapon,
        Walk_NoHand,
        Walk_Weapon,
        Win_1,
        Win_2,
        Jump,
        Jump_Landing,
        Jump_attack

    }

    public enum Emoji
    {
        Emoji_Angel_Ring,
        Emoji_Bad,
        Emoji_Bomb,
        Emoji_Coin,
        Emoji_Deadly,
        Emoji_Devil_Ring,
        Emoji_Heart,
        Emoji_Note,
        Emoji_Star,
        Emoji_Smile,
        Emoji_Sad

    }


    [Space(10)]
    [Header("Change Enum")]

    public weapon Set_weapon;
    public Animation Set_Animation;
    public Emoji Set_Emoji;

    public void Start()
    {
        waepon_dropdown_create();
        animation_dropdown_create();
        emoji_dropdown_create();
        Debug.Log("pixel: " + PIXEL_list.Count);
        Debug.Log("shadow: " + Shadow_Transfrom.Count);
        
        for (int i = 0; i < PIXEL_list.Count; i++)
        {
            PIXEL_list[i].state.SetAnimation(0, "Idle_1", true);
        }
        Debug.Log(PIXEL_list.Count);
        //PIXEL_list[9].state.SetAnimation(0, "Attack_1", true);
        Level_Object[1].SetActive(false);
        Level_Object[2].SetActive(false);

    }

    
    public void Update()
    {
        for (int i = 0; i < Shadow_Transfrom.Count; i++)
        {
            Shadow_Transfrom[i].position = new Vector2(PIXEL_list[i].transform.position.x, Shadow_Transfrom[i].transform.position.y);
        }
    }

    public void wing()
    {
        if(!wing_toggle.isOn)
        {
            for(int i = 10; i <PIXEL_list.Count; i++)
            {
                PIXEL_list[i].Skeleton.SetAttachment("Back",null);
            }
        }

        else
        {
            for (int i = 10; i < PIXEL_list.Count; i++)
            {
                PIXEL_list[i].Skeleton.SetAttachment("Back", "Back");
            }
        }
    }

    public void Weapon_Chage()
    {
        for (int i = 0; i < PIXEL_list.Count; i++)
        {
            for (int a = 0; a < System.Enum.GetValues(typeof(weapon)).Length; a++)
            {
                if (PIXEL_list[i].gameObject.activeInHierarchy)
                    PIXEL_list[i].Skeleton.SetAttachment("Weapon", null);
            }
            if (PIXEL_list[i].gameObject.activeInHierarchy)
                PIXEL_list[i].Skeleton.SetAttachment("Weapon", Set_weapon.ToString());
        }

        weapon_dropdown.value = (int)Set_weapon;
    }

    public void Animation_Play()
    {
        for (int i = 0; i < PIXEL_list.Count; i++)
        {
            for (int a = 0; a < System.Enum.GetValues(typeof(Animation)).Length; a++)
            {
                if (PIXEL_list[i].gameObject.activeInHierarchy)
                    PIXEL_list[i].state.SetAnimation(0, Set_Animation.ToString(), true);
            }
        }
        animation_dropdown.value = (int)Set_Animation;
    }

    public void Level_Up()
    {
        if (level_count == 2)
        {
            Level_Object[level_count].SetActive(false);

            level_count = 0;

            Level_Object[level_count].SetActive(true);
        }

        else if (Level_Object[level_count].activeInHierarchy)
        {
            Level_Object[level_count].SetActive(false);

            Level_Object[level_count + 1].SetActive(true);

            level_count += 1;
        }

        levelup_button.transform.Find("Text").gameObject.GetComponent<Text>().text = level_count + 1 + " Level";

        weapon_dropdown.value = UI_Index[level_count].weapon_index;

        animation_dropdown.value = UI_Index[level_count].ani_index;
    }


    private void emoji_dropdown_create()
    {
        emoji_dropdown.onValueChanged.AddListener(delegate { emoji_dropdownValueChangedHandler(emoji_dropdown); });
        emoji_dropdown.options.Clear();

        for (var i = 0; i < System.Enum.GetValues(typeof(Emoji)).Length; i++)
        {
            emoji_dropdown.options.Add(new Dropdown.OptionData() { text = ((Emoji)i).ToString() });
        }
    }

    private void emoji_dropdownValueChangedHandler(Dropdown target)
    {
        for (int i = 0; i < PIXEL_list.Count; i++)
        {

            PIXEL_list[i].Skeleton.SetAttachment("Emoji", target.options[target.value].text);

        }

        Set_Emoji = (Emoji)target.value;

        //UI_Index[level_count].weapon_index = target.value;

    }

    private void waepon_dropdown_create()
    {
        weapon_dropdown.onValueChanged.AddListener(delegate { waepon_dropdownValueChangedHandler(weapon_dropdown); });
        weapon_dropdown.options.Clear();

        for (var i = 0; i < System.Enum.GetValues(typeof(weapon)).Length; i++)
        {
            weapon_dropdown.options.Add(new Dropdown.OptionData() { text = ((weapon)i).ToString() });
        }
    }

    private void waepon_dropdownValueChangedHandler(Dropdown target)
    {
        for (int i = 0; i < PIXEL_list.Count; i++)
        {
            if (target.value == 0)
                PIXEL_list[i].Skeleton.SetAttachment("Weapon", null);
            else
            {
                PIXEL_list[i].Skeleton.SetAttachment("Weapon", target.options[target.value].text);
            }
        }


        Set_weapon = (weapon)target.value;

        UI_Index[level_count].weapon_index = target.value;

    }

    private void animation_dropdown_create()
    {
        animation_dropdown.onValueChanged.AddListener(delegate { animation_dropdownValueChangedHandler(animation_dropdown); });
        animation_dropdown.options.Clear();

        for (var i = 0; i < System.Enum.GetValues(typeof(Animation)).Length; i++)
        {
            animation_dropdown.options.Add(new Dropdown.OptionData() { text = ((Animation)i).ToString() });
        }
    }

    private void animation_dropdownValueChangedHandler(Dropdown target)
    {
        for (int i = 0; i < PIXEL_list.Count; i++)
        {
            if (PIXEL_list[i].gameObject.activeInHierarchy)
                PIXEL_list[i].state.SetAnimation(0, target.options[target.value].text, loop_toggle.isOn);
        }

        Set_Animation = (Animation)target.value;

        UI_Index[level_count].ani_index = target.value;
    }


    /*
    private Rigidbody2D rig;
    private float xx;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }*/
}

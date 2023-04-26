using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GlobalDefine
{
    public const string formatSoundPath_Global = "Sounds/Global/{0}";
    public const string formatSoundPath_Game = "Sounds/GameScene/{0}";
    public const float defaultVolume = 1.0f;
    
    public static readonly Dictionary<ESoundSet, KeyValuePair<string, float>> SoundContainer = new Dictionary<ESoundSet, KeyValuePair<string, float>>()
    {
        //[FX] = (Prefab Name, Despawn Time)
        [ESoundSet.SOUND_BUTTON]        = new KeyValuePair<string, float>("insert_jewel", defaultVolume),
        [ESoundSet.SOUND_LEVEL_READY]   = new KeyValuePair<string, float>("Menu Alert", defaultVolume),
        [ESoundSet.SOUND_LEVEL_CLEAR]   = new KeyValuePair<string, float>("Game Win 2", defaultVolume),
        [ESoundSet.SOUND_LEVEL_FAIL]    = new KeyValuePair<string, float>("Soft Fail", defaultVolume),
        [ESoundSet.SOUND_WARNING]       = new KeyValuePair<string, float>("Pleasant Fail", defaultVolume),
        [ESoundSet.SOUND_GET_STAR]      = new KeyValuePair<string, float>("Congratsbell", defaultVolume),
        [ESoundSet.SOUND_GET_COIN]      = new KeyValuePair<string, float>("Collected Jewel", defaultVolume),
        [ESoundSet.SOUND_GET_ITEM]      = new KeyValuePair<string, float>("Menu Notification", defaultVolume),

        [ESoundSet.SOUND_ATTACK_NORMAL] = new KeyValuePair<string, float>("Bubble Pops 2", defaultVolume),
        [ESoundSet.SOUND_ATTACK_WOOD]   = new KeyValuePair<string, float>("Break Wood Box 1", defaultVolume),
        [ESoundSet.SOUND_ATTACK_IRON]   = new KeyValuePair<string, float>("Iron Metal Rack Hit 1", defaultVolume),
        [ESoundSet.SOUND_BRICK_DESTROY]   = new KeyValuePair<string, float>("weapon05", defaultVolume),

        [ESoundSet.SOUND_ITEM_EARTHQUAKE]   = new KeyValuePair<string, float>("quake", defaultVolume),
        [ESoundSet.SOUND_ITEM_ADD_BALL]   = new KeyValuePair<string, float>("Menu Notification", defaultVolume),
        [ESoundSet.SOUND_ITEM_BRICKS_DELETE]   = new KeyValuePair<string, float>("weapon06", defaultVolume),
        [ESoundSet.SOUND_ITEM_ADD_LASER_BRICKS]   = new KeyValuePair<string, float>("Menu Notification", defaultVolume),
        [ESoundSet.SOUND_ITEM_ADD_STEEL_BRICKS]   = new KeyValuePair<string, float>("Menu Button", defaultVolume),

        [ESoundSet.SOUND_SPECIAL_MISSILE]   = new KeyValuePair<string, float>("Fire - Spell Impact 2", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_EXPLOSION]   = new KeyValuePair<string, float>("Rock Hit", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_EXPLOSION_3x3]   = new KeyValuePair<string, float>("Small-Bomb-Blast_TTX022901", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_EXPLOSION_ALL]   = new KeyValuePair<string, float>("Small-Bomb-Blast_TTX022901", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_ADD_BALL]   = new KeyValuePair<string, float>("Menu Notification", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_LIGHTNING]   = new KeyValuePair<string, float>("Electric - Spell Impact 1", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_ARROW]   = new KeyValuePair<string, float>("Ice - Spell Impact 2", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_EARTHQUAKE]   = new KeyValuePair<string, float>("quake", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_KEY]   = new KeyValuePair<string, float>("Puzzle Game Piece 05", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_WORMHOLE]   = new KeyValuePair<string, float>("Dark Magic Entrapment Spell", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_OPEN_CLOSE]   = new KeyValuePair<string, float>("Madagascar Cockroach Hissing 04", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_LASER]   = new KeyValuePair<string, float>("weapon01", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_DIFFUSION]   = new KeyValuePair<string, float>("Quest_Game_Tribal_Organic_Irish_Hand_Drum_6_Soft_Ethnic", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_AMPLIFICATION]   = new KeyValuePair<string, float>("Quest_Game_Tribal_Organic_Irish_Hand_Drum_6_Soft_Ethnic", defaultVolume),
        [ESoundSet.SOUND_SPECIAL_POWERBALL]   = new KeyValuePair<string, float>("PyroFireBallBurst_zsbOB_03", defaultVolume),
    };

    ///<Summary>효과음 재생.</Summary>
    public static void PlaySoundFX(ESoundSet _sound)
    {
        if(!SoundContainer.ContainsKey(_sound)) return;

        //Debug.Log(CodeManager.GetMethodName() + string.Format(FORMAT_INT, _sound));

        KeyValuePair<string, float> _kv = SoundContainer[_sound];
        CSndManager.Inst.PlayFXSnds(string.Format(formatSoundPath_Game, _kv.Key), _kv.Value);
    }

    ///<Summary>효과음 재생. (Global 폴더.)</Summary>
    public static void PlaySoundFX_Global(ESoundSet _sound)
    {
        if(!SoundContainer.ContainsKey(_sound)) return;

        Debug.Log(CodeManager.GetMethodName() + string.Format(FORMAT_INT, _sound));

        KeyValuePair<string, float> _kv = SoundContainer[_sound];
        CSndManager.Inst.PlayFXSnds(string.Format(formatSoundPath_Global, _kv.Key), _kv.Value);
    }
}

public enum ESoundSet
{
    SOUND_BUTTON, // 버튼 터치.
    SOUND_LEVEL_READY, // 레벨 준비.
    SOUND_LEVEL_CLEAR, // 레벨 성공.
    SOUND_LEVEL_FAIL, // 레벨 실패.
    SOUND_WARNING, // 하단 경고.
    SOUND_GET_STAR, // 별 획득.
    SOUND_GET_COIN, // 재화 획득.
    SOUND_GET_ITEM, // 기타 아이템 획득.

    SOUND_ATTACK_NORMAL, // 일반 블럭 타격.
    SOUND_ATTACK_WOOD, // 나무 블럭 타격.
    SOUND_ATTACK_IRON, // 강철 블럭 타격.
    SOUND_BRICK_DESTROY, // 블럭 파괴.    

    SOUND_ITEM_EARTHQUAKE, // 지진.
    SOUND_ITEM_ADD_BALL, // 볼 추가.
    SOUND_ITEM_BRICKS_DELETE, // 하단 제거.
    SOUND_ITEM_ADD_LASER_BRICKS, // 레이저 추가.
    SOUND_ITEM_ADD_STEEL_BRICKS, // 강철 추가.

    SOUND_SPECIAL_MISSILE, // 미사일.
    SOUND_SPECIAL_EXPLOSION, // 폭탄.
    SOUND_SPECIAL_EXPLOSION_3x3, // 3x3 폭탄.
    SOUND_SPECIAL_EXPLOSION_ALL, // 대형 폭탄.
    SOUND_SPECIAL_ADD_BALL, // 볼 추가.
    SOUND_SPECIAL_LIGHTNING, // 번개.
    SOUND_SPECIAL_ARROW, // 화살.
    SOUND_SPECIAL_EARTHQUAKE, // 지진.
    SOUND_SPECIAL_KEY, // 열쇠 & 자물쇠.
    SOUND_SPECIAL_WORMHOLE, // 웜홀.
    SOUND_SPECIAL_OPEN_CLOSE, // 강철 블럭 열림 & 닫힘.
    SOUND_SPECIAL_LASER, // 레이저.
    SOUND_SPECIAL_DIFFUSION, // 확산.
    SOUND_SPECIAL_AMPLIFICATION, // 증폭.
    SOUND_SPECIAL_POWERBALL, // 파워볼.
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GlobalDefine
{
    // [Effect] Laser
    public static Vector3 FXLaser_Rotation_Horizontal = new Vector3(0, 0, 90f);
    public static Vector3 FXLaser_Rotation_Vertictal = Vector3.zero;

    // [Cell Effect] Bomb
    public static Vector3 FXBombFlame_Position_Default = new Vector3(25f, 25f, 0);
    public static Vector3 FXBombFlame_Position_3x3 = new Vector3(25f, 20f, 0);

    // [Cell Effect] Lightning
    public const float FXLightning_StartSizeY_Min = 0.05f;
    public const float FXLightning_StartSizeY_Max_Multiplier = 2f;

    // [Cell Effect] Missile
    public const float FXMissile_Time = 1f;
    public const float FXMissile_AngleOffset = -90f;

    // [Ball Effect] Power Ball
    public const float FXPowerBall_Size = 2f;

    public const string formatFXPath = "Prefabs/Engine/FX/{0}";
    
    public static readonly Dictionary<EFXSet, KeyValuePair<string, float>> FXContainer = new Dictionary<EFXSet, KeyValuePair<string, float>>()
    {
        //[FX] = (Prefab Name, Despawn Time)
        [EFXSet.FX_BREAK_BRICK]     = new KeyValuePair<string, float>("FX_BreakBrick_Square", 0.5f),        // [OK] 블럭 파괴.
        [EFXSet.FX_POWER_BALL_HIT]  = new KeyValuePair<string, float>("FX_FireCircle", 1.0f),               // [OK] 파워볼 히트.
        [EFXSet.FX_LASER]           = new KeyValuePair<string, float>("FX_Laser", 0.01f),                   // [OK] 레이저.
        [EFXSet.FX_EXPLOSION_3x3]   = new KeyValuePair<string, float>("FX_Explosion_3x3_01", 1.0f),         // [OK] 3x3 폭탄.
        [EFXSet.FX_MISSILE_BULLET]  = new KeyValuePair<string, float>("FX_MissileBullet", FXMissile_Time),  // [OK] 미사일.
        [EFXSet.FX_MISSILE_HEAD]    = new KeyValuePair<string, float>("FX_MissileHead", FXMissile_Time),    // [OK] 미사일 과녘.
        [EFXSet.FX_LIGHTNING]       = new KeyValuePair<string, float>("FX_Lightning", 0.2f),                // [OK] 번개.
        [EFXSet.FX_LIGHTNING_HIT]   = new KeyValuePair<string, float>("FX_Lightning_Hit", 0.02f),           // [OK] 번개 히트.
    };

    public static readonly Dictionary<EFXSet, string> CellFXContainer = new Dictionary<EFXSet, string>()
    {
        //[FX] = (Prefab Name)
        [EFXSet.FX_BOMB_FLAME]      = "FX_BombFlame",   // [OK] 폭탄 심지.
    };

    ///<Summary>이펙트 재생 (Scale/Rotation 변경하지 않음.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _position, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _position, _isWorldPosition);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>이펙트 재생 (Scale/Rotation 변경하지 않음.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _position, Color _startColor, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _position, _isWorldPosition);
        effect.GetComponent<FXSettings>().SetColor(_startColor);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>이펙트 재생 (Scale/Rotation 변경.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _position, Vector3 _rotation, Vector3 _scale, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _scale, _rotation, _position, _isWorldPosition);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>이펙트 재생 (Lightning 전용.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _position, float _angle, Vector3 _scale, float sizeY_Min, float sizeY_Max, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _scale, _position, _isWorldPosition);
        effect.GetComponent<FXSettings>().SetRotation(_angle);
        effect.GetComponent<FXSettings>().SetStartSizeY(sizeY_Min, sizeY_Max);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>이펙트 재생 (Missile 전용.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _position, float _angle, NSEngine.CEObj _moveTarget, Action<NSEngine.CEObj> completeCallback, float _moveTime, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _position, _isWorldPosition);
        effect.GetComponent<FXSettings>().SetRotation(_angle);
        effect.GetComponent<FXSettings>().SetMove(_moveTarget, completeCallback, _moveTime);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>셀 이펙트 추가.</Summary>
    public static void AddCellEffect(EFXSet _effect, Transform _parent, Vector3 _localPosition)
    {
        GameObject prefab = GameObject.Instantiate(Resources.Load<GameObject>(string.Format(formatFXPath, CellFXContainer[_effect])), _parent);

        prefab.transform.localPosition = _localPosition;
    }
}

public enum EFXSet
{
    FX_BREAK_BRICK, // 블럭 파괴.
    FX_POWER_BALL_HIT, // 파워볼 히트.
    FX_LASER, // 수직/수평/십자 레이저.
    FX_LIGHTNING, // 번개 블럭.
    FX_LIGHTNING_HIT, // 번개 블럭 히트.
    FX_EXPLOSION_3x3, // 3x3 폭탄.
    FX_BOMB_FLAME, // 폭탄 심지 효과.
    FX_MISSILE_BULLET, // 미사일.
    FX_MISSILE_HEAD, // 미사일 과녘.
}
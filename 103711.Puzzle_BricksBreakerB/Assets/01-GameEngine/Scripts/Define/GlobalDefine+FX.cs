using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GlobalDefine
{
    // [Effect] Laser
    public static Vector3 FXLaser_Rotation_Horizontal = new Vector3(0, 0, 90f);
    public static Vector3 FXLaser_Rotation_Vertictal = Vector3.zero;

    // [Cell Effect] Bomb
    public static Vector3 FXBombFlame_Position_Default = new Vector3(25f, 25f, 0);
    public static Vector3 FXBombFlame_Position_3x3 = new Vector3(25f, 20f, 0);

    public const string formatFXPath = "Prefabs/Engine/FX/{0}";
    
    public static readonly Dictionary<EFXSet, KeyValuePair<string, float>> FXContainer = new Dictionary<EFXSet, KeyValuePair<string, float>>()
    {
        //[FX] = (Prefab Name, Despawn Time)
        [EFXSet.FX_BREAK_BRICK]     = new KeyValuePair<string, float>("FX_BreakBrick_Square", 0.5f),    // [OK] 블럭 파괴.
        [EFXSet.FX_POWER_BALL_HIT]  = new KeyValuePair<string, float>("FX_FireCircle", 1.0f),           // [OK] 파워볼 히트.
        [EFXSet.FX_LASER]           = new KeyValuePair<string, float>("FX_Laser", 0.01f),               // [OK] 레이저.
        [EFXSet.FX_EXPLOSION_3x3]   = new KeyValuePair<string, float>("FX_Explosion_3x3_01", 1.0f),     // [OK] 3x3 폭탄.
        [EFXSet.FX_MISSILE]         = new KeyValuePair<string, float>("FX_Missile", 1.0f),              // [] 미사일.
        [EFXSet.FX_LIGHTNING]       = new KeyValuePair<string, float>("FX_Lightning", 1.0f),            // [] 번개.
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
    public static void ShowEffect(EFXSet _effect, Color _startColor, Vector3 _position, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _position, _isWorldPosition);
        effect.GetComponent<FXColor>().SetColor(_startColor);
        CSceneManager.ActiveSceneManager.DespawnObj(_kv.Key, effect.gameObject, _kv.Value);
    }

    ///<Summary>이펙트 재생 (Scale/Rotation 변경.)</Summary>
    public static void ShowEffect(EFXSet _effect, Vector3 _scale, Vector3 _rotation, Vector3 _position, bool _isWorldPosition = true)
    {
        KeyValuePair<string, float> _kv = FXContainer[_effect];
        Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(_kv.Key, _kv.Key, _scale, _rotation, _position, _isWorldPosition);
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
    FX_EXPLOSION_3x3, // 3x3 폭탄.
    FX_BOMB_FLAME, // 폭탄 심지 효과.
    FX_MISSILE, // 미사일.
}
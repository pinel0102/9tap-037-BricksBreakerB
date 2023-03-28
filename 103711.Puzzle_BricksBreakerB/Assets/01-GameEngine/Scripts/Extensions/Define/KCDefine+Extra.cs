using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static partial class KDefine {
	#region 기본
    // 식별자 {
	public const string G_KEY_IS_ONCE = "IsOnce";
	public const string G_KEY_IS_RAND = "IsRandom";
	public const string G_KEY_IS_OVERLAY = "IsOverlay";
	public const string G_KEY_IS_TRANSPARENT = "IsTransparent";
	public const string G_KEY_IS_CLEAR_TARGET = "IsClearTarget";
	public const string G_KEY_IS_SKILL_TARGET = "IsSkillTarget";
    public const string G_KEY_IS_SHIELD_CELL = "IsShieldCell";

	public const string G_KEY_IS_ENABLE_HIT = "IsEnableHit";
	public const string G_KEY_IS_ENABLE_COLOR = "IsEnableColor";
	public const string G_KEY_IS_ENABLE_CHANGE = "IsEnableChange";
	public const string G_KEY_IS_ENABLE_REFLECT = "IsEnableReflect";
	public const string G_KEY_IS_ENABLE_REFRACT = "IsEnableRefract";
	public const string G_KEY_IS_ENABLE_MOVE_DOWN = "IsEnableMoveDown";

	public const string G_KEY_HIT_SKILL_KINDS = "HitSkillKinds";
	public const string G_KEY_DESTROY_SKILL_KINDS = "DestroySkillKinds";

	public const string G_KEY_COLLIDER_TYPE = "ColliderType";
	public const string G_KEY_COLLIDER_SIZE = "ColliderSize";
	// 식별자 }

	// 이름
	public const string PS_OBJ_N_GLOW_IMG = "GLOW_IMG";
	#endregion // 기본

	#region 런타임 상수
	// 색상
	public static readonly Color PS_CLEAR_COLOR = new Color(0x17 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x17 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x2a / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 1.0f);
	#endregion // 런타임 상수
}

namespace NSEngine {
	/** 추가 상수 - 엔진 */
	public static partial class KDefine {
        // 단위 {
        public const int E_MAX_BOOSTER = 100;
        public const int E_EXTRA_NUM_CELLS_Y = 4;

        public const float E_SPEED_SHOOT = 1250.0f;
        public const float E_SCALE_BRICKS = 0.95f;

        public const float E_LENGTH_LINE = 100.0f;
        public const float E_MAX_LENGTH_LINE = 450.0f;

        public const float E_MIN_ANGLE_AIMING = 5.0f;
        public const float E_MAX_ANGLE_REFRACT = 60.0f;

        public const float E_DELTA_T_TIME_SCALE = 10.0f;
        public const float E_DURATION_MOVE_DOWN_ANI = 0.10f;
        // 단위 }
    }
}

/** 그리드 뷰 사이즈 */
public enum EViewSize {
	NONE = -1,
	DEF_SIZE_X = 11,
	DEF_SIZE_Y = 11,
    DEF_SIZE_Z = 1,
	[HideInInspector] MAX_VAL
}

public enum EColliderType {
	NONE = -1,
	CIRCLE,
	SQUARE,
	TRIANGLE,
	RIGHT_TRIANGLE,
	DIAMOND,

	CUSTOM_CIRCLE,
	CUSTOM_SQUARE,
	CUSTOM_TRIANGLE,
	CUSTOM_RIGHT_TRIANGLE,
	CUSTOM_DIAMOND,
	[HideInInspector] MAX_VAL
}
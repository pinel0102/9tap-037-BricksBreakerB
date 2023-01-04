using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/*
=====> 모듈 버전 정보 <=====
- 버전: "3.0.0"
- 관련 파일 개수: 29

=====> Experimental <=====
- RenderingMode.Deferred
- SpritePackerMode.SpriteAtlasV2
- LightingSettings.Lightmapper.ProgressiveGPU

=====> 씬 로딩 순서 <=====
- Init
- Start
- Setup (Additive)
- Agree (Additive)
- LateSetup (Additive)
- Title

=====> 소속 번호 <=====
- 0: 기타
- 1: 개인
- 2: 회사

=====> 종류 번호 <=====
- 0: 기타
- 1: 앱
- 2: 게임
- 3: 프레임워크

=====> 장르 번호 (게임) <=====
- 0: 기타
- 1: 퍼즐
- 2: 캐주얼

=====> 프로젝트 식별자 지정 방식 (10 자리) <=====
- 소속 번호 (1 자리) + 종류 번호 (1 자리) + 장르 번호 (1 자리) + 프로젝트 번호 (3 자리) + 서브 프로젝트 번호 (2 자리) + 확장 번호 (2 자리)

=====> 9tap 프로젝트 식별자 지정 방식 (6 자리) <=====
- 장르 번호 (1 자리) + 프로젝트 번호 (3 자리) + 서브 프로젝트 번호 (1 자리) + 확장 번호 (1 자리)

=====> Kinds 값 지정 방식 <=====
- Type 별로 100,000,000 단위로 값 지정
- Kinds Type 별로 100,000 단위로 값 지정
- Sub Kinds Type 별로 100 단위로 값 지정
- Detail Sub Kinds Type 별로 1 단위로 값 지정

=====> MsgPack 인덱스 지정 방식 <=====
- 1 ~ 9: 값 (기본) 형식 데이터
- 11 ~ 19: 값 (열거형) 형식 데이터
- 21 ~ 29: 값 (복합) 형식 데이터
- 31 ~ 39: 참조 (기본) 형식 데이터
- 41 ~ 49: 참조 (복합) 형식 데이터
- 51 ~ 109: 리스트 형식 데이터
- 111 ~ 169: 딕셔너리 형식 데이터

=====> MsgPack 인덱스 지정 방식 (CBaseInfo 상속 시) <=====
- 1 ~ 49: 복합 형식 데이터
- 51 ~ 109: 리스트 형식 데이터
- 111 ~ 169: 딕셔너리 형식 데이터

=====> 복합 형식 데이터 세부 범위 <=====
- 1 ~ 29: 값 형식 데이터
- 31 ~ 49: 참조 형식 데이터

=====> 리스트 형식 데이터 세부 범위 <=====
- 51 ~ 59: 값 (기본) 형식 데이터
- 61 ~ 69: 값 (열거형) 형식 데이터
- 71 ~ 79: 값 (복합) 형식 데이터
- 81 ~ 89: 참조 (기본) 형식 데이터
- 91 ~ 99: 참조 (복합) 형식 데이터
- 101 ~ 109: 참조 (복합) 형식 데이터 (리스트 or 딕셔너리)

=====> 딕셔너리 형식 데이터 세부 범위 <=====
- 111 ~ 119: 값 (기본) 형식 데이터
- 121 ~ 129: 값 (열거형) 형식 데이터
- 131 ~ 139: 값 (복합) 형식 데이터
- 141 ~ 149: 참조 (기본) 형식 데이터
- 151 ~ 159: 참조 (복합) 형식 데이터
- 161 ~ 169: 참조 (복합) 형식 데이터 (리스트 or 딕셔너리)
*/
#region 기본
/** POT */
public enum EPOT {
	NONE = -1,
	[HideInInspector] _2 = 2,
	[HideInInspector] _4 = 4,
	[HideInInspector] _8 = 8,
	[HideInInspector] _16 = 16,
	[HideInInspector] _32 = 32,
	[HideInInspector] _64 = 64,
	[HideInInspector] _128 = 128,
	_256 = 256,
	_512 = 512,
	_1024 = 1024,
	_2048 = 2048,
	_4096 = 4096,
	_8192 = 8192,
	[HideInInspector] MAX_VAL
}

/** 열거형 값 */
public enum EEnumVal {
	NONE = -1,
	TYPE = KCDefine.B_UNIT_KINDS_PER_TYPE,
	SUB_TYPE = KCDefine.B_UNIT_KINDS_PER_SUB_TYPE,
	KINDS_TYPE = KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE,
	SUB_KINDS_TYPE = KCDefine.B_UNIT_KINDS_PER_SUB_KINDS_TYPE,
	[HideInInspector] MAX_VAL,
}

/** 종류 그룹 타입 */
public enum EKindsGroupType {
	NONE = -1,
	TYPE,
	SUB_TYPE,
	KINDS_TYPE,
	SUB_KINDS_TYPE,
	[HideInInspector] MAX_VAL
}

/** 벡터 타입 */
public enum EVecType {
	NONE = -1,
	POS,
	DIRECTION,
	[HideInInspector] MAX_VAL
}

/** 수직 동기화 */
public enum EVSync {
	NONE = -1,
	NEVER,
	EVERY,
	SECOND,
	[HideInInspector] MAX_VAL
}

/** 그리드 기준점 */
public enum EGridPivot {
	NONE = -1,
	MID,
	DOWN,
	[HideInInspector] MAX_VAL
}

/** 터치 이벤트 */
public enum ETouchEvent {
	NONE = -1,
	BEGIN,
	MOVE,
	END,
	[HideInInspector] MAX_VAL
}

/** 마우스 버튼 */
public enum EMouseBtn {
	NONE = -1,
	LEFT,
	RIGHT,
	MID,
	[HideInInspector] MAX_VAL
}

/** 분석 */
public enum EAnalytics {
	NONE = -1,
	FLURRY,
	FIREBASE,
	APPS_FLYER,
	PLAYFAB,
	[HideInInspector] MAX_VAL
}

/** 폰트 세트 */
public enum EFontSet {
	NONE = -1,
	_1,
	_2,
	_3,
	_4,
	_5,
	_6,
	_7,
	_8,
	_9,
	[HideInInspector] MAX_VAL
}

/** 방향 */
public enum EDirection {
	NONE = -1,

	UP,
	DOWN,
	LEFT,
	RIGHT,

	UP_LEFT,
	UP_RIGHT,
	DOWN_LEFT,
	DOWN_RIGHT,

	LEFT_UP,
	LEFT_DOWN,
	RIGHT_UP,
	RIGHT_DOWN,

	VERTICAL,
	HORIZONTAL,
	DIAGONAL,

	[HideInInspector] MAX_VAL
}

/** 반전 */
public enum EReflection {
	NONE = -1,
	IGNORE,
	VERTICAL,
	HORIZONTAL,
	VERTICAL_HORIZONTAL,
	[HideInInspector] MAX_VAL
}

/** 투영 */
public enum EProjection {
	NONE = -1,
	_2D,
	_3D,
	[HideInInspector] MAX_VAL,
}

/** 직교 */
public enum EOrthogonal {
	NONE = -1,
	CW,
	CCW,
	[HideInInspector] MAX_VAL
}

/** 난이도 */
public enum EDifficulty {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	VERY_HARD,
	SUPER_HARD,
	ULTRA_HARD,
	[HideInInspector] MAX_VAL
}

/** 채우기 기준 */
public enum EFillOrigin {
	NONE = -1,
	_1,
	_2,
	_3,
	_4,
	[HideInInspector] MAX_VAL
}

/** 퀄리티 수준 */
public enum EQualityLevel {
	NONE = -1,
	[HideInInspector] VERY_LOW,
	[HideInInspector] LOW,
	NORM,
	HIGH,
	[HideInInspector] VERY_HIGH,
	ULTRA,
	[HideInInspector] MAX_VAL
}

/** 광원 맵 모드 */
public enum ELightmapMode {
	NONE = -1,
	NON_DIRECTIONAL,
	COMBINE_DIRECTIONAL,
	[HideInInspector] MAX_VAL
}

/** 안티 에일리어싱 수준 */
public enum EAAQuality {
	NONE = -1,
	DISABLE,
	_2x,
	_4x,
	_8x,
	[HideInInspector] MAX_VAL
}

/** 광원 맵 엔코딩 퀄리티 */
public enum ELightmapEncodingQuality {
	NONE = -1,
	LOW,
	NORM,
	HIGH,
	[HideInInspector] MAX_VAL
}

/** 그림자 옵션 */
public enum EShadowCascadesOpts {
	NONE = -1,
	NO_CASCADES = 1,
	TWO_CASCADES = 2,
	FOUR_CASCADES = 4,
	[HideInInspector] MAX_VAL
}

/** 약관 동의 팝업 */
public enum EAgreePopup {
	NONE = -1,
	KR,
	EU,
	[HideInInspector] MAX_VAL
}

/** 내비게이션 스택 이벤트 */
public enum ENavStackEvent {
	NONE = -1,
	TOP,
	REMOVE,
	BACK_KEY_DOWN,
	[HideInInspector] MAX_VAL
}

/** 시작 씬 이벤트 */
public enum EStartSceneEvent {
	NONE = -1,
	LOAD_START_SCENE,
	LOAD_SETUP_SCENE,
	LOAD_AGREE_SCENE,
	LOAD_LATE_SETUP_SCENE,
	LOAD_NEXT_SCENE,
	[HideInInspector] MAX_VAL
}

/** 키 타입 */
public enum EKeyType {
	NONE = -1,
	MULTI,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 값 타입 */
public enum EValType {
	NONE = -1,
	INT,
	REAL,
	[HideInInspector] MAX_VAL
}

/** 애니메이션 타입 */
public enum EAniType {
	NONE = -1,
	SCALE,
	DROPDOWN,
	SLIDE_LEFT,
	SLIDE_RIGHT,
	[HideInInspector] MAX_VAL
};

/** 유저 타입 */
public enum EUserType {
	NONE = -1,
	A,
	B,
	[HideInInspector] MAX_VAL
}

/** 디바이스 타입 */
public enum EDeviceType {
	NONE = -1,
	PHONE,
	TABLET,
	DESKTOP,
	CONSOLE,
	HANDHELD_CONSOLE,
	UNKNOWN,
	[HideInInspector] MAX_VAL
}

/** iOS 타입 */
public enum EiOSType {
	NONE = -1,
	APPLE,
	[HideInInspector] MAX_VAL
}

/** 안드로이드 타입 */
public enum EAndroidType {
	NONE = -1,
	GOOGLE,
	AMAZON,
	[HideInInspector] MAX_VAL
}

/** 독립 플랫폼 타입 */
public enum EStandaloneType {
	NONE = -1,
	MAC_STEAM,
	WNDS_STEAM,
	[HideInInspector] MAX_VAL
}

/** 진동 타입 */
public enum EVibrateType {
	NONE = -1,
	SELECTION,
	NOTIFICATION,
	IMPACT,
	[HideInInspector] MAX_VAL
}

/** 진동 스타일 */
public enum EVibrateStyle {
	NONE = -1,
	LIGHT,
	MEDIUM,
	HEAVY,
	[HideInInspector] MAX_VAL
}
#endregion // 기본

#region 조건부 상수
#if UNITY_EDITOR
/** DSP 버퍼 크기 */
public enum EDSPBufferSize {
	NONE = -1,
	DEFAULT,
	BEST_LATENCY = 256,
	GOOD_LATENCY = 512,
	BEST_PERFORMANCE = 1024,
	[HideInInspector] MAX_VAL
}

/** 종횡비 모드 */
public enum EAspectRatioMode {
	NONE = -1,
	LEGACY_WIDE_SCREEN,
	NATIVE_ASPECT_RATIO,
	CUSTOM,
	[HideInInspector] MAX_VAL
}

/** 가속도 갱신 주기 */
public enum EAccelerometerFrequency {
	NONE = -1,
	DISABLE,
	FREQUENCY_15_HZ = 15,
	FREQUENCY_30_HZ = 30,
	FREQUENCY_60_HZ = 60,
	FREQUENCY_100_HZ = 100,
	[HideInInspector] MAX_VAL
}

/** 빌드 모드 */
public enum EBuildMode {
	NONE = -1,
	DEBUG,
	RELEASE,
	STORE,
	[HideInInspector] MAX_VAL
}

/** 텍스처 압축 */
public enum ETexCompression {
	NONE = -1,
	LEGACY,
	DEFAULT,
	CUSTOM,
	[HideInInspector] MAX_VAL
}

#if BURST_COMPILER_MODULE_ENABLE
/** 버스트 컴파일러 최적화 */
public enum EBurstCompilerOptimization {
	NONE = -1,
	DEF,
	PERFORMANCE,
	SIZE,
	FAST_COMPILATION,
	BALANCED,
	[HideInInspector] MAX_VAL
}
#endif // #if BURST_COMPILER_MODULE_ENABLE
#endif // #if UNITY_EDITOR

#if ADS_MODULE_ENABLE
/** 광고 플랫폼 */
public enum EAdsPlatform {
	NONE = -1,
	ADMOB,
	IRON_SRC,
	[HideInInspector] MAX_VAL
}

/** 배너 광고 위치 */
public enum EBannerAdsPos {
	NONE = -1,
	UP,
	DOWN,
	[HideInInspector] MAX_VAL
}
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
/** 결제 플랫폼 */
public enum EPurchasePlatform {
	NONE = -1,
	GOOGLE,
	AMAZON,
	[HideInInspector] MAX_VAL
}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 조건부 상수

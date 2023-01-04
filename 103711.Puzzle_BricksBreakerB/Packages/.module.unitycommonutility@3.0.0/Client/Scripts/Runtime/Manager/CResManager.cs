using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.U2D;
using TMPro;

#if ADDRESSABLES_MODULE_ENABLE
using UnityEngine.AddressableAssets;
#endif // #if ADDRESSABLES_MODULE_ENABLE

#if POST_PROCESSING_MODULE_ENABLE
using UnityEngine.Rendering.PostProcessing;
#endif // #if POST_PROCESSING_MODULE_ENABLE

/** 리소스 관리자 */
public partial class CResManager : CSingleton<CResManager> {
	#region 변수
	private Dictionary<System.Type, Dictionary<string, Object>> m_oResDictContainer = new Dictionary<System.Type, Dictionary<string, Object>>() {
		[typeof(Shader)] = new Dictionary<string, Object>(),

		[typeof(Font)] = new Dictionary<string, Object>(),
		[typeof(Mesh)] = new Dictionary<string, Object>(),
		[typeof(TMP_FontAsset)] = new Dictionary<string, Object>(),

		[typeof(Sprite)] = new Dictionary<string, Object>(),
		[typeof(Texture)] = new Dictionary<string, Object>(),
		[typeof(Material)] = new Dictionary<string, Object>(),
		[typeof(SpriteAtlas)] = new Dictionary<string, Object>(),

		[typeof(AudioClip)] = new Dictionary<string, Object>(),
		[typeof(AnimationClip)] = new Dictionary<string, Object>(),

		[typeof(TextAsset)] = new Dictionary<string, Object>(),
		[typeof(GameObject)] = new Dictionary<string, Object>(),
		[typeof(ScriptableObject)] = new Dictionary<string, Object>(),
		[typeof(RuntimeAnimatorController)] = new Dictionary<string, Object>(),

#if POST_PROCESSING_MODULE_ENABLE
		[typeof(PostProcessProfile)] = new Dictionary<string, Object>()
#endif // #if POST_PROCESSING_MODULE_ENABLE
	};

	private Dictionary<System.Type, System.Func<string, Object>> m_oResCreatorDict = new Dictionary<System.Type, System.Func<string, Object>>() {
		[typeof(Shader)] = Shader.Find,

		[typeof(Font)] = Resources.Load<Font>,
		[typeof(Mesh)] = Resources.Load<Mesh>,
		[typeof(TMP_FontAsset)] = Resources.Load<TMP_FontAsset>,

		[typeof(Sprite)] = Resources.Load<Sprite>,
		[typeof(Texture)] = Resources.Load<Texture>,
		[typeof(Material)] = Resources.Load<Material>,
		[typeof(SpriteAtlas)] = Resources.Load<SpriteAtlas>,

		[typeof(AudioClip)] = Resources.Load<AudioClip>,
		[typeof(AnimationClip)] = Resources.Load<AnimationClip>,

		[typeof(TextAsset)] = Resources.Load<TextAsset>,
		[typeof(GameObject)] = Resources.Load<GameObject>,
		[typeof(ScriptableObject)] = Resources.Load<ScriptableObject>,
		[typeof(RuntimeAnimatorController)] = Resources.Load<RuntimeAnimatorController>,

#if POST_PROCESSING_MODULE_ENABLE
		[typeof(PostProcessProfile)] = Resources.Load<PostProcessProfile>
#endif // #if POST_PROCESSING_MODULE_ENABLE
	};
	#endregion // 변수

	#region 프로퍼티
	public List<System.Type> ResTypeList => m_oResDictContainer.Keys.ToList();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetupDefResources();
	}

	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();
		m_oResDictContainer.Clear();
	}

	/** 스프라이트 아틀라스를 로드한다 */
	public void LoadSpriteAtlas(string a_oFilePath, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oFilePath.ExIsValid());
		var oSpriteAtlas = this.GetRes<SpriteAtlas>(a_oFilePath, a_bIsAutoCreate);

		// 스프라이트 아틀라스가 존재 할 경우
		if(oSpriteAtlas != null && oSpriteAtlas.spriteCount > KCDefine.B_VAL_0_INT) {
			var oSprites = new Sprite[oSpriteAtlas.spriteCount];
			oSpriteAtlas.GetSprites(oSprites);

			for(int i = 0; i < oSprites.Length; ++i) {
				this.AddRes<Sprite>(oSprites[i].name.Replace(KCDefine.U_IMG_N_CLONE_SPRITE, string.Empty), oSprites[i]);
			}
		}
	}

	/** 기본 리소스를 설정한다 */
	private void SetupDefResources() {
		// 메쉬를 설정한다 {
		var oVertexList = new List<Vector3>() {
			(Vector3.down + Vector3.left) / KCDefine.B_VAL_2_REAL, (Vector3.up + Vector3.left) / KCDefine.B_VAL_2_REAL, (Vector3.up + Vector3.right) / KCDefine.B_VAL_2_REAL, (Vector3.down + Vector3.right) / KCDefine.B_VAL_2_REAL
		};

		var oIdxList = new List<int>() {
			KCDefine.B_VAL_0_INT, KCDefine.B_VAL_1_INT, KCDefine.B_VAL_2_INT, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_2_INT, KCDefine.B_VAL_3_INT
		};

		var oNormalList = new List<Vector3>() {
			Vector3.back, Vector3.back, Vector3.back, Vector3.back
		};

		var oUVList = new List<Vector2>() {
			KCDefine.B_ANCHOR_DOWN_LEFT, KCDefine.B_ANCHOR_UP_LEFT, KCDefine.B_ANCHOR_UP_RIGHT, KCDefine.B_ANCHOR_DOWN_RIGHT
		};

		var oMesh = CFactory.MakeMesh(KCDefine.U_MESH_P_DEF, oVertexList, oIdxList, oNormalList, oUVList);
		this.AddRes<Mesh>(KCDefine.U_MESH_P_DEF, oMesh);
		// 메쉬를 설정한다 }

		// 스프라이트를 설정한다
		var oSprite = CFactory.MakeSprite(KCDefine.U_IMG_P_DEF, Texture2D.whiteTexture, Rect.MinMaxRect(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL), KCDefine.B_ANCHOR_MID_CENTER);
		this.AddRes<Sprite>(KCDefine.U_IMG_P_DEF, oSprite);
	}
	#endregion // 함수

	#region 제네릭 함수
	/** 리소스를 반환한다 */
	public T GetRes<T>(string a_oKey, bool a_bIsAutoCreate = true) where T : Object {
		CAccess.Assert(a_oKey.ExIsValid() && m_oResDictContainer.ContainsKey(typeof(T)));
		var oResDict = m_oResDictContainer[typeof(T)];

		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !oResDict.ContainsKey(a_oKey)) {
			oResDict.TryAdd(a_oKey, m_oResCreatorDict.GetValueOrDefault(typeof(T))(a_oKey) as T);
		}

		return oResDict.GetValueOrDefault(a_oKey, default(T)) as T;
	}

	/** 스크립트 객체를 반환한다 */
	public T GetScriptableObj<T>(string a_oKey, bool a_bIsAutoCreate = true) where T : ScriptableObject {
		CAccess.Assert(a_oKey.ExIsValid());
		return this.GetRes<ScriptableObject>(a_oKey, a_bIsAutoCreate) as T;
	}

	/** 리소스를 추가한다 */
	public void AddRes<T>(string a_oKey, T a_tRes, bool a_bIsReplace = false, bool a_bIsEnableAssert = true) where T : Object {
		CAccess.Assert(!a_bIsEnableAssert || (a_tRes != null && a_oKey.ExIsValid() && m_oResDictContainer.ContainsKey(typeof(T))));

		// 리소스 추가가 가능 할 경우
		if(a_tRes != null && (a_bIsReplace || !m_oResDictContainer[typeof(T)].ContainsKey(a_oKey))) {
			m_oResDictContainer[typeof(T)].ExReplaceVal(a_oKey, a_tRes);
		}
	}

	/** 리소스 생성자를 추가한다 */
	public void AddResCreator<T>(System.Func<string, Object> a_oCreator, bool a_bIsReplace = false, bool a_bIsEnableAssert = true) where T : Object {
		CAccess.Assert(!a_bIsEnableAssert || a_oCreator != null);

		// 리소스 생성자 추가가 가능 할 경우
		if(a_oCreator != null && (a_bIsReplace || !m_oResCreatorDict.ContainsKey(typeof(T)))) {
			m_oResCreatorDict.ExReplaceVal(typeof(T), a_oCreator);
		}
	}

	/** 리소스를 제거한다 */
	public void RemoveRes<T>(string a_oKey, bool a_bIsEnableAssert = true) where T : Object {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKey.ExIsValid() && m_oResDictContainer.ContainsKey(typeof(T))));

		// 리소스가 존재 할 경우
		if(a_oKey.ExIsValid() && m_oResDictContainer.TryGetValue(typeof(T), out Dictionary<string, Object> oResDict)) {
			oResDict.ExRemoveVal(a_oKey, a_bIsEnableAssert);
		}
	}

	/** 리소스를 제거한다 */
	public void RemoveResources<T>(bool a_bIsEnableAssert = true) where T : Object {
		CAccess.Assert(!a_bIsEnableAssert || m_oResDictContainer.ContainsKey(typeof(T)));
		m_oResDictContainer.ExRemoveVal(typeof(T), a_bIsEnableAssert);
	}

	/** 리소스 생성자를 제거한다 */
	public void RemoveResCreator<T>(bool a_bIsEnableAssert = true) where T : Object {
		CAccess.Assert(!a_bIsEnableAssert || m_oResCreatorDict.ContainsKey(typeof(T)));
		m_oResCreatorDict.ExRemoveVal(typeof(T), a_bIsEnableAssert);
	}

	/** 리소스를 로드한다 */
	public List<T> LoadResources<T>(string a_oFilePath) where T : Object {
		CAccess.Assert(a_oFilePath.ExIsValid());
		var oResources = Resources.LoadAll<T>(a_oFilePath);

		for(int i = 0; i < oResources.Length; ++i) {
			this.AddRes<T>(oResources[i].name, oResources[i]);
		}

		return oResources.ToList();
	}
	#endregion // 제네릭 함수
}

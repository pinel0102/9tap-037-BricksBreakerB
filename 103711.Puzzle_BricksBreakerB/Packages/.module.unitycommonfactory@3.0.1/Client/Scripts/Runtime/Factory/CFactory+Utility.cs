using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;
using DG.Tweening.Core;
using EnhancedUI.EnhancedScroller;
using AOP;

/** 유틸리티 팩토리 */
public static partial class CFactory {
	#region 클래스 함수
	/** 고유 레벨 식별자를 생성한다 */
	public static ulong MakeULevelID(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return CFactory.MakeUStageID(a_nStageID, a_nChapterID) + (ulong)a_nLevelID;
	}

	/** 고유 스테이지 식별자를 생성한다 */
	public static ulong MakeUStageID(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return CFactory.MakeUChapterID(a_nChapterID) + ((ulong)a_nStageID * KCDefine.B_UNIT_IDS_PER_IDS_02);
	}

	/** 고유 챕터 식별자를 생성한다 */
	public static ulong MakeUChapterID(int a_nChapterID) {
		return (ulong)a_nChapterID * KCDefine.B_UNIT_IDS_PER_IDS_03;
	}

	/** 경로 정보를 생성한다 */
	public static CPathInfo MakePathInfo(Vector3Int a_stIdx, int a_nCost = KCDefine.B_VAL_0_INT) {
		return new CPathInfo() {
			m_nCost = a_nCost, m_stIdx = a_stIdx, m_oPrevPathInfo = null
		};
	}

	/** 그래디언트를 생성한다 */
	public static Gradient MakeGradient(Color a_stColor) {
		return new Gradient() {
			colorKeys = new GradientColorKey[] {
				new GradientColorKey(a_stColor, KCDefine.B_VAL_0_REAL), new GradientColorKey(a_stColor, KCDefine.B_VAL_1_REAL)
			},

			alphaKeys = new GradientAlphaKey[] {
				new GradientAlphaKey(a_stColor.a, KCDefine.B_VAL_0_REAL), new GradientAlphaKey(a_stColor.a, KCDefine.B_VAL_1_REAL)
			}
		};
	}

	/** 애니메이션을 생성한다 */
	public static DG.Tweening.Tween MakeAni(DOGetter<float> a_oGetter, DOSetter<float> a_oSetter, System.Action a_oInitCallback, System.Action<float> a_oSetterCallback, float a_fVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oGetter != null && a_oSetter != null);
		a_oInitCallback?.Invoke();

		return DOTween.To(a_oGetter, (a_fVal) => { a_oSetter(a_fVal); a_oSetterCallback?.Invoke(a_fVal); }, a_fVal, a_fDuration).SetAutoKill().SetEase(a_eEase).SetUpdate(a_bIsRealtime);
	}

	/** 시퀀스를 생성한다 */
	public static Sequence MakeSequence(DG.Tweening.Tween a_oAni, System.Action<Sequence> a_oCallback, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsJoin = false, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oAni != null);

		return CFactory.MakeSequence(new List<DG.Tweening.Tween>() {
			a_oAni
		}, a_oCallback, a_fDelay, a_bIsJoin, a_bIsRealtime);
	}

	/** 시퀀스를 생성한다 */
	public static Sequence MakeSequence(List<DG.Tweening.Tween> a_oAniList, System.Action<Sequence> a_oCallback, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsJoin = false, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oAniList != null);
		var oAni = DOTween.Sequence().SetAutoKill().SetUpdate(a_bIsRealtime);

		for(int i = 0; i < a_oAniList.Count; ++i) {
			// 조인 모드 일 경우
			if(a_bIsJoin) {
				oAni.Join(a_oAniList[i]);
			} else {
				oAni.Append(a_oAniList[i]);
			}
		}

		var oSequence = DOTween.Sequence().SetAutoKill().SetDelay(a_fDelay).SetUpdate(a_bIsRealtime).Append(oAni);
		return oSequence.AppendCallback(() => a_oCallback?.Invoke(oSequence));
	}

	/** 메쉬를 생성한다 */
	public static Mesh MakeMesh(string a_oName, List<Vector3> a_oVertexList, List<int> a_oIdxList, List<Vector3> a_oNormalList, List<Vector2> a_oUVList, MeshTopology a_eTopology = MeshTopology.Triangles) {
		var oMesh = new Mesh();
		oMesh.name = a_oName;

		oMesh.SetVertices(a_oVertexList);
		oMesh.SetIndices(a_oIdxList, MeshTopology.Triangles, KCDefine.B_VAL_0_INT);
		oMesh.SetNormals(a_oNormalList);
		oMesh.SetUVs(KCDefine.B_VAL_0_INT, a_oUVList);

		return oMesh;
	}

	/** 텍스처를 생성한다 */
	public static Texture2D MakeTex2D(string a_oName, Vector3Int a_stSize, TextureFormat a_eFmt = TextureFormat.RGBA32, bool a_bIsEnableMipMap = true) {
		var oTex2D = new Texture2D(a_stSize.x, a_stSize.y, a_eFmt, a_bIsEnableMipMap);
		oTex2D.name = a_oName;

		return oTex2D;
	}

	/** 스프라이트를 생성한다 */
	public static Sprite MakeSprite(string a_oName, Texture2D a_oTex2D, Rect a_stRect, Vector3 a_stPivot, float a_fPixelsPerUnit = KCDefine.B_UNIT_PIXELS_PER_UNIT) {
		var oSprite = Sprite.Create(a_oTex2D, a_stRect, a_stPivot, a_fPixelsPerUnit);
		oSprite.name = a_oName;

		return oSprite;
	}

	/** 객체를 생선한다 */
	public static GameObject CreateObj(string a_oName, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid());
		return CFactory.CreateObj(a_oName, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 객체를 생선한다 */
	public static GameObject CreateObj(string a_oName, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid());
		return CFactory.CreateObj(a_oName, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 객체를 생선한다 */
	public static GameObject CreateObj(string a_oName, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid());

		var oObj = new GameObject(a_oName);
		oObj.transform.localScale = a_stScale;
		oObj.transform.localEulerAngles = a_stAngle;
		oObj.transform.localPosition = a_stPos;

		oObj.ExSetParent(a_oParent, a_bIsStayWorldState);
		return oObj;
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, string a_oObjPath, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, a_oObjPath, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, a_oObjPath, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stScale, a_stAngle, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, a_oOrigin, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, a_oOrigin, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static GameObject CreateCloneObj(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());

		var oObj = GameObject.Instantiate(a_oOrigin, a_oParent?.transform, a_bIsStayWorldState);
		oObj.name = a_oName;
		oObj.transform.localScale = a_stScale;
		oObj.transform.localEulerAngles = a_stAngle;
		oObj.transform.localPosition = a_stPos;

		return oObj;
	}

	/** 터치 응답자를 생성한다 */
	public static GameObject CreateTouchResponder(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateTouchResponder(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stSize, a_stPos, a_stColor);
	}

	/** 터치 응답자를 생성한다 */
	public static GameObject CreateTouchResponder(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());

		var oObj = CFactory.CreateCloneObj(a_oName, a_oOrigin, a_oParent);
		oObj.GetComponentInChildren<Image>().color = a_stColor;

		// 트랜스 폼이 존재 할 경우
		if(oObj.transform as RectTransform != null) {
			(oObj.transform as RectTransform).pivot = KCDefine.B_ANCHOR_MID_CENTER;
			(oObj.transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_MID_CENTER;
			(oObj.transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_MID_CENTER;
			(oObj.transform as RectTransform).sizeDelta = a_stSize;
			(oObj.transform as RectTransform).anchoredPosition = a_stPos;
		}

		return oObj;
	}

	/** 드래그 응답자를 생성한다 */
	public static GameObject CreateDragResponder(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateDragResponder(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stSize, a_stPos, a_stColor);
	}

	/** 드래그 응답자를 생성한다 */
	public static GameObject CreateDragResponder(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateTouchResponder(a_oName, a_oOrigin, a_oParent, a_stSize, a_stPos, a_stColor);
	}

	/** 객체 풀을 생성한다 */
	public static ObjectPool CreateObjsPool(string a_oObjPath, GameObject a_oParent, int a_nNumObjs = KCDefine.U_SIZE_OBJS_POOL_01) {
		CAccess.Assert(a_oObjPath.ExIsValid());
		return CFactory.CreateObjsPool(Resources.Load<GameObject>(a_oObjPath), a_oParent, a_nNumObjs);
	}

	/** 객체 풀을 생성한다 */
	public static ObjectPool CreateObjsPool(GameObject a_oOrigin, GameObject a_oParent, int a_nNumObjs = KCDefine.U_SIZE_OBJS_POOL_01) {
		CAccess.Assert(a_oOrigin != null);
		return new ObjectPool(a_oOrigin, a_oParent?.transform, a_nNumObjs);
	}

	/** 객체를 제거한다 */
	public static void RemoveObj(Object a_oObj, bool a_bIsRemoveAsset = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oObj != null);

		// 객체가 존재 할 경우
		if(a_oObj != null) {
			// 앱이 실행 중 일 경우
			if(Application.isPlaying) {
				GameObject.Destroy(a_oObj);
			} else {
				GameObject.DestroyImmediate(a_oObj, a_bIsRemoveAsset);
			}
		}
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction<bool>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction<int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction<float>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction<string>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, UnityAction<int, int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, GameObject, EnhancedScrollerCellView)> MakeKeyInfos<T>(List<(T, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject, EnhancedScrollerCellView)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction<bool>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction<int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction<float>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction<string>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, UnityAction<int, int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, EnhancedScrollerCellView)> MakeKeyInfos<T>(List<(T, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, EnhancedScrollerCellView)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction<bool>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction<int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction<float>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction<string>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, UnityAction<int, int>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject, EnhancedScrollerCellView)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject, EnhancedScrollerCellView)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4, a_oKeyInfoList[i].Item5));
		}

		return oKeyInfoList;
	}

	/** 객체를 생선한다 */
	public static T CreateObj<T>(string a_oName, GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid());
		return CFactory.CreateObj<T>(a_oName, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 객체를 생선한다 */
	public static T CreateObj<T>(string a_oName, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid());
		return CFactory.CreateObj<T>(a_oName, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 객체를 생선한다 */
	public static T CreateObj<T>(string a_oName, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid());
		return CFactory.CreateObj(a_oName, a_oParent, a_stScale, a_stAngle, a_stPos, a_bIsStayWorldState).ExAddComponent<T>();
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, string a_oObjPath, GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, a_oObjPath, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, a_oObjPath, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stScale, a_stAngle, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, a_oOrigin, a_oParent, Vector3.zero, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, a_oOrigin, a_oParent, Vector3.one, Vector3.zero, a_stPos, a_bIsStayWorldState);
	}

	/** 사본 객체를 생성한다 */
	public static T CreateCloneObj<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool a_bIsStayWorldState = false) where T : Component {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj(a_oName, a_oOrigin, a_oParent, a_stScale, a_stAngle, a_stPos, a_bIsStayWorldState)?.GetComponentInChildren<T>();
	}

	/** 터치 응답자를 생성한다 */
	public static T CreateTouchResponder<T>(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateTouchResponder<T>(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stSize, a_stPos, a_stColor);
	}

	/** 터치 응답자를 생성한다 */
	public static T CreateTouchResponder<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) where T : Component {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateTouchResponder(a_oName, a_oOrigin, a_oParent, a_stSize, a_stPos, a_stColor)?.GetComponentInChildren<T>();
	}

	/** 드래그 응답자를 생성한다 */
	public static T CreateDragResponder<T>(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CFactory.CreateDragResponder<T>(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent, a_stSize, a_stPos, a_stColor);
	}

	/** 드래그 응답자를 생성한다 */
	public static T CreateDragResponder<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stSize, Vector3 a_stPos, Color a_stColor) where T : Component {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateDragResponder(a_oName, a_oOrigin, a_oParent, a_stSize, a_stPos, a_stColor)?.GetComponentInChildren<T>();
	}

	/** 컴포넌트를 추가한다 */
	private static T ExAddComponent<T>(this GameObject a_oSender, bool a_bIsEnableAssert = true) where T : Component {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);
		return (a_oSender != null) ? a_oSender.TryGetComponent(out T oComponent) ? oComponent : a_oSender.AddComponent<T>() : null;
	}
	#endregion // 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	/** 유저 정보 노드를 생성한다 */
	public static List<string> MakeUserInfoNodes() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_USER_INFOS
		};
	}

	/** 타겟 정보 노드를 생성한다 */
	public static List<string> MakeTargetInfoNodes() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_TARGET_INFOS
		};
	}

	/** 결제 정보 노드를 생성한다 */
	public static List<string> MakePurchaseInfoNodes() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_PURCHASE_INFOS
		};
	}
#endif // #if FIREBASE_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}

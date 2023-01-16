using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using TMPro;
using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

/** 설정 함수 */
public static partial class CFunc {
	#region 클래스 함수
	/** 입력을 설정한다 */
	public static void SetupInputs(List<(GameObject, UnityAction<string>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<InputField>()?.onEndEdit.AddListener(a_oKeyInfoList[i].Item2);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<InputField>()?.onValueChanged.AddListener(a_oKeyInfoList[i].Item2);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupInputs(List<(string, GameObject, UnityAction<string>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<InputField>(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item3);
				a_oKeyInfoList[i].Item2?.ExFindComponent<InputField>(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupTMPInputs(List<(GameObject, UnityAction<string>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<TMP_InputField>()?.onEndEdit.AddListener(a_oKeyInfoList[i].Item2);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<TMP_InputField>()?.onValueChanged.AddListener(a_oKeyInfoList[i].Item2);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupTMPInputs(List<(string, GameObject, UnityAction<string>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<TMP_InputField>(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item3);
				a_oKeyInfoList[i].Item2?.ExFindComponent<TMP_InputField>(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 버튼을 설정한다 */
	public static void SetupButtons(List<(GameObject, UnityAction)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<Button>()?.onClick.AddListener(a_oKeyInfoList[i].Item2);
			}
		}
	}

	/** 버튼을 설정한다 */
	public static void SetupButtons(List<(string, GameObject, UnityAction)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<Button>(a_oKeyInfoList[i].Item1)?.onClick.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 스크롤 바를 설정한다 */
	public static void SetupScrollBars(List<(GameObject, UnityAction<float>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<Scrollbar>()?.onValueChanged.AddListener(a_oKeyInfoList[i].Item2);
			}
		}
	}

	/** 스크롤 바를 설정한다 */
	public static void SetupScrollBars(List<(string, GameObject, UnityAction<float>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<Scrollbar>(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 스프라이트를 설정한다 */
	public static void SetupSprites(List<GameObject> a_oKeyList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyList.ExIsValid()) {
			for(int i = 0; i < a_oKeyList.Count; ++i) {
				a_oKeyList[i]?.GetComponentInChildren<SpriteRenderer>()?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 스프라이트를 설정한다 */
	public static void SetupSprites(List<(string, GameObject)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<SpriteRenderer>(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 파티클 효과를 설정한다 */
	public static void SetupParticleFXs(List<GameObject> a_oKeyList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyList.ExIsValid()) {
			for(int i = 0; i < a_oKeyList.Count; ++i) {
				a_oKeyList[i]?.GetComponentInChildren<ParticleSystem>()?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 파티클 효과를 설정한다 */
	public static void SetupParticleFXs(List<(string, GameObject)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<ParticleSystem>(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 레이아웃 그룹을 설정한다 */
	public static void SetupLayoutGroups(List<GameObject> a_oKeyList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyList.ExIsValid()) {
			for(int i = 0; i < a_oKeyList.Count; ++i) {
				a_oKeyList[i]?.GetComponentInChildren<HorizontalOrVerticalLayoutGroup>()?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 레이아웃 그룹을 설정한다 */
	public static void SetupLayoutGroups(List<(string, GameObject)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<HorizontalOrVerticalLayoutGroup>(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 스크롤 스냅을 설정한다 */
	public static void SetupScrollSnaps(List<(GameObject, UnityAction<int, int>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<SimpleScrollSnap>()?.OnPanelCentered.AddListener(a_oKeyInfoList[i].Item2);
			}
		}
	}

	/** 스크롤 스냅을 설정한다 */
	public static void SetupScrollSnaps(List<(string, GameObject, UnityAction<int, int>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<SimpleScrollSnap>(a_oKeyInfoList[i].Item1)?.OnPanelCentered.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 스크롤러 정보를 설정한다 */
	public static void SetupScrollerInfos(List<(GameObject, IEnhancedScrollerDelegate)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<EnhancedScroller>()?.ExSetDelegate(a_oKeyInfoList[i].Item2, a_bIsEnableAssert);
			}
		}
	}

	/** 스크롤러 정보를 설정한다 */
	public static void SetupScrollerInfos(List<(string, GameObject, IEnhancedScrollerDelegate)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<EnhancedScroller>(a_oKeyInfoList[i].Item1)?.ExSetDelegate(a_oKeyInfoList[i].Item3, a_bIsEnableAssert);
			}
		}
	}

	/** 화면 UI 를 설정한다 */
	public static void SetupScreenUIs(GameObject a_oScreenUIs, int a_nSortingOrder, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oScreenUIs != null);

		// 객체가 존재 할 경우
		if(a_oScreenUIs != null) {
			var oCanvas = a_oScreenUIs.GetComponentInChildren<Canvas>();
			oCanvas.pixelPerfect = false;
			oCanvas.sortingOrder = a_nSortingOrder;
			oCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

			var oCanvasScaler = a_oScreenUIs.GetComponentInChildren<CanvasScaler>();
			oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			oCanvasScaler.referenceResolution = KCDefine.B_SCREEN_SIZE;
			oCanvasScaler.referencePixelsPerUnit = KCDefine.B_UNIT_REF_PIXELS_PER_UNIT;
		}
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V>(List<(K, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2?.GetComponentInChildren<V>());
			}
		}
	}

	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V>(List<(K, string, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item3?.ExFindComponent<V>(a_oKeyInfoList[i].Item2));
			}
		}
	}

	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				var oComponent = a_oKeyInfoList[i].Item3?.ExFindComponent<V>(a_oKeyInfoList[i].Item2);

				// 컴포넌트가 존재 할 경우
				if(oComponent != null) {
					a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, oComponent);
				} else {
					a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, (a_oKeyInfoList[i].Item4 == null) ? CFactory.CreateObj<V>(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3) : CFactory.CreateCloneObj<V>(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item4, a_oKeyInfoList[i].Item3));
				}
			}
		}
	}

	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V1, V2>(List<(K, GameObject, V2)> a_oKeyInfoList, Dictionary<K, (V1, V2)> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V1 : Component where V2 : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, (a_oKeyInfoList[i].Item2?.GetComponentInChildren<V1>(), a_oKeyInfoList[i].Item3));
			}
		}
	}

	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V1, V2>(List<(K, string, GameObject, V2)> a_oKeyInfoList, Dictionary<K, (V1, V2)> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V1 : Component where V2 : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, (a_oKeyInfoList[i].Item3?.ExFindComponent<V1>(a_oKeyInfoList[i].Item2), a_oKeyInfoList[i].Item4));
			}
		}
	}

	/** 컴포넌트를 설정한다 */
	public static void SetupComponents<K, V1, V2>(List<(K, string, GameObject, GameObject, V2)> a_oKeyInfoList, Dictionary<K, (V1, V2)> a_oOutComponentDict, bool a_bIsEnableAssert = true) where V1 : Component where V2 : Component {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutComponentDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				var oComponent = a_oKeyInfoList[i].Item3?.ExFindComponent<V1>(a_oKeyInfoList[i].Item2);

				// 컴포넌트가 존재 할 경우
				if(oComponent != null) {
					a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, (oComponent, a_oKeyInfoList[i].Item5));
				} else {
					a_oOutComponentDict.ExReplaceVal(a_oKeyInfoList[i].Item1, ((a_oKeyInfoList[i].Item4 == null) ? CFactory.CreateObj<V1>(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3) : CFactory.CreateCloneObj<V1>(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item4, a_oKeyInfoList[i].Item3), a_oKeyInfoList[i].Item5));
				}
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupInputs<K>(List<(K, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item3);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupInputs<K>(List<(K, string, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item4);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupInputs<K>(List<(K, string, GameObject, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item5);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupTMPInputs<K>(List<(K, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, TMP_InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item3);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupTMPInputs<K>(List<(K, string, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, TMP_InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item4);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 입력을 설정한다 */
	public static void SetupTMPInputs<K>(List<(K, string, GameObject, GameObject, UnityAction<string>)> a_oKeyInfoList, Dictionary<K, TMP_InputField> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onEndEdit.AddListener(a_oKeyInfoList[i].Item5);
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 스크롤 바를 설정한다 */
	public static void SetupScrollBars<K>(List<(K, GameObject, UnityAction<float>)> a_oKeyInfoList, Dictionary<K, Scrollbar> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 스크롤 바를 설정한다 */
	public static void SetupScrollBars<K>(List<(K, string, GameObject, UnityAction<float>)> a_oKeyInfoList, Dictionary<K, Scrollbar> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 스크롤 바를 설정한다 */
	public static void SetupScrollBars<K>(List<(K, string, GameObject, GameObject, UnityAction<float>)> a_oKeyInfoList, Dictionary<K, Scrollbar> a_oOutInputDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutInputDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutInputDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutInputDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onValueChanged.AddListener(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 버튼을 설정한다 */
	public static void SetupButtons<K>(List<(K, GameObject, UnityAction)> a_oKeyInfoList, Dictionary<K, Button> a_oOutBtnDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutBtnDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutBtnDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onClick.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 버튼을 설정한다 */
	public static void SetupButtons<K>(List<(K, string, GameObject, UnityAction)> a_oKeyInfoList, Dictionary<K, Button> a_oOutBtnDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutBtnDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutBtnDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onClick.AddListener(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 버튼을 설정한다 */
	public static void SetupButtons<K>(List<(K, string, GameObject, GameObject, UnityAction)> a_oKeyInfoList, Dictionary<K, Button> a_oOutBtnDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutBtnDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutBtnDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutBtnDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.onClick.AddListener(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 스프라이트를 설정한다 */
	public static void SetupSprites<K>(List<(K, GameObject)> a_oKeyInfoList, Dictionary<K, SpriteRenderer> a_oOutSpriteDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutSpriteDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutSpriteDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 스프라이트를 설정한다 */
	public static void SetupSprites<K>(List<(K, string, GameObject)> a_oKeyInfoList, Dictionary<K, SpriteRenderer> a_oOutSpriteDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutSpriteDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutSpriteDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 스프라이트를 설정한다 */
	public static void SetupSprites<K>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Dictionary<K, SpriteRenderer> a_oOutSpriteDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutSpriteDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutSpriteDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutSpriteDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 파티클 효과를 설정한다 */
	public static void SetupParticleFXs<K>(List<(K, GameObject)> a_oKeyInfoList, Dictionary<K, ParticleSystem> a_oOutParticleDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutParticleDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutParticleDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 파티클 효과를 설정한다 */
	public static void SetupParticleFXs<K>(List<(K, string, GameObject)> a_oKeyInfoList, Dictionary<K, ParticleSystem> a_oOutParticleDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutParticleDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutParticleDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 파티클 효과를 설정한다 */
	public static void SetupParticleFXs<K>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Dictionary<K, ParticleSystem> a_oOutParticleDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutParticleDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutParticleDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutParticleDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 레이아웃 그룹을 설정한다 */
	public static void SetupLayoutGroups<K, V>(List<(K, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutLayoutGroupDict, bool a_bIsEnableAssert = true) where V : HorizontalOrVerticalLayoutGroup {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutLayoutGroupDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutLayoutGroupDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 레이아웃 그룹을 설정한다 */
	public static void SetupLayoutGroups<K, V>(List<(K, string, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutLayoutGroupDict, bool a_bIsEnableAssert = true) where V : HorizontalOrVerticalLayoutGroup {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutLayoutGroupDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutLayoutGroupDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 레이아웃 그룹을 설정한다 */
	public static void SetupLayoutGroups<K, V>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Dictionary<K, V> a_oOutLayoutGroupDict, bool a_bIsEnableAssert = true) where V : HorizontalOrVerticalLayoutGroup {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutLayoutGroupDict != null) {
			CFunc.SetupComponents(a_oKeyInfoList, a_oOutLayoutGroupDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutLayoutGroupDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExReset(a_bIsEnableAssert);
			}
		}
	}

	/** 스크롤 스냅을 설정한다 */
	public static void SetupScrollSnaps<K>(List<(K, GameObject, UnityAction<int, int>)> a_oKeyInfoList, Dictionary<K, SimpleScrollSnap> a_oOutScrollSnapDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutScrollSnapDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollSnapDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.OnPanelCentered.AddListener(a_oKeyInfoList[i].Item3);
			}
		}
	}

	/** 스크롤 스냅을 설정한다 */
	public static void SetupScrollSnaps<K>(List<(K, string, GameObject, UnityAction<int, int>)> a_oKeyInfoList, Dictionary<K, SimpleScrollSnap> a_oOutScrollSnapDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutScrollSnapDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollSnapDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.OnPanelCentered.AddListener(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 스크롤 스냅을 설정한다 */
	public static void SetupScrollSnaps<K>(List<(K, string, GameObject, GameObject, UnityAction<int, int>)> a_oKeyInfoList, Dictionary<K, SimpleScrollSnap> a_oOutScrollSnapDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollSnapDict != null) {
			CFunc.SetupComponents(CFactory.MakeKeyInfos(a_oKeyInfoList), a_oOutScrollSnapDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollSnapDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.OnPanelCentered.AddListener(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 스크롤러 정보를 설정한다 */
	public static void SetupScrollerInfos<K>(List<(K, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList, Dictionary<K, STScrollerInfo> a_oOutScrollerInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null) {
			var oScrollerInfoDict = new Dictionary<K, (EnhancedScroller, EnhancedScrollerCellView)>();
			CFunc.SetupComponents<K, EnhancedScroller, EnhancedScrollerCellView>(CFactory.MakeKeyInfos(a_oKeyInfoList), oScrollerInfoDict, a_bIsEnableAssert);

			oScrollerInfoDict.ExCopyTo(a_oOutScrollerInfoDict, (a_stScrollerInfo) => new STScrollerInfo() {
				m_oScroller = a_stScrollerInfo.Item1, m_oScrollerCellView = a_stScrollerInfo.Item2
			}, false, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollerInfoDict.GetValueOrDefault(a_oKeyInfoList[i].Item1).m_oScroller?.ExSetDelegate(a_oKeyInfoList[i].Item4, a_bIsEnableAssert);
			}
		}
	}

	/** 스크롤러 정보를 설정한다 */
	public static void SetupScrollerInfos<K>(List<(K, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList, Dictionary<K, STScrollerInfo> a_oOutScrollerInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null) {
			var oScrollerInfoDict = new Dictionary<K, (EnhancedScroller, EnhancedScrollerCellView)>();
			CFunc.SetupComponents<K, EnhancedScroller, EnhancedScrollerCellView>(CFactory.MakeKeyInfos(a_oKeyInfoList), oScrollerInfoDict, a_bIsEnableAssert);

			oScrollerInfoDict.ExCopyTo(a_oOutScrollerInfoDict, (a_stScrollerInfo) => new STScrollerInfo() {
				m_oScroller = a_stScrollerInfo.Item1, m_oScrollerCellView = a_stScrollerInfo.Item2
			}, false, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollerInfoDict.GetValueOrDefault(a_oKeyInfoList[i].Item1).m_oScroller?.ExSetDelegate(a_oKeyInfoList[i].Item5, a_bIsEnableAssert);
			}
		}
	}

	/** 스크롤러 정보를 설정한다 */
	public static void SetupScrollerInfos<K>(List<(K, string, GameObject, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)> a_oKeyInfoList, Dictionary<K, STScrollerInfo> a_oOutScrollerInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutScrollerInfoDict != null) {
			var oScrollerInfoDict = new Dictionary<K, (EnhancedScroller, EnhancedScrollerCellView)>();
			CFunc.SetupComponents<K, EnhancedScroller, EnhancedScrollerCellView>(CFactory.MakeKeyInfos(a_oKeyInfoList), oScrollerInfoDict, a_bIsEnableAssert);

			oScrollerInfoDict.ExCopyTo(a_oOutScrollerInfoDict, (a_stScrollerInfo) => new STScrollerInfo() {
				m_oScroller = a_stScrollerInfo.Item1, m_oScrollerCellView = a_stScrollerInfo.Item2
			}, false, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutScrollerInfoDict.GetValueOrDefault(a_oKeyInfoList[i].Item1).m_oScroller?.ExSetDelegate(a_oKeyInfoList[i].Item6, a_bIsEnableAssert);
			}
		}
	}

	/** 객체를 설정한다 */
	public static void SetupObjs<K>(List<(K, string, GameObject)> a_oKeyInfoList, Dictionary<K, GameObject> a_oOutObjDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutObjDict.ExReplaceVal(a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item3?.ExFindChild(a_oKeyInfoList[i].Item2));
			}
		}
	}

	/** 객체를 설정한다 */
	public static void SetupObjs<K>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Dictionary<K, GameObject> a_oOutObjDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				var oObj = a_oKeyInfoList[i].Item3?.ExFindChild(a_oKeyInfoList[i].Item2);

				// 객체가 존재 할 경우
				if(oObj != null) {
					a_oOutObjDict.ExReplaceVal(a_oKeyInfoList[i].Item1, oObj);
				} else {
					a_oOutObjDict.ExReplaceVal(a_oKeyInfoList[i].Item1, (a_oKeyInfoList[i].Item4 == null) ? CFactory.CreateObj(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3) : CFactory.CreateCloneObj(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item4, a_oKeyInfoList[i].Item3));
				}
			}
		}
	}

	/** 터치 응답자를 설정한다 */
	public static void SetupTouchResponders<K>(List<(K, string, GameObject, GameObject)> a_oKeyInfoList, Vector3 a_stSize, Dictionary<K, GameObject> a_oOutObjDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutObjDict != null) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				var oTouchResponder = a_oKeyInfoList[i].Item3?.ExFindChild(a_oKeyInfoList[i].Item2);

				// 터치 응답자가 존재 할 경우
				if(oTouchResponder != null) {
					a_oOutObjDict.ExReplaceVal(a_oKeyInfoList[i].Item1, oTouchResponder);
				} else {
					a_oOutObjDict.ExReplaceVal(a_oKeyInfoList[i].Item1, CFactory.CreateTouchResponder(a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item4, a_oKeyInfoList[i].Item3, a_stSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT));
				}

				a_oOutObjDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.ExSetRaycastTarget<Image>(true, a_bIsEnableAssert);
			}
		}
	}
	#endregion // 제네릭 클래스 함수
}

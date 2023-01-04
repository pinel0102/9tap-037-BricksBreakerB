using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif // #if UNITY_IOS

/** 에디터 기본 접근 확장 */
public static partial class CEditorExtension {
	#region 클래스 함수
	/** 상태를 리셋한다 */
	public static void ExReset(this LightProbeGroup a_oSender, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 라인 효과가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.probePositions = new Vector3[] {
				new Vector3(-KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL),
				new Vector3(-KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL),
				new Vector3(KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL),
				new Vector3(KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL),

				new Vector3(-KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL),
				new Vector3(-KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL),
				new Vector3(KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL),
				new Vector3(KCDefine.B_VAL_1_REAL, -KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL)
			};
		}
	}

	/** 픽셀을 변경한다 */
	public static void ExSetPixels(this Texture2D a_oSender, Color a_stColor, int a_nMipLevel = KCDefine.B_VAL_0_INT, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 텍스처가 존재 할 경우
		if(a_oSender != null) {
			for(int i = 0; i < a_oSender.height; ++i) {
				for(int j = 0; j < a_oSender.width; ++j) {
					a_oSender.SetPixel(j, i, a_stColor);
				}
			}

			a_oSender.Apply();
		}
	}

	/** 픽셀을 변경한다 */
	public static void ExSetPixels(this Texture2D a_oSender, List<Color> a_oColorList, int a_nMipLevel = KCDefine.B_VAL_0_INT, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_oColorList != null));

		// 텍스처가 존재 할 경우
		if(a_oSender != null && a_oColorList != null) {
			a_oSender.SetPixels(a_oColorList.ToArray(), a_nMipLevel);
			a_oSender.Apply();
		}
	}

	/** 직렬화 프로퍼티 값을 변경한다 */
	public static void ExSetPropertyVal(this SerializedObject a_oSender, string a_oName, System.Action<SerializedProperty> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_oName.ExIsValid()));

		// 객체가 존재 할 경우
		if(a_oSender != null && a_oName.ExIsValid()) {
			try {
				a_oCallback?.Invoke(a_oSender.FindProperty(a_oName));
			} finally {
				a_oSender.ApplyModifiedProperties();
				a_oSender.Update();
			}
		}
	}
	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_IOS
	/** 문자열을 추가한다 */
	public static void ExAddStr(this PlistElementArray a_oSender, string a_oStr) {
		CAccess.Assert(a_oSender != null && a_oStr.ExIsValid());

		// 문자열 추가가 가능 할 경우
		if(a_oSender != null && !a_oSender.ExIsContains(a_oStr)) {
			a_oSender.AddString(a_oStr);
		}
	}
#endif // #if UNITY_IOS
	#endregion // 조건부 클래스 함수
}
#endif // #if UNITY_EDITOR

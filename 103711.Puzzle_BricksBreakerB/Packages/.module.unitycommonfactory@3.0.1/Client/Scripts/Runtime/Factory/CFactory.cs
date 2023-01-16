using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 기본 팩토리 */
public static partial class CFactory {
	#region 클래스 함수
	/** 지역화 경로를 생성한다 */
	public static string MakeLocalizePath(string a_oBasePath, string a_oLanguage) {
		CAccess.Assert(a_oBasePath.ExIsValid() && a_oLanguage.ExIsValid());
		return a_oBasePath.ExGetReplaceFileNamePath(string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, a_oBasePath.ExGetFileName(false), a_oLanguage));
	}

	/** 지역화 경로를 생성한다 */
	public static string MakeLocalizePath(string a_oBasePath, string a_oDefLocalizePath, string a_oCountryCode, string a_oLanguage) {
		CAccess.Assert(a_oBasePath.ExIsValid() && a_oDefLocalizePath.ExIsValid() && a_oCountryCode.ExIsValid());
		string oFilePath = a_oLanguage.ExIsValidLanguage() ? CFactory.MakeLocalizePath(a_oBasePath, a_oLanguage) : CFactory.MakeLocalizePath(a_oBasePath, a_oCountryCode);

		return CAccess.IsExistsRes<TextAsset>(oFilePath, true) ? oFilePath : a_oDefLocalizePath;
	}

	/** 정수 값을 생성한다 */
	public static List<int> MakeIntVals(int a_nMin, int a_nNumVals) {
		CAccess.Assert(a_nNumVals > KCDefine.B_VAL_0_INT);
		return CFactory.MakeVals<int>(a_nNumVals, (a_nIdx) => a_nMin + a_nIdx);
	}

	/** 정수 재배치 값을 생성한다 */
	public static List<int> MakeIntShuffleVals(int a_nMin, int a_nNumVals) {
		CAccess.Assert(a_nNumVals > KCDefine.B_VAL_0_INT);
		return CFactory.MakeShuffleVals<int>(a_nNumVals, (a_nIdx) => a_nMin + a_nIdx);
	}

	/** 디렉토리를 생성한다 */
	public static DirectoryInfo MakeDirectories(string a_oDirPath) {
		CAccess.Assert(a_oDirPath.ExIsValid());

		// 디렉토리가 없을 경우
		if(!Directory.Exists(a_oDirPath)) {
			Directory.CreateDirectory(a_oDirPath);
		}

		return new DirectoryInfo(a_oDirPath);
	}

	/** 대기 객체를 생성한다 */
	public static IEnumerator CoCreateWaitForSecs(float a_fDeltaTime, bool a_bIsRealtime = false) {
		CAccess.Assert(a_fDeltaTime.ExIsGreateEquals(KCDefine.B_VAL_0_REAL));

		// 리얼 타임 모드 일 경우
		if(a_bIsRealtime) {
			yield return new WaitForSecondsRealtime(a_fDeltaTime);
		} else {
			yield return new WaitForSeconds(a_fDeltaTime);
		}
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 값을 생성한다 */
	public static List<T> MakeVals<T>(int a_nNumVals, System.Func<int, T> a_oCallback) {
		CAccess.Assert(a_oCallback != null && a_nNumVals > KCDefine.B_VAL_0_INT);
		var oValList = new List<T>();

		for(int i = 0; i < a_nNumVals; ++i) {
			oValList.Add(a_oCallback.Invoke(i));
		}

		return oValList;
	}

	/** 재배치 값을 생성한다 */
	public static List<T> MakeShuffleVals<T>(int a_nNumVals, System.Func<int, T> a_oCallback) {
		CAccess.Assert(a_oCallback != null && a_nNumVals > KCDefine.B_VAL_0_INT);

		var oValList = CFactory.MakeVals<T>(a_nNumVals, a_oCallback);
		oValList.ExShuffle();

		return oValList;
	}

	/** 값을 교환한다 */
	private static void ExSwap<T>(this List<T> a_oSender, int a_nIdx01, int a_nIdx02, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_oSender.ExIsValidIdx(a_nIdx01) && a_oSender.ExIsValidIdx(a_nIdx02)));

		// 인덱스가 유효 할 경우
		if(a_oSender != null && (a_oSender.ExIsValidIdx(a_nIdx01) && a_oSender.ExIsValidIdx(a_nIdx02))) {
			T tTemp = a_oSender[a_nIdx01];
			a_oSender[a_nIdx01] = a_oSender[a_nIdx02];
			a_oSender[a_nIdx02] = tTemp;
		}
	}

	/** 값을 재배치한다 */
	private static void ExShuffle<T>(this List<T> a_oSender, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 값 재배치가 가능 할 경우
		if(a_oSender != null) {
			for(int i = 0; i < a_oSender.Count; ++i) {
				a_oSender.ExSwap(i, Random.Range(KCDefine.B_VAL_0_INT, a_oSender.Count), a_bIsEnableAssert);
			}
		}
	}
	#endregion // 제네릭 클래스 함수
}

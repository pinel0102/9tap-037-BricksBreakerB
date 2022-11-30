using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 기본 접근자 */
public static partial class CAccess {
	#region 클래스 프로퍼티
	public static bool IsSupportMSAA => SystemInfo.supportsMultisampledTextures > KCDefine.B_VAL_0_INT;
	public static string MidnightDeltaTimeStr => new System.DateTime(CAccess.MidnightDeltaTime.Ticks).ToString(KCDefine.B_DATE_T_FMT_HH_MM_SS);

	public static System.DateTime MidnightTime => System.DateTime.Today.AddDays(KCDefine.B_VAL_1_REAL);
	public static System.TimeSpan MidnightDeltaTime => CAccess.MidnightTime - System.DateTime.Now;
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 유저 문자열을 반환한다 */
	public static string GetUserStr(EUserType a_eUserType) {
		// 유저 타입이 유효하지 않을 경우
		if(!a_eUserType.ExIsValid()) {
			return KCDefine.B_TOKEN_USER_UNKNOWN;
		}

		return (a_eUserType == EUserType.A) ? KCDefine.B_TOKEN_USER_A : KCDefine.B_TOKEN_USER_B;
	}

	/** 버전 문자열을 반환한다 */
	public static string GetVerStr(string a_oVer, EUserType a_eUserType) {
		string oUserStr = CAccess.GetUserStr(a_eUserType);
		return string.Format(KCDefine.B_TEXT_FMT_VER, a_oVer, oUserStr);
	}

	/** 읽기용 스트림을 반환한다 */
	public static FileStream GetReadStream(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return File.Exists(a_oFilePath) ? File.Open(a_oFilePath, FileMode.Open, FileAccess.Read) : null;
	}

	/** 쓰기용 스트림을 반환한다 */
	public static FileStream GetWriteStream(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		string oDirPath = Path.GetDirectoryName(a_oFilePath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);

		// 디렉토리가 없을 경우
		if(oDirPath.ExIsValid() && !Directory.Exists(oDirPath)) {
			Directory.CreateDirectory(oDirPath);
		}

		return File.Open(a_oFilePath, FileMode.Create, FileAccess.Write);
	}

	/** 랜덤 확률을 반환한다 */
	public static (int, float) GetRandPercent(List<float> a_oPercentList) {
		CAccess.Assert(a_oPercentList.ExIsValid());

		float fPercent = Random.Range(KCDefine.B_VAL_0_REAL, a_oPercentList.Sum((a_fPercent) => a_fPercent));
		float fComparePercent = KCDefine.B_VAL_0_REAL;

		for(int i = 0; i < a_oPercentList.Count - KCDefine.B_VAL_1_INT; ++i) {
			fComparePercent += Mathf.Abs(a_oPercentList[i]);

			// 확률을 만족 할 경우
			if(fPercent.ExIsLessEquals(fComparePercent) && !Mathf.Abs(a_oPercentList[i]).ExIsEquals(KCDefine.B_VAL_0_REAL)) {
				return (i, a_oPercentList[i]);
			}
		}

		return (a_oPercentList.Count - KCDefine.B_VAL_1_INT, a_oPercentList.LastOrDefault());
	}

	/** 조건을 검사한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void Assert(bool a_bIsTrue) {
		UnityEngine.Assertions.Assert.IsTrue(a_bIsTrue);
	}

	/** 값을 비교한다 */
	private static int ExCompare(this float a_fSender, float a_fRhs) {
		// 값이 동일 할 경우
		if(a_fSender.ExIsEquals(a_fRhs)) {
			return KCDefine.B_COMPARE_EQUALS;
		}

		return a_fSender.ExIsLess(a_fRhs) ? KCDefine.B_COMPARE_LESS : KCDefine.B_COMPARE_GREATE;
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 열거형 값을 반환한다 */
	public static List<T> GetEnumVals<T>() {
		return (System.Enum.GetValues(typeof(T)) as T[]).ToList();
	}

	/** 열거형 문자열을 반환한다 */
	public static List<string> GetEnumStrs<T>() {
		var oEnumStrList = new List<string>();
		var oEnumValList = CAccess.GetEnumVals<T>();

		for(int i = 0; i < oEnumValList.Count; ++i) {
			oEnumStrList.Add(oEnumValList[i].ToString());
		}

		return oEnumStrList;
	}
	#endregion // 제네릭 클래스 함수
}

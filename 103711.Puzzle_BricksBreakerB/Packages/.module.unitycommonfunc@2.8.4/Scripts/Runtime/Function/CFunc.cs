using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MessagePack;

/** 기본 함수 */
public static partial class CFunc {
	#region 변수
	private static Dictionary<LogType, System.Action<string>> m_oLogFuncDict = new Dictionary<LogType, System.Action<string>>() {
		[LogType.Log] = UnityEngine.Debug.Log,
		[LogType.Warning] = UnityEngine.Debug.LogWarning,
		[LogType.Error] = UnityEngine.Debug.LogError
	};
	#endregion // 변수

	#region 클래스 함수
	/** 파일을 복사한다 */
	public static void CopyFile(string a_oSrcPath, string a_oDestPath, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 파일 복사가 가능 할 경우
		if((bIsValid && File.Exists(a_oSrcPath)) && (a_bIsOverwrite || !File.Exists(a_oDestPath))) {
			var oBytes = CFunc.ReadBytes(a_oSrcPath, false);
			CFunc.WriteBytes(a_oDestPath, oBytes, false, null, a_bIsEnableAssert);
		}
	}

	/** 파일을 복사한다 */
	public static void CopyFile(string a_oSrcPath, string a_oDestPath, string a_oIgnoreToken, System.Text.Encoding a_oEncoding = null, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 파일 복사가 가능 할 경우
		if((bIsValid && File.Exists(a_oSrcPath)) && (a_bIsOverwrite || !File.Exists(a_oDestPath))) {
			var oStrLines = CFunc.ReadStrLines(a_oSrcPath, a_oEncoding ?? System.Text.Encoding.Default);
			var oStrBuilder = new System.Text.StringBuilder();

			for(int i = 0; i < oStrLines.Length; ++i) {
				// 문자열이 유효 할 경우
				if(oStrLines[i] != null && !oStrLines[i].Contains(a_oIgnoreToken)) {
					oStrBuilder.AppendLine(oStrLines[i]);
				}
			}

			CFunc.WriteStr(a_oDestPath, oStrBuilder.ToString(), false, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}

	/** 파일을 복사한다 */
	public static void CopyFile(string a_oSrcPath, string a_oDestPath, string a_oTarget, string a_oReplace, System.Text.Encoding a_oEncoding = null, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 파일 복사가 가능 할 경우
		if((bIsValid && File.Exists(a_oSrcPath)) && (a_bIsOverwrite || !File.Exists(a_oDestPath))) {
			var oStrLines = CFunc.ReadStrLines(a_oSrcPath, a_oEncoding ?? System.Text.Encoding.Default);
			var oStrBuilder = new System.Text.StringBuilder();

			for(int i = 0; i < oStrLines.Length; ++i) {
				// 문자열이 유효 할 경우
				if(oStrLines[i] != null) {
					oStrBuilder.AppendLine(oStrLines[i].Replace(a_oTarget, a_oReplace));
				}
			}

			CFunc.WriteStr(a_oDestPath, oStrBuilder.ToString(), false, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}

	/** 디렉토리를 복사한다 */
	public static void CopyDir(string a_oSrcPath, string a_oDestPath, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 디렉토리 복사가 가능 할 경우
		if((bIsValid && Directory.Exists(a_oSrcPath)) && (a_bIsOverwrite || !Directory.Exists(a_oDestPath))) {
			CFactory.RemoveDir(a_oDestPath);

			CFunc.EnumerateDirectories(a_oSrcPath, (a_oDirPathList, a_oFilePathList) => {
				for(int i = 0; i < a_oFilePathList.Count; ++i) {
					string oDestFilePath = a_oFilePathList[i].Replace(a_oSrcPath, a_oDestPath);
					CFunc.CopyFile(a_oFilePathList[i], oDestFilePath, a_bIsOverwrite);
				}

				return true;
			});
		}
	}

	/** 디렉토리를 순회한다 */
	public static void EnumerateDirectories(string a_oDirPath, System.Func<List<string>, List<string>, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oCallback != null && a_oDirPath.ExIsValid()));
		bool bIsValid = a_oCallback != null && a_oDirPath.ExIsValid();

		// 디렉토리가 존재 할 경우
		if(bIsValid && Directory.Exists(a_oDirPath)) {
			var oDirPaths = Directory.GetDirectories(a_oDirPath);

			// 디렉토리 순회가 가능 할 경우
			if(a_oCallback(oDirPaths.ToList(), Directory.GetFiles(a_oDirPath).ToList())) {
				for(int i = 0; i < oDirPaths.Length; ++i) {
					CFunc.EnumerateDirectories(oDirPaths[i], a_oCallback);
				}
			}
		}
	}

	/** 바이트를 읽어들인다 */
	public static byte[] ReadBytes(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			var oBytes = File.ReadAllBytes(a_oFilePath);
			return a_bIsBase64 ? System.Convert.FromBase64String((a_oEncoding ?? System.Text.Encoding.Default).GetString(oBytes)) : oBytes;
		}

		return null;
	}

	/** 바이트를 읽어들인다 */
	public static byte[] ReadBytes(Stream a_oStream, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oStream != null);
		var oBytes = new byte[a_oStream.Length];

		a_oStream.Read(oBytes);
		return a_bIsBase64 ? System.Convert.FromBase64String((a_oEncoding ?? System.Text.Encoding.Default).GetString(oBytes)) : oBytes;
	}

	/** 바이트를 읽어들인다 */
	public static byte[] ReadBytesFromRes(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		var oTextAsset = Resources.Load<TextAsset>(a_oFilePath);

		// 텍스트 에셋이 존재 할 경우
		if(oTextAsset.ExIsValid()) {
			return a_bIsBase64 ? System.Convert.FromBase64String((a_oEncoding ?? System.Text.Encoding.Default).GetString(oTextAsset.bytes)) : oTextAsset.bytes;
		}

		return null;
	}

	/** 문자열을 읽어들인다 */
	public static string ReadStr(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			var oBytes = CFunc.ReadBytes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default);
			return a_bIsBase64 ? (a_oEncoding ?? System.Text.Encoding.Default).GetString(oBytes) : File.ReadAllText(a_oFilePath, a_oEncoding ?? System.Text.Encoding.Default);
		}

		return string.Empty;
	}

	/** 문자열을 읽어들인다 */
	public static string ReadStr(Stream a_oStream, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oStream != null);
		var oBytes = CFunc.ReadBytes(a_oStream, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default);

		return (a_oEncoding ?? System.Text.Encoding.Default).GetString(oBytes);
	}

	/** 문자열을 읽어들인다 */
	public static string ReadStrFromRes(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		var oTextAsset = Resources.Load<TextAsset>(a_oFilePath);

		// 텍스트 에셋이 존재 할 경우
		if(oTextAsset.ExIsValid()) {
			var oBytes = CFunc.ReadBytesFromRes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default);
			return a_bIsBase64 ? (a_oEncoding ?? System.Text.Encoding.Default).GetString(oBytes) : oTextAsset.text;
		}

		return string.Empty;
	}

	/** 문자열 라인을 읽어들인다 */
	public static string[] ReadStrLines(string a_oFilePath, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return File.Exists(a_oFilePath) ? File.ReadAllLines(a_oFilePath, a_oEncoding ?? System.Text.Encoding.Default) : null;
	}

	/** 바이트를 기록한다 */
	public static void WriteBytes(string a_oFilePath, byte[] a_oBytes, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oBytes != null && a_oFilePath.ExIsValid()));

		// 기록이 가능 할 경우
		if(a_oBytes != null && a_oFilePath.ExIsValid()) {
			using(var oWStream = CAccess.GetWriteStream(a_oFilePath)) {
				CFunc.WriteBytes(oWStream, a_oBytes, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
			}
		}
	}

	/** 바이트를 기록한다 */
	public static void WriteBytes(FileStream a_oWStream, byte[] a_oBytes, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oWStream != null && a_oBytes != null));

		// 스트림이 존재 할 경우
		if(a_oWStream != null && a_oBytes != null) {
			try {
				string oBase64Str = System.Convert.ToBase64String(a_oBytes, KCDefine.B_VAL_0_INT, a_oBytes.Length);
				a_oWStream.Write(a_bIsBase64 ? (a_oEncoding ?? System.Text.Encoding.Default).GetBytes(oBase64Str) : a_oBytes);
			} finally {
				a_oWStream.Flush(true);
			}
		}
	}

	/** 문자열을 기록한다 */
	public static void WriteStr(string a_oFilePath, string a_oStr, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oStr != null && a_oFilePath.ExIsValid()));

		// 기록이 가능 할 경우
		if(a_oStr != null && a_oFilePath.ExIsValid()) {
			using(var oWStream = CAccess.GetWriteStream(a_oFilePath)) {
				CFunc.WriteStr(oWStream, a_oStr, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
			}
		}
	}

	/** 문자열을 기록한다 */
	public static void WriteStr(FileStream a_oWStream, string a_oStr, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oWStream != null && a_oStr != null));

		// 스트림이 존재 할 경우
		if(a_oWStream != null && a_oStr != null) {
			CFunc.WriteBytes(a_oWStream, (a_oEncoding ?? System.Text.Encoding.Default).GetBytes(a_oStr), a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke(ref System.Action a_oAction) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke();
		}
	}

	/** 값을 교환한다 */
	public static void LessCorrectSwap(ref float a_fLhs, ref float a_fRhs) {
		// 보정이 필요 할 경우
		if(a_fLhs.ExIsGreate(a_fRhs)) {
			CFunc.Swap(ref a_fLhs, ref a_fRhs);
		}
	}

	/** 값을 교환한다 */
	public static void LessCorrectSwap(ref double a_dblLhs, ref double a_dblRhs) {
		// 보정이 필요 할 경우
		if(a_dblLhs.ExIsGreate(a_dblRhs)) {
			CFunc.Swap(ref a_dblLhs, ref a_dblRhs);
		}
	}

	/** 값을 교환한다 */
	public static void GreateCorrectSwap(ref float a_fLhs, ref float a_fRhs) {
		// 보정이 필요 할 경우
		if(a_fLhs.ExIsLess(a_fRhs)) {
			CFunc.Swap(ref a_fLhs, ref a_fRhs);
		}
	}

	/** 값을 교환한다 */
	public static void GreateCorrectSwap(ref double a_dblLhs, ref double a_dblRhs) {
		// 보정이 필요 할 경우
		if(a_dblLhs.ExIsLess(a_dblRhs)) {
			CFunc.Swap(ref a_dblLhs, ref a_dblRhs);
		}
	}

	/** 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLog(string a_oLog) {
		CAccess.Assert(a_oLog != null);
		CFunc.DoShowLog(LogType.Log, a_oLog);
	}

	/** 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLog(string a_oLog, Color a_stColor) {
		CAccess.Assert(a_oLog != null);
		CFunc.DoShowLog(LogType.Log, a_oLog.ExGetColorFmtStr(a_stColor));
	}

	/** 경고 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLogWarning(string a_oLog) {
		CFunc.DoShowLog(LogType.Warning, a_oLog);
	}

	/** 경고 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLogWarning(string a_oLog, Color a_stColor) {
		CAccess.Assert(a_oLog != null);
		CFunc.DoShowLog(LogType.Warning, a_oLog.ExGetColorFmtStr(a_stColor));
	}

	/** 에러 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLogError(string a_oLog) {
		CFunc.DoShowLog(LogType.Error, a_oLog);
	}

	/** 에러 로그를 출력한다 */
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLogError(string a_oLog, Color a_stColor) {
		CAccess.Assert(a_oLog != null);
		CFunc.DoShowLog(LogType.Error, a_oLog.ExGetColorFmtStr(a_stColor));
	}

	/** 로그를 출력한다 */
	private static void DoShowLog(LogType a_eLogType, string a_oLog) {
		bool bIsValid = CFunc.m_oLogFuncDict.TryGetValue(a_eLogType, out System.Action<string> oLogFunc);
		CAccess.Assert(bIsValid);

		oLogFunc?.Invoke(a_oLog);
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 값을 교환한다 */
	public static void Swap<T>(ref T a_tLhs, ref T a_tRhs) {
		T tTemp = a_tLhs;
		a_tLhs = a_tRhs;
		a_tRhs = tTemp;
	}

	/** 값을 교환한다 */
	public static void LessCorrectSwap<T>(ref T a_tLhs, ref T a_tRhs) where T : System.IComparable<T> {
		// 보정이 필요 할 경우
		if(a_tLhs.CompareTo(a_tRhs) > KCDefine.B_COMPARE_EQUALS) {
			CFunc.Swap(ref a_tLhs, ref a_tRhs);
		}
	}

	/** 값을 교환한다 */
	public static void GreateCorrectSwap<T>(ref T a_tLhs, ref T a_tRhs) where T : System.IComparable<T> {
		// 보정이 필요 할 경우
		if(a_tLhs.CompareTo(a_tRhs) < KCDefine.B_COMPARE_EQUALS) {
			CFunc.Swap(ref a_tLhs, ref a_tRhs);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01>(ref System.Action<T01> a_oAction, T01 a_tParams01) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02>(ref System.Action<T01, T02> a_oAction, T01 a_tParams01, T02 a_tParams02) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03>(ref System.Action<T01, T02, T03> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04>(ref System.Action<T01, T02, T03, T04> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04, T05>(ref System.Action<T01, T02, T03, T04, T05> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04, T05, T06>(ref System.Action<T01, T02, T03, T04, T05, T06> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04, T05, T06, T07>(ref System.Action<T01, T02, T03, T04, T05, T06, T07> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04, T05, T06, T07, T08>(ref System.Action<T01, T02, T03, T04, T05, T06, T07, T08> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07, T08 a_tParams08) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07, a_tParams08);
		}
	}

	/** 함수를 호출한다 */
	public static void Invoke<T01, T02, T03, T04, T05, T06, T07, T08, T09>(ref System.Action<T01, T02, T03, T04, T05, T06, T07, T08, T09> a_oAction, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07, T08 a_tParams08, T09 a_tParams09) {
		var oAction = a_oAction;

		try {
			a_oAction = null;
		} finally {
			oAction?.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07, a_tParams08, a_tParams09);
		}
	}

	/** 함수를 호출한다 */
	public static Result Invoke<Result>(ref System.Func<Result> a_oFunc) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke();
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, Result>(ref System.Func<T01, Result> a_oFunc, T01 a_tParams01) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, Result>(ref System.Func<T01, T02, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, Result>(ref System.Func<T01, T02, T03, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, Result>(ref System.Func<T01, T02, T03, T04, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, T05, Result>(ref System.Func<T01, T02, T03, T04, T05, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, T05, T06, Result>(ref System.Func<T01, T02, T03, T04, T05, T06, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, T05, T06, T07, Result>(ref System.Func<T01, T02, T03, T04, T05, T06, T07, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, T05, T06, T07, T08, Result>(ref System.Func<T01, T02, T03, T04, T05, T06, T07, T08, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07, T08 a_tParams08) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07, a_tParams08);
	}

	/** 함수를 호출한다 */
	public static Result Invoke<T01, T02, T03, T04, T05, T06, T07, T08, T09, Result>(ref System.Func<T01, T02, T03, T04, T05, T06, T07, T08, T09, Result> a_oFunc, T01 a_tParams01, T02 a_tParams02, T03 a_tParams03, T04 a_tParams04, T05 a_tParams05, T06 a_tParams06, T07 a_tParams07, T08 a_tParams08, T09 a_tParams09) {
		CAccess.Assert(a_oFunc != null);
		var oFunc = a_oFunc;

		try {
			a_oFunc = null;
		} finally {
			// Do Something
		}

		return oFunc.Invoke(a_tParams01, a_tParams02, a_tParams03, a_tParams04, a_tParams05, a_tParams06, a_tParams07, a_tParams08, a_tParams09);
	}

	/** 메세지 팩 객체를 읽어들인다 */
	public static T ReadMsgPackObj<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return MessagePackSerializer.Deserialize<T>(CFunc.ReadBytes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default));
	}

	/** 메세지 팩 객체를 읽어들인다 */
	public static T ReadMsgPackObjFromRes<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return MessagePackSerializer.Deserialize<T>(CFunc.ReadBytesFromRes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default));
	}

	/** 메세지 팩 JSON 객체를 읽어들인다 */
	public static T ReadMsgPackJSONObj<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadStr(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default).ExMsgPackJSONStrToObj<T>();
	}

	/** 메세지 팩 JSON 객체를 읽어들인다 */
	public static T ReadMsgPackJSONObjFromRes<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadStrFromRes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default).ExMsgPackJSONStrToObj<T>();
	}

	/** 메세지 팩 객체를 기록한다 */
	public static void WriteMsgPackObj<T>(string a_oFilePath, T a_oObj, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oFilePath.ExIsValid());

		// 경로가 유효 할 경우
		if(a_oFilePath.ExIsValid()) {
			CFunc.WriteBytes(a_oFilePath, MessagePackSerializer.Serialize<T>(a_oObj), a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}

	/** 메세지 팩 JSON 객체를 기록한다 */
	public static void WriteMsgPackJSONObj<T>(string a_oFilePath, T a_oObj, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oFilePath.ExIsValid());

		// 경로가 유효 할 경우
		if(a_oFilePath.ExIsValid()) {
			CFunc.WriteStr(a_oFilePath, a_oObj.ExToMsgPackJSONStr(), a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}
	#endregion // 제네릭 클래스 함수

	#region 조건부 제네릭 클래스 함수
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	/** JSON 객체를 읽어들인다 */
	public static T ReadJSONObj<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadStr(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default).ExJSONStrToObj<T>();
	}

	/** JSON 객체를 읽어들인다 */
	public static T ReadJSONObjFromRes<T>(string a_oFilePath, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadStrFromRes(a_oFilePath, a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default).ExJSONStrToObj<T>();
	}

	/** JSON 객체를 기록한다 */
	public static void WriteJSONObj<T>(string a_oFilePath, T a_oObj, bool a_bIsBase64, System.Text.Encoding a_oEncoding = null, bool a_bIsNeedsRoot = false, bool a_bIsPretty = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oFilePath.ExIsValid());

		// 경로가 유효 할 경우
		if(a_oFilePath.ExIsValid()) {
			CFunc.WriteStr(a_oFilePath, a_oObj.ExToJSONStr(a_bIsNeedsRoot, a_bIsPretty), a_bIsBase64, a_oEncoding ?? System.Text.Encoding.Default, a_bIsEnableAssert);
		}
	}
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	#endregion // 조건부 제네릭 클래스 함수
}

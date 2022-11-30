using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 기본 확장 클래스 */
public static partial class Extension {
#region 클래스 함수
	/** 타겟 정보 고유 식별자 => 종류로 변환한다 */
	public static int ExUniqueTargetInfoIDToKinds(this ulong a_nSender) {
		return (int)(a_nSender & uint.MaxValue);
	}

	/** 타겟 정보 고유 식별자 => 타겟 종류로 변환한다 */
	public static EAbilityKinds ExUniqueTargetInfoIDToTargetKinds(this ulong a_nSender) {
		return (EAbilityKinds)((a_nSender >> (sizeof(int) * KCDefine.B_UNIT_BITS_PER_BYTE)) & uint.MaxValue);
	}

	/** JSON 문자열 => 타겟 정보로 변환한다 */
	public static Dictionary<ulong, STTargetInfo> ExJSONStrToTargetInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		var oTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

#if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender);
		var oTargetInfos = oJSONNode[KCDefine.B_KEY_ROOT];

		for(int i = 0; i < oTargetInfos.Count; ++i) {
			var stTargetInfo = oTargetInfos[i].ToString().ExJSONStrToObj<STTargetInfo>();
			oTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
#endif // #if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE

		return oTargetInfoDict;
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

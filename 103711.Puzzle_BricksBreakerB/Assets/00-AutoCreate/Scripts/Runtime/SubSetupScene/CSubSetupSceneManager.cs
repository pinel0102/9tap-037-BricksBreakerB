using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
namespace SetupScene {
	/** 서브 설정 씬 관리자 */
	public partial class CSubSetupSceneManager : CSetupSceneManager {
		#region 변수
		[SerializeField] private SystemLanguage m_eSystemLanguage = SystemLanguage.Unknown;
		#endregion // 변수

		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 저장소를 로드한다
			CAppInfoStorage.Inst.LoadAppInfo();
            GlobalDefine.LoadUserData();
			CGameInfoStorage.Inst.LoadGameInfo();

			// 테이블을 로드한다
			CEtcInfoTable.Inst.LoadEtcInfos();
			CLevelInfoTable.Inst.LoadLevelInfos();

			CMissionInfoTable.Inst.LoadMissionInfos();
			CRewardInfoTable.Inst.LoadRewardInfos();
			CResInfoTable.Inst.LoadResInfos();

			CItemInfoTable.Inst.LoadItemInfos();
			CSkillInfoTable.Inst.LoadSkillInfos();
			CObjInfoTable.Inst.LoadObjInfos();
			CAbilityInfoTable.Inst.LoadAbilityInfos();
			CProductTradeInfoTable.Inst.LoadProductTradeInfos();

			// 공용 캐릭터 유저 정보가 없을 경우
			if(!CUserInfoStorage.Inst.TryGetCharacterUserInfo(KDefine.G_CHARACTER_ID_COMMON, out CCharacterUserInfo oCharacterUserInfo)) {
				oCharacterUserInfo = Factory.MakeCharacterUserInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01, new STIDInfo(KDefine.G_CHARACTER_ID_COMMON), STIdxInfo.INVALID);
				oCharacterUserInfo.m_oAbilityTargetInfoDict.ExReplaceTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, KCDefine.B_VAL_1_INT);

				CUserInfoStorage.Inst.AddCharacterUserInfo(oCharacterUserInfo);
                GlobalDefine.SaveUserData();
			}

			// 공용 앱 정보를 설정한다 {
			CCommonAppInfoStorage.Inst.SetStoreURL(Access.StoreURL);

#if ADS_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.SetAdsPlatform(CPluginInfoTable.Inst.AdsPlatform);
#endif // #if ADS_MODULE_ENABLE

#if LOCALIZE_TEST_ENABLE
			CCommonAppInfoStorage.Inst.SetSystemLanguage(m_eSystemLanguage);
#else
			CCommonAppInfoStorage.Inst.SetSystemLanguage(Application.systemLanguage);
#endif // #if LOCALIZE_TEST_ENABLE
			// 공용 앱 정보를 설정한다 }
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}
		#endregion // 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE

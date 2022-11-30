using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif // #if UNITY_IOS

/** 빌드 처리자 */
[InitializeOnLoad]
public static partial class CBuildProcessor {
#region 변수
	private static Dictionary<BuildTarget, System.Action<BuildTarget, string>> m_oPostProcessHandlerDict = new Dictionary<BuildTarget, System.Action<BuildTarget, string>>() {
		[BuildTarget.iOS] = CBuildProcessor.HandleiOSPostProcessBuild,
		[BuildTarget.Android] = CBuildProcessor.HandleAndroidPostProcessBuild,
		[BuildTarget.StandaloneOSX] = CBuildProcessor.HandleStandalonePostProcessBuild,
		[BuildTarget.StandaloneWindows] = CBuildProcessor.HandleStandalonePostProcessBuild,
		[BuildTarget.StandaloneWindows64] = CBuildProcessor.HandleStandalonePostProcessBuild
	};
#endregion // 변수

#region 클래스 함수
	/** 빌드가 완료 되었을 경우 */
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
		// 배치 모드가 아닐 경우
		if(!Application.isBatchMode) {
			EditorUtility.RevealInFinder(a_oPath);
		}

		CBuildProcessor.m_oPostProcessHandlerDict.GetValueOrDefault(a_eTarget)?.Invoke(a_eTarget, a_oPath);
	}

	/** 빌드가 완료 되었을 경우 */
	[PostProcessBuild(int.MaxValue)]
	public static void OnLatePostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPodsPath = string.Format(KCEditorDefine.B_DATA_P_FMT_COCOA_PODS, a_oPath);
		string oPlistPath = string.Format(KCEditorDefine.B_PLIST_P_FMT_IOS, a_oPath);
		string oPBXProjPath = string.Format(KCEditorDefine.B_PROJ_P_FMT_COCOA_PODS, a_oPath);

		// Plist 파일이 존재 할 경우
		if(File.Exists(oPlistPath)) {
			var oDoc = new PlistDocument();
			oDoc.ReadFromFile(oPlistPath);

			var oDeviceCapabilityList = oDoc.ExGetArray(KCEditorDefine.B_KEY_IOS_DEVICE_CAPABILITIES);
			oDeviceCapabilityList.ExAddStr(KCEditorDefine.B_TEXT_IOS_METAL);
			oDeviceCapabilityList.ExAddStr(KCEditorDefine.B_TEXT_IOS_ARM_64);

			oDoc.WriteToFile(oPlistPath);
		}

		// 프로젝트 파일이 존재 할 경우
		if(File.Exists(oPBXProjPath)) {
			var oPBXProj = new PBXProject();
			oPBXProj.ReadFromFile(oPBXProjPath);

			string oMainGUID = oPBXProj.GetUnityMainTargetGuid();
			string oFrameworkGUID = oPBXProj.GetUnityFrameworkTargetGuid();

			for(int i = 0; i < KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST.Count; ++i) {
				oPBXProj.RemoveFrameworkFromProject(oMainGUID, KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST[i]);
				oPBXProj.RemoveFrameworkFromProject(oFrameworkGUID, KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST[i]);
			}

			// 코코아 포드 파일이 존재 할 경우
			if(File.Exists(oPodsPath)) {
				CEditorFunc.ExecuteCmdLine(string.Format(KCEditorDefine.B_BUILD_CMD_FMT_IOS_COCOA_PODS, a_oPath), false);
				oPBXProj.AddBuildProperty(oPBXProj.ProjectGuid(), KCEditorDefine.B_PROPERTY_N_IOS_USER_HEADER_SEARCH_PATHS, KCEditorDefine.B_SEARCH_P_IOS_PODS);
				
				oPBXProj.SetBuildProperty(oPBXProj.ProjectGuid(), KCEditorDefine.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCEditorDefine.B_TEXT_IOS_FALSE);
				oPBXProj.SetBuildProperty(oPBXProj.ProjectGuid(), KCEditorDefine.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCEditorDefine.B_TEXT_IOS_FALSE);
			}

			oPBXProj.WriteToFile(oPBXProjPath);
		}
#endif // #if UNITY_IOS
	}

	/** iOS 빌드 완료를 처리한다 */
	private static void HandleiOSPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPlistPath = string.Format(KCEditorDefine.B_PLIST_P_FMT_IOS, a_oPath);
		string oPBXProjPath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 파일이 존재 할 경우
		if(File.Exists(oPlistPath)) {
			var oDoc = new PlistDocument();
			oDoc.ReadFromFile(oPlistPath);
			oDoc.root.SetBoolean(KCEditorDefine.B_KEY_IOS_FIREBASE_APP_STORE_RECEIPT_URL_CHECK_ENABLE, false);
			
			oDoc.root.SetString(KCEditorDefine.B_KEY_IOS_ENCRYPTION_ENABLE, KCEditorDefine.B_TEXT_IOS_FALSE);
			oDoc.root.SetString(KCEditorDefine.B_KEY_IOS_USER_TRACKING_USAGE_DESC, KEditorDefine.B_IOS_USER_TRACKING_USAGE_DESC);

			var oDeviceCapabilityList = oDoc.ExGetArray(KCEditorDefine.B_KEY_IOS_DEVICE_CAPABILITIES);
			oDeviceCapabilityList.values.Clear();
			
			for(int i = 0; i < KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST.Count; ++i) {
				var oAdsNetworkItemList = oDoc.ExGetArray(KCEditorDefine.B_KEY_IOS_ADS_NETWORK_ITEMS);

				// 광고 네트워크 식별자가 없을 경우
				if(!oAdsNetworkItemList.ExIsContainsAdsNetworkID(KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST[i])) {
					var oAdsNetworkIDInfoDict = oAdsNetworkItemList.AddDict();
					oAdsNetworkIDInfoDict.SetString(KCEditorDefine.B_KEY_IOS_ADS_NETWORK_ID, KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST[i]);
				}
			}
			
			oDoc.WriteToFile(oPlistPath);
		}

		// 프로젝트 파일이 존재 할 경우
		if(File.Exists(oPBXProjPath)) {
			var oPBXProj = new PBXProject();
			oPBXProj.ReadFromFile(oPBXProjPath);

			string oMainGUID = oPBXProj.GetUnityMainTargetGuid();
			string oFrameworkGUID = oPBXProj.GetUnityFrameworkTargetGuid();

			oPBXProj.SetBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCEditorDefine.B_TEXT_IOS_FALSE);
			oPBXProj.SetBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCEditorDefine.B_TEXT_IOS_FALSE);

			oPBXProj.SetBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCEditorDefine.B_TEXT_IOS_FALSE);
			oPBXProj.SetBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCEditorDefine.B_TEXT_IOS_FALSE);

			for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_FRAMEWORK_LIST.Count; ++i) {
				oPBXProj.AddFrameworkToProject(oMainGUID, KEditorDefine.B_IOS_EXTRA_FRAMEWORK_LIST[i], false);
				oPBXProj.AddFrameworkToProject(oFrameworkGUID, KEditorDefine.B_IOS_EXTRA_FRAMEWORK_LIST[i], false);
			}

#if NEVER_USE_THIS
			// FIXME: 비활성 처리 (필요 시 활성 및 사용 가능)
			for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPE_LIST.Count; ++i) {
				oPBXProj.AddCapability(oMainGUID, KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPE_LIST[i]);
			}
#endif // #if NEVER_USE_THIS

			// 전처리기 심볼 정보 테이블이 존재 할 경우
			if(CPlatformOptsSetter.DefineSymbolDictContainer != null && CPlatformOptsSetter.DefineSymbolDictContainer.TryGetValue(BuildTargetGroup.iOS, out List<string> oDefineSymbolList)) {
				for(int i = 0; i < oDefineSymbolList.Count; ++i) {
					oPBXProj.AddBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
					oPBXProj.AddBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
				}
			}

			oPBXProj.WriteToFile(oPBXProjPath);

#if NEVER_USE_THIS
			// FIXME: 비활성 처리 (필요 시 활성 및 사용 가능) {
			var oCapability = new ProjectCapabilityManager(oPBXProjPath, KCEditorDefine.B_ENTITLEMENTS_P_CAPABILITY_IOS, null, oMainGUID);
			
			for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPE_LIST.Count; ++i) {
				var oCapabilityType = KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPE_LIST[i];

				// 애플 로그인 타입 일 경우
				if(oCapabilityType.Equals(PBXCapabilityType.SignInWithApple)) {
					oCapability.AddSignInWithApple();
				}
				// 푸시 알림 타입 일 경우
				else if(oCapabilityType.Equals(PBXCapabilityType.PushNotifications)) {
					oCapability.AddPushNotifications(CPlatformBuilder.BuildMode != EBuildMode.STORE);
				}
				// 게임 센터 타입 일 경우
				else if(oCapabilityType.Equals(PBXCapabilityType.GameCenter)) {
					oCapability.AddGameCenter();
				}
				// 결제 타입 일 경우
				else if(oCapabilityType.Equals(PBXCapabilityType.InAppPurchase)) {
					oCapability.AddInAppPurchase();
				}
			}

			oCapability.WriteToFile();
			// FIXME: 비활성 처리 (필요 시 활성 및 사용 가능) }
#endif // #if NEVER_USE_THIS
		}
#endif // #if UNITY_IOS
	}

	/** 안드로이드 빌드 완료를 처리한다 */
	private static void HandleAndroidPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_ANDROID
		// Do Something
#endif // #if UNITY_ANDROID
	}

	/** 독립 플랫폼 빌드 완료를 처리한다 */
	private static void HandleStandalonePostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_STANDALONE
		string oPath = Path.GetDirectoryName(a_oPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);
		string oDestPath = string.Format(KCEditorDefine.B_DIR_P_FMT_EXTERNAL_DATAS_STANDALONE, oPath);

		CFunc.CopyDir(KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS, oDestPath);
#endif // #if UNITY_STANDALONE
	}
#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

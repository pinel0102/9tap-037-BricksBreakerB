using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/*
Admob 설정
- iOS App ID: ca-app-pub-8822822499150620~1354544428
- iOS Banner Ads ID: ca-app-pub-8822822499150620/8271306569
- iOS Reward Ads ID: ca-app-pub-8822822499150620/1962431972
- iOS Fullscreen Ads ID: ca-app-pub-8822822499150620/7804402708

- Android App ID: ca-app-pub-8822822499150620~8161842966
- Android Banner Ads ID: ca-app-pub-8822822499150620/4769392868
- Android Reward Ads ID: ca-app-pub-8822822499150620/6629269445
- Android Fullscreen Ads ID: ca-app-pub-8822822499150620/4386249489

Iron Source 설정
- iOS App Key: 1707b7945
- Android App Key: 1707b0635

Flurry 설정
- iOS API Key: SNFZB8CP9KFZQTDXK6HG
- Android API Key: 9RCTG3VW8J7457JMS7CJ

Facebook 설정
- App Name: Sample
- App ID: 646060797250571
- Client Token: cea82f0672b06cf906840c3f00fb22c7
*/
/** 스크립트 객체 생성자 */
public static class CScriptableObjCreator {
	#region 클래스 함수
	/** 옵션 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "OptsInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateOptsInfoTable() {
		var oOptsInfoTable = CEditorFactory.CreateScriptableObj<COptsInfoTable>();

		oOptsInfoTable.SetEtcOptsInfo(new STEtcOptsInfo() {
			m_bIsEnableTitleScene = false
		});

		oOptsInfoTable.SetSndOptsInfo(new STSndOptsInfo() {
			m_nNumRealVoices = 32,
			m_nNumVirtualVoices = 512
		});

		oOptsInfoTable.SetBuildOptsInfo(new STBuildOptsInfo() {
			m_bIsPreBakeCollisionMesh = false,
			m_bIsPreserveFrameBufferAlpha = false,

			m_oCameraDesc = string.Empty,
			m_oLocationDesc = string.Empty,
			m_oBluetoothDesc = string.Empty,
			m_oMicrophoneDesc = string.Empty,
			m_oInputSystemMotionDesc = string.Empty,

			m_eNormapMapEncoding = NormalMapEncoding.DXT5nm,
			m_eLightmapper = LightingSettings.Lightmapper.ProgressiveGPU,

			m_stiOSBuildOptsInfo = new STiOSBuildOptsInfo() {
				m_bIsEnableProMotion = false,
				m_bIsEnableInputSystemMotion = false
			},

			m_stAndroidBuildOptsInfo = new STAndroidBuildOptsInfo() {
				m_bIsUseAPKExpansionFiles = false
			},

			m_stStandaloneBuildOptsInfo = new STStandaloneBuildOptsInfo() {
				// Do Something
			}
		});

		oOptsInfoTable.SetQualityOptsInfo(new STQualityOptsInfo() {
			m_bIsEnableRealtimeGI = false,
			m_bIsEnableRealtimeReflectionProbes = false,
			m_bIsEnableRealtimeEnvironmentLighting = false,

			m_eQualityLevel = EQualityLevel.NORM,
			m_eMixedLightingMode = MixedLightingMode.Subtractive,
			m_eLightmapEncodingQuality = ELightmapEncodingQuality.NORM,

			m_stNormQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._1024,
					m_eLightmapMode = ELightmapMode.NON_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.Shadowmask,
					m_eLightmapCompression = LightmapCompression.NormalQuality,
					m_eShadowResolution = ShadowResolution.Medium
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._256,
					m_eMainLightShadowResolution = EPOT._1024,
					m_eAdditionalShadowResolution = EPOT._1024
				}
			},

			m_stHighQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._2048,
					m_eLightmapMode = ELightmapMode.COMBINE_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.DistanceShadowmask,
					m_eLightmapCompression = LightmapCompression.HighQuality,
					m_eShadowResolution = ShadowResolution.High
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._512,
					m_eMainLightShadowResolution = EPOT._2048,
					m_eAdditionalShadowResolution = EPOT._2048
				}
			},

			m_stUltraQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._2048,
					m_eLightmapMode = ELightmapMode.COMBINE_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.DistanceShadowmask,
					m_eLightmapCompression = LightmapCompression.HighQuality,
					m_eShadowResolution = ShadowResolution.VeryHigh
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._512,
					m_eMainLightShadowResolution = EPOT._2048,
					m_eAdditionalShadowResolution = EPOT._2048
				}
			}
		});
	}

	/** 빌드 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "BuildInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateBuildInfoTable() {
		var oBuildInfoTable = CEditorFactory.CreateScriptableObj<CBuildInfoTable>();

		oBuildInfoTable.SetJenkinsInfo(new STJenkinsInfo() {
			m_oUserID = "dante",
			m_oBranch = "main",
			m_oSrc = "0000000000.Common/0300010101.Sample_Unity",
			m_oProjRoot = "Client",
			m_oWorkspace = "/Users/dante/Documents/jenkins/workspace",
			m_oBuildToken = "JenkinsBuild",
			m_oAccessToken = "11769da7a267fd572b450b15e0e71b2f67",
			m_oBuildURLFmt = "http://127.0.0.1:8080/{0}/buildWithParameters"
		});

		oBuildInfoTable.SetCommonBuildInfo(new STCommonBuildInfo() {
			// Do Something
		});

		oBuildInfoTable.SetiOSBuildInfo(new STiOSBuildInfo() {
			m_oTeamID = "58364U6PXL",
			m_oTargetOSVer = "12.0",
			m_oDevProfileID = "",
			m_oStoreProfileID = ""
		});

		oBuildInfoTable.SetAndroidBuildInfo(new STAndroidBuildInfo() {
			m_oKeystorePath = "Keystore.keystore",
			m_oKeyaliasName = "Keystore",
			m_oKeystorePassword = "NSString132!",
			m_oKeyaliasPassword = "NSString132!",
			m_eMinSDKVer = AndroidSdkVersions.AndroidApiLevel22,
			m_eTargetSDKVer = AndroidSdkVersions.AndroidApiLevelAuto
		});

		oBuildInfoTable.SetStandaloneBuildInfo(new STStandaloneBuildInfo() {
			m_oCategory = "public.app-category.games",
			m_oTargetOSVer = "10.13.0"
		});
	}

	/** 전처리기 심볼 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "DefineSymbolInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateDefineSymbolInfoTable() {
		var oDefineSymbolInfoTable = CEditorFactory.CreateScriptableObj<CDefineSymbolInfoTable>();

		oDefineSymbolInfoTable.SetCommonDefineSymbols(new List<string>() {
			KCEditorDefine.DS_DEFINE_S_MODE_2D_ENABLE,
			KCEditorDefine.DS_DEFINE_S_MODE_PORTRAIT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_TEX_FMT_CORRECT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_SPRITE_PIXELS_PER_UNIT_CORRECT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_UNIVERSAL_RENDERING_PIPELINE_ENABLE,
			KCEditorDefine.DS_DEFINE_S_ADDRESSABLES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_BURST_COMPILER_ENABLE,
			KCEditorDefine.DS_DEFINE_S_LOCALIZE_ENABLE,
			KCEditorDefine.DS_DEFINE_S_SCENE_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_EDITOR_SCENE_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_PREFAB_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_UTILITY_SCRIPT_TEMPLATES_ENABLE
		});

		oDefineSymbolInfoTable.SetSubCommonDefineSymbols(new List<string>() {
			KCEditorDefine.DS_DEFINE_S_DEVELOPMENT_PROJ
		});
	}

	/** 프로젝트 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "ProjInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateProjInfoTable() {
		var oProjInfoTable = CEditorFactory.CreateScriptableObj<CProjInfoTable>();

		oProjInfoTable.SetCompanyInfo(new STCompanyInfo() {
			m_oCompany = "LKStudio",
			m_oPrivacyURL = "https://www.ninetap.com/privacy_policy.html",
			m_oServicesURL = "https://www.ninetap.com/terms_of_service.html",
			m_oSupportsMail = "lkstudio.dante@gmail.com"
		});

		oProjInfoTable.SetCommonProjInfo(new STCommonProjInfo() {
			m_oAppID = "lkstudio.dante.sample",
			m_oProjName = "0300010101.Sample_Unity",
			m_oProductName = "Sample_Unity",
			m_oExtraProjDirName = "0300010101.Sample_Unity"
		});

		oProjInfoTable.SetiOSAppleProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = "1309472470"
		});

		oProjInfoTable.SetAndroidGoogleProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetAndroidAmazonProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetStandaloneMacSteamProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetStandaloneWndsSteamProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});
	}

	/** 디바이스 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "DeviceInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateDeviceInfoTable() {
		var oDeviceInfoTable = CEditorFactory.CreateScriptableObj<CDeviceInfoTable>();

		oDeviceInfoTable.SetDeviceInfo(new STDeviceInfo() {
#if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE
			m_oiOSAdmobTestDeviceIDList = new List<string>() {
				// Do Something
			},

			m_oAndroidAdmobTestDeviceIDList = new List<string>() {
				"938274EB10E16F425E5293F48651E5FE"
			}
#endif // #if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE
		});
	}

	/** 지역화 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "LocalizeInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateLocalizeInfoTable() {
		var oLocalizeInfoTable = CEditorFactory.CreateScriptableObj<CLocalizeInfoTable>();

		oLocalizeInfoTable.SetLocalizeInfos(new List<STLocalizeInfo>() {
			new STLocalizeInfo() {
				m_oCountryCode = string.Empty,
				m_eSystemLanguage = SystemLanguage.Korean,

				m_oFontSetInfoList = new List<STFontSetInfo>() {
					new STFontSetInfo() {
						m_eSet = EFontSet._1, m_oPath = KCDefine.U_FONT_P_G_KOREAN
					},

					new STFontSetInfo() {
						m_eSet = EFontSet._2, m_oPath = KCDefine.U_FONT_P_G_KOREAN
					}
				}
			},

			new STLocalizeInfo() {
				m_oCountryCode = string.Empty,
				m_eSystemLanguage = SystemLanguage.English,

				m_oFontSetInfoList = new List<STFontSetInfo> () {
					new STFontSetInfo() {
						m_eSet = EFontSet._1, m_oPath = KCDefine.U_FONT_P_G_DEF
					},

					new STFontSetInfo() {
						m_eSet = EFontSet._2, m_oPath = KCDefine.U_FONT_P_G_ENGLISH
					}
				}
			}
		});
	}

	/** 플러그인 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "PluginInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreatePluginInfoTable() {
		var oPluginInfoTable = CEditorFactory.CreateScriptableObj<CPluginInfoTable>();

#if ADS_MODULE_ENABLE
		oPluginInfoTable.SetAdsPlatform(EAdsPlatform.NONE);
		oPluginInfoTable.SetBannerAdsPos(EBannerAdsPos.NONE);

#if ADMOB_ADS_ENABLE
		oPluginInfoTable.SetiOSAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-8822822499150620/8271306569", m_oRewardAdsID = "ca-app-pub-8822822499150620/1962431972", m_oFullscreenAdsID = "ca-app-pub-8822822499150620/7804402708"
		});

		oPluginInfoTable.SetAndroidAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-8822822499150620/4769392868", m_oRewardAdsID = "ca-app-pub-8822822499150620/6629269445", m_oFullscreenAdsID = "ca-app-pub-8822822499150620/4386249489"
		});
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		oPluginInfoTable.SetEnableIronSrcBannerAds(false);
		oPluginInfoTable.SetEnableIronSrcRewardAds(false);
		oPluginInfoTable.SetEnableIronSrcFullscreenAds(false);

		oPluginInfoTable.SetiOSIronSrcPluginInfo(new STIronSrcPluginInfo() {
			m_oAppKey = "aca5425d"
		});

		oPluginInfoTable.SetAndroidIronSrcPluginInfo(new STIronSrcPluginInfo() {
			m_oAppKey = "b8c2c725"
		});
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		oPluginInfoTable.SetiOSFlurryAPIKey("SNFZB8CP9KFZQTDXK6HG");
		oPluginInfoTable.SetAndroidFlurryAPIKey("9RCTG3VW8J7457JMS7CJ");
#endif // #if FLURRY_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		oPluginInfoTable.SetAppsFlyerPluginInfo(new STAppsFlyerPluginInfo() {
			m_oDevKey = "J7eXAem8sBRuHTr3iX58d5"
		});
#endif // #if APPS_FLYER_MODULE_ENABLE
	}
	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	/** 상품 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "ProductInfoTable", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateProductInfoTable() {
		var oProductInfoTable = CEditorFactory.CreateScriptableObj<CProductInfoTable>();

		oProductInfoTable.SetCommonProductInfos(new List<STProductInfo>() {
			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.beginner", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_BEGINNER_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.expert", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_EXPERT_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.pro", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_PRO_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.c.single.coinsbox", m_eProductType = ProductType.Consumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_COINS_BOX_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.single.removeads", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_REMOVE_ADS_TEXT, m_oDesc = string.Empty
				}
			}
		});
	}
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}
#endif // #if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEngine.U2D;
using UnityEngine.TextCore.LowLevel;
using UnityEditor;
using UnityEditor.U2D;
using TMPro;

/** 에셋 임포터 */
[InitializeOnLoad]
public partial class CAssetImporter : AssetPostprocessor {
	#region 함수
	/** 에셋을 추가 할 경우 */
	public virtual void OnPreprocessAsset() {
		var oAssetImporterInfoList = new List<(bool, System.Action<AssetImporter>)>() {
			(this.assetImporter as TrueTypeFontImporter != null, this.SetupFontImporter),
			(this.assetImporter as AudioImporter != null, this.SetupAudioImporter),
			(this.assetImporter as ModelImporter != null, this.SetupModelImporter),
			(this.assetImporter as ShaderImporter != null, this.SetupShaderImporter),
			(this.assetImporter as TextureImporter != null, this.SetupTexImporter)
		};

		for(int i = 0; i < oAssetImporterInfoList.Count; ++i) {
			// 에셋 임포터가 존재 할 경우
			if(oAssetImporterInfoList[i].Item1) {
				oAssetImporterInfoList[i].Item2(this.assetImporter);
			}
		}
	}

	/** 폰트 임포터를 설정한다 */
	protected virtual void SetupFontImporter(AssetImporter a_oImporter) {
		var oFontImporter = a_oImporter as TrueTypeFontImporter;
		oFontImporter.includeFontData = true;
		oFontImporter.shouldRoundAdvanceValue = true;
		oFontImporter.fontTextureCase = FontTextureCase.Dynamic;
		oFontImporter.fontRenderingMode = FontRenderingMode.Smooth;
		oFontImporter.ascentCalculationMode = AscentCalculationMode.FaceAscender;
	}

	/** 오디오 임포터를 설정한다 */
	protected virtual void SetupAudioImporter(AssetImporter a_oImporter) {
		var oAudioImporter = a_oImporter as AudioImporter;
		oAudioImporter.ambisonic = true;
		oAudioImporter.forceToMono = false;
		oAudioImporter.preloadAudioData = true;
		oAudioImporter.loadInBackground = true;

		// 오디오 임포터 샘플을 설정한다 {
		var stDefSampleSettings = oAudioImporter.defaultSampleSettings;
		CAssetImporter.SetupAudioImporterSampleSettings(oAudioImporter, ref stDefSampleSettings, true);

		oAudioImporter.defaultSampleSettings = stDefSampleSettings;

		for(int i = 0; i < KCEditorDefine.B_AUDIO_IMPORTER_PLATFORM_NAME_LIST.Count; ++i) {
#if AUDIO_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE
			var stSampleSettings = oAudioImporter.GetOverrideSampleSettings(KCEditorDefine.B_AUDIO_IMPORTER_PLATFORM_NAME_LIST[i]);
			CAssetImporter.SetupAudioImporterSampleSettings(oAudioImporter, ref stSampleSettings, false);

			oAudioImporter.SetOverrideSampleSettings(KCEditorDefine.B_AUDIO_IMPORTER_PLATFORM_NAME_LIST[i], stSampleSettings);
#else
			oAudioImporter.ClearSampleSettingOverride(KCEditorDefine.B_AUDIO_IMPORTER_PLATFORM_NAME_LIST[i]);
#endif // #if AUDIO_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE
		}
		// 오디오 임포터 샘플을 설정한다 }
	}

	/** 쉐이더 임포터를 설정한다 */
	protected virtual void SetupShaderImporter(AssetImporter a_oImporter) {
		// Do Something
	}

	/** 모델 임포터를 설정한다 */
	protected virtual void SetupModelImporter(AssetImporter a_oImporter) {
		var oModelImporter = a_oImporter as ModelImporter;
		oModelImporter.optimizeBones = true;
		oModelImporter.generateSecondaryUV = true;
		oModelImporter.optimizeMeshVertices = true;
		oModelImporter.optimizeMeshPolygons = true;
	}

	/** 텍스처 임포터를 설정한다 */
	protected virtual void SetupTexImporter(AssetImporter a_oImporter) {
		var oTexImporter = a_oImporter as TextureImporter;
		oTexImporter.mipmapEnabled = true;
		oTexImporter.alphaIsTransparency = KCEditorDefine.B_ENABLE_ALPHA_TRANSPARENCY_TEX_TYPE_LIST.Contains(oTexImporter.textureType);

		oTexImporter.sRGBTexture = !oTexImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_IGNORE_LINEAR_PIPELINE) && KCEditorDefine.B_ENABLE_SRGB_TEX_TYPE_LIST.Contains(oTexImporter.textureType);
		oTexImporter.ignorePngGamma = oTexImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_IGNORE_LINEAR_PIPELINE);

		oTexImporter.npotScale = KCEditorDefine.B_IGNORE_NON_POT_SCALE_TEX_TYPE_LIST.Contains(oTexImporter.textureType) ? TextureImporterNPOTScale.None : TextureImporterNPOTScale.ToNearest;
		oTexImporter.alphaSource = TextureImporterAlphaSource.FromInput;
		oTexImporter.mipmapFilter = TextureImporterMipFilter.BoxFilter;
		oTexImporter.spritePackingTag = string.Empty;

#if SPRITE_PIXELS_PER_UNIT_CORRECT_ENABLE
		oTexImporter.spritePixelsPerUnit = KCDefine.B_UNIT_PIXELS_PER_UNIT;
#endif // #if SPRITE_PIXELS_PER_UNIT_CORRECT_ENABLE

		// 랩 모드 설정이 가능 할 경우
		if(!KCEditorDefine.B_IGNORE_WRAP_MODE_TEX_TYPE_LIST.Contains(oTexImporter.textureType)) {
			oTexImporter.wrapMode = oTexImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_TEX_FIX_REPEAT_WRAP) ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
		}

		// 필터 모드 설정이 가능 할 경우
		if(!KCEditorDefine.B_IGNORE_FILTER_MODE_TEX_TYPE_LIST.Contains(oTexImporter.textureType)) {
			oTexImporter.filterMode = oTexImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_TEX_FIX_POINT_FILTER) ? FilterMode.Point : FilterMode.Bilinear;
		}

		// 텍스처를 설정한다 {
		var oTexSettings = new TextureImporterSettings();
		oTexImporter.ReadTextureSettings(oTexSettings);

		oTexSettings.readable = oTexImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_ENABLE_READABLE);
		oTexSettings.spriteGenerateFallbackPhysicsShape = true;
		oTexSettings.spriteMeshType = SpriteMeshType.FullRect;

		oTexImporter.SetTextureSettings(oTexSettings);
		// 텍스처를 설정한다 }

		// 텍스처 임포터 플랫폼을 설정한다 {
		var oDefPlatformSettings = oTexImporter.GetDefaultPlatformTextureSettings();
		CAssetImporter.SetupTexImporterPlatformSettings(oTexImporter, oDefPlatformSettings, true, false);

		oTexImporter.SetPlatformTextureSettings(oDefPlatformSettings);

		for(int i = 0; i < KCEditorDefine.B_TEX_IMPORTER_PLATFORM_NAME_LIST.Count; ++i) {
			var oPlatformSettings = oTexImporter.GetPlatformTextureSettings(KCEditorDefine.B_TEX_IMPORTER_PLATFORM_NAME_LIST[i]);
			CAssetImporter.SetupTexImporterPlatformSettings(oTexImporter, oPlatformSettings, false, false);

			oTexImporter.SetPlatformTextureSettings(oPlatformSettings);
		}
		// 텍스처 임포터 플랫폼을 설정한다 }
	}
	#endregion // 함수

	#region 클래스 함수
	/** 에셋을 추가했을 경우 */
	private static void OnPostprocessAllAssets(string[] a_oImportAssets, string[] a_oRemoveAssets, string[] a_oMoveAssets, string[] a_oMoveAssetPaths) {
		for(int i = 0; i < a_oImportAssets.Length; ++i) {
			var oMaterial = CEditorFunc.FindAsset<Material>(a_oImportAssets[i]);
			var oSpriteAtlas = CEditorFunc.FindAsset<SpriteAtlas>(a_oImportAssets[i]);
			var oTMPFontAsset = CEditorFunc.FindAsset<TMP_FontAsset>(a_oImportAssets[i]);

			// 재질 일 경우
			if(oMaterial != null && a_oImportAssets[i].Contains(KCDefine.B_FILE_EXTENSION_MAT)) {
				CAssetImporter.SetupMaterial(oMaterial, a_oImportAssets[i], a_oRemoveAssets, a_oMoveAssets, a_oMoveAssetPaths);
			}
			// 스프라이트 아틀라스 일 경우
			else if(oSpriteAtlas != null && a_oImportAssets[i].Contains(KCDefine.B_FILE_EXTENSION_SPRITE_ATLAS)) {
				CAssetImporter.SetupSpriteAtlas(oSpriteAtlas, a_oImportAssets[i], a_oRemoveAssets, a_oMoveAssets, a_oMoveAssetPaths);
			}
			// TMP 폰트 에셋 일 경우
			else if(oTMPFontAsset != null && a_oImportAssets[i].Contains(KCDefine.B_FILE_EXTENSION_TMP_FONT_ASSET)) {
				CAssetImporter.SetupTMPFontAsset(oTMPFontAsset, a_oImportAssets[i], a_oRemoveAssets, a_oMoveAssets, a_oMoveAssetPaths);
			}
		}
	}

	/** 재질을 설정한다 */
	private static void SetupMaterial(Material a_oMaterial, string a_oImportAsset, string[] a_oRemoveAssets, string[] a_oMoveAssets, string[] a_oMoveAssetPaths) {
#if SAMPLE_PROJ
		a_oMaterial.EnableKeyword(KCEditorDefine.DS_DEFINE_S_SAMPLE_PROJ);
#else
		a_oMaterial.DisableKeyword(KCEditorDefine.DS_DEFINE_S_SAMPLE_PROJ);
#endif // #if SAMPLE_PROJ
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void SetupSpriteAtlas(SpriteAtlas a_oSpriteAtlas, string a_oImportAsset, string[] a_oRemoveAssets, string[] a_oMoveAssets, string[] a_oMoveAssetPaths) {
		var oTexSettings = a_oSpriteAtlas.GetTextureSettings();
		oTexSettings.sRGB = !a_oImportAsset.Contains(KCDefine.B_NAME_PATTERN_IGNORE_LINEAR_PIPELINE);
		oTexSettings.readable = a_oImportAsset.Contains(KCDefine.B_NAME_PATTERN_ENABLE_READABLE);
		oTexSettings.filterMode = a_oImportAsset.Contains(KCDefine.B_NAME_PATTERN_TEX_FIX_POINT_FILTER) ? FilterMode.Point : FilterMode.Bilinear;
		oTexSettings.generateMipMaps = true;

		var oPackingSettings = a_oSpriteAtlas.GetPackingSettings();
		oPackingSettings.enableRotation = false;
		oPackingSettings.enableTightPacking = false;
		oPackingSettings.padding = KCDefine.B_VAL_4_INT;

		a_oSpriteAtlas.SetIncludeInBuild(true);
		a_oSpriteAtlas.SetTextureSettings(oTexSettings);
		a_oSpriteAtlas.SetPackingSettings(oPackingSettings);

		// 텍스처 임포터 플랫폼을 설정한다 {
		var oDefPlatformSettings = a_oSpriteAtlas.GetPlatformSettings(KCEditorDefine.B_TEX_IMPORTER_PLATFORM_N_DEF);
		CAssetImporter.SetupTexImporterPlatformSettings(oDefPlatformSettings, a_oImportAsset, true, false);

		a_oSpriteAtlas.SetPlatformSettings(oDefPlatformSettings);

		for(int i = 0; i < KCEditorDefine.B_TEX_IMPORTER_PLATFORM_NAME_LIST.Count; ++i) {
			var oPlatformSettings = a_oSpriteAtlas.GetPlatformSettings(KCEditorDefine.B_TEX_IMPORTER_PLATFORM_NAME_LIST[i]);
			CAssetImporter.SetupTexImporterPlatformSettings(oPlatformSettings, a_oImportAsset, false, false);

			a_oSpriteAtlas.SetPlatformSettings(oPlatformSettings);
		}
		// 텍스처 임포터 플랫폼을 설정한다 }
	}

	/** TMP 폰트 에셋을 설정한다 */
	private static void SetupTMPFontAsset(TMP_FontAsset a_oTMPFontAsset, string a_oImportAsset, string[] a_oRemoveAssets, string[] a_oMoveAssets, string[] a_oMoveAssetPaths) {
		a_oTMPFontAsset.isMultiAtlasTexturesEnabled = true;
		a_oTMPFontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;

		a_oTMPFontAsset.ExSetRuntimePropertyVal<TMP_FontAsset>(KCEditorDefine.B_PROPERTY_N_CLEAR_DYNAMIC_DATA_ON_BUILD, true);
		a_oTMPFontAsset.ExSetRuntimePropertyVal<TMP_FontAsset>(KCEditorDefine.B_PROPERTY_N_ATLAS_RENDER_MODE, GlyphRenderMode.SDFAA_HINTED);

		a_oTMPFontAsset.ExSetRuntimePropertyVal<TMP_FontAsset>(KCEditorDefine.B_PROPERTY_N_ATLAS_WIDTH, (int)EPOT._2048);
		a_oTMPFontAsset.ExSetRuntimePropertyVal<TMP_FontAsset>(KCEditorDefine.B_PROPERTY_N_ATLAS_HEIGHT, (int)EPOT._2048);
	}

	/** 오디오 임포터 샘플을 설정한다 */
	private static void SetupAudioImporterSampleSettings(AudioImporter a_oImporter, ref AudioImporterSampleSettings a_rstSampleSettings, bool a_bIsDefPlatformSettings) {
		a_rstSampleSettings.quality = KCDefine.B_VAL_1_REAL;
		a_rstSampleSettings.loadType = (a_oImporter != null && a_oImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_AUDIO_FIX_COMPRESS_IN_MEMORY)) ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
		a_rstSampleSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;

#if AUDIO_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE
		bool bIsEnableSetupAudioFmt = a_bIsDefPlatformSettings;
#else
		bool bIsEnableSetupAudioFmt = true;
#endif // #if AUDIO_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE

		// 오디오 포맷 설정이 가능 할 경우
		if(bIsEnableSetupAudioFmt) {
			a_rstSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
		}
	}

	/** 텍스처 임포터 플랫폼을 설정한다 */
	private static void SetupTexImporterPlatformSettings(TextureImporterPlatformSettings a_oPlatformSettings, string a_oImportAsset, bool a_bIsDefPlatformSettings, bool a_bIsEnableAssert = true) {
		CAssetImporter.SetupTexImporterPlatformSettings(null, a_oPlatformSettings, a_bIsDefPlatformSettings, a_bIsEnableAssert);
	}

	/** 텍스처 임포터 플랫폼을 설정한다 */
	private static void SetupTexImporterPlatformSettings(TextureImporter a_oImporter, TextureImporterPlatformSettings a_oPlatformSettings, bool a_bIsDefPlatformSettings, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPlatformSettings != null);

		// 플랫폼 설정이 존재 할 경우
		if(a_oPlatformSettings != null) {
			a_oPlatformSettings.maxTextureSize = (int)EPOT._2048;
			a_oPlatformSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
			a_oPlatformSettings.compressionQuality = KCDefine.B_UNIT_DIGITS_PER_HUNDRED;
			a_oPlatformSettings.textureCompression = TextureImporterCompression.CompressedHQ;
			a_oPlatformSettings.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;

#if TEX_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE
			bool bIsEnableSetupTexFmt = a_bIsDefPlatformSettings;
			a_oPlatformSettings.overridden = !a_bIsDefPlatformSettings;
#else
			bool bIsEnableSetupTexFmt = true;
			a_oPlatformSettings.overridden = false;
#endif // #if TEX_IMPORTER_PLATFORM_SETTINGS_OVERRIDE_ENABLE

			// 텍스처 포맷 설정이 가능 할 경우
			if(bIsEnableSetupTexFmt) {
				a_oPlatformSettings.crunchedCompression = true;
				a_oPlatformSettings.allowsAlphaSplitting = false;

#if TEX_FMT_CORRECT_ENABLE
				a_oPlatformSettings.format = (a_oImporter != null && KCEditorDefine.B_IGNORE_RGBA_32_FMT_TEX_TYPE_LIST.Contains(a_oImporter.textureType) && !a_oImporter.assetPath.Contains(KCDefine.B_NAME_PATTERN_IGNORE_TEX_COMPRESS)) ? TextureImporterFormat.Automatic : TextureImporterFormat.RGBA32;
#endif // #if TEX_FMT_CORRECT_ENABLE
			}
			// 기본 플랫폼 설정이 아닐 경우
			else if(!a_bIsDefPlatformSettings && a_oPlatformSettings.format < TextureImporterFormat.Automatic) {
				a_oPlatformSettings.format = (a_oImporter != null && KCEditorDefine.B_IGNORE_RGBA_32_FMT_TEX_TYPE_LIST.Contains(a_oImporter.textureType)) ? a_oPlatformSettings.format : TextureImporterFormat.RGBA32;
			}
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

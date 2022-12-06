#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 서브 에셋 임포터 */
[InitializeOnLoad]
public partial class CSubAssetImporter : CAssetImporter {
#region 함수
	/** 에셋을 추가 할 경우 */
	public override void OnPreprocessAsset() {
		// Do Something
	}

	/** 오디오 임포터를 설정한다 */
	protected override void SetupAudioImporter(AssetImporter a_oImporter) {
		// Do Something
	}

	/** 쉐이더 임포터를 설정한다 */
	protected override void SetupShaderImporter(AssetImporter a_oImporter) {
		// Do Something
	}

	/** 모델 임포터를 설정한다 */
	protected override void SetupModelImporter(AssetImporter a_oImporter) {
		// Do Something
	}

	/** 텍스처 임포터를 설정한다 */
	protected override void SetupTexImporter(AssetImporter a_oImporter) {
		// Do Something
	}
#endregion // 함수
}
#endif // #if UNITY_EDITOR
#endif // #if SCRIPT_TEMPLATE_ONLY

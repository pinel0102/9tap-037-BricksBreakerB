using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if PURCHASE_MODULE_ENABLE
using System.IO;
using UnityEngine.Purchasing;

/** 상품 정보 */
[System.Serializable]
public struct STProductInfo {
	public STCommonInfo m_stCommonInfo;

	public string m_oID;
	public ProductType m_eProductType;

	#region 상수
	public static readonly STProductInfo INVALID = new STProductInfo() {
		// Do Something
	};
	#endregion // 상수

	#region 함수
	/** 생성자 */
	public STProductInfo(SimpleJSON.JSONNode a_oProductInfo) {
		m_stCommonInfo = new STCommonInfo(a_oProductInfo);

		m_oID = a_oProductInfo[KCDefine.U_KEY_ID];
		m_eProductType = a_oProductInfo[KCDefine.U_KEY_PRODUCT_TYPE].ExIsValid() ? (ProductType)a_oProductInfo[KCDefine.U_KEY_PRODUCT_TYPE].AsInt : ProductType.Consumable;
	}
	#endregion // 함수
}
#endif // #if PURCHASE_MODULE_ENABLE

/** 상품 정보 테이블 */
public partial class CProductInfoTable : CScriptableObj<CProductInfoTable> {
#if PURCHASE_MODULE_ENABLE
	#region 변수
	[Header("=====> Common Product Info <=====")]
	[SerializeField] private List<STProductInfo> m_oCommonProductInfoList = new List<STProductInfo>();

	[Header("=====> iOS Product Info <=====")]
	[SerializeField] private List<STProductInfo> m_oiOSAppleProductInfoList = new List<STProductInfo>();

	[Header("=====> Android Product Info <=====")]
	[SerializeField] private List<STProductInfo> m_oAndroidGoogleProductInfoList = new List<STProductInfo>();
	[SerializeField] private List<STProductInfo> m_oAndroidAmazonProductInfoList = new List<STProductInfo>();
	#endregion // 변수

	#region 프로퍼티
	public List<STProductInfo> ProductInfoList { get; } = new List<STProductInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ProductInfoList.ExAddVals(m_oCommonProductInfoList);

#if UNITY_IOS
		this.ProductInfoList.ExAddVals(m_oiOSAppleProductInfoList);
#elif UNITY_ANDROID
#if ANDROID_AMAZON_PLATFORM
		this.ProductInfoList.ExAddVals(m_oAndroidAmazonProductInfoList);
#else
		this.ProductInfoList.ExAddVals(m_oAndroidGoogleProductInfoList);
#endif // #if ANDROID_AMAZON_PLATFORM
#endif // #if UNITY_IOS
	}

	/** 상품 정보 인덱스를 반환한다 */
	public int GetProductInfoIdx(string a_oID) {
		return this.ProductInfoList.FindIndex((a_stProductInfo) => a_stProductInfo.m_oID.Equals(a_oID));
	}

	/** 상품 정보를 반환한다 */
	public STProductInfo GetProductInfo(int a_nIdx) {
		bool bIsValid = this.TryGetProductInfo(a_nIdx, out STProductInfo stProductInfo);
		CAccess.Assert(bIsValid);

		return stProductInfo;
	}

	/** 상품 정보를 반환한다 */
	public STProductInfo GetProductInfo(string a_oID) {
		bool bIsValid = this.TryGetProductInfo(a_oID, out STProductInfo stProductInfo);
		CAccess.Assert(bIsValid);

		return stProductInfo;
	}

	/** 상품 정보를 반환한다 */
	public bool TryGetProductInfo(int a_nIdx, out STProductInfo a_stOutProductInfo) {
		a_stOutProductInfo = this.ProductInfoList.ExGetVal(a_nIdx, STProductInfo.INVALID);
		return this.ProductInfoList.ExIsValidIdx(a_nIdx);
	}

	/** 상품 정보를 반환한다 */
	public bool TryGetProductInfo(string a_oID, out STProductInfo a_stOutProductInfo) {
		int nIdx = this.ProductInfoList.FindIndex((a_stProductInfo) => a_stProductInfo.m_oID.Equals(a_oID));
		return this.TryGetProductInfo(nIdx, out a_stOutProductInfo);
	}

	/** 상품 정보를 로드한다 */
	public List<STProductInfo> LoadProductInfos() {
		return this.LoadProductInfos(CAccess.ProductInfoTableLoadPath);
	}

	/** 상품 정보를 저장한다 */
	public void SaveProductInfos(string a_oJSONStr) {
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		CFunc.WriteStr(CAccess.ProductInfoTableSavePath, a_oJSONStr, false);
#else
		CFunc.WriteStr(CAccess.ProductInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 정보를 로드한다 */
	private List<STProductInfo> LoadProductInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadProductInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadProductInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false));
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 정보를 로드한다 */
	private List<STProductInfo> DoLoadProductInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);

		var oProductInfosList = new List<SimpleJSON.JSONNode>();
		oProductInfosList.ExAddVal(oJSONNode[KCDefine.B_KEY_COMMON]);
		oProductInfosList.ExAddVal(oJSONNode[CCommonAppInfoStorage.Inst.Platform]);

		for(int i = 0; i < oProductInfosList.Count; ++i) {
			for(int j = 0; j < oProductInfosList[i].Count; ++j) {
				var stProductInfo = new STProductInfo(oProductInfosList[i][j]);

				// 상품 정보 추가 가능 할 경우
				if(!this.TryGetProductInfo(stProductInfo.m_oID, out STProductInfo stOutProductInfo) || oProductInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.ProductInfoList.ExReplaceVal(stProductInfo, (a_stCompareProductInfo) => a_stCompareProductInfo.m_oID.Equals(stProductInfo.m_oID));
				}
			}
		}

		return this.ProductInfoList;
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 공용 상품 정보를 변경한다 */
	public void SetCommonProductInfos(List<STProductInfo> a_oProductInfoList) {
		m_oCommonProductInfoList = a_oProductInfoList;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
#endif // #if PURCHASE_MODULE_ENABLE
}

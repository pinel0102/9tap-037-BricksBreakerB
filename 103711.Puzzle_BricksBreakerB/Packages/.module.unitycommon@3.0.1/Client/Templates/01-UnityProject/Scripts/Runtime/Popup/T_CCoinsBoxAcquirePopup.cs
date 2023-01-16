#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 코인 상자 획득 팝업 */
public partial class CCoinsBoxAcquirePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PREV_NUM_COINS_BOX_COINS,
		NUM_COINS_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public long m_nNumCoinsBoxCoins;
	}

	#region 변수
	private Dictionary<EKey, long> m_oIntDict = new Dictionary<EKey, long>() {
		[EKey.PREV_NUM_COINS_BOX_COINS] = KCDefine.B_VAL_0_INT
	};

	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.NUM_COINS_TEXT, $"{EKey.NUM_COINS_TEXT}", this.Contents)
		}, m_oTextDict);

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		var stValInfo = new STValInfo(a_stParams.m_nNumCoinsBoxCoins, EValType.INT);
		m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] = (long)Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_COINS_BOX_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS);

		Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, new STTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_COINS_BOX_COINS, stValInfo), true);
		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 변경한다 */
	private void UpdateUIsState() {
		// 객체를 갱신한다
		m_oSaveUIs?.SetActive(m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] < KDefine.G_MAX_NUM_COINS_BOX_COINS);
		m_oFullUIs?.SetActive(m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] >= KDefine.G_MAX_NUM_COINS_BOX_COINS);

		// 텍스트를 갱신한다
		m_oTextDict[EKey.NUM_COINS_TEXT]?.ExSetText($"{m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS]}", EFontSet._1, false);

		this.SubUpdateUIsState();
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/** 터치 사운드 재생자 */
public partial class CTouchSndPlayer : CComponent, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_TOUCH,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
	#endregion // 변수

	#region 프로퍼티
	protected virtual string BeginSndPath => KCDefine.U_SND_P_G_SFX_TOUCH_BEGIN;
	protected virtual string EndSndPath => KCDefine.U_SND_P_G_SFX_TOUCH_END;
	#endregion // 프로퍼티

	#region IPointerEnterHandler
	/** 영역에 들어왔을 경우 */
	public virtual void OnPointerEnter(PointerEventData a_oEventData) {
		m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, true);
	}
	#endregion // IPointerEnterHandler

	#region IPointerExitHandler
	/** 영역에서 벗어났을 경우 */
	public virtual void OnPointerExit(PointerEventData a_oEventData) {
		m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, false);
	}
	#endregion // IPointerExitHandler

	#region IPointerUpHandler
	/** 터치를 종료했을 경우 */
	public virtual void OnPointerUp(PointerEventData a_oEventData) {
		// 터치 종료 사운드 경로가 유효 할 경우
		if(m_oBoolDict.GetValueOrDefault(EKey.IS_TOUCH) && this.EndSndPath.ExIsValid()) {
			CSndManager.Inst.PlayFXSnds(this.EndSndPath);
		}

		m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, false);
	}
	#endregion // IPointerUpHandler

	#region IPointerDownHandler
	/** 터치를 시작했을 경우 */
	public virtual void OnPointerDown(PointerEventData a_oEventData) {
		// 터치 시작 사운드 경로가 유효 할 경우
		if(this.BeginSndPath.ExIsValid()) {
			CSndManager.Inst.PlayFXSnds(this.BeginSndPath);
		}

		m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, true);
	}
	#endregion // IPointerDownHandler
}

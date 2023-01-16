using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;

/** 컴포넌트 */
public abstract partial class CComponent : MonoBehaviour, IUpdater {
	#region 프로퍼티
	public bool IsDestroy { get; private set; } = false;
	public bool IsIgnoreAni { get; private set; } = false;
	public bool IsIgnoreNavStackEvent { get; private set; } = false;

	public CComponent Owner { get; private set; } = null;

	public System.Action<CComponent> DestroyCallback { get; private set; } = null;
	public System.Action<CComponent> ScheduleCallback { get; private set; } = null;
	public System.Action<CComponent> NavStackCallback { get; private set; } = null;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public virtual void Awake() {
		// Do Something
	}

	/** 초기화 */
	public virtual void Start() {
#if UNITY_EDITOR
		this.SetupScriptOrder();
#endif // #if UNITY_EDITOR
	}

	/** 상태를 리셋한다 */
	public virtual void Reset() {
		// Do Something
	}

	/** 제거 되었을 경우 */
	public virtual void OnDestroy() {
		this.IsDestroy = true;

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			this.DestroyCallback?.Invoke(this);
			this.ScheduleCallback?.Invoke(this);
			this.NavStackCallback?.Invoke(this);
		}
	}

	/** 상태를 갱신한다 */
	public virtual void OnUpdate(float a_fDeltaTime) {
		// Do Something
	}

	/** 상태를 갱신한다 */
	public virtual void OnLateUpdate(float a_fDeltaTime) {
		// Do Something
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public virtual void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		// 백 키 눌림 이벤트 일 경우
		if(!this.IsIgnoreNavStackEvent && a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			CNavStackManager.Inst.RemoveComponent(this);
		}
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 스크립트 순서를 설정한다 */
	protected virtual void SetupScriptOrder() {
		// Do Something
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

/** 컴포넌트 - 접근 */
public abstract partial class CComponent : MonoBehaviour, IUpdater {
	#region 함수
	/** 애니메이션 무시 여부를 변경한다 */
	public void SetIgnoreAni(bool a_bIsIgnore) {
		this.IsIgnoreAni = a_bIsIgnore;
	}

	/** 네비게이션 스택 이벤트 무시 여부를 변경한다 */
	public void SetIgnoreNavStackEvent(bool a_bIsIgnore) {
		this.IsIgnoreNavStackEvent = a_bIsIgnore;
	}

	/** 소유자를 변경한다 */
	public void SetOwner(CComponent a_oComponent) {
		this.Owner = a_oComponent;
	}

	/** 제거 콜백을 변경한다 */
	public void SetDestroyCallback(System.Action<CComponent> a_oCallback) {
		this.DestroyCallback = a_oCallback;
	}

	/** 스케줄 콜백을 변경한다 */
	public void SetScheduleCallback(System.Action<CComponent> a_oCallback) {
		this.ScheduleCallback = a_oCallback;
	}

	/** 내비게이션 스택 콜백을 변경한다 */
	public void SetNavStackCallback(System.Action<CComponent> a_oCallback) {
		this.NavStackCallback = a_oCallback;
	}
	#endregion // 함수

	#region 제네릭 함수
	/** 소유자를 반환한다 */
	public T GetOwner<T>() where T : CComponent {
		return this.Owner as T;
	}
	#endregion // 제네릭 함수
}

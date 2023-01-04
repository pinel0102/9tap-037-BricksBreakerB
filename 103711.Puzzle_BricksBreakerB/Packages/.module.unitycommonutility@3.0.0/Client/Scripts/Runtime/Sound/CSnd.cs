using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 사운드 */
public partial class CSnd : CComponent {
	#region 변수
	private AudioSource m_oAudioSrc = null;
	#endregion // 변수

	#region 프로퍼티
	public bool IsMute => m_oAudioSrc.mute;
	public bool IsPlaying => m_oAudioSrc.isPlaying;
	public bool IsIgnoreEffects => !m_oAudioSrc.bypassEffects;
	public bool IsIgnoreReverbZones => !m_oAudioSrc.bypassReverbZones;
	public bool IsIgnoreListenerEffects => !m_oAudioSrc.bypassListenerEffects;

	public float Volume => m_oAudioSrc.volume;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		m_oAudioSrc = this.gameObject.GetComponentInChildren<AudioSource>();
		m_oAudioSrc.playOnAwake = false;
	}

	/** 음소거 여부를 변경한다 */
	public void SetIsMute(bool a_bIsMute) {
		m_oAudioSrc.mute = a_bIsMute;
	}

	/** 효과 무시 여부를 변경한다 */
	public void SetIsIgnoreEffects(bool a_bIsIgnore) {
		m_oAudioSrc.bypassEffects = !a_bIsIgnore;
	}

	/** 리버브 존 무시 여부를 변경한다 */
	public void SetIsIgnoreReverbZones(bool a_bIsIgnore) {
		m_oAudioSrc.bypassReverbZones = !a_bIsIgnore;
	}

	/** 수신자 효과 무시 여부를 변경한다 */
	public void SetIsIgnoreListenerEffects(bool a_bIsIgnore) {
		m_oAudioSrc.bypassListenerEffects = !a_bIsIgnore;
	}

	/** 볼륨을 변경한다 */
	public void SetVolume(float a_fVolume) {
		m_oAudioSrc.volume = Mathf.Clamp01(a_fVolume);
	}

	/** 사운드를 재생한다 */
	public void PlaySnd(AudioClip a_oAudioClip, bool a_bIsLoop, bool a_bIs3DSnd, float a_fPitch = KCDefine.B_VAL_1_REAL, float a_fPanStereo = KCDefine.B_VAL_1_REAL) {
		m_oAudioSrc.pitch = a_fPitch;
		m_oAudioSrc.panStereo = a_fPanStereo;

		m_oAudioSrc.ExPlay(a_oAudioClip, a_bIsLoop, a_bIs3DSnd, false);
	}

	/** 사운드를 재개한다 */
	public void ResumeSnd() {
		m_oAudioSrc.UnPause();
	}

	/** 사운드를 정지한다 */
	public void PauseSnd() {
		m_oAudioSrc.Pause();
	}

	/** 사운드를 중지한다 */
	public void StopSnd() {
		m_oAudioSrc.Stop();
	}
	#endregion // 함수
}

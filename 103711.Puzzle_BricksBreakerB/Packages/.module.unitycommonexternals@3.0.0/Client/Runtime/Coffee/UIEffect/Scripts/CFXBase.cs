using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// FIXME: dante (기초 효과 클래스 추가) {
namespace Coffee.UIExtensions {
	/** 효과 */
	public abstract partial class CFXBase : UIEffectBase {
		#region 프로퍼티
		public abstract float effectFactor { get; set; }
		#endregion // 프로퍼티
	}
}
// FIXME: dante (기초 효과 클래스 추가) }

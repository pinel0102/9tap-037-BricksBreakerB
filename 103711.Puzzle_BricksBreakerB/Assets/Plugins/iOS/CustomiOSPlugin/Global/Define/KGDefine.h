//
//  KGDefine.h
//  Unity-iPhone
//
//  Created by 이상동 on 2020/08/24.
//

#ifndef KGDefine_h
#define KGDefine_h

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <StoreKit/StoreKit.h>
#import <MessageUI/MessageUI.h>
#import <AdSupport/AdSupport.h>
#import <AudioToolbox/AudioToolbox.h>

#import "Unity/UnityInterface.h"

#if defined IRON_SRC_ADS_ENABLE
#import <FBAudienceNetwork/FBAdSettings.h>
#endif			// #if defined IRON_SRC_ADS_ENABLE

// 기타 {
#define G_EMPTY_STR				("")
#define G_EMPTY_DICT			(@{})

#define G_IDX_INVALID			(-1)
// 기타 }

// 값 {
#define G_VAL_0_INT			(0)
#define G_VAL_1_INT			(1)
#define G_VAL_2_INT			(2)
#define G_VAL_3_INT			(3)
#define G_VAL_4_INT			(4)
#define G_VAL_5_INT			(5)
#define G_VAL_6_INT			(6)
#define G_VAL_7_INT			(7)
#define G_VAL_8_INT			(8)
#define G_VAL_9_INT			(9)

#define G_VAL_0_REAL			(0.0f)
#define G_VAL_1_REAL			(1.0f)
#define G_VAL_2_REAL			(2.0f)
#define G_VAL_3_REAL			(3.0f)
#define G_VAL_4_REAL			(4.0f)
#define G_VAL_5_REAL			(5.0f)
#define G_VAL_6_REAL			(6.0f)
#define G_VAL_7_REAL			(7.0f)
#define G_VAL_8_REAL			(8.0f)
#define G_VAL_9_REAL			(9.0f)
// 값 }

// 결과
#define G_RESULT_TRUE			("True")
#define G_RESULT_FALSE			("False")

// 비율
#define G_SCALE_ACTIVITY_INDICATOR					(0.25f)
#define G_OFFSET_SCALE_ACTIVITY_INDICATOR			(0.0f)

// 버전
#define G_MIN_VER_FEEDBACK_GENERATOR			10.0
#define G_MIN_VER_IMPACT_INTENSITY				13.0
#define G_MIN_VER_INDICATOR						13.0

// 명령어 {
#define G_CMD_GET_DEVICE_ID				("GetDeviceID")
#define G_CMD_GET_COUNTRY_CODE			("GetCountryCode")
#define G_CMD_GET_STORE_VER				("GetStoreVer")

#define G_CMD_SET_ENABLE_ADS_TRACKING			("SetEnableAdsTracking")

#define G_CMD_SHOW_ALERT			("ShowAlert")
#define G_CMD_SHOW_TOAST			("ShowToast")

#define G_CMD_MAIL				("Mail")
#define G_CMD_VIBRATE			("Vibrate")
#define G_CMD_INDICATOR			("Indicator")
// 명령어 }

// 식별자 {
#define G_ID_KEYCHAIN_DEVICE			("KeychainDeviceID")

#define G_KEY_CMD			("Cmd")
#define G_KEY_MSG			("Msg")

#define G_KEY_APP_ID			("AppID")
#define G_KEY_VER				("Ver")
#define G_KEY_TIMEOUT			("Timeout")

#define G_KEY_ALERT_TITLE					("Title")
#define G_KEY_ALERT_MSG						("Msg")
#define G_KEY_ALERT_OK_BTN_TEXT				("OKBtnText")
#define G_KEY_ALERT_CANCEL_BTN_TEXT			("CancelBtnText")

#define G_KEY_STORE_VER					("Ver")
#define G_KEY_STORE_VER_RESULT			("Results")

#define G_KEY_MAIL_TITLE				("Title")
#define G_KEY_MAIL_MSG					("Msg")
#define G_KEY_MAIL_RECIPIENT			("Recipient")

#define G_KEY_VIBRATE_TYPE				("Type")
#define G_KEY_VIBRATE_STYLE				("Style")
#define G_KEY_VIBRATE_INTENSITY			("Intensity")

#define G_KEY_DEVICE_MS_VER				("Ver")
#define G_KEY_DEVICE_MS_RESULT			("Result")
// 식별자 }

// 디바이스 메세지 전송자
#define G_OBJ_N_DEVICE_MS_DEVICE_MSG_RECEIVER			("CDeviceMsgReceiver")
#define G_FUNC_N_DEVICE_MS_DEVICE_MSG_HANDLER			("OnReceiveDeviceMsg")

// 네트워크 {
#define G_HTTP_METHOD_GET			("GET")
#define G_HTTP_METHOD_POST			("POST")

#define G_URL_FMT_MAIL				("mailto:%@?subject=%@&body=%@")
#define G_URL_FMT_STORE_VER			("http://itunes.apple.com/lookup?bundleId=%@")
// 네트워크 }

/** 진동 타입 */
enum class EVibrateType {
	NONE = -1,
	SELECTION,
	NOTIFICATION,
	IMPACT,
	MAX_VAL
};

/** 진동 스타일 */
enum class EVibrateStyle {
	NONE = -1,
	LIGHT,
	MEDIUM,
	HEAVY,
	MAX_VAL
};

#endif /* KGDefine_h */

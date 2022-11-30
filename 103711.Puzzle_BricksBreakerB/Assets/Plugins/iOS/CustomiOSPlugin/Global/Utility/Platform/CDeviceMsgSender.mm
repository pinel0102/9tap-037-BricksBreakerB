//
//  CDeviceMsgSender.m
//  Unity-iPhone
//
//  Created by 이상동 on 2020/01/11.
//

#import "CDeviceMsgSender.h"
#import "../../Function/GFunc.h"

/** 전역 변수 */
static CDeviceMsgSender *g_pInst = nil;

/** 디바이스 메세지 전송자 */
@implementation CDeviceMsgSender
#pragma mark - 초기화
/** 객체를 생성한다 */
+ (id)alloc {
	@synchronized(CDeviceMsgSender.class) {
		// 인스턴스가 없을 경우
		if(g_pInst == nil) {
			g_pInst = [[super alloc] init];
		}
	}
	
	return g_pInst;
}

#pragma mark - 인스턴스 메서드
/** 디바이스 식별자 반환 메세지를 전송한다 */
- (void)sendGetDeviceIDMsg:(NSString *)a_oDeviceID {
	[self send:@(G_CMD_GET_DEVICE_ID) withDeviceMsg:a_oDeviceID];
}

/** 국가 코드 반환 메세지를 전송한다 */
- (void)sendGetCountryCodeMsg:(NSString *)a_pCountryCode {
	[self send:@(G_CMD_GET_COUNTRY_CODE) withDeviceMsg:a_pCountryCode];
}

/** 스토어 버전 반환 메세지를 전송한다 */
- (void)sendGetStoreVerMsg:(NSString *)a_pVer withResult:(BOOL)a_bIsSuccess {
	NSString *pStr = GFunc::ConvertBoolToStr(a_bIsSuccess);
	NSDictionary *pDataDict = [NSDictionary dictionaryWithObjectsAndKeys:a_pVer, @(G_KEY_DEVICE_MS_VER), pStr, @(G_KEY_DEVICE_MS_RESULT), nil];
	
	[self send:@(G_CMD_GET_STORE_VER) withDeviceMsg:GFunc::ConvertObjToJSONStr(pDataDict, NULL)];
}

/** 경고 창 출력 메세지를 전송한다 */
- (void)sendShowAlertMsg:(BOOL)a_bIsOK {
	[self send:@(G_CMD_SHOW_ALERT) withDeviceMsg:GFunc::ConvertBoolToStr(a_bIsOK)];
}

/** 디바이스 메세지를 전송한다 */
- (void)send:(NSString *)a_pCmd withDeviceMsg:(NSString *)a_pMsg {
	NSLog(@"CiOSPlugin.sendWithDeviceMsg: %@, %@", a_pCmd, a_pMsg);
	NSDictionary *pDataDict = [NSDictionary dictionaryWithObjectsAndKeys:a_pCmd, @(G_KEY_CMD), a_pMsg, @(G_KEY_MSG), nil];

	UnitySendMessage(G_OBJ_N_DEVICE_MS_DEVICE_MSG_RECEIVER, G_FUNC_N_DEVICE_MS_DEVICE_MSG_HANDLER, GFunc::ConvertObjToJSONStr(pDataDict, NULL).UTF8String);
}

#pragma mark - 클래스 메서드
/** 인스턴스를 반환한다 */
+ (instancetype)sharedInst {
	@synchronized(CDeviceMsgSender.class) {
		// 인스턴스가 없을 경우
		if(g_pInst == nil) {
			g_pInst = [[CDeviceMsgSender alloc] init];
		}
	}
	
	return g_pInst;
}
@end			// CDeviceMsgSender

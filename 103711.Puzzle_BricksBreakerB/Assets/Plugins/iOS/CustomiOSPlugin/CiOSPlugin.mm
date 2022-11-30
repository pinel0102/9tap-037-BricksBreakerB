//
//  CiOSPlugin.m
//  Unity-iPhone
//
//  Created by 이상동 on 2020/02/26.
//

#import "CiOSPlugin.h"
#import "Global/Function/GFunc.h"
#import "Global/Utility/Platform/CDeviceMsgSender.h"

/** 전역 변수 */
static CiOSPlugin *g_pInst = nil;

/** 유니티 메세지 정보 */
@interface CUnityMsgInfo : NSObject {
	// Do Something
}

// 프로퍼티
@property (nonatomic, copy) NSString *unityCmd;
@property (nonatomic, copy) NSString *unityMsg;

/** 초기화 */
- (id)init:(NSString *)a_pCmd withMsg:(NSString *)a_pMsg;
@end			// CUnityMsgInfo

/** 유니티 메세지 정보 */
@implementation CUnityMsgInfo
#pragma mark - 인스턴스 메서드
/** 초기화 */
- (id)init:(NSString *)a_pCmd withMsg:(NSString *)a_pMsg {
	/** 초기화 되었을 경우 */
	if(self = [super init]) {
		self.unityCmd = a_pCmd;
		self.unityMsg = a_pMsg;
	}
	
	return self;
}
@end			// CUnityMsgInfo

/** iOS 플러그인 - Private */
@interface CiOSPlugin (Private) {
	// Do Something
}

/** 디바이스 식별자 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetDeviceIDMsg:(NSString *)a_pMsg;

/** 국가 코드 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetCountryCodeMsg:(NSString *)a_pMsg;

/** 스토어 버전 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetStoreVerMsg:(NSString *)a_pMsg;

/** 광고 추적 여부 변경 메세지를 수신했을 경우 */
- (void)onReceiveSetEnableAdsTrackingMsg:(NSString *)a_pMsg;

/** 경고 창 출력 메세지를 수신했을 경우 */
- (void)onReceiveShowAlertMsg:(NSString *)a_pMsg;

/** 토스트 출력 메세지를 출력한다 */
- (void)onReceiveShowToastMsg:(NSString *)a_pMsg;

/** 메일 메세지를 수신했을 경우 */
- (void)onReceiveMailMsg:(NSString *)a_pMsg;

/** 진동 메세지를 수신했을 경우 */
- (void)onReceiveVibrateMsg:(NSString *)a_pMsg;

/** 인디케이터 메세지를 수신했을 경우 */
- (void)onReceiveIndicatorMsg:(NSString *)a_pMsg;

/** 임팩트 진동 메세지를 수신했을 경우 */
- (void)onReceiveImpactVibrateMsg:(NSDictionary *)a_pDataDict withVibrateStyle:(EVibrateStyle)a_eVibrateStyle;
@end			// CiOSPlugin (Private)

extern "C" {
	/** 유니티 메세지를 수신했을 경우 */
	void OnReceiveUnityMsg(const char *a_pszCmd, const char *a_pszMsg) {
		NSLog(@"CiOSPlugin.OnReceiveUnityMsg: %@, %@", @(a_pszCmd), @(a_pszMsg));
		[CiOSPlugin.sharedInst.unityMsgInfoList addObject:[[CUnityMsgInfo alloc] init:@(a_pszCmd) withMsg:@(a_pszMsg)]];
		
		while(CiOSPlugin.sharedInst.unityMsgInfoList.count > G_VAL_0_INT) {
			CUnityMsgInfo *pUnityMsgInfo = (CUnityMsgInfo *)[CiOSPlugin.sharedInst.unityMsgInfoList objectAtIndex:G_VAL_0_INT];
			
			// 유니티 메세지 정보가 존재 할 경우
			if(pUnityMsgInfo != nil) {
				NSString *pMsg = pUnityMsgInfo.unityMsg;
				NSString *pSelectorName = (NSString *)[CiOSPlugin.sharedInst.unityMsgHandlerDict objectForKey:pUnityMsgInfo.unityCmd];
				
				// 유니티 메세지 처리자가 존재 할 경우
				if(GFunc::IsValid(pSelectorName)) {
					NSInvocation *pInvocation = [NSInvocation invocationWithMethodSignature:[CiOSPlugin.sharedInst methodSignatureForSelector:NSSelectorFromString(pSelectorName)]];
					pInvocation.selector = NSSelectorFromString(pSelectorName);
					
					[pInvocation setArgument:&pMsg atIndex:G_VAL_2_INT];
					[pInvocation invokeWithTarget:CiOSPlugin.sharedInst];
				}
			}
				
			[CiOSPlugin.sharedInst.unityMsgInfoList removeObjectAtIndex:G_VAL_0_INT];
		}
	}
}

/** iOS 플러그인 */
@implementation CiOSPlugin
#pragma mark - 프로퍼티
@synthesize impactGeneratorList = m_pImpactGeneratorList;
@synthesize unityMsgInfoList = m_pUnityMsgInfoList;
@synthesize unityMsgHandlerDict = m_pUnityMsgHandlerDict;

@synthesize keychainItemWrapper = m_pKeychainItemWrapper;
@synthesize activityIndicatorView = m_pActivityIndicatorView;

@synthesize selectionGenerator = m_pSelectionGenerator;
@synthesize notificationGenerator = m_pNotificationGenerator;

#pragma mark - 인터페이스
/** 메일이 완료 되었을 경우 */
- (void)mailComposeController:(MFMailComposeViewController *)a_pSender didFinishWithResult:(MFMailComposeResult)a_eResult error:(NSError *)a_pError {
	[a_pSender dismissViewControllerAnimated:YES completion:NULL];
}

#pragma mark - 초기화
/** 객체를 생성한다 */
+ (id)alloc {
	@synchronized(CiOSPlugin.class) {
		// 인스턴스가 없을 경우
		if(g_pInst == nil) {
			g_pInst = [[super alloc] init];
		}
	}
	
	return g_pInst;
}

#pragma mark - 인스턴스 메서드
/** 디바이스 식별자를 반환한다 */
- (NSString *)deviceID {
	// 디바이스 식별자가 유효하지 않을 경우
	if(!GFunc::IsValid((NSString *)[self.keychainItemWrapper objectForKey:(__bridge id)kSecAttrAccount])) {
		[self.keychainItemWrapper setObject:UIDevice.currentDevice.identifierForVendor.UUIDString forKey:(__bridge id)kSecAttrAccount];
	}
	
	return (NSString *)[self.keychainItemWrapper objectForKey:(__bridge id)kSecAttrAccount];
}

/** 유니티 메세지 정보를 반환한다 */
- (NSMutableArray *)unityMsgInfoList {
	// 유니티 메세지 정보가 없을 경우
	if(m_pUnityMsgInfoList == nil) {
		m_pUnityMsgInfoList = [[NSMutableArray alloc] init];
	}
	
	return m_pUnityMsgInfoList;
}

/** 유니티 메세지 처리자를 반환한다 */
- (NSDictionary *)unityMsgHandlerDict {
	// 유니티 메세지 처리자가 없을 경우
	if(m_pUnityMsgHandlerDict == nil) {
		NSMutableDictionary *pMsgHandlerDict = [[NSMutableDictionary alloc] init];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveGetDeviceIDMsg:)) forKey:@(G_CMD_GET_DEVICE_ID)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveGetCountryCodeMsg:)) forKey:@(G_CMD_GET_COUNTRY_CODE)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveGetStoreVerMsg:)) forKey:@(G_CMD_GET_STORE_VER)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveSetEnableAdsTrackingMsg:)) forKey:@(G_CMD_SET_ENABLE_ADS_TRACKING)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveShowAlertMsg:)) forKey:@(G_CMD_SHOW_ALERT)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveShowToastMsg:)) forKey:@(G_CMD_SHOW_TOAST)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveMailMsg:)) forKey:@(G_CMD_MAIL)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveVibrateMsg:)) forKey:@(G_CMD_VIBRATE)];
		[pMsgHandlerDict setObject:NSStringFromSelector(@selector(onReceiveIndicatorMsg:)) forKey:@(G_CMD_INDICATOR)];
		
		m_pUnityMsgHandlerDict = pMsgHandlerDict;
	}
	
	return m_pUnityMsgHandlerDict;
}

/** 키체인 아이템 래퍼를 반환한다 */
- (KeychainItemWrapper *)keychainItemWrapper {
	// 키체인 아이템 래퍼가 없을 경우
	if(m_pKeychainItemWrapper == nil) {
		m_pKeychainItemWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:@(G_ID_KEYCHAIN_DEVICE) accessGroup:nil];
	}
	
	return m_pKeychainItemWrapper;
}

/** 액티비티 인디케이터 뷰를 반환한다 */
- (UIActivityIndicatorView *)activityIndicatorView {
	// 인디케이터가 없을 경우
	if(m_pActivityIndicatorView == nil) {
		UIActivityIndicatorViewStyle eIndicatorViewStyle = UIActivityIndicatorViewStyleWhiteLarge;
		
		// 신규 인디케이터를 지원 할 경우
		if(@available(iOS G_MIN_VER_INDICATOR, *)) {
			eIndicatorViewStyle = UIActivityIndicatorViewStyleLarge;
		}
		
		m_pActivityIndicatorView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:eIndicatorViewStyle];
		m_pActivityIndicatorView.color = [UIColor colorWithWhite:G_VAL_1_REAL alpha:G_VAL_1_REAL];
		m_pActivityIndicatorView.center = self.rootViewController.view.center;
		m_pActivityIndicatorView.hidesWhenStopped = YES;
		
		// 위치를 설정한다 {
		float fSize = MIN(self.rootViewController.view.bounds.size.width, self.rootViewController.view.bounds.size.height) * G_SCALE_ACTIVITY_INDICATOR;
		CGAffineTransform stTransform = CGAffineTransformScale(m_pActivityIndicatorView.transform, fSize / m_pActivityIndicatorView.bounds.size.width, fSize / m_pActivityIndicatorView.bounds.size.height);
		
		m_pActivityIndicatorView.transform = CGAffineTransformTranslate(stTransform, G_VAL_0_REAL, MIN(self.rootViewController.view.bounds.size.width, self.rootViewController.view.bounds.size.height) * -G_OFFSET_SCALE_ACTIVITY_INDICATOR);
		[self.rootViewController.view addSubview:m_pActivityIndicatorView];
		// 위치를 설정한다 }
	}
	
	return m_pActivityIndicatorView;
}

/** 충격 피드백 생성자를 반환한다 */
- (NSArray *)impactGeneratorList {
	// 충격 피드백 생성자가 없을 경우
	if(m_pImpactGeneratorList == nil) {
		UIImpactFeedbackGenerator *pLightGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
		UIImpactFeedbackGenerator *pMediumGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
		UIImpactFeedbackGenerator *pHeavyGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
		
		m_pImpactGeneratorList = [[NSArray alloc] initWithObjects:pLightGenerator, pMediumGenerator, pHeavyGenerator, nil];
	}
	
	return m_pImpactGeneratorList;
}

/** 선택 피드백 생성자를 반환한다 */
- (UISelectionFeedbackGenerator *)selectionGenerator {
	// 선택 피드백 생성자가 없을 경우
	if(m_pSelectionGenerator == nil) {
		m_pSelectionGenerator = [[UISelectionFeedbackGenerator alloc] init];
	}
	
	return m_pSelectionGenerator;
}

/** 경고 피드백 생성자를 반환한다 */
- (UINotificationFeedbackGenerator *)notificationGenerator {
	// 경고 피드백 생성자가 없을 경우
	if(m_pNotificationGenerator == nil) {
		m_pNotificationGenerator = [[UINotificationFeedbackGenerator alloc] init];
	}
	
	return m_pNotificationGenerator;
}

/** 루트 뷰 컨트롤러를 반환한다 */
- (UIViewController *)rootViewController {
	return self.unityAppController.rootViewController;
}

/** 유니티 앱 컨트롤러를 반환한다 */
- (UnityAppController *)unityAppController {
	return (UnityAppController *)UIApplication.sharedApplication.delegate;
}

/** 디바이스 식별자 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetDeviceIDMsg:(NSString *)a_pMsg {
	[CDeviceMsgSender.sharedInst sendGetDeviceIDMsg:self.deviceID];
}

/** 국가 코드 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetCountryCodeMsg:(NSString *)a_pMsg {
	[CDeviceMsgSender.sharedInst sendGetCountryCodeMsg:NSLocale.currentLocale.countryCode];
}

/** 스토어 버전 반환 메세지를 수신했을 경우 */
- (void)onReceiveGetStoreVerMsg:(NSString *)a_pMsg {
	NSDictionary *pDataDict = (NSDictionary *)GFunc::ConvertJSONStrToObj(a_pMsg, NULL);
	NSMutableURLRequest *pURLRequest = GFunc::MakeURLRequest([NSString stringWithFormat:@(G_URL_FMT_STORE_VER), (NSString *)[pDataDict objectForKey:@(G_KEY_APP_ID)]], @(G_HTTP_METHOD_GET), ((NSString *)[pDataDict objectForKey:@(G_KEY_TIMEOUT)]).doubleValue);
	
	// 데이터를 수신했을 경우
	[NSURLSession.sharedSession dataTaskWithRequest:pURLRequest completionHandler:^void(NSData *a_pData, NSURLResponse *a_pResponse, NSError *a_pError) {
		NSLog(@"CiOSPlugin.onReceiveGetStoreVerMsg: %@", a_pData);
		
		// 스토어 버전 로드에 실패했을 경우
		if(a_pError != nil || (a_pData == nil || a_pResponse == nil)) {
			NSLog(@"CiOSPlugin.onReceiveGetStoreVerMsg Fail: %@", a_pError);
			[CDeviceMsgSender.sharedInst sendGetStoreVerMsg:(NSString *)[pDataDict objectForKey:@(G_KEY_VER)] withResult:NO];
		} else {
			NSArray *pVerInfoList = (NSArray *)[(NSDictionary *)GFunc::ConvertJSONStrToObj([[NSString alloc] initWithData:a_pData encoding:NSUTF8StringEncoding], NULL) objectForKey:@(G_KEY_STORE_VER_RESULT)];
			NSDictionary *pVerInfoDict = (NSDictionary *)[pVerInfoList lastObject];
			NSString *pStoreVer = (NSString *)[pVerInfoDict objectForKey:@(G_KEY_STORE_VER)];
			
			NSLog(@"CiOSPlugin.onReceiveGetStoreVerMsg Success: %@", pStoreVer);
			
			// 스토어 버전이 유효 할 경우
			if(GFunc::IsValid(pStoreVer)) {
				[CDeviceMsgSender.sharedInst sendGetStoreVerMsg:pStoreVer withResult:YES];
			} else {
				[CDeviceMsgSender.sharedInst sendGetStoreVerMsg:(NSString *)[pDataDict objectForKey:@(G_KEY_VER)] withResult:NO];
			}
		}
	}];
}

/** 광고 추적 여부 변경 메세지를 수신했을 경우 */
- (void)onReceiveSetEnableAdsTrackingMsg:(NSString *)a_pMsg {
#if defined IRON_SRC_ADS_ENABLE
	[FBAdSettings setAdvertiserTrackingEnabled:GFunc::ConvertStrToBool(a_pMsg)];
#endif			// #if defined IRON_SRC_ADS_ENABLE
}

/** 경고 창 출력 메세지를 수신했을 경우 */
- (void)onReceiveShowAlertMsg:(NSString *)a_pMsg {
	NSDictionary *pDataDict = (NSDictionary *)GFunc::ConvertJSONStrToObj(a_pMsg, NULL);
	UIAlertController *pAlertController = [UIAlertController alertControllerWithTitle:(NSString *)[pDataDict objectForKey:@(G_KEY_ALERT_TITLE)] message:(NSString *)[pDataDict objectForKey:@(G_KEY_ALERT_MSG)] preferredStyle:UIAlertControllerStyleAlert];
	
	NSString *pCancelBtnText = (NSString *)[pDataDict objectForKey:@(G_KEY_ALERT_CANCEL_BTN_TEXT)];

	// 확인 버튼을 눌렀을 경우
	[pAlertController addAction:[UIAlertAction actionWithTitle:(NSString *)[pDataDict objectForKey:@(G_KEY_ALERT_OK_BTN_TEXT)] style:UIAlertActionStyleDefault handler:^void(UIAlertAction *a_pSender) {
		[CDeviceMsgSender.sharedInst sendShowAlertMsg:YES];
	}]];

	// 취소 버튼 텍스트 존재 할 경우
	if(GFunc::IsValid(pCancelBtnText)) {
		// 취소 버튼을 눌렀을 경우
		[pAlertController addAction:[UIAlertAction actionWithTitle:pCancelBtnText style:UIAlertActionStyleCancel handler:^void(UIAlertAction *a_pSender) {
			[CDeviceMsgSender.sharedInst sendShowAlertMsg:NO];
		}]];
	}
	
	[self.rootViewController presentViewController:pAlertController animated:YES completion:NULL];
}

/** 토스트 출력 메세지를 수신했을 경우 */
- (void)onReceiveShowToastMsg:(NSString *)a_pMsg {
	// Do Something
}

/** 메일 메세지를 수신했을 경우 */
- (void)onReceiveMailMsg:(NSString *)a_pMsg {
	NSDictionary *pDataDict = (NSDictionary *)GFunc::ConvertJSONStrToObj(a_pMsg, NULL);
	
	// 메일 전송이 가능 할 경우
	if([MFMailComposeViewController canSendMail]) {
		MFMailComposeViewController *pMailViewController = [[MFMailComposeViewController alloc] init];
		pMailViewController.mailComposeDelegate = self;
		
		[pMailViewController setSubject:(NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_TITLE)]];
		[pMailViewController setMessageBody:(NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_MSG)] isHTML:NO];
		[pMailViewController setToRecipients:[NSArray arrayWithObjects:(NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_RECIPIENT)], nil]];
		
		[self.rootViewController presentViewController:pMailViewController animated:YES completion:NULL];
	} else {
		NSString *pTitle = [(NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_TITLE)] stringByAddingPercentEncodingWithAllowedCharacters:NSCharacterSet.URLUserAllowedCharacterSet];
		NSString *pMsg = [(NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_MSG)] stringByAddingPercentEncodingWithAllowedCharacters:NSCharacterSet.URLUserAllowedCharacterSet];		
		NSString *pURL = [NSString stringWithFormat:@(G_URL_FMT_MAIL), (NSString *)[pDataDict objectForKey:@(G_KEY_MAIL_RECIPIENT)], pTitle, pMsg, nil];
		
		[UIApplication.sharedApplication openURL:[NSURL URLWithString:pURL] options:G_EMPTY_DICT completionHandler:nil];
	}
}

/** 진동 메세지를 수신했을 경우 */
- (void)onReceiveVibrateMsg:(NSString *)a_pMsg {
	NSDictionary *pDataDict = (NSDictionary *)GFunc::ConvertJSONStrToObj(a_pMsg, NULL);
	
	EVibrateType eVibrateType = (EVibrateType)((NSString *)[pDataDict objectForKey:@(G_KEY_VIBRATE_TYPE)]).intValue;
	EVibrateStyle eVibrateStyle = (EVibrateStyle)((NSString *)[pDataDict objectForKey:@(G_KEY_VIBRATE_STYLE)]).intValue;
	
	// 진동 타입이 유효 할 경우
	if(GFunc::IsValid(eVibrateType)) {
		// 햅틱 진동을 지원 할 경우
		if(@available(iOS G_MIN_VER_FEEDBACK_GENERATOR, *)) {
			switch(eVibrateType) {
				case EVibrateType::SELECTION: [self.selectionGenerator selectionChanged]; break;
				case EVibrateType::NOTIFICATION: [self.notificationGenerator notificationOccurred:(UINotificationFeedbackType)eVibrateStyle]; break;
				case EVibrateType::IMPACT: [self onReceiveImpactVibrateMsg:pDataDict withVibrateStyle:eVibrateStyle]; break;
			}
		} else {
			AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
		}
	}
}

/** 인디케이터 메세지를 수신했을 경우 */
- (void)onReceiveIndicatorMsg:(NSString *)a_pMsg {
	// 출력 모드 일 경우
	if(GFunc::ConvertStrToBool(a_pMsg)) {
		[self.activityIndicatorView startAnimating];
	} else {
		[self.activityIndicatorView stopAnimating];
	}
}

/** 임팩트 진동 메세지를 수신했을 경우 */
- (void)onReceiveImpactVibrateMsg:(NSDictionary *)a_pDataDict withVibrateStyle:(EVibrateStyle)a_eVibrateStyle {
	float fIntensity = ((NSString *)[a_pDataDict objectForKey:@(G_KEY_VIBRATE_INTENSITY)]).floatValue;
	UIImpactFeedbackGenerator *pImpactGenerator = (UIImpactFeedbackGenerator *)[self.impactGeneratorList objectAtIndex:(UIImpactFeedbackStyle)a_eVibrateStyle];
	
	// 진동 세기를 지원 할 경우
	if(@available(iOS G_MIN_VER_IMPACT_INTENSITY, *)) {
		[pImpactGenerator impactOccurredWithIntensity:fIntensity];
	} else {
		[pImpactGenerator impactOccurred];
	}
}

#pragma mark - 클래스 메서드
/** 인스턴스를 반환한다 */
+ (instancetype)sharedInst {
	@synchronized(CiOSPlugin.class) {
		// 인스턴스가 없을 경우
		if(g_pInst == nil) {
			g_pInst = [[CiOSPlugin alloc] init];
		}
	}
	
	return g_pInst;
}
@end			// CiOSPlugin

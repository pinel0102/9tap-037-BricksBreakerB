//
//  CiOSPlugin.h
//  Unity-iPhone
//
//  Created by 이상동 on 2020/02/26.
//
#pragma once

#import "Global/Define/KGDefine.h"
#import "Global/Utility/External/Keychain/KeychainItemWrapper.h"
#import "../UnityAppController.h"

NS_ASSUME_NONNULL_BEGIN

/** iOS 플러그인 */
@interface CiOSPlugin : NSObject <MFMailComposeViewControllerDelegate> {
	NSArray *m_pImpactGeneratorList;
	NSMutableArray *m_pUnityMsgInfoList;
	NSDictionary *m_pUnityMsgHandlerDict;
	
	KeychainItemWrapper *m_pKeychainItemWrapper;
	UIActivityIndicatorView *m_pActivityIndicatorView;
	
	UISelectionFeedbackGenerator *m_pSelectionGenerator;
	UINotificationFeedbackGenerator *m_pNotificationGenerator;
}

// 프로퍼티 {
@property (nonatomic, copy, readonly) NSString *deviceID;
@property (nonatomic, strong, readonly) NSArray *impactGeneratorList;
@property (nonatomic, strong, readonly) NSMutableArray *unityMsgInfoList;
@property (nonatomic, strong, readonly) NSDictionary *unityMsgHandlerDict;

@property (nonatomic, strong, readonly) KeychainItemWrapper *keychainItemWrapper;
@property (nonatomic, strong, readonly) UIActivityIndicatorView *activityIndicatorView;

@property (nonatomic, strong, readonly) UISelectionFeedbackGenerator *selectionGenerator;
@property (nonatomic, strong, readonly) UINotificationFeedbackGenerator *notificationGenerator;

@property (nonatomic, strong, readonly) UIViewController *rootViewController;
@property (nonatomic, strong, readonly) UnityAppController *unityAppController;
// 프로퍼티 }

/** 인스턴스를 반환한다 */
+ (instancetype)sharedInst;
@end			// CiOSPlugin

NS_ASSUME_NONNULL_END

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public partial class CUserInfoStorage
{
    [Header("★ [Parameter] Subscription")]
    public bool subs_isActivate;
    public DateTime subs_ExpireDate;
    public TimeSpan subs_RemainingTime;

    public void InitSubscriptionInfo()
    {
        subs_isActivate = false;
        subs_ExpireDate = DateTime.Today;
        subs_RemainingTime = TimeSpan.Zero;
    }

    public void SetSubscriptionInfo(SubscriptionInfo info)
    {
        /*Debug.Log("product id is: " + info.getProductId());
        Debug.Log("purchase date is: " + info.getPurchaseDate());
        Debug.Log("subscription next billing date is: " + info.getExpireDate());
        Debug.Log("is subscribed? " + info.isSubscribed().ToString());
        Debug.Log("is expired? " + info.isExpired().ToString());
        Debug.Log("is cancelled? " + info.isCancelled());
        Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
        Debug.Log("product is auto renewing? " + info.isAutoRenewing());
        Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
        Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
        Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
        Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
        Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());*/

        subs_isActivate = true;
        subs_ExpireDate = info.getExpireDate();
        subs_RemainingTime = info.getRemainingTime();
    }
}
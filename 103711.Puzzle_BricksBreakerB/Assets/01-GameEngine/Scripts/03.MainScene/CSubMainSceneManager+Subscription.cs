using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Purchasing;
#endif // #if UNITY_EDITOR

namespace MainScene {
    public partial class CSubMainSceneManager
    {
        private CPurchaseManager cPurchaseManager { get { return CPurchaseManager.Inst; } }
        private Coroutine cCheckSubscriptions;

        public void InitSubscriptions()
        {
            Debug.Log(CodeManager.GetMethodName());

            if (cCheckSubscriptions != null) StopCoroutine(cCheckSubscriptions);
            cCheckSubscriptions = StartCoroutine(CO_CheckSubscriptions());
        }

        private IEnumerator CO_CheckSubscriptions()
        {
            CUserInfoStorage.Inst.InitSubscriptionInfo();

            yield return null;

#if UNITY_EDITOR
            RefreshSubscriptions();
#else
            while(!cPurchaseManager.IsInit)
            {
                yield return null;
            }

            CheckSubscriptions();
            RefreshSubscriptions();
#endif
        }

        private void CheckSubscriptions()
        {
            Debug.Log(CodeManager.GetMethodName());

            var productTradeInfoList = Factory.MakeProductTradeInfos(KDefine.G_PRODUCT_KINDS_STORE_LIST).Values.ToList();
            
            for(int i=0; i < productTradeInfoList.Count; i++)
            {
                var stProductInfo = CProductInfoTable.Inst.GetProductInfo(productTradeInfoList[i].m_nProductIdx);
                if (stProductInfo.m_eProductType == ProductType.Subscription)
                {
                    Product _product = cPurchaseManager.GetProduct(stProductInfo.m_oID);
                    if (_product != null)
                    {
                        SubscriptionInfo info = GetSubscriptionInfo(_product);
                        switch(info.isSubscribed())
                        {
                            case Result.True:
                                CUserInfoStorage.Inst.SetSubscriptionInfo(info);
                                break;
                        }
                    }
                }
            }
        }

        private void RefreshSubscriptions()
        {
            if (!GlobalDefine.isLevelEditor)
            {
                Debug.Log(CodeManager.GetMethodName());
            
                CGameInfoStorage gameInfoStorage = CGameInfoStorage.Inst;

                if (Access.IsEnableSubscriptionAlert(gameInfoStorage.PlayCharacterID))
                {
                    CCharacterGameInfo characterGameinfo = gameInfoStorage.GetCharacterGameInfo(gameInfoStorage.PlayCharacterID);
                    characterGameinfo.SubscriptionAlertTime = DateTime.Today;
                    gameInfoStorage.SaveGameInfo();

                    if (CUserInfoStorage.Inst.subs_isActivate)
                    {
                        GetSubscriptionRewards();
                    }
                    else
                    {
                        OnClick_OpenPopup_Subscription();
                    }
                }
            }
        }

        private void GetSubscriptionRewards()
        {
            Debug.Log(CodeManager.GetMethodName());

            //
        }

        private SubscriptionInfo GetSubscriptionInfo(string _productID)
        {
            return GetSubscriptionInfo(cPurchaseManager.GetProduct(_productID));
        }

        private SubscriptionInfo GetSubscriptionInfo(Product _product)
        {
            if (_product.definition.type == ProductType.Subscription)
            {
                SubscriptionManager p = new SubscriptionManager(_product, null);
                SubscriptionInfo info = p.getSubscriptionInfo();
                return info;
            }
            else
            {
                Debug.LogWarning(CodeManager.GetMethodName() + string.Format("{0} : {1}", _product.definition.id, _product.definition.type));
                return null;
            }
        }
    }
}

#endif
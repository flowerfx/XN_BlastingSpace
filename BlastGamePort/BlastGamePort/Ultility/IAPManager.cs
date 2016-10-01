using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if ! OS_W8
using StoreLauncher;
#endif
namespace BlastGamePort
{
    public class IAPManager
    {
        public static List<string> StoreItems = new List<string>();
#if ! OS_W8
        public static StoreBase _store;
#endif
        public static bool _isStoreEnabled = false;
        //
        public static string CurrentReceipt = "";
        //
        private static int NumberGetItemStore = 0;
        public static bool IsGetAllItemSuccess
        {
            get { 
#if ! OS_W8
                return NumberGetItemStore == StoreItems.Count; 
#else
                return true;
#endif
            }
        }
        //
        public static bool IsOnPurchaseItemProcess = false;
        public static bool ResultPurchaseItem = false;
        //
        public static void OnInitialize()
        {
#if ! OS_W8
            if (Environment.OSVersion.Version.Major >= 8)
            {
                _store = StoreLauncher.StoreLauncher.GetStoreInterface("StoreWrapper.Store, StoreWrapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            }
            //
            if (_store != null)
            {
                StoreItems.Add("Small_Coin_Pack");
                StoreItems.Add("Medium_Coin_Pack");
                StoreItems.Add("Mega_Coin_Pack");

                _isStoreEnabled = true;
                //
                var productLicenses = _store.LicenseInformation.ProductLicenses;
                for (int i = 0; i < StoreItems.Count; i++)
                {
                    if (!productLicenses.ContainsKey(StoreItems[i]))
                        continue;
                    if (productLicenses[StoreItems[i]].IsConsumable && productLicenses[StoreItems[i]].IsActive)
                    {
                        Game1.GlobalCash += IAPMenu.ValuePacks[i];
                        _store.ReportProductFulfillment(StoreItems[i]);
                        // and store the data in the app if needed
                    }                  
                }
                //
                if (productLicenses.Count > 0)
                    SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());

            }
            else
            {
                _isStoreEnabled = false;
                return;
            }
            //
#endif
        }
        //
        public static void OnGetInventory()
        {
#if ! OS_W8
            if (!_isStoreEnabled)
                return;

            PopUp.OnShow(12, 0);

            NumberGetItemStore = 0;

            var productListAsync = _store.LoadListingInformationAsync();
            productListAsync.Completed = (async, status) =>
                {
                    PopUp.OnClose();
                    try
                    {
                        foreach (string key in productListAsync.GetResults().ProductListings.Keys)
                        {
                            int count = _store.LicenseInformation.ProductLicenses.Count;
                            if (StoreItems.Contains(key))
                            {
                                NumberGetItemStore++;
                                if(StoreItems[0] == key)
                                    IAPMenu.Instance.ListPrices[0] = (productListAsync.GetResults().ProductListings[key].FormattedPrice);
                                if (StoreItems[1] == key)
                                    IAPMenu.Instance.ListPrices[1] = (productListAsync.GetResults().ProductListings[key].FormattedPrice);
                                if (StoreItems[2] == key)
                                    IAPMenu.Instance.ListPrices[2] = (productListAsync.GetResults().ProductListings[key].FormattedPrice);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        NumberGetItemStore = 0;
                        PopUp.OnShow(13, 1);
                        IAPMenu.Instance.CurrentStatePopUp = 2;
                    }
                };
#endif
       }

        public static void OnPurchaseItem(int idx)
        {
#if ! OS_W8
            if (!_isStoreEnabled)
                return;
            string _key = StoreItems[idx];
            PopUp.OnShow(12, 0);
            IsOnPurchaseItemProcess = true;
            string pID = _store.mListingInformation.ProductListings[_key].ProductId;
            var requestPurchaseAsync = _store.RequestProductPurchaseAsync(pID, false);

            requestPurchaseAsync.Completed = (_async, _status) =>
            {
                try
                {
                    PopUp.OnClose();
                    if (_status == StoreAsyncStatus.Completed)
                    {
                        IsOnPurchaseItemProcess = false;
                        IAPMenu.Instance.OnPurchaseItem(true);
                    }
                    else
                    {
                        IsOnPurchaseItemProcess = false;
                        IAPMenu.Instance.OnPurchaseItem(false);
                    }
                }
                catch (Exception e)
                {
                    IsOnPurchaseItemProcess = false;
                    IAPMenu.Instance.OnPurchaseItem(false);
                }
            };
#endif
        }

        public static void DoFulfillment(int goldCount, string key)
        {
            var productLicenses = _store.LicenseInformation.ProductLicenses;

            // Check fulfillment for consumable products with variable asset counts
            Game1.GlobalCash += goldCount;
            if (!productLicenses.ContainsKey(key))
                return;
            if (productLicenses[key].IsConsumable && productLicenses[key].IsActive)
            {
                // Report item fulfilled, so it can be purchased again
                _store.ReportProductFulfillment(key);
            }          
        }        
    }
}

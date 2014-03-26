#if UNITY_ANDROID
using UnityEngine;

// Example script showing how you can easily call into the AdMobPlugin.
public class AdMobPluginDemoScriptAndroid : MonoBehaviour {

    void Start()
    {
		print("Started");
		AdMobPluginAndroid.CreateBannerView("a1533275dd5e219",
		                                    AdMobPluginAndroid.AndroidAdSize.SmartBanner,
                                     false);
        print("Created Banner View");
		AdMobPluginAndroid.RequestBannerAd(true);
        print("Requested Banner Ad");
    }

    void OnEnable()
    {
		AdMobPluginAndroid.ShowBannerView();
		print("Registering for AdMob Events");
		AdMobPluginAndroid.ReceivedAd += HandleReceivedAd;
		AdMobPluginAndroid.FailedToReceiveAd += HandleFailedToReceiveAd;
		AdMobPluginAndroid.ShowingOverlay += HandleShowingOverlay;
		AdMobPluginAndroid.DismissedOverlay += HandleDismissedOverlay;
		AdMobPluginAndroid.LeavingApplication += HandleLeavingApplication;
    }

    void OnDisable()
    {
		AdMobPluginAndroid.HideBannerView();
        print("Unregistering for AdMob Events");
		AdMobPluginAndroid.ReceivedAd -= HandleReceivedAd;
		AdMobPluginAndroid.FailedToReceiveAd -= HandleFailedToReceiveAd;
		AdMobPluginAndroid.ShowingOverlay -= HandleShowingOverlay;
		AdMobPluginAndroid.DismissedOverlay -= HandleDismissedOverlay;
		AdMobPluginAndroid.LeavingApplication -= HandleLeavingApplication;
    }

    public void HandleReceivedAd()
    {
        print("HandleReceivedAd event received");
    }

    public void HandleFailedToReceiveAd(string message)
    {
        print("HandleFailedToReceiveAd event received with message:");
        print(message);
    }

    public void HandleShowingOverlay()
    {
        print("HandleShowingOverlay event received");
    }

    public void HandleDismissedOverlay()
    {
        print("HandleDismissedOverlay event received");
    }

    public void HandleLeavingApplication()
    {
        print("HandleLeavingApplication event received");
    }
}
#endif
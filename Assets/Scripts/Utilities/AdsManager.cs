// Created by Binh Bui on 06, 27, 2021

using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class AdsManager : MonoBehaviour
    {
        private RewardedAd _rewardedAd;
        public UnityAction OnEarnedReward;

        public static AdsManager Instance { get; private set; }

        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            MobileAds.Initialize(initStatus =>
            {
                Debug.Log(initStatus);
            });
            string adUnitId;
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-6254144234932394/6106182426";
            // test id: "ca-app-pub-3940256099942544/5224354917"
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

            _rewardedAd = new RewardedAd(adUnitId);

            // Called when an ad request has successfully loaded.
            _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            _rewardedAd.OnAdClosed += HandleRewardedAdClosed;

            RequestRewardedAd();
        }

        private void RequestRewardedAd()
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            _rewardedAd.LoadAd(request);
        }

        public void ShowReward()
        {
            if (_rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();
            }
        }

        private void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdLoaded event received");
        }

        private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log($"HandleRewardedAdFailedToLoad event received with message: {args.LoadAdError}");
        }

        private void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdOpening event received");
        }

        private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.Log($"HandleRewardedAdFailedToShow event received with message: {args.AdError.GetMessage()}");
        }

        private void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            RequestRewardedAd();
        }

        private void HandleUserEarnedReward(object sender, Reward args)
        {
            var type = args.Type;
            var amount = args.Amount;
            Debug.Log($"HandleRewardedAdRewarded event received for {amount} {type}");
        }
    }
}
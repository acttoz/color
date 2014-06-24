using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class ADS : MonoBehaviour
{
		InterstitialAd interstitial; 
		// Use this for initialization
		void Start ()
		{
				// Initialize an InterstitialAd.
//				interstitial = new InterstitialAd ("ca-app-pub-6805343954393346/2888331311");
//				// Register for ad events.
//				interstitial.AdLoaded += delegate(object sender, System.EventArgs args) {
//				};
////				interstitial.AdFailedToLoad += delegate(object sender, AdFailToLoadEventArgs args) {
////				};
//				interstitial.AdOpened += delegate(object sender, System.EventArgs args) {
//				};
//				interstitial.AdClosing += delegate(object sender, System.EventArgs args) {
//				};
//				interstitial.AdClosed += delegate(object sender, System.EventArgs args) {
////						interstitial.Destroy ();
////						interstitial = CreateAndLoadInterstitial ();
//				};
//		 
//				interstitial.AdLeftApplication += delegate(object sender, System.EventArgs args) {
//				};
//				// Load the InterstitialAd with an AdRequest.
//				interstitial.LoadAd (new AdRequest.Builder ().Build ());

				// Initialize an InterstitialAd.
				InterstitialAd interstitial = new InterstitialAd ("ca-app-pub-6805343954393346/2888331311");
				// Create an empty ad request.
				AdRequest request = new AdRequest.Builder ().Build ();
				// Load the interstitial with the request.
				interstitial.LoadAd (request);
		}

		 
				
		// Update is called once per frame
		public void showAd ()
		{
//				if (interstitial.isLoaded ()) {
				interstitial.Show ();
//				}
		}


}

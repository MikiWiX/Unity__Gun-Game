using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class Community : MonoBehaviour
{
    public static Community Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!", gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        Debug.Log("Init");
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    public static void FeedShareFB()
    {
        Debug.Log("Share");
        FB.FeedShare(
            string.Empty,
            new Uri("https://developers.facebook.com/"),
            "Test Title",
            "Test caption",
            "Test Description",
            new Uri("http://i.imgur.com/zkYlB.jpg"),
            string.Empty, 
            ShareCallback);
    }

    public static void LinkShareFB()
    {
        Debug.Log("Share");
        FB.ShareLink(
            new Uri("https://developers.facebook.com/"),
            "Test Title",
            "Test Description",
            new Uri("http://i.imgur.com/zkYlB.jpg"),
            ShareCallback);
    }

    private static void ShareCallback (IShareResult result) {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
            Debug.Log("ShareLink Error: "+result.Error);
        } else if (!String.IsNullOrEmpty(result.PostId)) {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        } else {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public class LoadAB : MonoBehaviour {

    // Use this for initialization
    string path = "AssetBundles";
    string pathCapsule = "AssetBundles/capsulewall.unity3d";
    string pathShare = "AssetBundles/share.unity3d";
    string capsuleWall = "CapsuleWall";
    void Start () {
        LoadDependeciesManifest();
        //0
        //AssetBundle ab1 = AssetBundle.LoadFromFile(path);
        //AssetBundle ab2 = AssetBundle.LoadFromFile(pathShare);
        //GameObject go1 = ab1.LoadAsset<GameObject>("CapsuleWall");        
        //Instantiate(go1);
        //1
        //StartCoroutine(LoadFromMemoryAsync(pathCapsule));
        //2
        //StartCoroutine("LoadUsedWWW");
        //3
        StartCoroutine("LoadUsedWebRequest");
    }

    // Update is called once per frame
    void Update () {

	}

    IEnumerator LoadFromMemoryAsync(string path)
    {
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        yield return createRequest;
        AssetBundle ab = createRequest.assetBundle;
        var gameObject = ab.LoadAsset<GameObject>(capsuleWall);
        Instantiate(gameObject);
    }
    IEnumerator LoadUsedWWW()
    {
        while (!Caching.ready)
        {
            yield return null;
        }
        //local
        //var www = WWW.LoadFromCacheOrDownload(@"file:///E:\U3D_Workplace\源码工程、PPT\源码工程、PPT\AssetBundleProject\AssetBundleProject\AssetBundles\capsulewall.unity3d", 1);
        //web
        var www = WWW.LoadFromCacheOrDownload(@"http://localhost/AssetBundles/capsulewall.unity3d",1);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        var myLoadedAB = www.assetBundle;
        var gameObject = myLoadedAB.LoadAsset<GameObject>(capsuleWall);
        Instantiate(gameObject);
    }
    IEnumerator LoadUsedWebRequest()
    {
        string url = "file:///E:/git_U3D/AssetBundleProject/AssetBundleProject/AssetBundles/capsulewall.unity3d";
        //string url = "http://localhost/AssetBundles/capsulewall.unity3d";
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(url);
        yield return request.SendWebRequest();
        //1
        //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //2
        AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        //var gameObject = ab.LoadAsset<GameObject>(capsuleWall);
        AssetBundleRequest abRequest = ab.LoadAssetAsync<GameObject>(capsuleWall);        
        yield return abRequest;
        var gameObject = abRequest.asset;
        Instantiate(gameObject);                
    }
    private void LoadDependeciesManifest()
    {
        AssetBundle ab = AssetBundle.LoadFromFile("AssetBundles/AssetBundles");
        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] dependencies = manifest.GetAllDependencies("capsulewall.unity3d");
        foreach (var dependence in dependencies)
        {
            Debug.Log(dependence);
            AssetBundle.LoadFromFile(Path.Combine(path, dependence));
        }        
    }
}

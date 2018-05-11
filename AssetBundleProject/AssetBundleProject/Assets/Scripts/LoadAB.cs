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
        LoadFromFile(pathCapsule);
        //1
        //StartCoroutine(LoadFromMemoryAsync(pathCapsule));
        //2
        //StartCoroutine("LoadUsedWWW");
        //3
        //StartCoroutine("LoadUsedWebRequest");


        //StartCoroutine(LoadFormFileAsync(pathCapsule));
    }

    // Update is called once per frame
    void Update () {

	}
    //本地同步加载
    private void LoadFromFile(string path)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        var go = ab.LoadAsset<GameObject>(capsuleWall);
        Instantiate(go);
    }
    //从本地异步加载
    IEnumerator LoadFormFileAsync(string path)
    {
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(path);
        yield return createRequest;
        AssetBundle ab = createRequest.assetBundle;
        var gameObject = ab.LoadAsset<GameObject>(capsuleWall);
        Instantiate(gameObject);
    }
    //从内存中异步加载
    IEnumerator LoadFromMemoryAsync(string path)
    {
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        yield return createRequest;
        AssetBundle ab = createRequest.assetBundle;
        var gameObject = ab.LoadAsset<GameObject>(capsuleWall);
        Instantiate(gameObject);
    }
    //用WWW加载
    IEnumerator LoadUsedWWW()
    {
        while (!Caching.ready)
        {
            yield return null;
        }
        //local
        //var www = WWW.LoadFromCacheOrDownload(@"file:///E:/git_U3D/AssetBundleProject/AssetBundleProject/AssetBundles/capsulewall.unity3d", 1);
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
    //用UnityWebRequest加载
    IEnumerator LoadUsedWebRequest()
    {
        string url = "file:///E:/git_U3D/AssetBundleProject/AssetBundleProject/AssetBundles/capsulewall.unity3d";
        //string url = "http://localhost/AssetBundles/capsulewall.unity3d
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
        //如同加载ab，加载manifest也是类似的方式
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

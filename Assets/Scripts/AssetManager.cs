using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager
{
    private static AssetManager m_instance;

    public static AssetManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new AssetManager();
            }

            return m_instance;
        }
    }

    public void RequestAsset<T>(string assetName, Action<T> onSuccess, Action onFailure = null)
    {
        var asyncHandle = Addressables.LoadAssetAsync<T>(assetName);
        asyncHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onSuccess?.Invoke(asyncHandle.Result);
            }
            else
            {
                onFailure?.Invoke();
                Debug.Log($"Oh noes! Asset {assetName} failed to load!");
            }
        };
    }

    public void LoadScene(string address)
    {
        Addressables.LoadSceneAsync(address);
    }
}
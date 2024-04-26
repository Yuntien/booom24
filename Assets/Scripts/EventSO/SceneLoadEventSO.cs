using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<AssetReference, bool, LoadState> OnEventRaised;

    public void RaiseEvent(AssetReference sceneToLoad, bool showFadeCanvas, LoadState loadState)
    {
        OnEventRaised?.Invoke(sceneToLoad, showFadeCanvas, loadState);
    }
}

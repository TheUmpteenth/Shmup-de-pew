using UnityEngine;

public class FrontendController : MonoBehaviour
{
    private void Awake()
    {
        InputController.OnFirePressed += PlayRequested;
    }

    private void OnDestroy()
    {
        InputController.OnFirePressed -= PlayRequested;
    }

    private void PlayRequested()
    {
        AssetManager.Instance.LoadScene("Assets/Scenes/MainScene.unity");
        InputController.OnFirePressed -= PlayRequested;
    }
}

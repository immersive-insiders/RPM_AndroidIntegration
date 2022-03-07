using UnityEngine;
using UnityEngine.Events;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class AvatarImporter : MonoBehaviour
{
    [SerializeField] private WebView webView;

    private GameObject importedAvatar;

    public GameObject ImportedAvatar
    {
        get
        {
            return importedAvatar;
        }
        set
        {
            importedAvatar = value;
        }
    }

    private void Start()
    {
        webView.CreateWebView();
        webView.OnAvatarCreated = ImportAvatar;
    }

    private void ImportAvatar(string url)
    {
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(url, null,StoreAvatar);
    }

    private void StoreAvatar(GameObject avatar, AvatarMetaData meta)
    {
        importedAvatar = avatar;
        importedAvatar.transform.localScale = Vector3.one * 0.2f; //scaling it to 0.2 of its original size
        importedAvatar.SetActive(false);
    }

}

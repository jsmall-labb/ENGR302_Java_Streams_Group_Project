using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUIScript : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button joinBtn;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        joinBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}

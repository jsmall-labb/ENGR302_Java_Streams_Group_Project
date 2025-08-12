using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManagerUI
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button JoinBtn;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        JoinBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}

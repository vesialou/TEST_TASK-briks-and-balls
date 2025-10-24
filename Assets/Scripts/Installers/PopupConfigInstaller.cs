using BricksAndBalls.UI.Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/PopupConfigInstaller")]
public class PopupConfigInstaller : ScriptableObjectInstaller<PopupConfigInstaller>
{
    [SerializeField] private PopupRegistry popupRegistry;

    public override void InstallBindings()
    {
        Container.Bind<PopupRegistry>().FromInstance(popupRegistry).AsSingle();
    }
}
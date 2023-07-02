using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] Menu menu;
    public override void InstallBindings()
    {
        Container.Bind<Menu>().FromInstance(menu).AsSingle();
    }
}
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] TestLobby lobby;
    public override void InstallBindings()
    {
        Container.Bind<TestLobby>().FromInstance(lobby).AsSingle();
    }
}
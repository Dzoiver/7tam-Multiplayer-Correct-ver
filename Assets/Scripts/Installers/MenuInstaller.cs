using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] TestLobby lobby;
    [SerializeField] MessageBox mesBox;
    public override void InstallBindings()
    {
        Container.Bind<TestLobby>().FromInstance(lobby).AsSingle();
        Container.Bind<MessageBox>().FromInstance(mesBox).AsSingle();
    }
}
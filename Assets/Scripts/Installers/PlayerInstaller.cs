using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] GameFlow gameflow;
    public override void InstallBindings()
    {
        Container.Bind<GameFlow>().FromInstance(gameflow).AsSingle();
    }
}
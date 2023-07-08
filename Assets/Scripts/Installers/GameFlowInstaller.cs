using UnityEngine;
using Zenject;

public class GameFlowInstaller : MonoInstaller
{
    [SerializeField] TopDownCharacterController player;
    public override void InstallBindings()
    {
        Container.Bind<TopDownCharacterController>().FromComponentInNewPrefab(player).AsSingle();
    }
}
using UnityEngine;
using Zenject;

public class ProjectileInstaller : MonoInstaller
{
    [SerializeField] FloatingJoystick shootingJoystick;
    public override void InstallBindings()
    {
        Container.Bind<FloatingJoystick>().FromInstance(shootingJoystick).AsSingle();
    }
}
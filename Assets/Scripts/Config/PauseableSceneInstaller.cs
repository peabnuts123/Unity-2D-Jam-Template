using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseableSceneInstaller : MonoInstaller
{
    [NotNull]
    public PauseManager singleton_PauseManager;

    public override void InstallBindings()
    {
        // Singletons
        Container.Bind<PauseManager>().FromInstance(singleton_PauseManager);
    }
}

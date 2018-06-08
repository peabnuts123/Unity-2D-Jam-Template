using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MasterInstaller : MonoInstaller
{
    public ThreadedCoroutine singleton_ThreadedCoroutine;

    public override void InstallBindings()
    {
        // Singletons
        Container.Bind<ThreadedCoroutine>().FromInstance(singleton_ThreadedCoroutine);

        // Self References
        Container.Bind<Rigidbody2D>().FromComponentSibling();
    }

}

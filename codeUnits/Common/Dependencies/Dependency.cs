using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dependency : MonoBehaviour
{
    protected virtual void BindAll(MonoBehaviour mono) {    }
    protected void FindAllObjectToBind()
    {

        MonoBehaviour[] monoInScene = FindObjectsByType<MonoBehaviour>(sortMode: FindObjectsSortMode.InstanceID);

        for (int i = 0; i < monoInScene.Length; i++)
        {
            BindAll(monoInScene[i]);

        }
    }
    protected virtual void Bind<T>(MonoBehaviour bindObject, MonoBehaviour target) where T : class
    {
        if (target is IDependency<T>) (target as IDependency<T>).Construct(bindObject as T);
    }

}

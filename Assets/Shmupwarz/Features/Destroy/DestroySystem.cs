using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DestroySystem : IReactiveSystem, ISetPool {
    public TriggerOnEvent trigger { get { return Matcher.Destroy.OnEntityAdded(); } }

    Pool pool;

    public void SetPool(Pool pool) {
        this.pool = pool;
    }

    public void Execute(List<Entity> entities) {

        //Debug.Log("Destroy");

        foreach (var e in entities) {
            pool.DestroyEntity(e);
        }
    }
}


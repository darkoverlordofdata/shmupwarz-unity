using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProcessInputSystem : IReactiveSystem, ISetPool {
    public TriggerOnEvent trigger { get { return Matcher.Input.OnEntityAdded(); } }

    Pool _pool;

    public void SetPool(Pool pool) {
    }

    public void Execute(List<Entity> entities) {

        Debug.Log("Process Input");

    }
}


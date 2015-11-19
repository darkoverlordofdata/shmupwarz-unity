using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem, ISetPool {

    Pool _pool;
    Group _group;

    public void SetPool(Pool pool) {
        _pool = pool;
        _group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Velocity));
    }

    public void Execute() {

        foreach (var e in _group.GetEntities()) {
            e.position.x += (e.velocity.x * (Time.deltaTime/100));
            e.position.y += (e.velocity.y * (Time.deltaTime/100));
            e.position.z += (e.velocity.z * (Time.deltaTime/100));
            
        }
    }
}


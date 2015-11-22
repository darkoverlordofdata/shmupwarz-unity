using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem, ISetPool {

    Group group;

    public void SetPool(Pool pool) {
        group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Velocity));
    }

    public void Execute() {
    
        float delta = Time.deltaTime/100;

        foreach (var e in group.GetEntities()) {
            e.position.x += (e.velocity.x * delta);
            e.position.y += (e.velocity.y * delta);
            e.position.z += (e.velocity.z * delta);
            
        }
    }
}


using Entitas;
using UnityEngine;
using System.Collections.Generic;
public class RemoveOffscreenShipsSystem : ISetPool, IExecuteSystem {

    Group group;

    public void SetPool(Pool pool) {
        //_pool = pool;
        group = pool.GetGroup(Matcher.AllOf(Matcher.Velocity, Matcher.Position, Matcher.Health, Matcher.Bounds));
    }

    public void Execute() {
        foreach (var e in group.GetEntities()) {
            if (e.position.y < (0 - e.bounds.radius)) {
                if (!e.isPlayer) e.isDestroy = true;
            }
        }
    }

}
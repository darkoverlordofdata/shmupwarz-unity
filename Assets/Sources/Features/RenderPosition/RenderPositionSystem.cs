using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public class RenderPositionSystem : IExecuteSystem, ISetPool {

    Pool _pool;
    Group _group;

    public void SetPool(Pool pool) {
        _pool = pool;
        _group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Resource));
    }

    public void Execute() {

        foreach (var e in _group.GetEntities()) {
            var pos = e.position;
            e.view.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }
}


using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public class RenderPositionSystem : IExecuteSystem, ISetPool {

    Group group;

    public void SetPool(Pool pool) {
        group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Resource));
    }

    public void Execute() {

        foreach (var e in group.GetEntities()) {
            var pos = e.position;
            e.view.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }
}


using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public class RemoveViewSystem : IReactiveSystem, ISetPool, IEnsureComponents {
    public TriggerOnEvent trigger { get { return Matcher.Resource.OnEntityRemoved(); } }

    public IMatcher ensureComponents { get { return Matcher.View; } }

    public void SetPool(Pool pool) {
        pool.GetGroup(Matcher.View).OnEntityRemoved += onEntityRemoved;
    }

    void onEntityRemoved(Group group, Entity entity, int index, IComponent component) {
        var viewComponent = (ViewComponent)component;
        var gameObject = viewComponent.gameObject;
        Object.Destroy(gameObject);
    }

    public void Execute(List<Entity> entities) {
        foreach (var e in entities) {
            e.RemoveView();
        }
    }
}

using Entitas;
using System.Collections.Generic;
public class RemoveOffscreenShipsSystem : ISetPool, IExecuteSystem {

    Pool _pool;
    Group _group;

    public void SetPool(Pool pool) {
        //_pool = pool;
        //_group = pool.GetGroup(Matcher.AllOf(Matcher.Velocity, Matcher.Position, Matcher.Health, Matcher.Bounds));
    }

    public void Execute() {
        //foreach (var e in _group.GetEntities()) {
        //}
    }

}
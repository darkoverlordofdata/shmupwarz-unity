using Entitas;
using System.Collections.Generic;
public class ColorAnimationSystem : ISetPool, IExecuteSystem {

    Pool _pool;
    Group _group;

    public void SetPool(Pool pool) {
        //_pool = pool;
        //_group = pool.GetGroup(Matcher.AllOf(Matcher.ColorAnimation));
    }

    public void Execute() {
        //foreach (var e in _group.GetEntities()) {
        //}
    }

}
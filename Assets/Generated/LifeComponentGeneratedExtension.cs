using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public LifeComponent life { get { return (LifeComponent)GetComponent(ComponentIds.Life); } }

        public bool hasLife { get { return HasComponent(ComponentIds.Life); } }

        static readonly Stack<LifeComponent> _lifeComponentPool = new Stack<LifeComponent>();

        public static void ClearLifeComponentPool() {
            _lifeComponentPool.Clear();
        }

        public Entity AddLife(int newCount) {
            var component = _lifeComponentPool.Count > 0 ? _lifeComponentPool.Pop() : new LifeComponent();
            component.count = newCount;
            return AddComponent(ComponentIds.Life, component);
        }

        public Entity ReplaceLife(int newCount) {
            var previousComponent = hasLife ? life : null;
            var component = _lifeComponentPool.Count > 0 ? _lifeComponentPool.Pop() : new LifeComponent();
            component.count = newCount;
            ReplaceComponent(ComponentIds.Life, component);
            if (previousComponent != null) {
                _lifeComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveLife() {
            var component = life;
            RemoveComponent(ComponentIds.Life);
            _lifeComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherLife;

        public static IMatcher Life {
            get {
                if (_matcherLife == null) {
                    _matcherLife = Matcher.AllOf(ComponentIds.Life);
                }

                return _matcherLife;
            }
        }
    }
}

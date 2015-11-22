using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public ExpiresComponent expires { get { return (ExpiresComponent)GetComponent(ComponentIds.Expires); } }

        public bool hasExpires { get { return HasComponent(ComponentIds.Expires); } }

        static readonly Stack<ExpiresComponent> _expiresComponentPool = new Stack<ExpiresComponent>();

        public static void ClearExpiresComponentPool() {
            _expiresComponentPool.Clear();
        }

        public Entity AddExpires(float newDelay) {
            var component = _expiresComponentPool.Count > 0 ? _expiresComponentPool.Pop() : new ExpiresComponent();
            component.delay = newDelay;
            return AddComponent(ComponentIds.Expires, component);
        }

        public Entity ReplaceExpires(float newDelay) {
            var previousComponent = hasExpires ? expires : null;
            var component = _expiresComponentPool.Count > 0 ? _expiresComponentPool.Pop() : new ExpiresComponent();
            component.delay = newDelay;
            ReplaceComponent(ComponentIds.Expires, component);
            if (previousComponent != null) {
                _expiresComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveExpires() {
            var component = expires;
            RemoveComponent(ComponentIds.Expires);
            _expiresComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherExpires;

        public static IMatcher Expires {
            get {
                if (_matcherExpires == null) {
                    _matcherExpires = Matcher.AllOf(ComponentIds.Expires);
                }

                return _matcherExpires;
            }
        }
    }
}

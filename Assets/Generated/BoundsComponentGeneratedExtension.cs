using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public BoundsComponent bounds { get { return (BoundsComponent)GetComponent(ComponentIds.Bounds); } }

        public bool hasBounds { get { return HasComponent(ComponentIds.Bounds); } }

        static readonly Stack<BoundsComponent> _boundsComponentPool = new Stack<BoundsComponent>();

        public static void ClearBoundsComponentPool() {
            _boundsComponentPool.Clear();
        }

        public Entity AddBounds(float newRadius) {
            var component = _boundsComponentPool.Count > 0 ? _boundsComponentPool.Pop() : new BoundsComponent();
            component.radius = newRadius;
            return AddComponent(ComponentIds.Bounds, component);
        }

        public Entity ReplaceBounds(float newRadius) {
            var previousComponent = hasBounds ? bounds : null;
            var component = _boundsComponentPool.Count > 0 ? _boundsComponentPool.Pop() : new BoundsComponent();
            component.radius = newRadius;
            ReplaceComponent(ComponentIds.Bounds, component);
            if (previousComponent != null) {
                _boundsComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveBounds() {
            var component = bounds;
            RemoveComponent(ComponentIds.Bounds);
            _boundsComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherBounds;

        public static IMatcher Bounds {
            get {
                if (_matcherBounds == null) {
                    _matcherBounds = Matcher.AllOf(ComponentIds.Bounds);
                }

                return _matcherBounds;
            }
        }
    }
}

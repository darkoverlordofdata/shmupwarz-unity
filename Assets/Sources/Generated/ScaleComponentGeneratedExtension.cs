using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public ScaleComponent scale { get { return (ScaleComponent)GetComponent(ComponentIds.Scale); } }

        public bool hasScale { get { return HasComponent(ComponentIds.Scale); } }

        static readonly Stack<ScaleComponent> _scaleComponentPool = new Stack<ScaleComponent>();

        public static void ClearScaleComponentPool() {
            _scaleComponentPool.Clear();
        }

        public Entity AddScale(float newX, float newY) {
            var component = _scaleComponentPool.Count > 0 ? _scaleComponentPool.Pop() : new ScaleComponent();
            component.x = newX;
            component.y = newY;
            return AddComponent(ComponentIds.Scale, component);
        }

        public Entity ReplaceScale(float newX, float newY) {
            var previousComponent = hasScale ? scale : null;
            var component = _scaleComponentPool.Count > 0 ? _scaleComponentPool.Pop() : new ScaleComponent();
            component.x = newX;
            component.y = newY;
            ReplaceComponent(ComponentIds.Scale, component);
            if (previousComponent != null) {
                _scaleComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveScale() {
            var component = scale;
            RemoveComponent(ComponentIds.Scale);
            _scaleComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherScale;

        public static IMatcher Scale {
            get {
                if (_matcherScale == null) {
                    _matcherScale = Matcher.AllOf(ComponentIds.Scale);
                }

                return _matcherScale;
            }
        }
    }
}

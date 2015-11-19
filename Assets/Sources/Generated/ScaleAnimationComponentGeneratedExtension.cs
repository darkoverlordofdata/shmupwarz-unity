using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public ScaleAnimationComponent scaleAnimation { get { return (ScaleAnimationComponent)GetComponent(ComponentIds.ScaleAnimation); } }

        public bool hasScaleAnimation { get { return HasComponent(ComponentIds.ScaleAnimation); } }

        static readonly Stack<ScaleAnimationComponent> _scaleAnimationComponentPool = new Stack<ScaleAnimationComponent>();

        public static void ClearScaleAnimationComponentPool() {
            _scaleAnimationComponentPool.Clear();
        }

        public Entity AddScaleAnimation(float newMin, float newMax, float newSpeed, bool newRepeat, bool newActive) {
            var component = _scaleAnimationComponentPool.Count > 0 ? _scaleAnimationComponentPool.Pop() : new ScaleAnimationComponent();
            component.min = newMin;
            component.max = newMax;
            component.speed = newSpeed;
            component.repeat = newRepeat;
            component.active = newActive;
            return AddComponent(ComponentIds.ScaleAnimation, component);
        }

        public Entity ReplaceScaleAnimation(float newMin, float newMax, float newSpeed, bool newRepeat, bool newActive) {
            var previousComponent = hasScaleAnimation ? scaleAnimation : null;
            var component = _scaleAnimationComponentPool.Count > 0 ? _scaleAnimationComponentPool.Pop() : new ScaleAnimationComponent();
            component.min = newMin;
            component.max = newMax;
            component.speed = newSpeed;
            component.repeat = newRepeat;
            component.active = newActive;
            ReplaceComponent(ComponentIds.ScaleAnimation, component);
            if (previousComponent != null) {
                _scaleAnimationComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveScaleAnimation() {
            var component = scaleAnimation;
            RemoveComponent(ComponentIds.ScaleAnimation);
            _scaleAnimationComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherScaleAnimation;

        public static IMatcher ScaleAnimation {
            get {
                if (_matcherScaleAnimation == null) {
                    _matcherScaleAnimation = Matcher.AllOf(ComponentIds.ScaleAnimation);
                }

                return _matcherScaleAnimation;
            }
        }
    }
}

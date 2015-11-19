using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public ColorAnimationComponent colorAnimation { get { return (ColorAnimationComponent)GetComponent(ComponentIds.ColorAnimation); } }

        public bool hasColorAnimation { get { return HasComponent(ComponentIds.ColorAnimation); } }

        static readonly Stack<ColorAnimationComponent> _colorAnimationComponentPool = new Stack<ColorAnimationComponent>();

        public static void ClearColorAnimationComponentPool() {
            _colorAnimationComponentPool.Clear();
        }

        public Entity AddColorAnimation(float newRedMin, float newRedMax, float newRedSpeed, float newGreenMin, float newGreenMax, float newGreenSpeed, float newBlueMin, float newBlueMax, float newBlueSpeed, float newAlphaMin, float newAlphaMax, float newAlphaSpeed, bool newRedAnimate, bool newGreenAnimate, bool newBlueAnimate, bool newAlphaAnimate, bool newRepeat) {
            var component = _colorAnimationComponentPool.Count > 0 ? _colorAnimationComponentPool.Pop() : new ColorAnimationComponent();
            component.redMin = newRedMin;
            component.redMax = newRedMax;
            component.redSpeed = newRedSpeed;
            component.greenMin = newGreenMin;
            component.greenMax = newGreenMax;
            component.greenSpeed = newGreenSpeed;
            component.blueMin = newBlueMin;
            component.blueMax = newBlueMax;
            component.blueSpeed = newBlueSpeed;
            component.alphaMin = newAlphaMin;
            component.alphaMax = newAlphaMax;
            component.alphaSpeed = newAlphaSpeed;
            component.redAnimate = newRedAnimate;
            component.greenAnimate = newGreenAnimate;
            component.blueAnimate = newBlueAnimate;
            component.alphaAnimate = newAlphaAnimate;
            component.repeat = newRepeat;
            return AddComponent(ComponentIds.ColorAnimation, component);
        }

        public Entity ReplaceColorAnimation(float newRedMin, float newRedMax, float newRedSpeed, float newGreenMin, float newGreenMax, float newGreenSpeed, float newBlueMin, float newBlueMax, float newBlueSpeed, float newAlphaMin, float newAlphaMax, float newAlphaSpeed, bool newRedAnimate, bool newGreenAnimate, bool newBlueAnimate, bool newAlphaAnimate, bool newRepeat) {
            var previousComponent = hasColorAnimation ? colorAnimation : null;
            var component = _colorAnimationComponentPool.Count > 0 ? _colorAnimationComponentPool.Pop() : new ColorAnimationComponent();
            component.redMin = newRedMin;
            component.redMax = newRedMax;
            component.redSpeed = newRedSpeed;
            component.greenMin = newGreenMin;
            component.greenMax = newGreenMax;
            component.greenSpeed = newGreenSpeed;
            component.blueMin = newBlueMin;
            component.blueMax = newBlueMax;
            component.blueSpeed = newBlueSpeed;
            component.alphaMin = newAlphaMin;
            component.alphaMax = newAlphaMax;
            component.alphaSpeed = newAlphaSpeed;
            component.redAnimate = newRedAnimate;
            component.greenAnimate = newGreenAnimate;
            component.blueAnimate = newBlueAnimate;
            component.alphaAnimate = newAlphaAnimate;
            component.repeat = newRepeat;
            ReplaceComponent(ComponentIds.ColorAnimation, component);
            if (previousComponent != null) {
                _colorAnimationComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveColorAnimation() {
            var component = colorAnimation;
            RemoveComponent(ComponentIds.ColorAnimation);
            _colorAnimationComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherColorAnimation;

        public static IMatcher ColorAnimation {
            get {
                if (_matcherColorAnimation == null) {
                    _matcherColorAnimation = Matcher.AllOf(ComponentIds.ColorAnimation);
                }

                return _matcherColorAnimation;
            }
        }
    }
}

using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public SoundEffectComponent soundEffect { get { return (SoundEffectComponent)GetComponent(ComponentIds.SoundEffect); } }

        public bool hasSoundEffect { get { return HasComponent(ComponentIds.SoundEffect); } }

        static readonly Stack<SoundEffectComponent> _soundEffectComponentPool = new Stack<SoundEffectComponent>();

        public static void ClearSoundEffectComponentPool() {
            _soundEffectComponentPool.Clear();
        }

        public Entity AddSoundEffect(float newEffect) {
            var component = _soundEffectComponentPool.Count > 0 ? _soundEffectComponentPool.Pop() : new SoundEffectComponent();
            component.effect = newEffect;
            return AddComponent(ComponentIds.SoundEffect, component);
        }

        public Entity ReplaceSoundEffect(float newEffect) {
            var previousComponent = hasSoundEffect ? soundEffect : null;
            var component = _soundEffectComponentPool.Count > 0 ? _soundEffectComponentPool.Pop() : new SoundEffectComponent();
            component.effect = newEffect;
            ReplaceComponent(ComponentIds.SoundEffect, component);
            if (previousComponent != null) {
                _soundEffectComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveSoundEffect() {
            var component = soundEffect;
            RemoveComponent(ComponentIds.SoundEffect);
            _soundEffectComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherSoundEffect;

        public static IMatcher SoundEffect {
            get {
                if (_matcherSoundEffect == null) {
                    _matcherSoundEffect = Matcher.AllOf(ComponentIds.SoundEffect);
                }

                return _matcherSoundEffect;
            }
        }
    }
}

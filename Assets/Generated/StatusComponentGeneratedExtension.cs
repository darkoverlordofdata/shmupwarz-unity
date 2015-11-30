using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public StatusComponent status { get { return (StatusComponent)GetComponent(ComponentIds.Status); } }

        public bool hasStatus { get { return HasComponent(ComponentIds.Status); } }

        static readonly Stack<StatusComponent> _statusComponentPool = new Stack<StatusComponent>();

        public static void ClearStatusComponentPool() {
            _statusComponentPool.Clear();
        }

        public Entity AddStatus(float newPercent, float newImmunity) {
            var component = _statusComponentPool.Count > 0 ? _statusComponentPool.Pop() : new StatusComponent();
            component.percent = newPercent;
            component.immunity = newImmunity;
            return AddComponent(ComponentIds.Status, component);
        }

        public Entity ReplaceStatus(float newPercent, float newImmunity) {
            var previousComponent = hasStatus ? status : null;
            var component = _statusComponentPool.Count > 0 ? _statusComponentPool.Pop() : new StatusComponent();
            component.percent = newPercent;
            component.immunity = newImmunity;
            ReplaceComponent(ComponentIds.Status, component);
            if (previousComponent != null) {
                _statusComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveStatus() {
            var component = status;
            RemoveComponent(ComponentIds.Status);
            _statusComponentPool.Push(component);
            return this;
        }
    }

    public partial class Pool {
        public Entity statusEntity { get { return GetGroup(Matcher.Status).GetSingleEntity(); } }

        public StatusComponent status { get { return statusEntity.status; } }

        public bool hasStatus { get { return statusEntity != null; } }

        public Entity SetStatus(float newPercent, float newImmunity) {
            if (hasStatus) {
                throw new SingleEntityException(Matcher.Status);
            }
            var entity = CreateEntity();
            entity.AddStatus(newPercent, newImmunity);
            return entity;
        }

        public Entity ReplaceStatus(float newPercent, float newImmunity) {
            var entity = statusEntity;
            if (entity == null) {
                entity = SetStatus(newPercent, newImmunity);
            } else {
                entity.ReplaceStatus(newPercent, newImmunity);
            }

            return entity;
        }

        public void RemoveStatus() {
            DestroyEntity(statusEntity);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherStatus;

        public static IMatcher Status {
            get {
                if (_matcherStatus == null) {
                    _matcherStatus = Matcher.AllOf(ComponentIds.Status);
                }

                return _matcherStatus;
            }
        }
    }
}

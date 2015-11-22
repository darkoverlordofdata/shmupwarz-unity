namespace Entitas {
    public partial class Entity {
        static readonly FiringComponent firingComponent = new FiringComponent();

        public bool isFiring {
            get { return HasComponent(ComponentIds.Firing); }
            set {
                if (value != isFiring) {
                    if (value) {
                        AddComponent(ComponentIds.Firing, firingComponent);
                    } else {
                        RemoveComponent(ComponentIds.Firing);
                    }
                }
            }
        }

        public Entity IsFiring(bool value) {
            isFiring = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity firingEntity { get { return GetGroup(Matcher.Firing).GetSingleEntity(); } }

        public bool isFiring {
            get { return firingEntity != null; }
            set {
                var entity = firingEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isFiring = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static IMatcher _matcherFiring;

        public static IMatcher Firing {
            get {
                if (_matcherFiring == null) {
                    _matcherFiring = Matcher.AllOf(ComponentIds.Firing);
                }

                return _matcherFiring;
            }
        }
    }
}

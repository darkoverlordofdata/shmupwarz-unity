namespace Entitas {
    public partial class Entity {
        static readonly MineComponent mineComponent = new MineComponent();

        public bool isMine {
            get { return HasComponent(ComponentIds.Mine); }
            set {
                if (value != isMine) {
                    if (value) {
                        AddComponent(ComponentIds.Mine, mineComponent);
                    } else {
                        RemoveComponent(ComponentIds.Mine);
                    }
                }
            }
        }

        public Entity IsMine(bool value) {
            isMine = value;
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherMine;

        public static IMatcher Mine {
            get {
                if (_matcherMine == null) {
                    _matcherMine = Matcher.AllOf(ComponentIds.Mine);
                }

                return _matcherMine;
            }
        }
    }
}

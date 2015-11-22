namespace Entitas {
    public partial class Entity {
        static readonly ParallaxStarComponent parallaxStarComponent = new ParallaxStarComponent();

        public bool isParallaxStar {
            get { return HasComponent(ComponentIds.ParallaxStar); }
            set {
                if (value != isParallaxStar) {
                    if (value) {
                        AddComponent(ComponentIds.ParallaxStar, parallaxStarComponent);
                    } else {
                        RemoveComponent(ComponentIds.ParallaxStar);
                    }
                }
            }
        }

        public Entity IsParallaxStar(bool value) {
            isParallaxStar = value;
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherParallaxStar;

        public static IMatcher ParallaxStar {
            get {
                if (_matcherParallaxStar == null) {
                    _matcherParallaxStar = Matcher.AllOf(ComponentIds.ParallaxStar);
                }

                return _matcherParallaxStar;
            }
        }
    }
}

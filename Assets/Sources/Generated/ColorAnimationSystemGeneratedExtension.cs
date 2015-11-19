namespace Entitas {
    public partial class Pool {
        public ISystem CreateColorAnimationSystem() {
            return this.CreateSystem<ColorAnimationSystem>();
        }
    }
}
namespace Entitas {
    public partial class Pool {
        public ISystem CreateScaleAnimationSystem() {
            return this.CreateSystem<ScaleAnimationSystem>();
        }
    }
}
namespace Entitas {
    public partial class Pool {
        public ISystem CreateParallaxStarRepeatingSystem() {
            return this.CreateSystem<ParallaxStarRepeatingSystem>();
        }
    }
}
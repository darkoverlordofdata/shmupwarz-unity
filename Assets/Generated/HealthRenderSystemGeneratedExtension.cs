namespace Entitas {
    public partial class Pool {
        public ISystem CreateHealthRenderSystem() {
            return this.CreateSystem<HealthRenderSystem>();
        }
    }
}
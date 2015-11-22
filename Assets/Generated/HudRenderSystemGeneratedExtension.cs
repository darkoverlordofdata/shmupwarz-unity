namespace Entitas {
    public partial class Pool {
        public ISystem CreateHudRenderSystem() {
            return this.CreateSystem<HudRenderSystem>();
        }
    }
}
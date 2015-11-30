using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class StatusComponent : IComponent {
    public float percent;
    public float immunity;
}


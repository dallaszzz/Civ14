using Content.Shared.Input;
using Robust.Shared.Input.Binding;


namespace Content.Client.Civ14.LookZoom;
public sealed class LookZoomSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
        .Bind(ContentKeyFunctions.LookZoom, new PointerInputCmdHandler(OnLookZoom))
        .Register<LookZoomSystem>();
    }

    private bool OnLookZoom(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (args.Session == null)
            return false;
        var session = args.Session;
        var entity = session.AttachedEntity;

        if (!TryComp<LookZoomComponent>(entity, out var comp))
            return false;

        return true;
    }
}

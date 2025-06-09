#nullable enable
using Content.Shared.Camera;
using Content.Shared.Input;
using Robust.Shared.Input.Binding;
using Content.Client.Movement.Components;
using Content.Client.Movement.Systems;
using Robust.Shared.Player;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Timing;
using Robust.Client.Player;


namespace Content.Client.Civ14.LookZoom;
public sealed class LookZoomSystem : EntitySystem
{
    [Dependency] private readonly EyeCursorOffsetSystem _eyeOffset = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LookZoomComponent, GetEyeOffsetRelayedEvent>(UpdateLookZoom);

        CommandBinds.Builder
        .Bind(ContentKeyFunctions.LookZoom, InputCmdHandler.FromDelegate(OnLookZoomHandler, handle: false, outsidePrediction: false))
        .Register<LookZoomSystem>();
    }

    public void OnLookZoomHandler(ICommonSession? session)
    {
        var uid = session?.AttachedEntity;

        if (!TryComp<LookZoomComponent>(uid, out var comp))
            return;

        if (_timing.CurTime < comp.DelayedTime)
            return;

        comp.DelayedTime = _timing.CurTime + TimeSpan.FromSeconds(1);
        comp.State = !comp.State;
    }
    public void UpdateLookZoom(EntityUid uid, LookZoomComponent comp, ref GetEyeOffsetRelayedEvent args)
    {
        if (comp.State == false)
            return;

        _handsSystem.TryGetActiveItem(comp.Owner, out var item);

        if (item != null && TryComp<EyeCursorOffsetComponent>(item, out var itemComp))
        {
            SetOffset(item.Value, args);
            return;
        }

        SetOffset(comp.Owner, args);
    }

    private void SetOffset(EntityUid uid, GetEyeOffsetRelayedEvent args)
    {

        var offset = _eyeOffset.OffsetAfterMouse(uid, null);
        if (offset == null)
            return;

        args.Offset += offset.Value;
    }
}

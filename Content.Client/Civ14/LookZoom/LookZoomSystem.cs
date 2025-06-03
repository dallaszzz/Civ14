#nullable enable
using Content.Shared.Camera;
using Content.Shared.Hands;
using Content.Shared.Input;
using Content.Shared.Movement.Components;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Content.Client.Movement.Components;
using Content.Shared.Wieldable.Components;
using Content.Client.Movement.Systems;
using Robust.Client.Player;
using Content.Shared.Inventory;
using Robust.Shared.Player;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Hands.Components;
using Robust.Shared.Timing;
using JetBrains.Annotations;


namespace Content.Client.Civ14.LookZoom;
public sealed class LookZoomSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventorySystem = default!;
    [Dependency] private readonly EyeCursorOffsetSystem _eyeOffset = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
        .Bind(ContentKeyFunctions.LookZoom, InputCmdHandler.FromDelegate(OnLookZoomHandler, handle: false, outsidePrediction: false))
        .Register<LookZoomSystem>();
    }

    public void OnLookZoomHandler(ICommonSession? session)
    {
        var entity = session?.AttachedEntity;

        if (!TryComp<LookZoomComponent>(entity, out var comp))
            return;

        UpdateLookZoom(entity.Value);
    }
    public void UpdateLookZoom(EntityUid uid)
    {
        if (!_handsSystem.TryGetActiveItem(uid, out var item))
            return;

        if (!TryComp<EyeCursorOffsetComponent>(uid, out var comp))
            return;

        if (!TryComp<EyeComponent>(uid, out var eye))
            return;

        var offset = _eyeOffset.OffsetAfterMouse(uid, null);
        if (offset == null)
            return;

        eye.Offset += offset.Value;
    }
}

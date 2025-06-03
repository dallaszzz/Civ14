using Content.Shared._Stalker.Lay;
using System.Numerics;

namespace Content.Client.Civ14.LookZoom;

[RegisterComponent]
public sealed partial class LookZoomComponent : Component
{
    [DataField]
    public bool State = false;

    [DataField]
    public TimeSpan CoolDown = TimeSpan.Zero;

    [DataField]
    public Vector2 SavedOffset;
}

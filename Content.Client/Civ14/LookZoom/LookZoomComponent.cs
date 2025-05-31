using Content.Shared._Stalker.Lay;

namespace Content.Client.Civ14.LookZoom;

[RegisterComponent]
public sealed partial class LookZoomComponent : Component
{
    [DataField]
    public bool State = false;
    [DataField]
    public TimeSpan CoolDown = TimeSpan.Zero;
}

using System.Collections.Generic;
using Umbra.Widgets;
using Umbra.Common;
using Dalamud.Plugin.Services;
using Dalamud.Game.Gui.Toast;
using Umbra.Game;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;

namespace Umbra.VisibilityCompatibility.Widgets;

[ToolbarWidget(
    "VisibilityCompatibility",
    "Visibility Widget",
    "A widget for Visibility plugin."
)]
public class VisibilityCompatibility(
    WidgetInfo                  info,
    string?                     guid         = null,
    Dictionary<string, object>? configValues = null
) : DefaultToolbarWidget(info, guid, configValues)
{
    public bool enabled = false;
    public override MenuPopup? Popup { get; } = null;

    private readonly IChatSender _chatSender = Framework.Service<IChatSender>();

    private IToastGui ToastGui { get; set; } = Framework.Service<IToastGui>();

    protected override IEnumerable<IWidgetConfigVariable> GetConfigVariables()
    {
        return [];
    }

    protected override void Initialize()
    {
        SetLeftIcon(60647);
        SetLabel(null);
        Una.Drawing.Color c = new(91, 91, 91);
        SetIconColor(c);

        Node.OnClick += _ => SwitchVisibilityState();
        Node.OnRightClick += _ => OpenVisibilityConfig();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void SwitchVisibilityState ()
    {
        Una.Drawing.Color c;
        if (enabled)
        {
            _chatSender.Send("/pvis enabled off");
            ToastGui.ShowNormal($"Players visible", new ToastOptions { Speed = ToastSpeed.Fast });
            c = new(91, 91, 91);
        }
        else
        {
            _chatSender.Send("/pvis enabled on");
            ToastGui.ShowNormal($"Players not visible", new ToastOptions { Speed = ToastSpeed.Fast });
            c = new(255, 255, 255);
        }
        SetIconColor(c);
        enabled = !enabled;
    }

    private void OpenVisibilityConfig ()
    {
        _chatSender.Send("/pvis");
    }
}

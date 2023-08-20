using Godot;

public partial class SubViewportExportFix : SubViewport
{
    public override void _Ready()
    {
        // For some reason sub-viewports are broken on export without this.
        RenderTargetUpdateMode = UpdateMode.WhenParentVisible;
    }
}

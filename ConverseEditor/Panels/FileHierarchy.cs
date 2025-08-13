using Hexa.NET.ImGui;

namespace ConverseEditor.Panels;

class FileHierarchy : Panel
{
    public FileHierarchy(Context ctx) : base(ctx) { }

    public override void RenderPanel()
    {
        foreach(var x in ctx.LoadedFiles.ToList())
        {
            ImGui.PushID(x.GetHashCode());
            if (ImGui.Selectable($"{ Path.GetFileNameWithoutExtension(x.FileName)} - {x.GetType().Name}", ctx.SelectedFile == x)) ctx.SelectFile(x);
            if (ImGui.BeginPopupContextItem($"Options {x.FilePath}"))
            {
                if (ImGui.MenuItem("Save")) x.SaveToFile(x.FilePath);

                if (ImGui.MenuItem("Close")) ctx.Close(x);

                ImGui.EndPopup();
            }
            ImGui.PopID();
        }
    }

    public override Properties GetProperties()
    {
        return new Properties("File Hierarchy", new(8, 20), new(160, 500));
    }
}

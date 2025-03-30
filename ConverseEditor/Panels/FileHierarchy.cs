using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverseEditor.Panels;

class FileHierarchy : Panel
{
    public override void RenderPanel()
    {
        foreach(var x in ConverseEditorApp.LoadedFiles.ToList())
        {
            ImGui.PushID(x.GetHashCode());
            if (ImGui.Selectable($"{ Path.GetFileNameWithoutExtension(x.FileName)} ({x.GetType().Name})", ConverseEditorApp.SelectedFile == x))
                ConverseEditorApp.SelectedFile = x;
            if (ImGui.BeginPopupContextItem($"Options {x.FilePath}"))
            {
                if (ImGui.MenuItem("Save"))
                    x.SaveToFile(x.FilePath);
                if (ImGui.MenuItem("Close"))
                {
                    if (ConverseEditorApp.SelectedFile == x)
                        ConverseEditorApp.SelectedFile = new();
                    ConverseEditorApp.LoadedFiles.Remove(x);
                }
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

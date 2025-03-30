using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Widgets.Dialogs;
using System.Security.Cryptography;

namespace ConverseEditor;

static class MenuBar
{
    public static OpenFileDialog ofd = new() { AllowMultipleSelection = true, OnlyAllowFilteredExtensions = true, AllowedExtensions = { Text.FileExtension, TextMeta.FileExtension, TextProject.FileExtension } };

    public static string CreateDefaultFilePath(string extension) => $"{Environment.CurrentDirectory}\\new_file{extension}";
    public static string CreateDefaultFileName(string extension) => $"new_file{extension}";

    public static void Render()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.BeginMenu("New"))
                {
                    if (ImGui.MenuItem("Text"))
                        ConverseEditorApp.LoadedFiles.Add(new Text() { FilePath = CreateDefaultFilePath(Text.FileExtension), FileName = CreateDefaultFileName(Text.FileExtension), Extension = Text.FileExtension });
                    if (ImGui.MenuItem("Text Meta"))
                        ConverseEditorApp.LoadedFiles.Add(new TextMeta() { FilePath = CreateDefaultFilePath(TextMeta.FileExtension), FileName = CreateDefaultFileName(TextMeta.FileExtension), Extension = TextMeta.FileExtension });
                    if (ImGui.MenuItem("Text Project"))
                        ConverseEditorApp.LoadedFiles.Add(new TextProject() { FilePath = CreateDefaultFilePath(TextProject.FileExtension), FileName = CreateDefaultFileName(TextProject.FileExtension), Extension = TextProject.FileExtension });
                    ImGui.EndMenu();
                }
                if (ImGui.MenuItem("Open"))
                    ofd.Show();
                if (ImGui.MenuItem("Save All"))
                    foreach (var i in ConverseEditorApp.LoadedFiles)
                        i.SaveToFile(i.FilePath);
                if (ImGui.MenuItem("Close All"))
                {
                    ConverseEditorApp.SelectedFile = new();
                    ConverseEditorApp.LoadedFiles.Clear();
                }
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        ofd.Draw(0);

        if(ofd.Result == DialogResult.Yes && ofd.SelectedFile != null)
        {
            foreach (var file in ofd.Selection)
                ConverseEditorApp.LoadFile(file);
            ofd.Reset();
            ofd.Close();
        }
    }
}

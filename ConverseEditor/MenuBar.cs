using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Widgets.Dialogs;
using System.Text.Json;

namespace ConverseEditor;

public class MenuBar
{
    Context ctx { get; }
    public OpenFileDialog ofd = new() { AllowMultipleSelection = true, OnlyAllowFilteredExtensions = true, AllowedExtensions = { Text.FileExtension, TextMeta.FileExtension, TextProject.FileExtension } };

    void Open() => ofd.Show();
    void Save() => ctx.SelectedFile.SaveToFile(ctx.SelectedFile.FilePath);
    void SaveAll() => ctx.SaveAll();
    void CloseAll() => ctx.CloseAll();

    bool CanSave() => ctx.SelectedFile != null;
    bool CanSaveAll() => ctx.LoadedFiles.Count() > 0;
    bool CanCloseAll() => ctx.LoadedFiles.Count() > 0;

    public MenuBar(Context ctx) => this.ctx = ctx;

    public void Render()
    {
        if (Utilities.IsControlDown() && ImGui.IsKeyPressed(ImGuiKey.O)) Open();

        if (Utilities.IsControlDown() && ImGui.IsKeyPressed(ImGuiKey.S) && CanSave()) Save();

        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.BeginMenu("New"))
                {
                    if (ImGui.MenuItem("Text")) ctx.LoadedFiles.Add(ctx.CreateFile<Text>());

                    if (ImGui.MenuItem("Text Meta")) ctx.LoadedFiles.Add(ctx.CreateFile<TextMeta>());

                    if (ImGui.MenuItem("Text Project")) ctx.LoadedFiles.Add(ctx.CreateFile<TextProject>());

                    ImGui.EndMenu();
                }

                if (ImGui.MenuItem("Open", "Ctrl+O", false)) Open();

                if (ImGui.MenuItem("Save All", "", false, CanSaveAll())) SaveAll(); 

                if (ImGui.MenuItem("Close All", "", false, CanCloseAll())) CloseAll();

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Options"))
            {
                var settingsMgr = SettingsManager.Instance;
                ref var settings = ref settingsMgr.settings;

                if (ImGui.BeginMenu("Theme", Directory.Exists("themes")))
                {
                    string pureName = settings.SelectedTheme;
                    if (ImGui.BeginCombo("###theme", pureName))
                    {
                        foreach (var item in ThemesManager.Instance.Themes)
                        {
                            bool selected = (pureName == item.Key);
                            if (ImGui.Selectable(item.Key, selected))
                            {
                                settings.SelectedTheme = item.Key;
                                settingsMgr.Save();
                                ThemesManager.Instance.SetTheme(item.Key);
                            }

                            var rootElement = item.Value.RootElement;
                            JsonElement metadata;
                            bool hasMetadata = rootElement.TryGetProperty("Metadata", out metadata);

                            if (hasMetadata && ImGui.BeginItemTooltip())
                            {
                                foreach (var i in metadata.EnumerateObject())
                                    ImGui.Text($"{i.Name}: {i.Value}");
                                ImGui.EndTooltip();
                            }

                            if (selected)
                                ImGui.SetItemDefaultFocus();
                        }

                        ImGui.EndCombo();
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("View"))
            {
                foreach (var i in ctx.panels)
                    ImGui.MenuItem(i.GetProperties().Name, "", ref i.Visible);

                ImGui.EndMenu();
            }

            string versionText = $"v{ConverseEditorApp.Version}";
            float right_text_width = ImGui.CalcTextSize(versionText).X;
            float menu_bar_width = ImGui.GetContentRegionAvail().X;
            float padding = 10;

            ImGui.SameLine(ImGui.GetWindowPos().X + ImGui.GetWindowSize().X - right_text_width - ImGui.GetStyle().ItemSpacing.X - padding);
            ImGui.Text(versionText);

            ImGui.EndMainMenuBar();
        }

        ofd.Draw(0);

        if (ofd.Result == DialogResult.Yes && ofd.SelectedFile != null)
        {
            foreach (var file in ofd.Selection)
                ctx.LoadFile(file);
            ofd.Reset();
            ofd.Close();
        }
    }
}

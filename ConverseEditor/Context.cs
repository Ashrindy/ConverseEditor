using AshDumpLib.HedgehogEngine.BINA.Converse;
using AshDumpLib.Helpers.Archives;
using ConverseEditor.Panels;
using Hexa.NET.ImGui;
using System.Numerics;

namespace ConverseEditor;

public sealed class Context
{
    static readonly Context instance = new Context();
    public static Context Instance { get { return instance; } }

    public List<Panel> panels = new();
    public MenuBar menuBar;

    public List<IFile> LoadedFiles = new();
    public IFile SelectedFile = new();

    public void AddPanel<T>() where T : Panel
    {
        var panel = (T)Activator.CreateInstance(typeof(T), this);
        panels.Add(panel);
    }

    public Context()
    {
        _ = SettingsManager.Instance;
        _ = ThemesManager.Instance;

        menuBar = new(this);
        AddPanel<FileHierarchy>();
        AddPanel<FileEditor>();
    }

    #region Rendering
    void RenderDockSpace()
    {
        ImGuiViewportPtr viewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewport.Pos);
        ImGui.SetNextWindowSize(viewport.Size);
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoTitleBar |
                                    ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                    ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus |
                                    ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoBackground;

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        ImGui.Begin("DockSpace Window", windowFlags);
        ImGui.PopStyleVar(3);

        ImGui.SetCursorPosY(ImGui.GetFrameHeight());

        ImGui.DockSpace(ImGui.GetID("DockSpace"), new Vector2(0, 0), ImGuiDockNodeFlags.PassthruCentralNode);
    }

    public void Render()
    {
        menuBar.Render();

        RenderDockSpace();

        foreach (var x in panels)
            x.Render();
    }
    #endregion

    #region File Handling
    public void LoadFile(string file)
    {
        switch (Path.GetExtension(file))
        {
            case Text.FileExtension:
                LoadedFiles.Add(new Text(file));
                break;

            case TextMeta.FileExtension:
                LoadedFiles.Add(new TextMeta(file));
                break;

            case TextProject.FileExtension:
                LoadedFiles.Add(new TextProject(file));
                break;
        }
    }

    public T CreateFile<T>() where T : IFile, new()
    {
        var fileExtension = (string)typeof(T).GetField("FileExtension").GetValue(null);
        return new T() { FilePath = $"{Environment.CurrentDirectory}\\new_file{fileExtension}", FileName = $"new_file{fileExtension}", Extension = fileExtension };
    }

    public void Save() => SelectedFile.SaveToFile(SelectedFile.FilePath); 

    public void SaveAll()
    {
        foreach (var i in LoadedFiles)
            i.SaveToFile(i.FilePath);
    }

    public void Close(IFile file)
    {
        if (file == SelectedFile)
            Close();
        else
            LoadedFiles.Remove(file);
    }

    public void Close()
    {
        LoadedFiles.Remove(SelectedFile);
        SelectedFile = new();
        ConverseEditorApp.Instance.SetTitleBarName(ConverseEditorApp.TitleName);
    }

    public void CloseAll()
    {
        LoadedFiles.Clear();
        SelectedFile = new();
        ConverseEditorApp.Instance.SetTitleBarName(ConverseEditorApp.TitleName);
    }

    public void SelectFile(IFile file)
    {
        SelectedFile = file;
        ConverseEditorApp.Instance.SetTitleBarName($"{ConverseEditorApp.TitleName} - {Path.GetFileNameWithoutExtension(SelectedFile.FileName)}");
    }
    #endregion
}

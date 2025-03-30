using AshDumpLib.HedgehogEngine.BINA.Converse;
using AshDumpLib.Helpers.Archives;
using ConverseEditor;
using ConverseEditor.Panels;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.OpenGL3;
using Hexa.NET.ImGui.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
using System.Numerics;

class ConverseEditorApp : GameWindow
{
    ImGuiContextPtr imGuiCtx = ImGui.CreateContext();
    Stopwatch stopWatch = new();
    float fpsLimit = 60;
    public List<Panel> panels = new();

    public static List<IFile> LoadedFiles = new();
    public static IFile SelectedFile = new();

    public void AddPanel<T>() where T : Panel, new() => panels.Add(new T());

    public ConverseEditorApp(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        InitImGUI();
        stopWatch.Start();
        AddPanel<FileHierarchy>();
        AddPanel<FileEditor>();
    }

    void InitImGUI()
    {
        ImGui.SetCurrentContext(imGuiCtx);

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.DisplaySize = new(ClientSize.X, ClientSize.Y);

        var style = ImGui.GetStyle();
        style.FrameRounding = 4;
        style.WindowBorderSize = 1;
        style.FrameBorderSize = 0;
        style.PopupBorderSize = 1;
        style.WindowRounding = 4;
        style.ChildRounding = 4;
        style.FrameRounding = 4;
        style.PopupRounding = 4;
        style.ScrollbarRounding = 12;
        style.CellPadding.X = 4;
        style.CellPadding.Y = 2;
        style.WindowTitleAlign.X = 0.50f;
        style.SelectableTextAlign.X = 0.03f;
        style.WindowMenuButtonPosition = ImGuiDir.Right;
        style.Colors[(int)ImGuiCol.WindowBg] = new(0, 0, 0, 1);

        InitImGUIFont();
        InitImGUIGLFW();
        InitImGUIOpenGL();
    }

    void InitImGUIFont()
    {
        ImGuiFontBuilder builder = new();
        builder
            .AddFontFromFileTTF("res/fonts/Inter.ttf", 14, [0x1, 0xFFFFF])
            .Build();
    }

    void InitImGUIOpenGL()
    {
        ImGuiImplOpenGL3.SetCurrentContext(ImGui.GetCurrentContext());
        ImGuiImplOpenGL3.Init((string)null!);
    }

    void InitImGUIGLFW()
    {
        ImGuiImplGLFW.SetCurrentContext(ImGui.GetCurrentContext());
        GLFWwindowPtr windowPtr = new();
        unsafe
        {
            windowPtr.Handle = (GLFWwindow*)WindowPtr;
        }
        ImGuiImplGLFW.InitForOpenGL(windowPtr, true);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
    }

    void FrameLimit()
    {
        float elapsedTime = (float)stopWatch.Elapsed.TotalSeconds;
        stopWatch.Restart();
        float frameTime = 1f / fpsLimit;
        if (elapsedTime < frameTime)
        {
            Thread.Sleep((int)((frameTime - elapsedTime) * 1000));
            elapsedTime = frameTime;
        }
    }

    void ImGuiBegin()
    {
        ImGui.SetCurrentContext(imGuiCtx);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        ImGui.SetNextFrameWantCaptureKeyboard(true);
        ImGui.SetNextFrameWantCaptureMouse(true);
        ImGuiImplOpenGL3.NewFrame();
        ImGuiImplGLFW.NewFrame();
        ImGui.NewFrame();
    }

    void ImGuiEnd()
    {
        var io = ImGui.GetIO();
        ImGui.Render();
        ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

        if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
        {
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
        }

        SwapBuffers();
    }

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
        ImGui.Begin("DockSpace Window", windowFlags);
        ImGui.PopStyleVar(2);

        ImGui.SetCursorPosY(ImGui.GetFrameHeight());

        ImGui.DockSpace(ImGui.GetID("DockSpace"), new Vector2(0, 0), ImGuiDockNodeFlags.PassthruCentralNode);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        FrameLimit();
        ImGuiBegin();

        MenuBar.Render();

        RenderDockSpace();

        foreach (var x in panels)
            x.Render();

        ImGui.End();

        ImGuiEnd();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        ImGui.GetIO().DisplaySize = new(e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
    }

    public static void LoadFile(string file)
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

    static void Main(string[] args)
    {
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var gameSettings = new GameWindowSettings();
        var nativeSettings = new NativeWindowSettings
        {
            ClientSize = new(800, 600),
            Title = "Converse Editor",
        };

        using var window = new ConverseEditorApp(gameSettings, nativeSettings);
        foreach (var i in args)
            LoadFile(i);
        window.Run();
    }
}
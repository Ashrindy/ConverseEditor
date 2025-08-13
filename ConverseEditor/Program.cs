using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.OpenGL3;
using Hexa.NET.ImGui.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;

class ConverseEditorApp : GameWindow
{
    public static string Version = "0.0.3";
    public static string TitleName = "Converse Editor";
    static ConverseEditorApp instance;
    public static ConverseEditorApp Instance { get { return instance; } }

    ImGuiContextPtr imGuiCtx = ImGui.CreateContext();
    Stopwatch stopWatch = new();
    float fpsLimit = 60;

    public ConverseEditorApp(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        InitImGUI();
        stopWatch.Start();
    }

    void InitImGUI()
    {
        ImGui.SetCurrentContext(imGuiCtx);

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.DisplaySize = new(ClientSize.X, ClientSize.Y);

        ImGui.StyleColorsDark();

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

    protected override void OnLoad() => base.OnLoad();

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
        var color = ImGui.GetStyle().Colors[(int)ImGuiCol.WindowBg];
        GL.ClearColor(color.X, color.Y, color.Z, color.W);
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

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        FrameLimit();
        ImGuiBegin();

        ConverseEditor.Context.Instance.Render();

        ImGui.End();

        ImGuiEnd();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        ImGui.GetIO().DisplaySize = new(e.Width, e.Height);
    }

    protected override void OnUnload() => base.OnUnload(); 

    public void SetTitleBarName(string name) => Title = name;

    static void Main(string[] args)
    {
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var gameSettings = new GameWindowSettings();
        var nativeSettings = new NativeWindowSettings
        {
            ClientSize = new(800, 600),
            Title = TitleName,
        };

        instance = new ConverseEditorApp(gameSettings, nativeSettings);
        foreach (var i in args)
            ConverseEditor.Context.Instance.LoadFile(i);
        instance.Run();
    }
}
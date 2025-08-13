using ConverseEditor;
using Hexa.NET.ImGui;
using System.Numerics;

namespace ConverseEditor;

public class Panel
{
    public struct Properties
    {
        public string Name;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Pivot;

        public Properties() { }
        public Properties(string name, Vector2 position, Vector2 size)
        {
            Name = name;
            Position = position;
            Size = size;
            Pivot = new(0, 0);
        }
        public Properties(string name, Vector2 position, Vector2 size, Vector2 pivot)
        {
            Name = name;
            Position = position;
            Size = size;
            Pivot = pivot;
        }
    }

    protected Context ctx { get; }
    public bool Visible = true;

    public Panel(Context ctx) => this.ctx = ctx;

    public virtual Properties GetProperties() { return new Properties { }; }
    public virtual void RenderPanel() { }
    public void Render()
    {
        if (!Visible) return;

        Properties traits = GetProperties();

        ImGui.SetNextWindowPos(traits.Position, ImGuiCond.FirstUseEver, traits.Pivot);
        ImGui.SetNextWindowSize(traits.Size, ImGuiCond.FirstUseEver);

        if (ImGui.Begin(traits.Name, ImGuiWindowFlags.NoCollapse))
            RenderPanel();

        ImGui.End();
    }
}

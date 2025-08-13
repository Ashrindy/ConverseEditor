using Hexa.NET.ImGui;
using System.Numerics;

namespace ConverseEditor.Editors;

static class Basic
{
    public static bool Editor(string label, ref AshDumpLib.Helpers.MathA.Crop value)
    {
        bool changed = false;
        if (ImGui.TreeNode(label))
        {
            changed |= Editor("Top", ref value.Top);
            changed |= Editor("Bottom", ref value.Bottom);
            changed |= Editor("Left", ref value.Left);
            changed |= Editor("Right", ref value.Right);
            ImGui.TreePop();
        }
        return changed;
    }

    public unsafe static bool Editor(string label, ref long value)
    {
        fixed (void* x = &value)
            return ImGui.DragScalar(label, ImGuiDataType.S64, x);
    }

    public static bool Editor(string label, ref AshDumpLib.Helpers.MathA.Color8 value)
    {
        bool changed = false;
        float[] color =
        {
            (float)value.r/255,
            (float)value.g/255,
            (float)value.b/255,
            (float)value.a/255,
        };
        if (changed |= ImGui.ColorEdit4(label, ref color[0]))
        {
            value.r = (byte)(color[0] * 255);
            value.g = (byte)(color[1] * 255);
            value.b = (byte)(color[2] * 255);
            value.a = (byte)(color[3] * 255);
        }
        return changed;
    }

    public static bool Editor(string label, ref int value)
    {
        return ImGui.DragInt(label, ref value);
    }

    public static bool Editor(string label, ref Vector4 value)
    {
        return ImGui.DragFloat4(label, ref value);
    }

    public static bool Editor(string label, ref Vector3 value)
    {
        return ImGui.DragFloat3(label, ref value);
    }

    public static bool Editor(string label, ref Vector2 value)
    {
        return ImGui.DragFloat2(label, ref value);
    }

    public static bool Editor(string label, ref int? value)
    {
        bool changed = false;
        if (value.HasValue)
        {
            int x = value.Value;
            changed |= ImGui.DragInt(label, ref x);
            value = x;
        }
        return changed;
    }

    public static bool Editor(string label, ref float? value)
    {
        bool changed = false;
        if (value.HasValue)
        {
            float x = value.Value;
            changed |= ImGui.DragFloat(label, ref x);
            value = x;
        }
        return changed;
    }

    public static bool Editor(string label, ref float value)
    {
        return ImGui.DragFloat(label, ref value);
    }

    public static bool Editor(string label, ref bool value)
    {
        return ImGui.Checkbox(label, ref value);
    }

    public static bool Editor(string label, ref string value, ulong valueLength)
    {
        return ImGui.InputText(label, ref value, (nuint)valueLength);
    }
}

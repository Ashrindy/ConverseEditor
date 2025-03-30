using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;
using System.Numerics;
using System.Threading.Channels;

namespace ConverseEditor.Editors;

static class TextEditors
{
    public static bool Editor(string label, ref Text.Character value, ref List<Text.Character> values)
    {
        bool changed = false;
        unsafe
        {
            fixed (long* x = &value.Unk)
                ImGui.PushID(x);
        }
        bool open = ImGui.TreeNode(label);
        if(ImGui.BeginPopupContextItem($"Options {value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Type", ref value.Type, 128);
            changed |= Basic.Editor("Unk", ref value.Unk);
            changed |= Basic.Editor("Name", ref value.Name, 128);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref Text.Character value)
    {
        bool changed = false;
        unsafe
        {
            fixed (long* x = &value.Unk)
                ImGui.PushID(x);
        }
        if (ImGui.TreeNode(label))
        {
            changed |= Basic.Editor("Type", ref value.Type, 128);
            changed |= Basic.Editor("Unk", ref value.Unk);
            changed |= Basic.Editor("Name", ref value.Name, 128);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref List<Text.Character> value)
    {
        bool changed = false;
        bool open = ImGui.TreeNode(label);
        if(ImGui.BeginPopupContextItem("Options Character"))
        {
            if (ImGui.Selectable("Add"))
                value.Add(new() { Name = "New Character", Type = "Speaker", Unk = 0});
            ImGui.EndPopup();
        }
        if (open)
        {
            for(int i = 0; i < value.Count; i++)
            {
                var x = value[i];
                if (changed |= Editor(x.Name, ref x, ref value))
                    value[i] = x;
            }
            ImGui.TreePop();
        }
        return changed;
    }

    static string curCharPopup = "";
    static string curFontLayPopup = "";

    public static bool Editor(ref Text.Entry value, ref List<Text.Entry> values)
    {
        bool changed = false;
        string id = "";
        unsafe
        {
            fixed (long* x = &value.ID)
            {
                ImGui.PushID(x);
                id = ((IntPtr)x).ToString();
            }
        }
        ImGui.BeginGroup();

        Vector2 textSize = ImGui.CalcTextSize(value.Text);
        float rowHeight = textSize.Y + 45;
        ImGui.PushItemWidth(300);
        if (changed |= Basic.Editor("##key", ref value.Key, 1024))
        {
            value.ID = Text.Entry.GenerateID(value.Key);
            value.FontLayout.EntryName = value.Key;
        }
        ImGui.PopItemWidth();
        ImGui.SameLine();

        var y = ImGui.GetWindowSize();

        string decodedText = TextCompute.DecodeText(value.Text);
        if (changed |= ImGui.InputTextMultiline("##text", ref decodedText, int.MaxValue / 4, new Vector2(0, rowHeight)))
            value.Text = TextCompute.EncodeString(decodedText);

        if (ImGui.Button("Edit Characters"))
            curCharPopup = $"Character Edit {id}";
        ImGui.SameLine();
        if (ImGui.Button("Edit Font and Layout"))
            curFontLayPopup = $"Font Layout Edit {id}";

        if (curCharPopup == $"Character Edit {id}")
        {
            bool panelOpen = true;
            if(ImGui.Begin($"Edit Character {value.Key}", ref panelOpen, ImGuiWindowFlags.NoCollapse))
                Editor("Characters", ref value.Characters);
            ImGui.End();
            if (!panelOpen)
                curCharPopup = "";
        }

        if (curFontLayPopup == $"Font Layout Edit {id}")
        {
            bool panelOpen = true;
            if (ImGui.Begin($"Edit Font and Layout {value.Key}", ref panelOpen, ImGuiWindowFlags.NoCollapse))
            {
                if (ImGui.TreeNode("Font"))
                {
                    TextProjectEditors.Editor(ref value.FontLayout.FontInfo.Data);
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("Layout"))
                {
                    TextProjectEditors.Editor(ref value.FontLayout.LayoutInfo.Data);
                    ImGui.TreePop();
                }
            }
            ImGui.End();
            if (!panelOpen)
                curFontLayPopup = "";
        }

        ImGui.EndGroup();
        if (ImGui.BeginPopupContextItem($"Options {value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(ref Text.Entry value)
    {
        bool changed = false;
        unsafe
        {
            fixed (long* id = &value.ID)
                ImGui.PushID(id);
        }
        ImGui.BeginGroup();

        Vector2 textSize = ImGui.CalcTextSize(value.Text);
        float rowHeight = textSize.Y + 45;
        ImGui.PushItemWidth(300);
        if (changed |= Basic.Editor("##key", ref value.Key, 1024))
            value.ID = Text.Entry.GenerateID(value.Key);
        ImGui.PopItemWidth();
        ImGui.SameLine();

        string decodedText = TextCompute.DecodeText(value.Text);
        if (changed |= ImGui.InputTextMultiline("##text", ref decodedText, int.MaxValue / 4, new Vector2(750, rowHeight)))
            value.Text = TextCompute.EncodeString(decodedText);

        if(ImGui.Button("Edit Characters"))
        {

        }

        ImGui.EndGroup();
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(ref Text value)
    {
        bool changed = false;
        changed |= Basic.Editor("Language Name", ref value.Language, 32);
        bool open = ImGui.TreeNode("Entries");
        if(ImGui.BeginPopupContextItem("Options Entries"))
        {
            if (ImGui.Selectable("Add"))
                value.Entries.Add(new());
            ImGui.EndPopup();
        }
        if (open)
        {
            for (int i = 0; i < value.Entries.Count; i++)
            {
                var x = value.Entries[i];
                if (changed |= Editor(ref x, ref value.Entries))
                    value.Entries[i] = x;
                if (value.Entries.Last() != x)
                    ImGui.Separator();
            }
            ImGui.TreePop();
        }
        return changed;
    }

    public static bool Editor(string label, ref Text value)
    {
        bool changed = false;
        if (ImGui.TreeNode(label))
        {
            changed |= Editor(ref value);
            ImGui.TreePop();
        }
        return changed;
    }
}

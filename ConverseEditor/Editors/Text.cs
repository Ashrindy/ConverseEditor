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
        ImGui.PushID(values.IndexOf(value));
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

        ImGui.PushID(value.GetHashCode());

        bool isOpen = ImGui.TreeNode(value.Key);

        if (ImGui.BeginPopupContextItem($"Options {value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }

        if (isOpen)
        {
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

            Editor("Characters", ref value.Characters);

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

            ImGui.TreePop();
        }
        
        ImGui.PopID();

        return changed;
    }

    public static bool Editor(ref Text value)
    {
        bool changed = false;
        changed |= Basic.Editor("Sheet Name", ref value.SheetName, 32);
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

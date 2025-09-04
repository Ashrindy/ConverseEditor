using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System.Threading.Channels;
using static AshDumpLib.HedgehogEngine.BINA.Converse.Text;

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

        string[] names = { "Forces", "Frontiers, SXSG" };
        int versionValue = value.Version == TextVersion.Frontiers ? 1 : 0;
        if (ImGui.BeginCombo("Version", names[versionValue]))
        {
            for(int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                bool isSelected = i == versionValue;
                if (ImGui.Selectable(name, isSelected))
                {
                    versionValue = i;
                    changed = true;
                }

                if (isSelected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }
        if (changed)
            value.Version = versionValue == 1 ? TextVersion.Frontiers : TextVersion.Forces;

        ImGui.Separator();

        switch (value.Version)
        {
            case TextVersion.Forces:
                {
                    bool isOpen = ImGui.TreeNode("Sheets");

                    if (ImGui.BeginPopupContextItem($"Sheet Options"))
                    {
                        if (ImGui.Selectable("Add"))
                            value.Sheets.Add(new() { Name = "New Sheet" });
                        ImGui.EndPopup();
                    }

                    if (isOpen)
                    {
                        for (int l = 0; l < value.Sheets.Count; l++)
                        {
                            var sheet = value.Sheets[l];
                            ImGui.PushID(sheet.GetHashCode());

                            bool isSheetOpen = ImGui.TreeNode(sheet.Name);

                            if (ImGui.BeginPopupContextItem($"Options {sheet}"))
                            {
                                if (ImGui.Selectable("Delete"))
                                    value.Sheets.Remove(sheet);
                                ImGui.EndPopup();
                            }

                            if (isSheetOpen)
                            {
                                changed |= Basic.Editor("Sheet Name", ref sheet.Name, 32);
                                bool open = ImGui.TreeNode("Entries");
                                if (ImGui.BeginPopupContextItem("Options Entries"))
                                {
                                    if (ImGui.Selectable("Add"))
                                        sheet.Entries.Add(new());
                                    ImGui.EndPopup();
                                }
                                if (open)
                                {
                                    for (int i = 0; i < sheet.Entries.Count; i++)
                                    {
                                        var x = sheet.Entries[i];
                                        if (changed |= Editor(ref x, ref sheet.Entries))
                                            sheet.Entries[i] = x;
                                    }
                                    ImGui.TreePop();
                                }
                                ImGui.TreePop();
                            }

                            ImGui.PopID();

                            if (changed)
                                value.Sheets[l] = sheet;
                        }

                        ImGui.TreePop();
                    }
                }
                break;

            case TextVersion.Frontiers or TextVersion.SXSG:
                {
                    var sheet = value.Sheets[0];
                    changed |= Basic.Editor("Sheet Name", ref sheet.Name, 32);
                    bool open = ImGui.TreeNode("Entries");
                    if (ImGui.BeginPopupContextItem("Options Entries"))
                    {
                        if (ImGui.Selectable("Add"))
                            sheet.Entries.Add(new());
                        ImGui.EndPopup();
                    }
                    if (open)
                    {
                        for (int i = 0; i < sheet.Entries.Count; i++)
                        {
                            var x = sheet.Entries[i];
                            if (changed |= Editor(ref x, ref sheet.Entries))
                                sheet.Entries[i] = x;
                        }
                        ImGui.TreePop();
                    }
                }
                break;
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

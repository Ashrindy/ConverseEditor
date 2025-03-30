using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;
using Newtonsoft.Json.Linq;

namespace ConverseEditor.Editors;

static class TextProjectEditors
{
    public static bool Editor(ref TextProject.LanguageSettings.Font.FontInfo value)
    {
        bool changed = false;
        changed |= Basic.Editor("ID Name", ref value.IDName, 128);
        changed |= Basic.Editor("Font Name", ref value.FontName, 128);
        changed |= Basic.Editor("Unk0", ref value.Unk0);
        changed |= Basic.Editor("Unk1", ref value.Unk1);
        changed |= Basic.Editor("Unk2", ref value.Unk2);
        changed |= Basic.Editor("Unk3", ref value.Unk3);
        changed |= Basic.Editor("Unk4", ref value.Unk4);
        changed |= Basic.Editor("Unk5", ref value.Unk5);
        changed |= Basic.Editor("Unk6", ref value.Unk6);
        changed |= Basic.Editor("Unk7", ref value.Unk7);
        changed |= Basic.Editor("Unk8", ref value.Unk8);
        changed |= Basic.Editor("Unk9", ref value.Unk9);
        changed |= Basic.Editor("Unk10", ref value.Unk10);
        changed |= Basic.Editor("Unk11", ref value.Unk11);
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.Font value, ref List<TextProject.LanguageSettings.Font> values)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Editor(ref value.Data);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.Font value)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Editor(ref value.Data);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(ref TextProject.LanguageSettings.Layout.LayoutInfo value)
    {
        bool changed = false;
        changed |= Basic.Editor("ID Name", ref value.IDName, 128);
        changed |= Basic.Editor("Unk0", ref value.Unk0);
        changed |= Basic.Editor("Unk1", ref value.Unk1);
        changed |= Basic.Editor("Unk2", ref value.Unk2);
        changed |= Basic.Editor("Unk3", ref value.Unk3);
        changed |= Basic.Editor("Unk4", ref value.Unk4);
        changed |= Basic.Editor("Unk5", ref value.Unk5);
        changed |= Basic.Editor("Unk6", ref value.Unk6);
        changed |= Basic.Editor("Unk7", ref value.Unk7);
        changed |= Basic.Editor("Unk8", ref value.Unk8);
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.Layout value, ref List<TextProject.LanguageSettings.Layout> values)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Editor(ref value.Data);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.Layout value)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Editor(ref value.Data);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Font> value, ref List<TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Font>> values)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            if (changed |= ImGui.Selectable("Add"))
                value.Values.Add(new() { Data = new() { IDName = "New Font", FontName = "new-font", Unk0 = 0, Unk1 = 0, Unk2 = 0, Unk3 = 0, Unk4 = 0, Unk5 = 0, Unk6 = 0, Unk7 = 0, Unk8 = 0, Unk9 = 0, Unk10 = 0, Unk11 = 0 } });
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Language Name", ref value.LanguageName, 32);
            for (int i = 0; i < value.Values.Count; i++)
            {
                var x = value.Values[i];
                if (changed |= Editor(x.Data.IDName, ref x, ref value.Values))
                    value.Values[i] = x;
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Font> value)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (changed |= ImGui.Selectable("Add"))
                value.Values.Add(new() { Data = new() { IDName = "New Font", FontName = "new-font", Unk0 = 0, Unk1 = 0, Unk2 = 0, Unk3 = 0, Unk4 = 0, Unk5 = 0, Unk6 = 0, Unk7 = 0, Unk8 = 0, Unk9 = 0, Unk10 = 0, Unk11 = 0 } });
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Language Name", ref value.LanguageName, 32);
            for (int i = 0; i < value.Values.Count; i++)
            {
                var x = value.Values[i];
                if(changed |= Editor(x.Data.IDName, ref x, ref value.Values))
                    value.Values[i] = x;
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Layout> value, ref List<TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Layout>> values)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            if (changed |= ImGui.Selectable("Add"))
                value.Values.Add(new() { Data = new() { IDName = "New Layout", Unk0 = 0, Unk1 = 0, Unk2 = 0, Unk3 = 0, Unk4 = 0, Unk5 = 0, Unk6 = 0, Unk7 = 0, Unk8 = 0 } });
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Language Name", ref value.LanguageName, 32);
            for (int i = 0; i < value.Values.Count; i++)
            {
                var x = value.Values[i];
                if (changed |= Editor(x.Data.IDName, ref x, ref value.Values))
                    value.Values[i] = x;
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings.LanguageSetting<TextProject.LanguageSettings.Layout> value)
    {
        bool changed = false;
        ImGui.PushID(value.GetHashCode());
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (changed |= ImGui.Selectable("Add"))
                value.Values.Add(new() { Data = new() { IDName = "New Layout", Unk0 = 0, Unk1 = 0, Unk2 = 0, Unk3 = 0, Unk4 = 0, Unk5 = 0, Unk6 = 0, Unk7 = 0, Unk8 = 0 } });
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Language Name", ref value.LanguageName, 32);
            for (int i = 0; i < value.Values.Count; i++)
            {
                var x = value.Values[i];
                if(changed |= Editor(x.Data.IDName, ref x, ref value.Values))
                    value.Values[i] = x;
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.LanguageSettings value)
    {
        bool changed = false;
        if (ImGui.TreeNode(label))
        {
            bool fontOpen = ImGui.TreeNode("Fonts");
            if (ImGui.BeginPopupContextItem("Options Fonts"))
            {
                if (changed |= ImGui.Selectable("Add"))
                    value.Fonts.Add(new() { LanguageName = "new" });
                ImGui.EndPopup();
            }
            if (fontOpen)
            {
                for (int i = 0; i < value.Fonts.Count; i++)
                {
                    var x = value.Fonts[i];
                    if(changed |= Editor(x.LanguageName, ref x, ref value.Fonts))
                        value.Fonts[i] = x;
                }
                ImGui.TreePop();
            }

            bool layoutOpen = ImGui.TreeNode("Layouts");
            if (ImGui.BeginPopupContextItem("Options Layouts"))
            {
                if (changed |= ImGui.Selectable("Add"))
                    value.Layouts.Add(new() { LanguageName = "new" });
                ImGui.EndPopup();
            }
            if (layoutOpen)
            {
                for (int i = 0; i < value.Layouts.Count; i++)
                {
                    var x = value.Layouts[i];
                    if(changed |= Editor(x.LanguageName, ref x, ref value.Layouts))
                        value.Layouts[i] = x;
                }
                ImGui.TreePop();
            }
            ImGui.TreePop();
        }
        return changed;
    }

    public static bool Editor(string label, ref TextProject.ProjectSettings.Color value, ref List<TextProject.ProjectSettings.Color> values)
    {
        bool changed = false;
        unsafe
        {
            fixed (byte* x = &value.Value.r)
                ImGui.PushID((void*)x);
        }
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Name", ref value.Name, 128);
            changed |= Basic.Editor("Value", ref value.Value);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.ProjectSettings.Color value)
    {
        bool changed = false;
        unsafe
        {
            fixed (byte* x = &value.Value.r)
                ImGui.PushID((void*)x);
        }
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Basic.Editor("Name", ref value.Name, 128);
            changed |= Basic.Editor("Value", ref value.Value);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.ProjectSettings.Language value, ref List<TextProject.ProjectSettings.Language> values)
    {
        bool changed = false;
        unsafe
        {
            fixed (long* x = &value.ID)
                ImGui.PushID(x);
        }
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Name", ref value.Name, 128);
            changed |= Basic.Editor("Short Name", ref value.ShortName, 32);
            changed |= Basic.Editor("ID", ref value.ID);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.ProjectSettings.Language value)
    {
        bool changed = false;
        unsafe
        {
            fixed (long* x = &value.ID)
                ImGui.PushID(x);
        }
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Basic.Editor("Name", ref value.Name, 128);
            changed |= Basic.Editor("Short Name", ref value.ShortName, 32);
            changed |= Basic.Editor("ID", ref value.ID);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextProject.ProjectSettings value)
    {
        bool changed = false;
        if (ImGui.TreeNode(label))
        {
            bool colorOpen = ImGui.TreeNode("Colors");
            if (ImGui.BeginPopupContextItem("Options Colors"))
            {
                if (changed |= ImGui.Selectable("Add"))
                    value.Colors.Add(new() { Name = "New Color", Value = new() { a = 0xFF } });
                ImGui.EndPopup();
            }
            if (colorOpen)
            {
                for (int i = 0; i < value.Colors.Count; i++)
                {
                    var x = value.Colors[i];
                    if(changed |= Editor(x.Name, ref x, ref value.Colors))
                        value.Colors[i] = x;
                }
                ImGui.TreePop();
            }
            bool langOpen = ImGui.TreeNode("Languages");
            if (ImGui.BeginPopupContextItem("Options Languages"))
            {
                if (changed |= ImGui.Selectable("Add"))
                    value.Languages.Add(new() { ID = value.Languages.Count, Name = "new", ShortName = "new" });
                ImGui.EndPopup();
            }
            if (langOpen)
            {
                for (int i = 0; i < value.Languages.Count; i++)
                {
                    var x = value.Languages[i];
                    if(changed |= Editor(x.Name, ref x, ref value.Languages))
                        value.Languages[i] = x;
                }
                ImGui.TreePop();
            }
            ImGui.TreePop();
        }
        return changed;
    }

    public static bool Editor(ref TextProject value)
    {
        bool changed = false;
        changed |= Editor("Project Settings", ref value.ProjSettings);
        changed |= Editor("Language Settings", ref value.LangSettings);
        return changed;
    }

    public static bool Editor(string label, ref TextProject value)
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

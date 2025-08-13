using AshDumpLib.HedgehogEngine.BINA.Converse;
using Hexa.NET.ImGui;

namespace ConverseEditor.Editors;

static class TextMetaEditors
{
    public static bool Editor(string label, ref TextMeta.IconData.Icon value, ref List<TextMeta.IconData.Icon> values)
    {
        bool changed = false;
        ImGui.PushID($"icon{values.IndexOf(value)}");
        bool open = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (open)
        {
            changed |= Basic.Editor("Icon Name", ref value.IconName, 128);
            changed |= Basic.Editor("Resource Name", ref value.ResourceName, 128);
            changed |= Basic.Editor("Unk0", ref value.Unk0);
            changed |= Basic.Editor("Unk1", ref value.Unk1);
            changed |= Basic.Editor("Unk2", ref value.Unk2);
            changed |= Basic.Editor("Unk3", ref value.Unk3);
            changed |= Basic.Editor("Image Crop", ref value.ImageCrop);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextMeta.IconData.Icon value)
    {
        bool changed = false;
        unsafe
        {
            fixed (float* x = &value.Unk0)
                ImGui.PushID(x);
        }
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Basic.Editor("Icon Name", ref value.IconName, 128);
            changed |= Basic.Editor("Resource Name", ref value.ResourceName, 128);
            changed |= Basic.Editor("Unk0", ref value.Unk0);
            changed |= Basic.Editor("Unk1", ref value.Unk1);
            changed |= Basic.Editor("Unk2", ref value.Unk2);
            changed |= Basic.Editor("Unk3", ref value.Unk3);
            changed |= Basic.Editor("Image Crop", ref value.ImageCrop);
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextMeta.IconData value)
    {
        bool changed = false;
        if (ImGui.TreeNode(label))
        {
            bool iconOpen = ImGui.TreeNode("Icons");
            if (ImGui.BeginPopupContextItem("Options Icons"))
            {
                if (ImGui.Selectable("Add"))
                    value.Icons.Add(new() { IconName = "New Icon", ResourceName = "new-icon", ImageCrop = new() { Right = 1, Bottom = 1 } });
                ImGui.EndPopup();
            }
            if (iconOpen)
            {
                for (int i = 0; i < value.Icons.Count; i++)
                {
                    var x = value.Icons[i];
                    if (changed |= Editor(x.IconName, ref x, ref value.Icons))
                        value.Icons[i] = x;
                }
                ImGui.TreePop();
            }

            bool resOpen = ImGui.TreeNode("Resources");
            if (ImGui.BeginPopupContextItem("Options Resources"))
            {
                if (ImGui.Selectable("Add"))
                    value.Resources.Add("New Resource");
                ImGui.EndPopup();
            }
            if (resOpen)
            {
                for (int i = 0; i < value.Resources.Count; i++)
                {
                    ImGui.PushID(i);
                    var x = value.Resources[i];
                    if (changed |= Basic.Editor("###", ref x, 128))
                        value.Resources[i] = x;
                    if (ImGui.BeginPopupContextItem($"Options_{i}_{x}"))
                    {
                        if (ImGui.Selectable("Delete"))
                            value.Resources.RemoveAt(i);
                        ImGui.EndPopup();
                    }
                    ImGui.PopID();
                }
                ImGui.TreePop();
            }
            ImGui.TreePop();
        }
        return changed;
    }

    public static bool Editor(string label, ref TextMeta.TypeFace value, ref List<TextMeta.TypeFace> values)
    {
        bool changed = false;
        ImGui.PushID($"typeface{values.IndexOf(value)}");
        bool mopen = ImGui.TreeNode(label + "###");
        if (ImGui.BeginPopupContextItem($"Options_{value}"))
        {
            if (ImGui.Selectable("Delete"))
                values.Remove(value);
            ImGui.EndPopup();
        }
        if (mopen)
        {
            changed |= Basic.Editor("Name 0", ref value.Name0, 128);
            changed |= Basic.Editor("Name 1", ref value.Name1, 128);
            changed |= Basic.Editor("Unk0", ref value.Unk0);
            changed |= Basic.Editor("Unk1", ref value.Unk1);
            changed |= Basic.Editor("Unk2", ref value.Unk2);
            changed |= Basic.Editor("Unk3", ref value.Unk3);
            bool open = ImGui.TreeNode("Parents");
            if (ImGui.BeginPopupContextItem("Options Parents"))
            {
                if (ImGui.Selectable("Add"))
                    value.Parents.Add("New Parent");
                ImGui.EndPopup();
            }
            if (open)
            {
                for(int i = 0; i < value.Parents.Count; i++)
                {
                    ImGui.PushID(i);
                    var x = value.Parents[i];
                    if(changed |= Basic.Editor("###", ref x, 128))
                        value.Parents[i] = x;
                    if(ImGui.BeginPopupContextItem($"Options_{i}_{x}"))
                    {
                        if (ImGui.Selectable("Delete"))
                            value.Parents.RemoveAt(i);
                        ImGui.EndPopup();
                    }
                    ImGui.PopID();
                }
                ImGui.TreePop();
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(string label, ref TextMeta.TypeFace value)
    {
        bool changed = false;
        unsafe
        {
            fixed (float* x = &value.Unk0)
                ImGui.PushID(x);
        }
        if (ImGui.TreeNode(label + "###"))
        {
            changed |= Basic.Editor("Name 0", ref value.Name0, 128);
            changed |= Basic.Editor("Name 1", ref value.Name1, 128);
            changed |= Basic.Editor("Unk0", ref value.Unk0);
            changed |= Basic.Editor("Unk1", ref value.Unk1);
            changed |= Basic.Editor("Unk2", ref value.Unk2);
            changed |= Basic.Editor("Unk3", ref value.Unk3);
            bool open = ImGui.TreeNode("Parents");
            if (ImGui.BeginPopupContextItem("Options Parents"))
            {
                if (ImGui.Selectable("Add"))
                    value.Parents.Add("New Parent");
                ImGui.EndPopup();
            }
            if (open)
            {
                for (int i = 0; i < value.Parents.Count; i++)
                {
                    ImGui.PushID(i);
                    var x = value.Parents[i];
                    if (changed |= Basic.Editor("###", ref x, 128))
                        value.Parents[i] = x;
                    if (ImGui.BeginPopupContextItem($"Options_{i}_{x}"))
                    {
                        if (ImGui.Selectable("Delete"))
                            value.Parents.RemoveAt(i);
                        ImGui.EndPopup();
                    }
                    ImGui.PopID();
                }
                ImGui.TreePop();
            }
            ImGui.TreePop();
        }
        ImGui.PopID();
        return changed;
    }

    public static bool Editor(ref TextMeta value)
    {
        bool changed = false;
        bool open = ImGui.TreeNode("Type Faces");
        if (ImGui.BeginPopupContextItem("Options Type Faces"))
        {
            if (ImGui.Selectable("Add"))
                value.TypeFaces.Add(new() { Name0 = "New Type Face", Name1 = "New Type Face"});
            ImGui.EndPopup();
        }
        if (open)
        {
            for (int i = 0; i < value.TypeFaces.Count; i++)
            {
                var x = value.TypeFaces[i];
                if (changed |= Editor(x.Name0, ref x, ref value.TypeFaces))
                    value.TypeFaces[i] = x;
            }
            ImGui.TreePop();
        }

        Editor("Images", ref value.Images);
        return changed;
    }

    public static bool Editor(string label, ref TextMeta value)
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

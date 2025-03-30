using AshDumpLib.HedgehogEngine.BINA.Converse;
using ConverseEditor.Editors;
using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConverseEditor.Panels;

class FileEditor : Panel
{
    public override void RenderPanel()
    {
        if (ConverseEditorApp.SelectedFile.GetType() == typeof(Text))
        {
            Text value = (Text)ConverseEditorApp.SelectedFile;
            TextEditors.Editor(ref value);
        }
        else if(ConverseEditorApp.SelectedFile.GetType() == typeof(TextMeta))
        {
            TextMeta value = (TextMeta)ConverseEditorApp.SelectedFile;
            TextMetaEditors.Editor(ref value);
        }
        else if (ConverseEditorApp.SelectedFile.GetType() == typeof(TextProject))
        {
            TextProject value = (TextProject)ConverseEditorApp.SelectedFile;
            TextProjectEditors.Editor(ref value);
        }
    }

    public override Properties GetProperties()
    {
        return new Properties("File Editor", new(160, 20), new(790, 500));
    }
}

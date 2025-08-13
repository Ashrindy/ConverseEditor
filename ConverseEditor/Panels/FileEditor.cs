using AshDumpLib.HedgehogEngine.BINA.Converse;
using ConverseEditor.Editors;

namespace ConverseEditor.Panels;

class FileEditor : Panel
{
    public FileEditor(Context ctx) : base(ctx) { }

    public override void RenderPanel()
    {
        var selectedFile = ctx.SelectedFile;
        if (selectedFile.GetType() == typeof(Text))
        {
            Text value = (Text)selectedFile;
            TextEditors.Editor(ref value);
        }
        else if(selectedFile.GetType() == typeof(TextMeta))
        {
            TextMeta value = (TextMeta)selectedFile;
            TextMetaEditors.Editor(ref value);
        }
        else if (selectedFile.GetType() == typeof(TextProject))
        {
            TextProject value = (TextProject)selectedFile;
            TextProjectEditors.Editor(ref value);
        }
    }

    public override Properties GetProperties()
    {
        return new Properties("File Editor", new(160, 20), new(790, 500));
    }
}

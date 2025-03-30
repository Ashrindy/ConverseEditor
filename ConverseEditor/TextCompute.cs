using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConverseEditor;

// All of this info was taken from PuyoTextEditor! https://github.com/nickworonekin/puyo-text-editor

static class TextCompute
{
    static ushort GetNameLength(ushort value) => (ushort)((((value & 0x0FF0) >> 4) / 2) - 1);
    static ushort SetNameLength(int value) => (ushort)((((value + 1) * 2) << 4) & 0x0FF0);

    public static string DecodeText(string text)
    {
        StringBuilder builder = new();
        for (int x = 0; x < text.Length; x++)
        {
            var i = text[x];
            switch(i & 0xF00F)
            {
                case 0xE000:
                    if(i == 0xE010)
                        builder.Append("</>");
                    else
                    {
                        x++;
                        var colorNameLength = GetNameLength(i) - 2;
                        var colorArgb = (uint)(text[x] << 16 | (text[x + 1]));
                        x += 2;
                        var colorName = new string(text.ToCharArray(), x, colorNameLength);
                        builder.Append(@$"<color name=""{colorName}"" value=""{colorArgb.ToString("X8")}>""");
                        x += colorNameLength;
                    }
                    break;
                case 0xE001:
                    x++;
                    var varNameLength = GetNameLength(i);
                    var varName = new string(text.ToCharArray(), x, varNameLength);

                    builder.Append(new XElement("var", new XAttribute("name", varName)));

                    x += varNameLength;
                    break;

                case 0xE005:
                    x++;

                    var imageNameLength = GetNameLength(i);
                    var imageName = new string(text.ToCharArray(), x, imageNameLength);

                    builder.Append(new XElement("image", new XAttribute("name", imageName)));

                    x += imageNameLength;

                    break;

                default:
                    builder.Append(i);
                    break;
            } 
        }
        return builder.ToString();
    }

    static void ParseXNode(XNode node, ref StringBuilder builder)
    {
        if (node.GetType() == typeof(XText))
        {
            XText text = (XText)node;
            builder.Append(text.Value);
        }
        else if(node.GetType() == typeof(XElement))
        {
            XElement elem = (XElement)node;
            switch (elem.Name.LocalName)
            {
                case "color":
                    var colorName = elem.Attribute("name").Value;
                    var colorArgb = uint.Parse(elem.Attribute("value").Value, NumberStyles.HexNumber);
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)(0xE000 | SetNameLength(colorName.Length + 2)))));
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)(colorArgb >> 16))));
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)(colorArgb & 0xFFFF))));
                    builder.Append(colorName.ToCharArray());
                    builder.Append('\0');
                    ParseXNode(elem, ref builder);
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)0xE010)));
                    break;

                case "var":
                    var varName = elem.Attribute("name").Value;
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)(0xE001 | SetNameLength(varName.Length)))));
                    builder.Append(varName);
                    builder.Append('\0');
                    break;

                case "image":
                    var imageName = elem.Attribute("name").Value;
                    builder.Append(Encoding.Unicode.GetString(BitConverter.GetBytes((ushort)(0xE005 | SetNameLength(imageName.Length)))));
                    builder.Append(imageName);
                    builder.Append('\0');
                    break;

                default:
                    builder.Append(elem.ToString());
                    break;
            }
        }
        if(node.NextNode != null)
            ParseXNode(node.NextNode, ref builder);
    }

    public static string EncodeString(string text)
    {
        StringBuilder builder = new();
        XElement xml = null;
        try
        {
            xml = XElement.Parse($"<root>{text}</root>");
        }
        catch (XmlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        if (xml != null)
        {
            if (xml.FirstNode != null)
                ParseXNode(xml.FirstNode, ref builder);
            return builder.ToString();
        }
        else
            return text;
    }
}
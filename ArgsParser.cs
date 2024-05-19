using System;
using System.Collections.Generic;

namespace _1bitToPng
{
    public class ArgsParser
    {
        readonly Type m_type = typeof(ArgsParser);
        public bool attr;
        public bool atlas;
        public bool inv;
        public bool trans;
        public int w;
        public int h;
        public string FileName = "";

        public void Parse(IEnumerable<string> args)
        {

            attr = false;
            atlas = false;
            inv = false;
            trans = false;
            w = 0;
            h = 0;
            FileName = "";
            
            foreach (var argument in args)
            {
                var a = argument.Trim();
                if (a.Contains("."))
                {
                    FileName = a;
                }
                else
                {
                    switch (a)
                    {
                        case string str when str.Contains("w"):
                            int.TryParse(str.Replace("w", ""), out w);
                            SetField<int>("w", w);
                            break;
                        case string str when str.Contains("h"):
                            int.TryParse(str.Replace("h", ""), out h);
                            SetField<int>("h", h);
                            break;
                        default:
                            SetField<bool>(a, true);
                            break;
                    }
                }
            }
        }


        public void Help()
        {
            Console.Out.WriteLine("Help:");
            Console.Out.WriteLine($"\tname.ext\tSpecify the file name in the local folder.");
            Console.Out.WriteLine($"\tatlas\tWill pack all sprites into an atlas. Otherwise, each sprite will be a separate file.");
            Console.Out.WriteLine($"\ttrans\tThe background of the sprites will be transparent. Otherwise the paper will be white or black.");
            Console.Out.WriteLine($"\tattr\tSprite attributes will be used. This will only work if each sprite is followed by its attributes.");
            Console.Out.WriteLine($"\tw\tThe width of each sprite. Example: w16");
            Console.Out.WriteLine($"\th\tThe height of each sprite. Example: h16");
            Console.Out.WriteLine($"\tinv\tThe ink and paper will be inverted.");
            Console.Out.WriteLine();
        }

        void SetField<T>(string argument, object value)
        {
            var field = m_type.GetField(argument);
            if (field != null)
            {
                field.SetValue(this, (T)value);
            }
            else
            {
                Console.Out.WriteLine($"\nExtra unused argument: {argument}\n");
            }
        }
    }
}
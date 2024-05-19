using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _1bitToPng.Converter
{
    public class BinToPng
    {
        readonly byte[] m_bytes;
        readonly bool[] m_pixels;
        readonly List<Bitmap> m_sprites;

        readonly Color m_transparent = Color.FromArgb(0, 1, 1, 1);

        int m_spriteWidth;
        int m_spriteHeight;

        public BinToPng(byte[] bytes)
        {
            m_bytes = bytes;
            m_sprites = new List<Bitmap>();
            m_pixels = ByteArrayToBoolPixels(bytes);
        }

        public void CreateSprites(int width, int height, bool attributes, bool invertColor, bool transparent)
        {
            m_spriteWidth = width;
            m_spriteHeight = height;
            var spriteSize = width * height;
            var spriteCount = m_pixels.Length / spriteSize;
            Console.Out.WriteLine($"Sprites count: {spriteCount}");
            var pixelOffset = 0;
            for (var i = 0; i < spriteCount; i++)
            {
                var newSprite = attributes ? DrawColorSprite(width, height, ref pixelOffset) : Draw1BitSprite(width, height, ref pixelOffset, invertColor, transparent);
                m_sprites.Add(newSprite);
            }
        }


        public void Save(bool asAtlas, string fileName)
        {
            var dir =  LoadBin.SaveFolder;
            if (asAtlas)
            {
                SaveAtlas(fileName,dir);
            }
            else
            {
                SaveEachSprite(fileName, dir);
            }
        }

        void SaveEachSprite(string fileName, string dir)
        {
            CreateFolder(dir);
            for (var i = 0; i < m_sprites.Count; i++)
            {
                var name = $"{dir}/{fileName}_Sprite_{i}.png";
                m_sprites[i].Save(name, ImageFormat.Png);
                Console.Out.WriteLine($"The sprite was created and saved with the name: {name}");
            }
        }

        void CreateFolder(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        void SaveAtlas(string fileName, string dir)
        {
            var spriteSize = m_spriteWidth * m_spriteHeight;
            var spriteCount = m_pixels.Length / spriteSize;
            var atlasSize = Math.Ceiling(Math.Sqrt(spriteCount));
            var atlasWidth = (int)(atlasSize * m_spriteWidth);
            var atlasHeight = (int)(atlasSize * m_spriteHeight);

            Console.Out.WriteLine($"Atlas size: width[{atlasWidth}]height[{atlasHeight}]");

            var bitmap = new Bitmap(atlasWidth, atlasHeight);
            var atlas = Graphics.FromImage(bitmap);
            atlas.Clear(Color.Transparent);

            var x = 0;
            var y = 0;
            foreach (var image in m_sprites)
            {
                atlas.DrawImage(image, x, y, m_spriteWidth, m_spriteHeight);
                x += m_spriteWidth;
                if (x < atlasWidth) continue;
                x = 0;
                y += m_spriteHeight;
            }

            CreateFolder(dir);
            var name = $"{dir}/{fileName}_Atlas.png";
            bitmap.Save(name, ImageFormat.Png);
            Console.Out.WriteLine($"The atlas was created and saved with the name: {name}");
        }

        Bitmap Draw1BitSprite(int width, int height, ref int pixelOffset, bool invertColor, bool transparent)
        {
            var sprite = new Bitmap(width, height);
            var ink = invertColor ? Color.Black : Color.White;
            var paper = transparent ? m_transparent : invertColor ? Color.White : Color.Black;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (m_pixels[pixelOffset++])
                    {
                        sprite.SetPixel(x, y, ink);
                    }
                    else
                    {
                        sprite.SetPixel(x, y, paper);
                    }
                }
            }

            return sprite;
        }

        Bitmap DrawColorSprite(int width, int height, ref int pixelOffset)
        {
            var attrSize = width * height / 64;
            var attrs = GetSpriteColors(attrSize, pixelOffset + width * height);
            var sprite = new Bitmap(width, height);
            var xSymbolSize = width / 8;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var attrId = x / 8 + y / 8 * xSymbolSize;
                    if (m_pixels[pixelOffset++])
                    {
                        sprite.SetPixel(x, y, attrs[attrId].Ink);
                    }
                    else
                    {
                        sprite.SetPixel(x, y, attrs[attrId].Paper);
                    }
                }
            }

            return sprite;
        }

        InkPaper[] GetSpriteColors(int attrSize, int attrOffset)
        {
            var attrs = new InkPaper[attrSize];
            for (var i = 0; i < attrs.Length; i++)
            {
                var color = ZxColor.GetColor(m_bytes[attrOffset++]);
                attrs[i] = color;
            }

            return attrs;
        }

        bool[] ByteArrayToBoolPixels(byte[] bytes)
        {
            var p = new List<bool>();
            foreach (var b in bytes)
            {
                for (var i = 0; i < 8; i++)
                {
                    var pix = ((b << i) & 0x80) == 0x80;
                    p.Add(pix);
                }
            }

            return p.ToArray();
        }
    }
}
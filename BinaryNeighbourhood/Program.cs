using System;
using System.Numerics;
using Raylib_cs;

namespace BinaryNeighbourhoods
{
    class Program
    {
        static readonly int[,] shiftMap = new int[5, 5]
        {
            { 5, 4, 2, 4, 5 },
            { 4, 3, 1, 3, 4 },
            { 2, 1, 0, 1, 2 },
            { 4, 3, 1, 3, 4 },
            { 5, 4, 2, 4, 5 },
        };

        static readonly int pixelPadding = 1;
        static readonly int neighbourhoodsPerRow = 8;
        static readonly int numberOfNeighbourhoods = 64;

        static readonly int textureWidth = pixelPadding + ((5 + pixelPadding) * neighbourhoodsPerRow);
        static readonly int textureHeight = pixelPadding + ((5 + pixelPadding) * (numberOfNeighbourhoods / neighbourhoodsPerRow));

        static readonly int textureScale = 15;

        static readonly Color colorA = new Color(119, 80, 204, 255);
        static readonly Color colorB = new Color(207, 83, 89, 255);

        static readonly Random rand = new Random();

        static void Main(string[] args)
        {
            Raylib.InitWindow(textureWidth * textureScale, textureHeight * textureScale, "Binary Neighbourhoods");

            RenderTexture2D renderTexture = Raylib.LoadRenderTexture(textureWidth, textureHeight);

            Raylib.BeginTextureMode(renderTexture);

            for (byte n = 0; n < numberOfNeighbourhoods; n++)
            {
                int nx = pixelPadding + ((5 + pixelPadding) * (n % neighbourhoodsPerRow));
                int ny = pixelPadding + ((5 + pixelPadding) * (n / neighbourhoodsPerRow));

                float lerpValue = rand.NextSingle();
                Color col = ColorLerp(colorA, colorB, lerpValue);

                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if ((n >> shiftMap[x, y] & 1) == 1)
                        {
                            Raylib.DrawPixel(nx + x, ny + y, col);
                        }
                    }
                }
            }

            Raylib.EndTextureMode();

            Texture2D tex = renderTexture.texture;

            while(!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.DrawTextureEx(tex, Vector2.Zero, 0, textureScale, Color.WHITE);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static Color ColorLerp(Color a, Color b, float t)
        {
            return new Color(
                (byte)Lerp(a.r, b.r, t),
                (byte)Lerp(a.g, b.g, t),
                (byte)Lerp(a.b, b.b, t),
                (byte)Lerp(a.a, b.a, t)
                );
        }

        static float Lerp(float a, float b, float t)
        {
            return (a * (1 - t)) + (b * t);
        }
    }
}
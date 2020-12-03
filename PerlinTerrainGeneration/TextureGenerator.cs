using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }


    public static Texture2D TextureFromHeightMap(float [,] heightMap)
    {
        // Width is the legnth of the x dimension we set as mapWidth
        int width = heightMap.GetLength(0);

        // Height is the legnth of the y dimension we set as mapHeight
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];


        // Cycle through each pixel in the map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Set each color to it's perlin noise value
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x,y]);
            }
        }

        return TextureFromColorMap(colorMap, width, height);
    }
}

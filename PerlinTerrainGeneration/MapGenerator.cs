using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public int seed;
    public Vector2 offset;
    public float lacunarity;
    public float scale;
    public int octaves;
    public Terrain terrain;
    public bool drawMap = false;

    float[,] previousNoiseMap;


    

    [Range(0,1)]
    public float persistance;
    public bool autoUpdate = false;

    public TerrainType[] regions;
    

    public void GenerateMap()
    {
        // Generate a perlin noise 2D float array
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, octaves, lacunarity, persistance, scale, offset);


        

        if (drawMap)
        {
            MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

            if (drawMode == DrawMode.NoiseMap)
            {
                // DRAW HEIGHTMAP


                mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));



            }
            else if (drawMode == DrawMode.ColorMap)
            {
                // DRAW COLOR MAP


                // Create an array of colors
                Color[] colorMap = new Color[mapWidth * mapHeight];


                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        float currentHeight = noiseMap[x, y];

                        for (int i = 0; i < regions.Length; i++)
                        {
                            if (currentHeight <= regions[i].height)
                            {
                                colorMap[y * mapWidth + x] = regions[i].color;
                                break;
                            }
                        }
                    }
                }


                mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));

            }
        }

        if (terrain != null)
        {

            // If we changed the map
            if (TextureGenerator.TextureFromHeightMap(noiseMap) != terrain.terrainData.heightmapTexture)
            {

                // Save the height map as a png

                SaveTextureAsPNG(TextureGenerator.TextureFromHeightMap(noiseMap), "Assets/Textures/heightMap.png");


                // Apply to terrain

                terrain.terrainData.SetHeights(0, 0, noiseMap);
                terrain.terrainData.SetHeightsDelayLOD(0, 0, noiseMap);
            }
        }


        previousNoiseMap = noiseMap;
    }

    private void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }


    public void SaveTextureAsPNG(Texture2D texture, string fullPath)
    {
        byte[] _bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + fullPath);
    }

}




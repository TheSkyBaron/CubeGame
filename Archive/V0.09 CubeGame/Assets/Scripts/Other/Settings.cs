
using UnityEngine;

namespace Game.Engine
{
    public static class Settings
    {
        // Engine

        // Workbench Mesh

        public static readonly int MaxVerticies = 1000000;

        public static readonly int MaxTriangles = MaxVerticies * 3;

        // Save file locations

        public static readonly string SaveFileType = ".Vehicle";

        public static readonly string SaveFileFoldersName = "Vehicles";

        public static string SaveFoldersPath = "";

        public static readonly string CoverImage = "Cover.png";

        // UI

        // Main Menu

        public static readonly float CreditsTextScrollOffset = 250;
        public static readonly float MenuDecorObjectSpinSpeed = 0.1f;
        public static readonly int DecorBlockID = 2;

        // Photoshoot

        public static readonly int PhotoshootImageWidth = 256;
        public static readonly int PhotoshootImageHeight = PhotoshootImageWidth;
        public static readonly Vector2Int PhotoshootStartPixelPosition = new Vector2Int(832,476);
    }
}
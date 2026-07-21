
using Unity.Mathematics;
using UnityEngine;

namespace Game.Engine
{
    public static class Settings
    {
        // Engine

        // Mathematics

        public static readonly float Epsilon = 0.1f;

        // Workbench Mesh

        public static readonly int MaxVerticies = 1000000;

        public static readonly int MaxTriangles = MaxVerticies * 3;

        public static readonly Vector3 NullVector3 = new(-1, -1, -1);

        public static readonly int3 Nullint3 = new(-9999, -9999, -9999);

        public static readonly int NullInt = -1;

        public static readonly int Air = 0;

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
        public static readonly Vector2Int PhotoshootStartPixelPosition = new(832,476);

        // Compiler

    }
}
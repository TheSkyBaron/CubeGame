using UnityEngine;

namespace Game.Data
{
    public static class PlayerSettings
    {
        public static float MouseSensevity { get; private set; } = 0.1f;

        public static void SetMouseSensevity(float Amount)
        {
            MouseSensevity = Mathf.Clamp01(Amount);
        }
    }
}


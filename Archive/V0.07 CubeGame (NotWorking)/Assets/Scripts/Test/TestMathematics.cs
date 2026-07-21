using Unity.Mathematics;
using UnityEngine;

namespace Game.Test
{
    public class TestMathematics : MonoBehaviour
    {
        void Start()
        {
            int3 Gridsize = new(160, 160, 160);
            for(int i = 0; i < 50;i++)
            {
                int3 RandomVal = new int3(i,(int)(0.5 * i),2*i);
                Debug.Log("Test Value: " + RandomVal);
                int Pos = Mathematics.CommonMathematics.PositionToArray(RandomVal, Gridsize);
                if (Mathematics.CommonMathematics.ArrayToPosition(Pos, Gridsize).Equals(RandomVal)) Debug.Log("Match!");
                else Debug.LogWarning("Error at :" + RandomVal);
            }
        }
    }
}



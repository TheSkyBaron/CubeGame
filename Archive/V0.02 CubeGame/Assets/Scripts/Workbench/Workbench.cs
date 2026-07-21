using Game.Grid;
using Game.Renderer;
using Unity.Mathematics;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private int3 WorkbenchSize = new(16, 16, 16);
    private int BlockPer1M = 4;
    private GridModule GridSystem;
    private IRenderer RendererModule = new WorkbenchRenderer();
    void Start()
    {
        GridSystem = new(WorkbenchSize, BlockPer1M);
       // GridSystem.SetBlock(new int3(WorkbenchSize.x / 2, WorkbenchSize.y / 2, WorkbenchSize.z / 2) * BlockPer1M, 1);
        GridSystem.MassSetBlock(new int3(64, 64, 64), new int3(0, 0, 0), 1);
        gameObject.GetComponent<MeshFilter>().mesh = RendererModule.Calculate(GridSystem.BuildableGrid,GridSystem.Gridsize);
    }
}

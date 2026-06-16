using Game.Grid;
using Game.Renderer;
using Unity.Mathematics;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private int3 WorkbenchSize = new(16, 16, 16);
    private readonly int BlockPer1M = 4;
    public GridModule GridSystem { get; private set; }
    private readonly IRenderer RendererModule = new WorkbenchRenderer();
    void Start()
    {
        GridSystem = new(WorkbenchSize, BlockPer1M);
        
        //GridSystem.MassSetBlock(new int3(64, 64, 64), new int3(0, 0, 0), 1);
        GridSystem.SetBlock(new int3(WorkbenchSize.x / 2, WorkbenchSize.y / 2, WorkbenchSize.z / 2) * BlockPer1M, 1);
        RenderWorkbench();
    }

    public void RenderWorkbench()
    {
        Mesh Calculated = RendererModule.Calculate(GridSystem.BuildableGrid, GridSystem.Gridsize);
        gameObject.GetComponent<MeshFilter>().sharedMesh = Calculated;
        gameObject.GetComponent<MeshCollider>().sharedMesh = Calculated;
    }
}

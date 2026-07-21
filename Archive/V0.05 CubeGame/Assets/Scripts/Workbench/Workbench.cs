using Game.Grid;
using Game.Utils;
using Game.Renderer;
using Unity.Mathematics;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private int3 WorkbenchSize = new(16, 16, 16);
    private readonly int BlockPer1M = 5;
    private GameObject PlayerObject;
    public GridModule GridSystem { get; private set; }
    private readonly IRenderer RendererModule = new WorkbenchRenderer();
    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        GridSystem = new(WorkbenchSize, BlockPer1M);
        //GridSystem.MassSetBlock(new int3(64, 64, 64), new int3(0, 0, 0), 1);
        ResetWorkbench();
    }

    public void RenderWorkbench()
    {
        Mesh Calculated = RendererModule.Calculate(GridSystem.BuildableGrid, GridSystem.Gridsize);
        gameObject.GetComponent<MeshFilter>().sharedMesh = Calculated;
        gameObject.GetComponent<MeshCollider>().sharedMesh = Calculated;
    }

    public void ResetWorkbench()
    {
        int3 WorkbenchCalculatedSize = WorkbenchSize * BlockPer1M;
        GridSystem.ResetGrid();
        GridSystem.SetBlock(new int3(WorkbenchCalculatedSize.x / 2, WorkbenchCalculatedSize.y / 2, WorkbenchCalculatedSize.z / 2), 1,Byte3.Zero);
        RenderWorkbench();
        PlayerObject.transform.position.Set(transform.position.x,transform.position.y,transform.position.z);
    }
}

using Game.Grid;
using Game.Utils;
using Game.Renderer;
using Unity.Mathematics;
using UnityEngine;
using Game.Player;

public class Workbench : MonoBehaviour
{
    private int3 WorkbenchSize = new(16, 16, 16);
    private readonly int BlockPer1M = 5;
    private GameObject PlayerObject;
    public GridModule GridSystem { get; private set; }
    private readonly IRenderer RendererModule = new WorkbenchRenderer();
    void Start()
    {
        int LoopCancel = 0;
        while (PlayerObject == null && LoopCancel < 100)
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            LoopCancel++;
        }

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
        Vector3 WorkbenchStartPosition = new((WorkbenchCalculatedSize.x + transform.position.x +1) / 2, (WorkbenchCalculatedSize.y + transform.position.y +1) / 2, (WorkbenchCalculatedSize.z + transform.position.z + 1) / 2);
        GridSystem.ResetGrid();
        GridSystem.SetBlock(new int3(WorkbenchCalculatedSize.x / 2, WorkbenchCalculatedSize.y / 2, WorkbenchCalculatedSize.z / 2), 1, Byte3.Zero);
        RenderWorkbench();
        PlayerObject.GetComponent<Rigidbody>().position = WorkbenchStartPosition;
        PlayerObject.GetComponent<PlayerWorkbenchController>().SetRotation(new Vector3(-45,45,0));
        PlayerObject.GetComponent<PlayerWorkbenchController>().SetZoom(15);
    }
}

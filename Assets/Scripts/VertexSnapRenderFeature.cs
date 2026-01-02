using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VertexSnapRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material vertexSnapMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public Settings settings = new Settings();

    class VertexSnapPass : ScriptableRenderPass
    {
        private Material snapMaterial;
        private RTHandle cameraColorHandle;
        private RTHandle tempColorHandle;

        public VertexSnapPass(Material material, RenderPassEvent passEvent)
        {
            snapMaterial = material;
            renderPassEvent = passEvent;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(
                ref tempColorHandle,
                desc,
                FilterMode.Bilinear,
                TextureWrapMode.Clamp,
                name: "_VertexSnapTempTexture"
            );
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (snapMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Vertex Snap Pass");

            // First blit: camera → temp (with material)
            Blit(cmd, cameraColorHandle, tempColorHandle, snapMaterial);

            // Second blit: temp → camera
            Blit(cmd, tempColorHandle, cameraColorHandle);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // RTHandles are reused; no manual release needed
        }
    }

    private VertexSnapPass snapPass;

    public override void Create()
    {
        snapPass = new VertexSnapPass(
            settings.vertexSnapMaterial,
            settings.renderPassEvent
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.vertexSnapMaterial == null)
            return;

        renderer.EnqueuePass(snapPass);
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationRenderFeature : ScriptableRendererFeature
{
    class PosterizationPass : ScriptableRenderPass
    {
        private Material posterizationMaterial;

        public PosterizationPass(Material material)
        {
            posterizationMaterial = material;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (posterizationMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Posterization Effect");
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Apply the posterization material
            cmd.Blit(source, source, posterizationMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] private Material posterizationMaterial;
    private PosterizationPass posterizationPass;

    public override void Create()
    {
        if (posterizationMaterial != null)
            posterizationPass = new PosterizationPass(posterizationMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (posterizationPass != null && posterizationMaterial != null)
            renderer.EnqueuePass(posterizationPass);
    }
}

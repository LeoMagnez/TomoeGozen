using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DepthNormalsFeature : ScriptableRendererFeature
{
    class Pass : ScriptableRenderPass
    {
        private Material material;
        private List<ShaderTagId> shaderTags;
        private FilteringSettings filteringSettings;
        private RenderTargetHandle destinationHandle;
        public Pass(Material material)
        {
            this.material = material;

            this.shaderTags = new List<ShaderTagId>()
            {
                new ShaderTagId("DepthOnly"),
                //new ShaderTagId("SRPDefaultUnlit"),
                //new ShaderTagId("UniversalForward"),
                //new ShaderTagId("LightweightForward"),
            };

            //Render opaque materials
            this.filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            destinationHandle.Init("_DepthNormalsTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(destinationHandle.id, cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(destinationHandle.Identifier());
            ConfigureClear(ClearFlag.All, Color.black);
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var drawSettings = CreateDrawingSettings(shaderTags, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            drawSettings.overrideMaterial = material;
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(destinationHandle.id);
        }


    }

    private Pass pass;


    public override void Create()
    {
        Material material = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
        this.pass = new Pass(material);

        pass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }


    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}



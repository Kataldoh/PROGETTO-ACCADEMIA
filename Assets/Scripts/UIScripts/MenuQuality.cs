using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class MenuQuality : MonoBehaviour
{
    public RenderPipelineAsset[] quality;

    public void Start()
    {
         //valuequality = QualitySettings.GetQualityLevel();
    }

    public void LowQuality()
    {
        QualitySettings.SetQualityLevel(0);
        QualitySettings.renderPipeline = quality[0];
    }
    public void MediumQuality()
    {
        QualitySettings.SetQualityLevel(1);
        QualitySettings.renderPipeline = quality[1];
    }
    public void HighQuality()
    {
        QualitySettings.SetQualityLevel(2);
        QualitySettings.renderPipeline = quality[2];
    }
}

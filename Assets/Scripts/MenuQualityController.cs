using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuQualityController : MonoBehaviour
{
    public RenderPipelineAsset[] quality;
    public TMP_Dropdown dropdown;

    public void Start()
    {
        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void QualityControl(int levelquality)
    {
        QualitySettings.SetQualityLevel(levelquality);
        QualitySettings.renderPipeline = quality[levelquality];
    }

}

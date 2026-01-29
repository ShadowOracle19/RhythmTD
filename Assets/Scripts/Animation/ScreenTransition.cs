using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenTransition : MonoBehaviour
{
    // VARIABLES

    public Animator animator;
    public bool transitionPlaying = false;
    
    public Material shaderMaterial;

    [Header("Shader Properties")]
    public Vector2 texDimensions = new Vector2(0.0f,0.0f);
    
    public float progress = 0.0f;

    public Vector2 startPosition = new Vector2(0.0f,0.0f);

    public int inverted = 1;
    public int transparent = 1;

    public float effectGradient = 0.0f;
    public float effectRotation = 0.0f;
    public float dots = 10.0f;
    public float outlineThickness = 0.0f;
    
    void Start()
    {
        //shaderMaterial = GetComponent<Renderer>().material;
    }
    
    public void PlayTransition()
    {
        StartCoroutine(Transition());
    }
    
    public IEnumerator Transition()
    {
        while (transitionPlaying)
        {
            UpdateShader();

            yield return null;
        }
    }

    public void UpdateShader()
    {
        shaderMaterial.SetVector("_TextureDimensions", texDimensions);
        shaderMaterial.SetFloat("_TransitionProgress", progress);
        shaderMaterial.SetVector("_StartPosition", startPosition);
        shaderMaterial.SetInt("_IsInverted", inverted);
        shaderMaterial.SetInt("_IsTransparent", transparent);
        shaderMaterial.SetFloat("_EffectGradient", effectGradient);
        shaderMaterial.SetFloat("_EffectRotation", effectRotation);
        shaderMaterial.SetFloat("_DotDensity", dots);
        shaderMaterial.SetFloat("_OutlineThickness", outlineThickness);
    }

    private void Update()
    {
        if (transitionPlaying)
        {
            PlayTransition();
        }
    }
    
}

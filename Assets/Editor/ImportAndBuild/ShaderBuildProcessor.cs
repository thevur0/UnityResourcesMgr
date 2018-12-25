
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// Simple example of stripping of a debug build configuration
class ShaderBuildProcessor : IPreprocessShaders
{
    List<ShaderKeyword> m_DelKeyword = new List<ShaderKeyword>();

    public ShaderBuildProcessor()
    {
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF9"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF8"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF7"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF6"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF5"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF4"));
        m_DelKeyword.Add(new ShaderKeyword("TIME_OFF3"));
    }

    // Multiple callback may be implemented. 
    // The first one executed is the one where callbackOrder is returning the smallest number.
    public int callbackOrder { get { return 0; } }

    public bool KeepVariant(Shader shader, ShaderSnippetData snippet, ShaderCompilerData shaderVariant)
    {

        foreach( var keyword in m_DelKeyword)
        {
            if (shaderVariant.shaderKeywordSet.IsEnabled(keyword))
                return false;
        }
        return true;
    }

    public void OnProcessShader(
        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderVariants)
    {
        int inputShaderVariantCount = shaderVariants.Count;

        for (int i = 0; i < shaderVariants.Count; ++i)
        {
            bool keepVariant = KeepVariant(shader, snippet, shaderVariants[i]);
            if (!keepVariant)
            {
                shaderVariants.RemoveAt(i);
                --i;
            }
        }
    }
}
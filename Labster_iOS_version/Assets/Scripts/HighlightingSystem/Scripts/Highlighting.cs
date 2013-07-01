using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Highlighting {
	// Shader replacement map ("source shader name", "replacement shader name")
	static private string[,] map = new string[,] {
		{"Bumped Diffuse", "Hidden/Highlighted/Bumped Diffuse"}, 
		{"Bumped Specular", "Hidden/Highlighted/Bumped Specular"}, 
		{"Diffuse", "Hidden/Highlighted/Diffuse"}, 
		{"Diffuse Detail", "Hidden/Highlighted/Diffuse Detail"}, 
		{"Parallax Diffuse", "Hidden/Highlighted/Parallax Diffuse"}, 
		{"Parallax Specular", "Hidden/Highlighted/Parallax Specular"}, 
		{"Specular", "Hidden/Highlighted/Specular"}, 
		{"VertexLit", "Hidden/Highlighted/VertexLit"},
		{"ReaShader/Transparent/Glass_clean_reflect_Mobile", "Hidden/Highlighted/Glass_clean_reflect_Mobile"},
        {"ReaShader/Transparent/GlassCleanMobile", "ReaShader/Transparent/GlassCleanMobile"},
//		{"ReaShader/Special/Hologram_simple", "Hidden/Highlighted/Hologram_simple"},
		{"Legacy Shaders/Lightmapped/Diffuse", "Hidden/Highlighted/Lightmapped/Diffuse"},
		{"ReaShader/UberShader_Opaque", "Hidden/Highlighted/ReaShader/UberShader_Opaque"},
		/* {"Transparent/VertexLit", "Hidden/Highlighted/Transparent/VertexLit"}, */
	};
	
	static private Dictionary<string, Shader> shaderMap;
	
	private static Shader _defaultHighlightingShader = null;
	protected static Shader defaultReplacementShader {
		get {
			return _defaultHighlightingShader;
		}
	}
	
	// Replacement shaders initialization
	static public void Init()
	{
		_defaultHighlightingShader = Resources.Load("HighlightingDefaultShader", typeof(Shader)) as Shader;
		shaderMap = new Dictionary<string, Shader>();
		
		for (int i = 0; i < map.GetLength(0); i++)
		{
			Shader ss = Shader.Find(map[i, 0]);
			
			if (ss == null)
			{
				Debug.LogWarning("Highlighting : Init() : Source shader with name '" + map[i, 0] + "' not found!");
				continue;
			}
			else
			{
				if (shaderMap.ContainsKey(ss.name))
				{
					Debug.LogWarning("Highlighting : Init() : Value for duplicated source shader name '" + map[i, 0] + "' will not be used!");
					continue;
				}
				
				Shader rs = Shader.Find(map[i, 1]);
				
				if (rs == null)
				{
					Debug.LogWarning("Highlighting : Init() : Replacement shader with name '" + map[i, 1] + "' for source shader with name '" + map[i, 0] + "' not found!");
					continue;
				}
				else{
					shaderMap.Add(ss.name, rs);
				}
			}
		}

		// Preload all shaders
		Shader.WarmupAllShaders();
	}
	
	static public Shader GetReplacementShader(Shader sourceShader)
	{
		if (shaderMap == null)
			Init();
		
		Shader replacementShader;
		
		shaderMap.TryGetValue(sourceShader.name, out replacementShader);
				
		if (replacementShader)
			return replacementShader;
		else
			return defaultReplacementShader;
	}
}
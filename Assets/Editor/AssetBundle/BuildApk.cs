using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class BuildApk {

	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			if(e.enabled && File.Exists(e.path))
				names.Add(e.path);
		}
		return names.ToArray();
	}
	[MenuItem("Tools/Release/BuildForAndroid")]
	static void BuildForAndroid()
	{
        string path = StringUtils.PathCombine(UnityUtils.UnityProjectPath(), UnityUtils.ProjectName)+".apk";
		BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
	}
}

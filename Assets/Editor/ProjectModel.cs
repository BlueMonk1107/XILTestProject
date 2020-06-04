using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 开发模式编辑器工具
/// </summary>
[InitializeOnLoad]
public class ProjectModel
{
	public enum ProjectModelSymbol
	{
		TEST_MODEL,
		BUILD_MODEL,
		COUNT
	}
	
	private const string _testName = "Tools/ChangeProjectModel/TestModel";
	private const string _buildName = "Tools/ChangeProjectModel/BuildModel";
	private const string _configKey = "ProjectModel";

	static ProjectModel()
	{
		if (!EditorPrefs.HasKey(_configKey))
		{
			EditorPrefs.SetString(_configKey,_testName);
			SetChecked(_testName);
			PlayerSettings.SetScriptingDefineSymbolsForGroup( 
				BuildTargetGroup.Standalone , GetSymbol(ProjectModelSymbol.TEST_MODEL));
		}
	}

	[MenuItem(_testName,true)]
	public static bool ChangeTestSelected()
	{
		SetChecked(_testName);
		return true;
	}
	
	[MenuItem(_testName,false)]
	public static void ChangeTestUnselected()
	{
		if (GetCurrentModel() != _testName)
		{
			EditorPrefs.SetString(_configKey,_testName);

			PlayerSettings.SetScriptingDefineSymbolsForGroup( 
				BuildTargetGroup.Standalone , GetSymbol(ProjectModelSymbol.TEST_MODEL));
		}
	}

	private static string GetSymbol(ProjectModelSymbol addSymbol)
	{
		List<string> newSymbols = new List<string>();
		string[] symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');

		foreach (string symbol in symbols)
		{
			bool contain = false;
			for (ProjectModelSymbol i = 0; i < ProjectModelSymbol.COUNT; i++)
			{
				if (symbol == i.ToString())
				{
					contain = true;
				}
			}

			if (!contain)
			{
				newSymbols.Add(symbol);
			}
		}
			
		newSymbols.Add(addSymbol.ToString());

		return string.Join(";", newSymbols);
	}
	
	[MenuItem(_buildName,true)]
	public static bool ChangeBuildSelected()
	{
		SetChecked(_buildName);
		return true;
	}
	
	[MenuItem(_buildName,false)]
	public static void ChangeBuildUnselected()
	{
		if (GetCurrentModel() != _buildName)
		{
			EditorPrefs.SetString(_configKey,_buildName);
			PlayerSettings.SetScriptingDefineSymbolsForGroup( 
				BuildTargetGroup.Standalone , GetSymbol(ProjectModelSymbol.BUILD_MODEL));
		}
	}

	private static void SetChecked(string name)
	{
		string value = GetCurrentModel();
		Menu.SetChecked(name, value == name);
	}

	private static string GetCurrentModel()
	{
		return EditorPrefs.GetString(_configKey, _testName);
	}
}

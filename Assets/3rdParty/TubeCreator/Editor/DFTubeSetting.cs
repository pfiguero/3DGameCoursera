// DFTubeSetting.cs Copyright (C) 2015 dabyon
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;

[System.Serializable]
public class DFTubeSetting
{
	const string KEY_STEP_ANGLE = "TubeCreator_StepAngle";
	const string KEY_STEP_PERCENT = "TubeCreator_StepPercent";
	const string KEY_SAVE_FOLDER = "TubeCreator_SaveFolder";

	const float DEFAULT_STEP_ANGLE = 5.0f;
	const float DEFAULT_STEP_PERCENT = 10.0f;
	public static string DEFAULT_SAVE_FOLDER = "Assets/Resources/Mesh";
	
	public float stepAngle = DEFAULT_STEP_ANGLE;
	public float stepPercent = DEFAULT_STEP_PERCENT;
	public string saveFolder = DEFAULT_SAVE_FOLDER;
	
	public static string ToBase64(string s)
	{
		return System.Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(s));
	}
	
	public static string FromBase64(string s)
	{
		return UTF8Encoding.UTF8.GetString(System.Convert.FromBase64String(s));
	}

	public void Save()
	{
		EditorPrefs.SetFloat(KEY_STEP_ANGLE, this.stepAngle);
		EditorPrefs.SetFloat(KEY_STEP_PERCENT, this.stepPercent);
		EditorPrefs.SetString(KEY_SAVE_FOLDER, ToBase64(this.saveFolder));
	}
	
	public void Load()
	{
		this.stepAngle = EditorPrefs.GetFloat(KEY_STEP_ANGLE);
		this.stepPercent = EditorPrefs.GetFloat(KEY_STEP_PERCENT);
		string s = EditorPrefs.GetString(KEY_SAVE_FOLDER);
		if (s.Length > 0) {
			this.saveFolder = FromBase64(s);
		} else {
			this.saveFolder = DEFAULT_SAVE_FOLDER;
		}
	}
}
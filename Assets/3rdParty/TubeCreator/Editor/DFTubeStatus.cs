// DFTubeStatus.cs Copyright (C) 2015 dabyon
using UnityEngine;
using System.Collections;

[System.Serializable]
public class DFTubeStatus
{
	public int shape = 0;
	public int shapeBak = -1;
	public bool isPreview = true;
	public bool isPlaying = false;
	public bool isEditStart = true;
	
	public bool foldoutDivision = true;
	public bool foldoutSize = true;
	public bool foldoutSurface = true;
	public bool foldoutCurve = true;
	public bool foldoutBend = true;
	public bool foldoutShift = true;
	public bool foldoutSetting = true;
	
	public string[] shapeNames;
}

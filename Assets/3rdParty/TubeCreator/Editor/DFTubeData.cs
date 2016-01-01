// DFTubeData.cs Copyright (C) 2015 dabyon
using UnityEngine;
using System.Collections;

[System.Serializable]
public class DFTubeData {
	const int DEFAULT_MAKE_SURFACE = 63;
	const int DEFAULT_DIVISION_Y = 20;
	const int DEFAULT_DIVISION_R = 20;
	const float DEFAULT_SIZE_Y = 2.0f;
	const float DEFAULT_SIZE_R = 0.5f;
	const float DEFAULT_THICKNESS = 30.0f;
	const float DEFAULT_ANGLE = 360;
	const bool DEFAULT_HAVE_TOP = true;
	const bool DEFAULT_HAVE_BOTTOM = true;
	const bool DEFAULT_IS_REVERSE = false;
	const bool DEFAULT_IS_MIRROR = false;
	const float DEFAULT_BEND_DEGREE = 0;
	const float DEFAULT_BEND_IRECTION = 0;
	const float DEFAULT_SHIFT = 0.0f;
	const float DEFAULT_SHIFT_DIRECTION = 0;
	
	public string name = "";
	public int makeSurface = 63;
	public int divisionY = DEFAULT_DIVISION_Y;
	public int divisionR = DEFAULT_DIVISION_R;
	public float sizeY = DEFAULT_SIZE_Y;
	public float sizeR = DEFAULT_SIZE_R;
	public float thickness = DEFAULT_THICKNESS;
	public float angle = DEFAULT_ANGLE;
	public AnimationCurve curveField = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
	public bool isReverse = DEFAULT_IS_REVERSE;
	public bool isMirror = DEFAULT_IS_MIRROR;
	public float bendDegree = DEFAULT_BEND_DEGREE;
	public float bendDirection = DEFAULT_BEND_IRECTION;
	public float shift = DEFAULT_SHIFT;
	public float shiftDirection = DEFAULT_BEND_IRECTION;
	
	public Vector3 previewPos = new Vector3(0.0f, 0.0f, 0.0f);
	public Quaternion previewRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
	public Vector3 previewScale = new Vector3(1.0f, 1.0f, 1.0f);
	
	public void Reset()
	{
		this.name = "";
		this.makeSurface = DEFAULT_MAKE_SURFACE;
		this.divisionY = DEFAULT_DIVISION_Y;
		this.divisionR = DEFAULT_DIVISION_R;
		this.sizeY = DEFAULT_SIZE_Y;
		this.sizeR = DEFAULT_SIZE_R;
		this.thickness = DEFAULT_THICKNESS;
		this.angle = DEFAULT_ANGLE;
		this.curveField = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
		this.isReverse = DEFAULT_IS_REVERSE;
		this.isMirror = DEFAULT_IS_MIRROR;
		this.bendDegree = DEFAULT_BEND_DEGREE;
		this.bendDirection = DEFAULT_BEND_IRECTION;
		this.shift = DEFAULT_SHIFT;
		this.shiftDirection = DEFAULT_SHIFT_DIRECTION;
	}
	
	public DFTubeData Get()
	{
		DFTubeData newObj = new DFTubeData();
		newObj.name = this.name;
		newObj.makeSurface = this.makeSurface;
		newObj.divisionY = this.divisionY;
		newObj.divisionR = this.divisionR;
		newObj.sizeY = this.sizeY;
		newObj.sizeR = this.sizeR;
		newObj.thickness = this.thickness;
		newObj.angle = this.angle;
		newObj.curveField = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
		newObj.curveField.keys = (Keyframe[])this.curveField.keys.Clone();
		newObj.curveField.preWrapMode = this.curveField.preWrapMode;
		newObj.curveField.postWrapMode = this.curveField.postWrapMode;
		newObj.isReverse = this.isReverse;
		newObj.isMirror = this.isMirror;
		newObj.bendDegree = this.bendDegree;
		newObj.bendDirection = this.bendDirection;
		newObj.shift = this.shift;
		newObj.shiftDirection = this.shiftDirection;
		
		return newObj;
	}
}

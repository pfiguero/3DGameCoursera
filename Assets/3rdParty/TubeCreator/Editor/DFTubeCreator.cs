// DFTubeCreator.cs Copyright (C) 2015 dabyon
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DFTubeCreator : EditorWindow
{
	public DFTubeSetting tubeSetting = new DFTubeSetting();
	public DFTubeData tubeData = new DFTubeData();
	public DFTubeDataList tubeDataList = new DFTubeDataList();
	public DFTubeStatus tubeCreatorStatus = new DFTubeStatus();
	public static DFTubeCreator tubeCreator;

	const string OBJECT_NAME = "Tube";
	const string MESH_NAME = "Tube";
	const string DEFAULT_SHADER = "Standard";
	const float MIN_SIZE = 0.001f;
	const float MIN_STEP_ANGLE = 0.001f;
	const float MIN_STEP_PERCENT = 0.001f;
	const int MAX_OBJ_COUNT = 99999;
	const string INDENT = "    ";
	
	public float thicknessRate = 0.0f;
	public float shiftRate = 0.0f;
	public float bendRad = 0.0f;
	public float bendDirectionRad = 0.0f;
	public float sinBendDirection = 0.0f;
	public float cosBendDirection = 0.0f;
	public float sinShiftDirection = 0.0f;
	public float cosShiftDirection = 0.0f;
	public float[] cosAngle;
	public float[] sinAngle;
	public bool is360 = true;
	
	public GameObject previewObj;
	public Material previewMaterial;
	public Material defaultMaterial;
	public List<int> arTriangle = new List<int>();
	public List<int> arTriangleN = new List<int>();
	public float meshCenterY = 0.0f;
	
	[MenuItem("GameObject/Create Other/TubeCreator")]
	static void Open()
	{
		tubeCreator = EditorWindow.GetWindow<DFTubeCreator>("TubeCreator");
	}

	void OnEnable()
	{
		LoadSetting();
		GetDefaultMaterial();
		SetPresetShapeData();
		MakeShapeNameList();
	}

	void OnDisable()
	{
		tubeSetting.Save();
		DestroyPreviewObj();
	}

	void LoadSetting()
	{
		tubeSetting.Load();
	}

	void GetDefaultMaterial()
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
		defaultMaterial = obj.GetComponent<MeshRenderer>().sharedMaterial;
		Editor.DestroyImmediate(obj);
		if (defaultMaterial == null) {
			defaultMaterial = new Material(Shader.Find(DEFAULT_SHADER));
		}
	}
	
	void SetPresetShapeData()
	{
		tubeDataList.tubeDatas.Clear();
		DFTubeData tubeData;

		tubeData = new DFTubeData();
		tubeData.name = "Tube";
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Half Tube";
		tubeData.angle = 180.0f;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Cylinder";
		tubeData.thickness = 100.0f;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Cone";
		tubeData.thickness = 100.0f;
		tubeData.curveField = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Rugby Ball";
		tubeData.thickness = 100.0f;
		tubeData.curveField = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
		Keyframe key = tubeData.curveField[0];
		key.inTangent = 2.0f;
		key.outTangent = 2.0f;
		tubeData.curveField.MoveKey(0, key);
		tubeData.isReverse = true;
		tubeData.isMirror = true;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Bowl";
		tubeData.sizeY = 0.0f;
		tubeData.thickness = 10.0f;
		tubeData.angle = 180.0f;
		tubeData.bendDegree = 180.0f;
		tubeData.bendDirection = 90.0f;
		tubeData.shiftDirection = 180.0f;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Donut";
		tubeData.sizeY = 4.0f;
		tubeData.sizeR = 0.2f;
		tubeData.thickness = 100.0f;
		tubeData.bendDegree = 360;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Screw";
		tubeData.divisionY = 40;
		tubeData.sizeY = 10.0f;
		tubeData.sizeR = 0.2f;
		tubeData.thickness = 100.0f;
		tubeData.bendDegree = 360.0f * 3;
		tubeData.shift = 1000.0f;
		tubeDataList.tubeDatas.Add(tubeData);

		tubeData = new DFTubeData();
		tubeData.name = "Pyramid";
		tubeData.divisionY = 40;
		tubeData.divisionR = 4;
		tubeData.sizeY = 1.0f;
		tubeData.sizeR = 1.0f;
		tubeData.thickness = 100.0f;
		tubeData.curveField = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		tubeDataList.tubeDatas.Add(tubeData);
	}

	void MakeShapeNameList()
	{
		tubeCreatorStatus.shapeNames = new string[tubeDataList.tubeDatas.Count];
		for (int i = 0; i < tubeDataList.tubeDatas.Count; i++) {
			tubeCreatorStatus.shapeNames[i] = tubeDataList.tubeDatas[i].name;
		}
	}

	void OnGUI()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode) {
			if (!tubeCreatorStatus.isPlaying) {
				tubeCreatorStatus.isPlaying = true;
				DestroyPreviewObj();
				tubeCreatorStatus.isPreview = false;
				tubeCreatorStatus.isEditStart = false;
			}
		} else if (tubeCreatorStatus.isPlaying) {
			tubeCreatorStatus.isPlaying = false;
			if (!EditorApplication.isPlaying) {
				tubeCreatorStatus.isPlaying = false;
			}
			tubeCreatorStatus.isPreview = true;
			tubeCreatorStatus.isEditStart = true;
		}

		EditorGUILayout.Space();

		EditorGUI.BeginDisabledGroup(tubeCreatorStatus.isPlaying == true);

		if (tubeCreatorStatus.shapeNames == null) {
			SetPresetShapeData();
			MakeShapeNameList();
		}
		tubeCreatorStatus.shape = EditorGUILayout.Popup("Preset Shape", tubeCreatorStatus.shape, tubeCreatorStatus.shapeNames);

		bool shapeChanged = (tubeCreatorStatus.shape != tubeCreatorStatus.shapeBak);

		tubeData.makeSurface = EditorGUILayout.MaskField("Surface", tubeData.makeSurface, new string[]{"Outside", "Inside", "Side End 1", "Side End 2", "Top End", "Bottom End"});

		if ((!EditorApplication.isPlaying) && shapeChanged) {
			tubeData.Reset();
			DestroyPreviewObj();

			if (tubeDataList.tubeDatas.Count > tubeCreatorStatus.shape) {
				tubeData = tubeDataList.tubeDatas[tubeCreatorStatus.shape].Get();
			}
		}

		tubeCreatorStatus.foldoutDivision = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutDivision, "Division");
		if (tubeCreatorStatus.foldoutDivision) {
			tubeData.divisionY = EditorGUILayout.IntSlider(INDENT + "Height", tubeData.divisionY, 1, 40);

			tubeData.divisionR = EditorGUILayout.IntSlider(INDENT + "Side", tubeData.divisionR, 1, 40);
		}

		tubeCreatorStatus.foldoutSize = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutSize, "Basic Size");
		if (tubeCreatorStatus.foldoutSize) {
			tubeData.sizeY = EditorGUILayout.FloatField(INDENT + "Height", tubeData.sizeY);
			if (tubeData.sizeY < MIN_SIZE) {
				tubeData.sizeY = MIN_SIZE;
			}

			tubeData.sizeR = EditorGUILayout.FloatField(INDENT + "Radius", tubeData.sizeR);
			if (tubeData.sizeR < MIN_SIZE) {
				tubeData.sizeR = MIN_SIZE;
			}

			float thicknessT = EditorGUILayout.Slider(INDENT + "Thickness (%)", tubeData.thickness, 0.0f, 100.0f);
			thicknessT = StepLength(thicknessT);
			tubeData.thickness = thicknessT;
			thicknessRate = thicknessT / 100.0f;

			float angleT = EditorGUILayout.Slider(INDENT + "Angle", tubeData.angle, 0.0f, 360.0f);
			angleT = stepAngle(angleT);
			tubeData.angle = angleT;
			is360 = (tubeData.angle == 360.0f)?true:false;
		}

		tubeCreatorStatus.foldoutCurve = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutCurve, "Side Shape");
		if (tubeCreatorStatus.foldoutCurve) {
			tubeData.curveField = EditorGUILayout.CurveField(INDENT + "Curve", tubeData.curveField, Color.green, new Rect(0.0f, 0.0f, 1.0f, 1.0f));

			tubeData.isReverse = EditorGUILayout.Toggle(INDENT + "Reverse", tubeData.isReverse);

			tubeData.isMirror = EditorGUILayout.Toggle(INDENT + "Mirror", tubeData.isMirror);
		}

		tubeCreatorStatus.foldoutBend = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutBend, "Bend");
		if (tubeCreatorStatus.foldoutBend) {
			float bendDegreeT = EditorGUILayout.Slider(INDENT + "Angle", tubeData.bendDegree, 0.0f, 360.0f * 3);
			bendDegreeT = stepAngle(bendDegreeT);
			tubeData.bendDegree = bendDegreeT;
			bendRad = tubeData.bendDegree * Mathf.Deg2Rad;

			float bendDirectionT = EditorGUILayout.Slider(INDENT + "Direction", tubeData.bendDirection, 0.0f, 360.0f);
			bendDirectionT = stepAngle(bendDirectionT);
			tubeData.bendDirection = bendDirectionT;
			bendDirectionRad = Mathf.Deg2Rad * tubeData.bendDirection;
			sinBendDirection = Mathf.Sin(bendDirectionRad);
			cosBendDirection = Mathf.Cos(bendDirectionRad);
		}

		tubeCreatorStatus.foldoutShift = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutShift, "Shift");
		if (tubeCreatorStatus.foldoutShift) {
			float shiftT = EditorGUILayout.Slider(INDENT + "Magnitude (%)", tubeData.shift, 0.0f, 5000.0f);
			shiftT = StepLength(shiftT);
			tubeData.shift = shiftT;
			shiftRate = shiftT / 100.0f;

			float shiftDirectionT = EditorGUILayout.Slider(INDENT + "Direction", tubeData.shiftDirection, 0.0f, 360.0f);
			shiftDirectionT = stepAngle(shiftDirectionT);
			tubeData.shiftDirection = shiftDirectionT;
			float shiftDirectionRad = Mathf.Deg2Rad * tubeData.shiftDirection;
			sinShiftDirection = Mathf.Sin(shiftDirectionRad);
			cosShiftDirection = Mathf.Cos(shiftDirectionRad);
		}

		tubeCreatorStatus.foldoutSetting = EditorGUILayout.Foldout(tubeCreatorStatus.foldoutSetting, "Setting");
		if (tubeCreatorStatus.foldoutSetting) {
			tubeSetting.stepAngle = EditorGUILayout.FloatField(INDENT + "Step Angle", tubeSetting.stepAngle);
			if (tubeSetting.stepAngle < MIN_STEP_ANGLE) {
				tubeSetting.stepAngle = MIN_STEP_ANGLE;
			}
			if (tubeSetting.stepAngle > 360.0f) {
				tubeSetting.stepAngle = 360.0f;
			}

			tubeSetting.stepPercent = EditorGUILayout.FloatField(INDENT + "Step %", tubeSetting.stepPercent);
			if (tubeSetting.stepPercent < MIN_STEP_PERCENT) {
				tubeSetting.stepPercent = MIN_STEP_PERCENT;
			}

			EditorGUILayout.BeginHorizontal();
			{
				tubeSetting.saveFolder = EditorGUILayout.TextField(INDENT + "Save Folder", tubeSetting.saveFolder);
				tubeSetting.saveFolder = FormatSaveFolder(tubeSetting.saveFolder);
				if (GUILayout.Button("Browse", GUILayout.Width(60))) {
					string folder = tubeSetting.saveFolder;
					if (!System.IO.Directory.Exists(folder)) {
						folder = DFTubeSetting.DEFAULT_SAVE_FOLDER;
						if (!System.IO.Directory.Exists(folder)) {
							folder = "";
						}
					}
					string dir = "";
					string name = "";
					if (folder.Length > 0) {
						dir = Path.GetDirectoryName(folder);
						name = Path.GetFileName(folder);
					}
					folder = EditorUtility.SaveFolderPanel("Save Mesh", dir, name);
					if (folder.Length > 0) {
						tubeSetting.saveFolder = FormatSaveFolder(folder);
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Select", GUILayout.Height(25))) {
			CreatePreview(true);
			SceneView.lastActiveSceneView.FrameSelected();
		}

		if (!EditorApplication.isPlaying) {
			if (GUI.changed || tubeCreatorStatus.isEditStart) {
				tubeCreatorStatus.isEditStart = false;
				CreatePreview(true);
			}
		}

		if (GUILayout.Button("Create", GUILayout.Height(25))) {
			if (CheckAndCreateFolder(tubeSetting.saveFolder)) {
				CreatePreview(true);
				string path = MakeAssetName(previewObj, tubeSetting.saveFolder);
				if (path.Length > 0) {
					Mesh mesh = previewObj.GetComponent<MeshFilter>().sharedMesh;
					mesh.name = previewObj.name;
					AssetDatabase.CreateAsset(mesh, path);
					AssetDatabase.SaveAssets();
					CreatePreview(false);
				}
			}
		}
		EditorGUI.EndDisabledGroup();
		
		tubeCreatorStatus.shapeBak = tubeCreatorStatus.shape;
	}
	
	float stepAngle(float value)
	{
		float v = Mathf.Round(value / tubeSetting.stepAngle) * (tubeSetting.stepAngle);
		return (Mathf.Round(v * 1000.0f) / 1000.0f);
	}
	
	float StepLength(float value)
	{
		float v = Mathf.Round(value / tubeSetting.stepPercent) * (tubeSetting.stepPercent);
		return (Mathf.Round(v * 1000.0f) / 1000.0f);
	}

	void CreatePreview(bool isDestroy)
	{
		if (isDestroy) {
			DestroyPreviewObj();
		}
		if (tubeCreatorStatus.isPreview) {
			if (!tubeCreatorStatus.isPlaying) {
				Mesh mesh = CreateMesh();
				
				if (tubeCreatorStatus.shape != tubeCreatorStatus.shapeBak) {
					AdjustPreviewPos();
				}
				
				previewObj = CreateGameObject(mesh);
				SetPreviewObjParams(previewObj);

				if (previewObj != null) {
					Selection.activeObject = previewObj;
				}
			}
		}
	}

	void AdjustPreviewPos()
	{
		if (SceneView.lastActiveSceneView != null) {
			Camera sceneCam = SceneView.lastActiveSceneView.camera;
			tubeData.previewPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f - meshCenterY * 0.2f, 4.0f));
		}
	}
	
	void SetPreviewObjParams(GameObject obj)
	{
		if (obj != null) {
			obj.transform.localPosition = tubeData.previewPos;
			obj.transform.localRotation = tubeData.previewRot;
			obj.transform.localScale = tubeData.previewScale;

			MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
			if (renderer == null) {
				renderer = obj.AddComponent<MeshRenderer>();
			}
			if (previewMaterial != null) {
				renderer.sharedMaterial = previewMaterial;
			} else {
				if (defaultMaterial == null) {
					GetDefaultMaterial();
				}
				renderer.sharedMaterial = defaultMaterial;
			}
		}
	}

	void BackupPreview()
	{
		tubeData.previewPos = previewObj.transform.localPosition;
		tubeData.previewRot = previewObj.transform.localRotation;
		tubeData.previewScale = previewObj.transform.localScale;
		
		MeshRenderer renderer = previewObj.GetComponent<MeshRenderer>();
		if (renderer != null) {
			previewMaterial = renderer.sharedMaterial;
		}
	}

	void DestroyPreviewObj()
	{
		if (previewObj != null) {
			BackupPreview();
			Editor.DestroyImmediate(previewObj);
			previewObj = null;
		}
	}
	
	string FormatSaveFolder(string s)
	{
		string newFolder = s;
		if (newFolder.Length > 0) {
			newFolder = newFolder.Trim();
			newFolder = newFolder.Replace("\\", "/");
			string root = Path.GetFullPath(".");
			root = root.Replace("\\", "/");
			if (newFolder.StartsWith(root)) {
				newFolder = newFolder.Substring(root.Length);
			}
			if (newFolder.StartsWith("/")) {
				newFolder = newFolder.Substring(1);
			}
			if (newFolder.EndsWith("/")) {
				newFolder = newFolder.Remove(newFolder.Length - 1);
			}
		}
		return newFolder;
	}

	GameObject CreateGameObject(Mesh mesh)
	{
		GameObject obj = new GameObject();
		obj.name = OBJECT_NAME;
		obj.AddComponent<MeshFilter>();   
		obj.AddComponent<MeshRenderer>();
		obj.GetComponent<MeshFilter>().sharedMesh = mesh;
		obj.GetComponent<MeshFilter>().sharedMesh.name = MESH_NAME;
		obj.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;

		return obj;
	}

	float EvaluateCurve(float pos)
	{
		float value = 0.0f;
		if (tubeData.isMirror) {
			if (!tubeData.isReverse) {
				if (pos <= 0.5f) {
					value = tubeData.curveField.Evaluate(1.0f - pos * 2.0f);
				} else {
					value = tubeData.curveField.Evaluate((pos - 0.5f) * 2.0f);
				}
			} else {
				if (pos <= 0.5f) {
					value = tubeData.curveField.Evaluate(pos * 2.0f);
				} else {
					value = tubeData.curveField.Evaluate(1.0f - (pos - 0.5f) * 2.0f);
				}
			}
		} else {
			if (!tubeData.isReverse) {
				pos = 1.0f - pos;
			}
			value = tubeData.curveField.Evaluate(pos);
		}

		return value;
	}
	
	void AddTriangle(List<int> arTriangle, Vector3[] vertices, int v1, int v2, int v3)
	{
		if (((vertices[v1] - vertices[v2]).magnitude > 0.0f)
			&& ((vertices[v2] - vertices[v3]).magnitude > 0.0f)
			&& ((vertices[v3] - vertices[v1]).magnitude > 0.0f)) {
			arTriangle.Add(v1);
			arTriangle.Add(v2);
			arTriangle.Add(v3);

			arTriangleN.Add(v1);
			arTriangleN.Add(v2);
			arTriangleN.Add(v3);
		}
	}

	void AddTriangleN(List<int> arTriangle, Vector3[] vertices, int v1, int v2, int v3, int vn1, int vn2, int vn3)
	{
		if (((vertices[v1] - vertices[v2]).magnitude > 0.0f)
		    && ((vertices[v2] - vertices[v3]).magnitude > 0.0f)
		    && ((vertices[v3] - vertices[v1]).magnitude > 0.0f)) {
			arTriangle.Add(v1);
			arTriangle.Add(v2);
			arTriangle.Add(v3);
			
			arTriangleN.Add(vn1);
			arTriangleN.Add(vn2);
			arTriangleN.Add(vn3);
		}
	}

	void FlipTriangle(List<int> arTriangle, int start, int end)
	{
		for (int i = start; i < end; i += 3) {
			int tmp = (int)arTriangle[i + 1];
			arTriangle[i + 1] = (int)arTriangle[i + 2];
			arTriangle[i + 2] = tmp;

			tmp = (int)arTriangleN[i + 1];
			arTriangleN[i + 1] = (int)arTriangleN[i + 2];
			arTriangleN[i + 2] = tmp;
		}
	}

	Vector3 GetBendPos(float x, float y, float z)
	{
		Vector3 ret = new Vector3(x, y, z);
		if (tubeData.bendDegree != 0) {
			float cr = tubeData.sizeY / bendRad;
			float dBendRad = y / tubeData.sizeY * bendRad;
			float xRotY = x * cosBendDirection - z * sinBendDirection;
			float zRotY = x * sinBendDirection + z * cosBendDirection;
			float d = cr - xRotY;
			float xBend = cr - d * Mathf.Cos(dBendRad);
			float zBend = zRotY;
			float xInvRotY = xBend * cosBendDirection + zBend * sinBendDirection;
			float zInvRotY = -xBend * sinBendDirection + zBend * cosBendDirection;
			float yBend = d * Mathf.Sin(dBendRad);
			
			ret = new Vector3(xInvRotY, yBend, zInvRotY);
		}
		if (shiftRate != 0.0f) {
			float s = y / tubeData.sizeY * tubeData.sizeR * shiftRate;
			ret.x += s * sinShiftDirection;
			ret.z += s * cosShiftDirection;
		}

		return ret;
	}
	
	Mesh CreateMesh()
	{
		if ((tubeData.sizeY <= 0.0f) || (tubeData.sizeR <= 0.0f) || (tubeData.angle == 0.0f)) {
			return (new Mesh());
		}
		int numR = tubeData.divisionR + 1;
		int numY = tubeData.divisionY + 1;
		float uvDividionR = (float)tubeData.divisionR;
		float dtheta = 2.0f * Mathf.PI / tubeData.divisionR;
		int numVertex = (numR * (numY + 2)) * 2;
		if (!is360) {
			numVertex += numY * 4;
			dtheta *= tubeData.angle / 360.0f;
			uvDividionR = tubeData.divisionR * 360.0f / (float)tubeData.angle;
		}
		cosAngle = new float[numR];
		sinAngle = new float[numR];
		for (int i = 0; i <= tubeData.divisionR; i++) {
			cosAngle[i] = Mathf.Cos((float)i * dtheta);
			sinAngle[i] = Mathf.Sin((float)i * dtheta);
		}

		bool isRing = false;
		if ((tubeData.bendDegree == 360.0f) && (shiftRate == 0.0f) && (EvaluateCurve(0.0f) == EvaluateCurve(1.0f))) {
			isRing = true;
		}

		Vector3[] vertices = new Vector3[numVertex];
		Vector2[] uv = new Vector2[numVertex];
		arTriangle.Clear();
		arTriangleN.Clear();

		int vtxCnt = 0;
		float maxR = 0.0f;
		int[] startVtxCnts = new int[2];
		for (int k = (((tubeData.makeSurface&1) != 0)?0:1); k <= ((((thicknessRate < 1.0f) && (tubeData.makeSurface & 2) != 0))?1:0); k++) {
			startVtxCnts[k] = vtxCnt;
			int startTriCnt = arTriangle.Count - 1;
			for (int i = 0; i <= tubeData.divisionY; i++) {
				float yRate = (float)i / tubeData.divisionY;
				float r = tubeData.sizeR * EvaluateCurve(yRate);
				float y = (float)i * tubeData.sizeY / tubeData.divisionY;
				if (r < 0.0f) {
					r = -r;
				}
				if (k > 0) {
					r -= thicknessRate * tubeData.sizeR;
					if (r < 0.0f) {
						r = 0.0f;
					}
				}
				if (r > maxR) {
					maxR = r;
				}
				for (int j = 0; j <= tubeData.divisionR; j++) {
					float u = j / uvDividionR;
					float v = yRate;
					float x = r * cosAngle[j];
					float z = r * sinAngle[j];
					vertices[vtxCnt] = GetBendPos(x, y, z);
					uv[vtxCnt++].Set(u, v);
				}
			}

			int ul = startVtxCnts[k];
			int ur = startVtxCnts[k] + 1;
			int dl = startVtxCnts[k] + numR;
			int dr = startVtxCnts[k] + numR + 1;
			for (int i = 0; i < tubeData.divisionY; i++) {
				for (int j = 0; j < tubeData.divisionR - 1; j++) {
					if (isRing && i == tubeData.divisionY - 1) {
						AddTriangleN(arTriangle, vertices, ul, dl, dr, ul, startVtxCnts[k] + j, startVtxCnts[k] + 1 + j);
						AddTriangleN(arTriangle, vertices, ur, ul, dr, ur, ul, startVtxCnts[k] + 1 + j);
						dl++;
						dr++;
						ur++;
						ul++;
					} else {					
						AddTriangle(arTriangle, vertices, ul, dl++, dr);
						AddTriangle(arTriangle, vertices, ur++, ul++, dr++);
					}
				}

				if (is360) {
					AddTriangleN(arTriangle, vertices, ul, dl, dr, ul, dl, startVtxCnts[k] + (i + 1) * numR);
					AddTriangleN(arTriangle, vertices, ur, ul, dr, startVtxCnts[k] + i * numR, ul,  startVtxCnts[k] + (i + 1) * numR);
				} else {
					AddTriangle(arTriangle, vertices, ul, dl, dr);
					AddTriangle(arTriangle, vertices, ur, ul, dr);
				}
				ul += 2;
				ur += 2;
				dl += 2;
				dr += 2;
			}

			if (k > 0) {
				FlipTriangle(arTriangle, startTriCnt, arTriangle.Count - 1);
			}
		}

		if (!isRing) {
			for (int i = (((tubeData.makeSurface&32) != 0)?0:1); i <= (((tubeData.makeSurface&16) != 0)?1:0); i++) {
				int startVtxCnt = vtxCnt;
				float yIn = tubeData.sizeY * i;
				float rOut = tubeData.sizeR * EvaluateCurve((float)i);
				if (rOut < 0.0f) {
					rOut = -rOut;
				}
					float rIn = rOut - thicknessRate * tubeData.sizeR;
				if (rIn < 0.0f) {
					rIn = 0.0f;
				}
				for (int j = 0; j <= tubeData.divisionR; j++) {
					float uOut = cosAngle[j] / 2.0f + 0.5f;
					float vOut = sinAngle[j] / 2.0f + 0.5f;
					float xOut = rOut * cosAngle[j];
					float zOut = rOut * sinAngle[j];
					vertices[vtxCnt] = GetBendPos(xOut, tubeData.sizeY * i, zOut);
					uv[vtxCnt++].Set(uOut, vOut);

					float uIn = 0.0f;
					float vIn = 0.0f;
					if (rOut > 0.0f) {
						uIn = rIn / rOut * cosAngle[j] / 2.0f + 0.5f;
						vIn = rIn / rOut * sinAngle[j] / 2.0f + 0.5f;
					}
					float xIn = rIn * cosAngle[j];
					float zIn = rIn * sinAngle[j];
					vertices[vtxCnt] = GetBendPos(xIn, yIn, zIn);
					uv[vtxCnt++].Set(uIn, vIn);
				}
				int startTriCnt = arTriangle.Count - 1;
				for (int j = 0; j < tubeData.divisionR - ((is360)?1:0); j++) {
					AddTriangle(arTriangle, vertices, startVtxCnt + j * 2, startVtxCnt + j * 2 + 2, startVtxCnt + j * 2 + 1);
					AddTriangle(arTriangle, vertices, startVtxCnt + j * 2 + 1, startVtxCnt + j * 2 + 2, startVtxCnt + j * 2 + 3);
				}
				if (is360) {
					AddTriangle(arTriangle, vertices, startVtxCnt + (tubeData.divisionR - 1) * 2, startVtxCnt, startVtxCnt + (tubeData.divisionR - 1) * 2 + 1);
					AddTriangle(arTriangle, vertices, startVtxCnt + (tubeData.divisionR - 1) * 2 + 1, startVtxCnt, startVtxCnt + 1);
				}
				if (i != 0) {
					 FlipTriangle(arTriangle, startTriCnt, arTriangle.Count - 1);
				}
			}
		}

		if ((!is360) && (maxR > 0.0f)) {
			int startVtxCnt = vtxCnt;
			for (int i = 0; i <= tubeData.divisionY; i++) {
				float y = (float)i * tubeData.sizeY / (tubeData.divisionY);
				float yRate = (float)i / (tubeData.divisionY);
				float rOut = tubeData.sizeR * EvaluateCurve(yRate);
				float rIn = rOut - thicknessRate * tubeData.sizeR;
				if (rIn < 0.0f) {
					rIn = 0.0f;
				}
				for (int j = 0; j <= tubeData.divisionR; j += tubeData.divisionR) {
					float xOut = rOut * cosAngle[j];
					float zOut = rOut * sinAngle[j];
					float uOut = 0.5f;
					if (rOut > 0.0f) {
						if (j == 0) {
							uOut += rOut / maxR / 2.0f;
						} else {
							uOut += -rOut / maxR / 2.0f;
						}
					}
					vertices[vtxCnt] = GetBendPos(xOut, y, zOut);
					uv[vtxCnt++].Set(uOut, yRate);

					float xIn = rIn * cosAngle[j];
					float zIn = rIn * sinAngle[j];
					float uIn = 0.5f;
					if (rOut > 0.0f) {
						if (j == 0) {
							uIn += rIn / maxR / 2.0f;
						} else {
							uIn += -rIn / maxR / 2.0f;
						}
					}
					vertices[vtxCnt] = GetBendPos(xIn, y, zIn);
					uv[vtxCnt++].Set(uIn, yRate);
				}

				if (i > 0) {
					if ((i == tubeData.divisionY) && isRing) {
						if ((tubeData.makeSurface&8) != 0) {
							AddTriangleN(arTriangle, vertices, vtxCnt - 1, vtxCnt - 5, vtxCnt - 2, startVtxCnt + 3, vtxCnt - 6, startVtxCnt + 2);
							AddTriangleN(arTriangle, vertices, vtxCnt - 2, vtxCnt - 5, vtxCnt - 6, startVtxCnt + 3, vtxCnt - 5, vtxCnt - 6);
						}
						if ((tubeData.makeSurface&4) != 0) {
							AddTriangleN(arTriangle, vertices, vtxCnt - 3, vtxCnt - 4, vtxCnt - 7, startVtxCnt + 1, startVtxCnt, vtxCnt - 8);
							AddTriangleN(arTriangle, vertices, vtxCnt - 4, vtxCnt - 8, vtxCnt - 7, startVtxCnt + 1, vtxCnt - 8, vtxCnt - 7);
						}
					} else {
						if ((tubeData.makeSurface&8) != 0) {
							AddTriangle(arTriangle, vertices, vtxCnt - 1, vtxCnt - 5, vtxCnt - 2);
							AddTriangle(arTriangle, vertices, vtxCnt - 2, vtxCnt - 5, vtxCnt - 6);
						}
						if ((tubeData.makeSurface&4) != 0) {
							AddTriangle(arTriangle, vertices, vtxCnt - 3, vtxCnt - 4, vtxCnt - 7);
							AddTriangle(arTriangle, vertices, vtxCnt - 4, vtxCnt - 8, vtxCnt - 7);
						}
					}
				}
			}
		}

		Mesh meshN = new Mesh();
		meshN.vertices = vertices;
		meshN.triangles = (int[])arTriangleN.ToArray();
		meshN.RecalculateNormals();
		Vector3[] normals = meshN.normals;
		for (int i = 0; i < arTriangle.Count; i++) {
			if ((int)arTriangle[i] != (int)arTriangleN[i]) {
				if (normals[(int)arTriangleN[i]].magnitude > 0.0f) {
					normals[(int)arTriangle[i]] = normals[(int)arTriangleN[i]];
				}
			}
		}
		meshN = null;

		List<Vector3> arVertice = new List<Vector3>();
		List<Vector2> arUv = new List<Vector2>();
		List<Vector3> arNormal = new List<Vector3>();
		int[] triangles = (int[])arTriangle.ToArray();
		int foundCnt = 0;
		for (int i = 0; i < numVertex; i++) {
			bool found = false;
			for (int j = 0; j < arTriangle.Count; j++) {
				if (i == (int)arTriangle[j]) {
					found = true;
					break;
				}
			}
			if (found) {
				arVertice.Add(vertices[i]);
				arUv.Add(uv[i]);
				arNormal.Add(normals[i]);
				foundCnt++;
			} else {
				for (int j = 0; j < triangles.Length; j++) {
					if (triangles[j] > foundCnt) {
						triangles[j]--;
					}
				}
			}
		}

		Mesh mesh = new Mesh();

		if (arVertice.Count > 0) {
			mesh.vertices = (Vector3[])arVertice.ToArray();
			mesh.uv = (Vector2[])arUv.ToArray(); 
			mesh.triangles = triangles;
			mesh.normals = (Vector3[])arNormal.ToArray();
			mesh.RecalculateBounds();
		 	 mesh.Optimize();
		
			float minY = mesh.vertices[0].y;
			float maxY = minY;
			for (int i = 1; i < mesh.vertices.Length; i++) {
				if (((Vector3)arVertice[i]).y > maxY) {
					maxY = mesh.vertices[i].y;
				} else if (((Vector3)arVertice[i]).y < minY) {
					minY = mesh.vertices[i].y;
				}
			}
			meshCenterY = (minY + maxY) / 2.0f;
		}

		return mesh;
	}

	static bool CheckAndCreateFolder(string folder)
	{
		bool ret = true;
		string[] folders = folder.Split(new char[]{'/','\\'}, System.StringSplitOptions.RemoveEmptyEntries);
		string parent = folders[0];
		for (int i = 1; i < folders.Length; i++) {
			string newFolder = parent + "/" + folders[i];
			if (System.IO.Directory.Exists(newFolder) != true) {
				try {
					UnityEditor.AssetDatabase.CreateFolder(parent, folders[i]);
				} catch (Exception e) {
					Debug.Log(e.ToString());
					ret = false;
					break;
				}
			}
			parent += "/" + folders[i];
		}
		if (ret) {
			AssetDatabase.Refresh();
		} else {
			if (System.IO.Directory.Exists(folder) != true) {
				EditorUtility.DisplayDialog(
					"TubeCreator",
					"Folder not found:\n\n" + folder,
					"OK"
					);
			}
		}

		return ret;
	}

	static string MakeAssetName(GameObject obj, string folder)
	{
		string ret = "";

		int cntFile = 1;
		string body = "";
		for (; cntFile <= MAX_OBJ_COUNT; cntFile++) {
			body = obj.name + "_" + cntFile.ToString("D" + MAX_OBJ_COUNT.ToString().Length);
			string path = folder + "/" + body + ".asset";
			if (System.IO.File.Exists(path) != true) {
				ret = path;
				obj.name = body;
				break;
			}
		}

		if (ret.Length == 0) {
			string msg = "Faild to save file.";
			if (cntFile > MAX_OBJ_COUNT) {
				msg += "\nDelete unnecessary mesh asset files.";
			}
			EditorUtility.DisplayDialog(
				"TubeCreator",
				msg,
				"OK"
			);
		}
		return ret;
	}
}

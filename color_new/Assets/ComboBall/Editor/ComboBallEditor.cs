#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ComboBall))]
public class ComboBallEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		ComboBall ball = target as ComboBall;
		if(GUILayout.Button("Apply Color Change", GUILayout.Width(200)))
		{
			Material matNormal, matDark;
			switch(ball.ballColor)
			{
			case ComboBall.BallColors.BLUE:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/BLUE.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/BLUE_D.mat", typeof(Material)) as Material;
				break;
			case ComboBall.BallColors.GREEN:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/GREEN.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/GREEN_D.mat", typeof(Material)) as Material;
				break;
			case ComboBall.BallColors.PINK:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/PINK.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/PINK_D.mat", typeof(Material)) as Material;
				break;
			case ComboBall.BallColors.PURPLE:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/PURPLE.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/PURPLE_D.mat", typeof(Material)) as Material;
				break;
			case ComboBall.BallColors.RED:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/RED.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/RED_D.mat", typeof(Material)) as Material;
				break;
			case ComboBall.BallColors.YELLOW:
				matNormal = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/YELLOW.mat", typeof(Material)) as Material;
				matDark = Resources.LoadAssetAtPath("Assets/Materials/ComboBalls/YELLOW_D.mat", typeof(Material)) as Material;
				break;
			default:
				Debug.LogWarning("Color Not Registered in ComboBallEditor, please check");
				matNormal = ball.renderer.material;
				matDark = ball.cancelMat;
				break;
			}
			ball.renderer.material = matNormal;
			ball.cancelMat = matDark;
		}
	}
}

#endif
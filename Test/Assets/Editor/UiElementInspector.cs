using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UiElement),true)]
public class UiElementInspector : Editor {
	public override void OnInspectorGUI(){
		if(GUILayout.Button("Refresh")){
			UiElement ele = target as UiElement;
			if(ele.UiResFile != null){
				Debug.Log(ele.UiResFile.name);
			}
		}

		base.OnInspectorGUI();
	} 

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(UiElement),true)]
public class UiElementInspector : Editor {
	public override void OnInspectorGUI(){
		if(GUILayout.Button("Refresh")){
			UiElement ele = target as UiElement;
			// if(ele.UiResFile != null){
				// string url = System.IO.Path.Combine(Application.dataPath,"Resources/UiFile");
				// GetAllFiles(url,"-");
				
				UIXMLLoader load = new UIXMLLoader();
				load.Load(ele.UiResFile);
			// }
		}

		base.OnInspectorGUI();
	}


	public void GetAllFiles(string url,string flag){
		DirectoryInfo info = new DirectoryInfo(url);
		var list = info.GetDirectories();
		for (int i = 0; i < list.Length; i++)
		{
			Debug.Log(flag + list[i].Name);
			GetAllFiles(list[i].FullName,flag+"-");
		}

		var fileList = info.GetFiles("*.xml");
		for (int i = 0; i < fileList.Length; i++)
		{
			Debug.Log(flag+fileList[i].Name);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiElement : MonoBehaviour {
	public TextAsset UiResFile;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class UIElement{
	public Vector2 pos;
	public Vector2 size;
	public string uiType;
	public string resource;
	public bool isComponent;
	public List<UIElement> childs;
}

public class UIElementImage:UIElement{
	public UIElementImage(){
		uiType = "Image";
	}
}

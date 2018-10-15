# -*- coding:utf-8 -*-
import os
import sys
import FairyGUIHelper
import FileHelper

PROJECT_DIR = os.path.abspath(os.path.dirname(__file__))


fileOpera = FileHelper.FileHelper()
filelist = fileOpera.getAllFiles("/Users/lee/Documents/project-other/proj.fishingUI/assets",1)
for fn in filelist:
	print fn
	pathInfo = fileOpera.getFilePathInfo(fn)
	if pathInfo[1] != ".DS_Store" and pathInfo[1] != "package":
		obj = FairyGUIHelper.FairyGUIHelper(fn,"CSharp.template")
		obj.generateTemplate()
		obj.write2file(pathInfo[0],pathInfo[1],".cs")	


fileOpera.copyFiles(PROJECT_DIR+"/output","/Users/lee/Documents/project-unity/TestGUI/Assets/Scripts/GenerateClass")





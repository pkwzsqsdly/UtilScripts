# -*- coding:utf-8 -*-

import xml.dom.minidom
import re
import os
PROJECT_DIR = os.path.abspath(os.path.dirname(__file__))

class FairyGUIHelper(object):
	def __init__(self,filePath,template):
		
		self._dom = xml.dom.minidom.parse(filePath)
		self._root = self._dom.documentElement.getElementsByTagName('displayList')[0]
		self._keyWords = {"image":"GImage",
							"component":"GComponent",
							"text":"GTextField",
							"group":"GGroup",
							"richtext":"GRichTextField",
							"graph":"GGraph",
							"list":"GList",
							"loader":"GLoader",
							"button":"GButton"}
		self._allElements = {}

		self.varText = ""
		self.bindText = ""
		self.funcText = ""

		# file = open(template, 'r')
		# self._template = file.read()
		# file.close()
		self.readTemplate(template)

		self.resolveAll()

	def resolveAll(self):
		for keyWord in self._keyWords.keys():
			eleList = self._root.getElementsByTagName(keyWord)
			self._allElements.setdefault(keyWord,eleList)


	def generateTemplate(self):
		self.varText = ""
		self.bindText = ""
		self.funcText = ""
		for keyWord in self._keyWords.keys():
			mlist = self._allElements[keyWord]
			for i in range(len(mlist)):
				theName = mlist[i].getAttribute("name")
				if self.checkVarDefault(theName) == False:
					typeKeyword = self._keyWords[keyWord]
					text = self.generateVar(typeKeyword,theName)
					self.varText += text + "\n"
					text = self.generateBinder(typeKeyword,theName)
					self.bindText += text + "\n"
					if self.checkButton(theName):
						self.bindText += self.generateEvent("onClick",theName) + "\n"

	def checkVarDefault(self,val):
		# 将正则表达式编译成Pattern对象
		pattern = re.compile(r'n\d+$')
		# 使用Pattern匹配文本，获得匹配结果，无法匹配时将返回None
		match = pattern.match(val)
		if match:
			# 使用Match获得分组信息
			print match.group()
			return True
		return False

	def checkButton(self,name):
		if name.startswith("Btn") == True:
			return True
		elif name.startswith("btn") == True:
			return True
		return False

	def generateVar(self,keyw,nam):
		lineWords = "\tpublic {keyWord} m_{name};"
		return lineWords.format(keyWord=keyw,name=nam)		
	
	def generateBinder(self,keyw,nam):
		lineWords = "\t\tm_{name} = ({keyWord})m_UI.GetChild(\"{name}\");"
		return lineWords.format(keyWord=keyw,name=nam)

	def generateEvent(self,funcType,nam):
		lineWords = "\t\tm_{name}.{func} = on{name}Click;"
		return lineWords.format(name=nam,func=funcType)

	def generateFunction(self,pre,name,param):
		lineWords = "private void {name}({param})\{"

	def getSplitType(self,pkg,name):
		strs = name.split('_')
		if len(strs) > 1:
			if strs[0] == "W":
				return self.getWindowCreateStr(pkg,name)
			else:
				return self.getDefaultCreateStr(pkg,name)
		return None

	def getWindowCreateStr(self,packageNam,comNam):
		windowStr = '''\t\tWindow win = new Window();
\t\twin.contentPane =UIPackage.CreateObject(\"{packageName}\", \"{componentName}\").asCom;
\t\twin.Show();
\t\treturn win.contentPane;'''
		return windowStr.format(packageName=packageNam,componentName=comNam)

	def getDefaultCreateStr(self,packageNam,comNam):
		defaultStr = '''\t\tvar ui = UIPackage.CreateObject(\"{packageName}\", \"{componentName}\").asCom;
\t\tui.SetSize(GRoot.inst.width, GRoot.inst.height);
\t\tui.AddRelation(GRoot.inst, RelationType.Size);
\t\treturn ui;'''
		return defaultStr.format(packageName=packageNam,componentName=comNam)

	def write2file(self,path,name,aft):
		output = self._template.replace("{componentName}","GComponent")
		packageName = path.split('/')[-1]
		
		creator = self.getSplitType(packageName,name)
		if creator == None:
			return

		output = output.replace("{packageName}",packageName)

		className = name.split('_')[-1]

		output = output.replace("{className}",className)
		output = output.replace("{createScripts}",creator)
		output = output.replace("{variable}",self.varText)
		output = output.replace("{content}",self.bindText)
		# output = self._template.format(className=name, componentName="GComponent",variable=self.varText,packageName=name,uiName=name,content=self.bindText)
		# self.varText+self.bindText + self.funcText
		path = os.path.join(PROJECT_DIR,"output/UI"+ className + aft)
		file = open(path,'w')
		file.write(output)
		file.close()

	def readTemplate(self,name):
		file = open(name,'r')
		self._template = file.read()
		file.close()

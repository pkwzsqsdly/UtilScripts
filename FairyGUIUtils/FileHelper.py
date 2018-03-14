# -*- coding:utf-8 -*-

import os
import shutil 

class FileHelper(object):
	def __init__(self):

		pass
	def getAllFiles(self,_findPath,_childNum = -1):
		FileNames = os.listdir(_findPath)
		fileList = []
		childDeep = -1
		if _childNum != None:
			childDeep = _childNum
		
		for fn in FileNames:
			fullfilename = os.path.join(_findPath, fn)
			if os.path.isfile(fullfilename):
				fileList.append(fullfilename)
			else:
				if childDeep == -1 or childDeep > 0:
					fileList.extend(self.getAllFiles(fullfilename,childDeep - 1))
		return fileList

	def getFilePathInfo(self,filename):
		(filepath,tempfilename) = os.path.split(filename)
		(shotname,extension) = os.path.splitext(tempfilename)
		return filepath,shotname,extension

	def copyFileByList(self,fileList,outPath):
		for fn in fileList:
			print "------>" + fn
			fileInfo = self.getFilePathInfo(fn)
			outFile = os.path.join(outPath,fileInfo[1]+fileInfo[2])
			shutil.copyfile(fn,outFile)

	def copyFiles(self,filePath,outPath,isfindChild = True):
		deepIdx = 1
		if isfindChild == True:
			deepIdx = -1

		resFileList = self.getAllFiles(filePath,deepIdx)
		
		self.copyFileByList(resFileList,outPath)

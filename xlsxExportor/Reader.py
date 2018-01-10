# coding=utf8
#!user/bin/python
import xlrd
import os
import XlsxData

class Reader(object):
    def __init__(self,filePath):
        self._sheetList = []
        self.loadXlsx(filePath)
    
    def loadXlsx(self,filePath):
        print(filePath)
        try:
            xlsx = xlrd.open_workbook(filePath)
            sheets = xlsx.sheets()
            for i in range(len(sheets)):
                self.newSheetData(sheets[i])
        except IOError:
            print("can not open xlsx " +filePath)
            
    def newSheetData(self,sheet):
        data = XlsxData.SheetData(sheet,5,1)
        data.resolve()
        self._sheetList.append(data)

    
    def getSheetDatas(self):
        return self._sheetList

    
class TemplateReadClass(object):
    def __init__(self,fileName,cells):
        self._fileName = fileName
        self._cells = cells
        self._varCalls = {}
        self._readCalls = {}
        self._tmStr = '''
using System.Collections.Generic;
using System.IO;
using LGame.Utils;
using LGame.Data;

public class {0} : LCfgData {{
{1}
\tpublic override void ResolveData(LByteArray bArr) {{
{2}
\t}}

}}
'''
        self._tmVar = ""
        self._tmRead = ""

        self.registCall(self._varCalls,"int",self.tmVarInt)
        self.registCall(self._varCalls,"float",self.tmVarFloat)
        self.registCall(self._varCalls,"string",self.tmVarString)

        self.registCall(self._readCalls,"int",self.tmReadInt)
        self.registCall(self._readCalls,"float",self.tmReadFloat)
        self.registCall(self._readCalls,"string",self.tmReadString)
    
    def executeCall(self,dic,key,val):
        call = dic.get(key,None)
        if call == None:
            return
        call(val)
        
    def registCall(self,dic,key,val):
        dic.setdefault(key,val)
    
    def tmVarString(self,key):
        tmp = "\tpublic string {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmVarInt(self,key):
        tmp = "\tpublic int {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmVarFloat(self,key):
        tmp = "\tpublic float {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmVarArrayInt(self,key):
        tmp = "\tpublic int[] {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmVarArrayString(self,key):
        tmp = "\tpublic string[] {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmVarArrayFloat(self,key):
        tmp = "\tpublic float[] {0} {{ get; private set; }}\n"
        self._tmVar = self._tmVar + tmp.format(key)

    def tmReadString(self,key):
        tmp = "\t\t{0} = bArr.ReadString();\n"
        self._tmRead = self._tmRead + tmp.format(key)

    def tmReadInt(self,key):
        tmp = "\t\t{0} = bArr.ReadInt();\n"
        self._tmRead = self._tmRead + tmp.format(key)

    def tmReadFloat(self,key):
        tmp = "\t\t{0} = bArr.ReadFloat();\n"
        self._tmRead = self._tmRead + tmp.format(key)

    def tmReadArrayInt(self,key):
        tmp = '''\t\tint lsLen{0} = bArr.ReadInt();\n
        \t\t{0} = new int[lsLen{0}];\n
        \t\tfor(int i = 0;i < lsLen{0}; i++)\n
        \t\t{0}[i] = bArr.ReadInt();\n'''
        self._tmRead = self._tmRead + tmp.format(key)

    def tmReadArrayString(self,key):
        tmp = '''\t\tstring lsLen{0} = bArr.ReadString();\n
        \t\t{0} = new string[lsLen{0}];\n
        \t\tfor(int i = 0;i < lsLen{0}; i++)\n
        \t\t{0}[i] = bArr.ReadString();\n'''
        self._tmRead = self._tmRead + tmp.format(key)

    def tmReadArrayFloat(self,key):
        tmp = '''\t\tfloat lsLen{0} = bArr.ReadFloat();\n
        \t\t{0} = new float[lsLen{0}];\n
        \t\tfor(int i = 0;i < lsLen{0}; i++)\n
        \t\t{0}[i] = bArr.ReadFloat();\n'''
        self._tmRead = self._tmRead + tmp.format(key)

    def combinClassText(self):
        return self._tmStr.format(self._fileName,self._tmVar,self._tmRead)

        
    def toClassFile(self,filePath):
        # def getTrans(self):
        # def getKey(self):
        # def setValue(self,val):
        # def getStrType(self):
        # def getValue(self):
        for i in range(len(self._cells)):
            cell = self._cells[i]
            self.executeCall(self._varCalls,cell.getStrType(),cell.getKey())
            self.executeCall(self._readCalls,cell.getStrType(),cell.getKey())
            
        fileName = self._fileName + ".cs"
        fullName = os.path.join(filePath,fileName)
        outfile = None
        try:
            outfile = open(fullName,"w")
        except IOError:
            print("can not open xlsx " +fullName)
        
        outfile.write(self.combinClassText())
        outfile.close()
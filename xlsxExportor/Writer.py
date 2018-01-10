# coding=utf8
#!user/bin/python

import os
import struct

class Writer(object):
    def __init__(self,sheetData,outputPath):
        self._sheetData = sheetData
        self._outputPath = outputPath
        self._writeCalls = {}
        self.registWriteCall("int",self.writeInt)
        self.registWriteCall("float",self.writeFloat)
        self.registWriteCall("string",self.writeString)

    def writeInt(self,val):
        a = struct.pack(">i",val)
        print("write int")
        self._file.write(a)
    
    def writeString(self,val):
        bys = val.encode("utf-8")
        strlen = len(bys)
        print("write string " + str(strlen))
        a = struct.pack(">i" + str(strlen)+"s",strlen,bys)
        self._file.write(a)
        
    def writeFloat(self,val):
        a = struct.pack(">f",val)
        self._file.write(a)
    
    def write(self,key,val):
        call = self._writeCalls.get(key,None)
        if call == None:
            return
        call(val)
        
    def registWriteCall(self,key,val):
        self._writeCalls.setdefault(key,val)

    def startWrite(self):
        self.initFile()
        cells = self._sheetData.getAllCells()
        row = len(cells)
        col = len(cells[0])
        for i in range(row):
            for j in range(col):
                self.writeCell(cells[i][j])

        self.writeFileDone()

    def writeCell(self,cell):
        stype = cell.getStrType()
        val = cell.getValue()
        self.write(stype,val)


    def initFile(self):
        fileName = self._sheetData.getName() + ".bin"
        fullName = os.path.join(self._outputPath,fileName)
        try:
            self._file = open(fullName,"wb")
        except IOError:
            print("can not open xlsx " +fullName)
   
    def writeFileDone(self):
        self._file.close()










      
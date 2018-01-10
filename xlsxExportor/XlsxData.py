# coding=utf8
#!user/bin/python
import xlrd
import sys


class CellData(object):
    def __init__(self,desc,key,stype,trans):
        #print("init cell")
        self._desc = desc
        self._value = None
        self._key = key
        self._stype = stype
        self._trans = trans
        
    def getTrans(self):
        return self._trans

    def getKey(self):
        return self._key
        
    def setValue(self,val):
        self._value = val

    def getStrType(self):
        return self._stype

    def getValue(self):
        return self._value
        
    def toString(self):
        tmp = self._desc + "("+ self._stype+") " + str(self._value)
        return tmp
    

class SheetData(object):
    def __init__(self,sheetData,startrow,startcol):
        print("sheet named:" + sheetData.name)
        self._sheet = sheetData
        self._cellList = []
        self._configs = []
        self._startRow = startrow
        self._startCol = startcol
        self._typeCall = {}
        
        self.registTypeCall("string",self.getString)
        self.registTypeCall("int",self.getInt)
        self.registTypeCall("float",self.getFloat)

        reload(sys)
        sys.setdefaultencoding('utf-8')
        
    def getName(self):
        return self._sheet.name
        
    def registTypeCall(self,stype,call):
        print("regist type" + stype)
        if self._typeCall.get(stype,None) != None:
            print("the key is exist="+stype)
            return
        self._typeCall.setdefault(stype,call)
        
    def resolve(self):
        nrow = self._sheet.nrows
        ncol = self._sheet.ncols
        for k in range(self._startRow):
            val = self._sheet.cell(k,0).value
            print (val)
            self._configs.append(str(val))
        
        for i in range(self._startRow,nrow):
            for j in range(self._startCol,ncol):
                cell = self.newCell(i,j)
                if cell == None:
                    continue
                #print(cell.getStrType())
                typecall = self._typeCall.get(cell.getStrType(),None)
                if typecall == None:
                    print("can not found type:"+ cell.getStrType())
                
                typecall(i,j,cell)
                self.addCell(i,j,cell)
    
    def getAllCells(self):
        return self._cellList        
        
    def dumpAll(self):
        row = len(self._cellList)
        col = len(self._cellList[0])
        for i in range(row):
            for j in range(col):
                print(self._cellList[i][j].toString())
        
        print(str(row) + "~" + str(col))
        
    def getString(self,i,j,cell):
        cellval = self._sheet.cell(i,j).value
        cell.setValue(str(cellval))
            
        #print("string = "+ str(cell.getValue()))
     
    def getConfig(self,idx):
        return self._configs[idx]
       
    def addCell(self,i,j,cell):
        row = i - self._startRow
        col = j - self._startCol
        if row >= len(self._cellList):
            tmp = []
            tmp.append(cell)
            self._cellList.append(tmp)
        else:
            self._cellList[row].append(cell)
      
    def getInt(self,i,j,cell):
        cellval = self._sheet.cell(i,j).value
        fval = float(cellval)
        cell.setValue(int(fval))
        #print("int = "+ str(cell.getValue()))

    def getFloat(self,i,j,cell):
        cellval = self._sheet.cell(i,j).value
        cell.setValue(float(cellval))
        #print("float = "+ str(cell.getValue()))
        
    def newCell(self,i,j):
        
        iscellexp= self._sheet.cell(3,j).value
        #print("new cell " + iscellexp)
        if iscellexp != 'yes':
            return None
            
        celldesc = self._sheet.cell(0,j).value
        cellkey = self._sheet.cell(1,j).value
        celltype= self._sheet.cell(2,j).value
        celltrans = self._sheet.cell(4,j).value
        
        celldata = CellData(celldesc,cellkey,celltype,celltrans)
        return celldata

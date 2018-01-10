# coding=utf8
#!user/bin/python
import Reader
import Writer
import os

xlsxFile = "元素表.xlsx"
loader = Reader.Reader(xlsxFile)
for v in loader.getSheetDatas():
    print(v.getConfig(0))
    print(v.getConfig(1))
    print(v.getConfig(2))
    tmp = Reader.TemplateReadClass(v.getName(),v.getAllCells()[0])
    tmp.toClassFile("/Users/lee/Downloads/xlsxExportor")
    print (tmp.combinClassText())
    # writer = Writer.Writer(v,"/Users/lee/Downloads/xlsxExportor")
    # writer.startWrite()








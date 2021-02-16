# coding=utf8
#!user/bin/python
import sys
# reload(sys)
# sys.setdefaultencoding('utf-8')
eles = ["地","水","火","风","光","暗"]
vals = []
	#[1,4,12,36,101,324]
dfts = {}

def check(el,va,i,c): 
    if i < c: 
        for tmp in range(len(eles)):
            newel = eles[tmp]
            newva = vals[tmp]
            fullel = el + newel
            fullva = va + newva 
            check(fullel,fullva,i+1,c)
    else:
        if(dfts.get(va,0) == 0):
            dfts.setdefault(va,1)
            print(el + " " + str(va))

def startByCount(count):
    startval = 1
    nowval = 1
    for i in range(len(eles)):
        if i > 1:
            nowval = nowval * count
        if i > 0:
            startval = nowval * (count + 1)       
        vals.append(startval)

    print(vals)   
    
    check("",0,0,count)  


startByCount(3)
print(len(dfts))
    

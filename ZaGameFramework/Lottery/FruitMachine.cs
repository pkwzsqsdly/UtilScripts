using System.Collections;
using System.Collections.Generic;
public class FruitData
{
	//结果数量
	public int resultCount;
	//预先旋转次数 一般等待获取服务器结果到达前一直转 但如果网速很快 需要先转几圈给个效果
	public int preRotate;
	//初始速度
	public float speedOnStart;
	//加速度
	public float speedAddup;
	public FruitData(int count,float speedStart)
	{
		this.resultCount = count;
		this.speedOnStart = speedStart;
		this.preRotate = 1;
		this.speedAddup = 0.2f;
	}
}
public class FruitEventArgs
{
	public int index;
}
public class FruitMachine {
	private System.Action<int> updateListenner;
	private System.Action<int> doneListenner;
	private int nCount;
	private int nCurrent;
	private int nEndIndex;
	private int nPreRound;
	private float fSpeed;
	private float fSpeedAdd;
	private float fSpeedConst;
	private bool isRunning;

	public FruitMachine(int max){
		nCount = max;
		fSpeedAdd = 0.25f;
		fSpeedConst = 1f;
		isRunning = false;
	}
	public FruitMachine BindListenner(System.Action<int> update,System.Action<int> done)
	{
		updateListenner = update;
		doneListenner = done;
		return this;
	}

	public bool IsRun(){
		return isRunning;
	}

	public void Start(int end){
		isRunning = true;	
		nEndIndex = end;
		nCurrent++;
		fSpeed = 1f;
		nPreRound = 1;
	}
	private void setInfo(){
		fSpeed = fSpeedConst;
		
		if(nPreRound == 0) fSpeedConst += fSpeedAdd;
		else fSpeedConst -= fSpeedAdd;

		if(fSpeedConst < 0.1f) fSpeedConst = 0.1f;
        else if(fSpeedConst > 1) fSpeedConst = 1;
	}

	public void onUpdate(){
		if(!isRunning) return;
		if(fSpeed > 0){
			fSpeed -= fSpeedAdd;
		}else{
			setInfo();
			toNext();
		}
	}

	private void toNext(){
		nCurrent++;
		if(nCurrent >= nCount){
			nCurrent = 0;
			if(nPreRound > 0){
				nPreRound--;
			}
		}
		if(updateListenner != null){
			updateListenner(nCurrent);
		}
		if(nPreRound == 0 && nEndIndex == nCurrent && fSpeed == 1){
			isRunning = false;
			if(doneListenner != null){
				doneListenner(nEndIndex);
			}
		}
	}
}



public class StoryRandomData {
	public int chance;
	public int state;
	public int weight;
	public int distance;
	public int forks;

	public int rangeMin;
	public int rangeMax;


	public StoryRandomData(int chance,int state)
	{
		this.chance = chance;
		this.state = state;
		weight = 0;
		var ran = new System.Random();
		distance = ran.Next(5,10);
		forks = ran.Next(0,3) + 1;
	}
}
using System.Collections.Generic;
namespace LG.TableUtil
{
	public interface IKeywordTypeParse{}

	public class TypeParsing
	{
		protected Dictionary<string,IKeywordTypeParse> kwTypeDic;
		public TypeParsing()
		{
			kwTypeDic = new Dictionary<string, IKeywordTypeParse>();
		}
		public void BindType(string key,IKeywordTypeParse kwParse)
		{
			if (!kwTypeDic.ContainsKey(key))
			{
				kwTypeDic.Add(key,kwParse);
			}
			else throw new System.Exception($"{key} is exsit!");
		}
		public IKeywordTypeParse SeekParse(string key)
		{
			return kwTypeDic.ContainsKey(key) ? kwTypeDic[key] : null;
		}
		public T SeekParse<T>(string key) where T : class,IKeywordTypeParse
		{
			return kwTypeDic.ContainsKey(key) ? (kwTypeDic[key] as T) : default(T);
		}
	}
}
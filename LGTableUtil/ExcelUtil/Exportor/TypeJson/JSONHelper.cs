using LitJson;
using System.Collections;

namespace LG.TableUtil.Json
{
    public class JSONHelper {
        public static string GetString(JsonData data,string key,string defalut = null){
            if(IsKeyExsit(data,key)){
                return (string)data[key];
            }
            return defalut;
        }

        public static int GetInt(JsonData data,string key,int defalut = 0){
            if(IsKeyExsit(data,key)){
                return (int)data[key];
            }
            return defalut;
        }

        public static string[] GetStringArray(JsonData data,string key){
            JsonData list = GetData(data,key);
            if(list == null || list.Count == 0) return null;
            string [] strArray = new string [list.Count];
            for(int i = 0; i < list.Count; i ++){
                strArray[i] = list[i].ToString();
            }
            return strArray;
        }

        public static double GetDouble(JsonData data,string key,float defalut = 0){
            if(IsKeyExsit(data,key)){
                return (double)data[key];
            }
            return defalut;
        }

        public static float GetFloat(JsonData data,string key,float defalut = 0){
            if(IsKeyExsit(data,key)){
                var valData = data[key];
                float val = 0;
                if(valData.IsInt){
                    val = (int)valData;
                }else if(valData.IsDouble){
                    double db = (double)valData;
                    val = (float)db;
                }else{
                    return defalut;
                }
                return val;
            }
            return defalut;
        }

        public static bool GetBool(JsonData data,string key,bool defalut = false){
            if(IsKeyExsit(data,key)){
                return (bool)data[key];
            }
            return defalut;
        }

        public static JsonData GetData(JsonData data,string key){
            if(IsKeyExsit(data,key)){
                return (JsonData)data[key];
            }
            return null;
        }
        
        public static bool IsKeyExsit(JsonData data,string key){
            IDictionary dic = data as IDictionary;
            if(dic == null) return false;
            return dic.Contains(key);
        }
        
    }
}
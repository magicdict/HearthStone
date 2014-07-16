using System;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Engine.Utility
{
    /// <summary>
    /// C# 关联工具方法
    /// </summary>
    public static class CSharpUtility
    {
        /// <summary>
        /// 转JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 文字列[最后带有分隔符]转字典
        /// </summary>
        /// <param name="DicString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToStringDictionary(this String DicString)
        {
            Dictionary<string, string> RtnDic = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(DicString))
            {
                String[] DicItem = DicString.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray()).Split(CardUtility.strSplitArrayMark.ToCharArray());
                foreach (var item in DicItem)
                {
                    var t = item.Split(CardUtility.strSplitKeyValueMark.ToCharArray());
                    RtnDic.Add(t[0], t[1]);
                }
            }
            return RtnDic;
        }
        /// <summary>
        /// 字典转文字列[最后带有分隔符]
        /// </summary>
        /// <param name="Dic"></param>
        /// <returns></returns>
        public static String FromStringDictionary(this Dictionary<String, String> Dic)
        {
            String Rtn = String.Empty;
            List<String> t = new List<string>();
            if (t.Count != 0)
            {
                foreach (var item in Dic)
                {
                    t.Add(item.Key + CardUtility.strSplitKeyValueMark + item.Value);
                }
                Rtn = t.ToListString() + CardUtility.strSplitArrayMark;
            }
            return Rtn;
        }
        /// <summary>
        /// 字符串列表转字符串
        /// </summary>
        /// <param name="StringList"></param>
        /// <returns></returns>
        public static String ToListString(this List<String> StringList)
        {
            if (StringList.Count == 0) return String.Empty;
            var rtn = String.Empty;
            for (int i = 0; i < StringList.Count; i++)
            {
                rtn += StringList[i] + CardUtility.strSplitArrayMark;
            }
            return rtn.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
        }
        #region"扩展方法"
        /// <summary>
        /// 深拷贝对象副本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            T copy = default(T);
            try
            {
                formatter.Serialize(ms, obj);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                copy = (T)formatter.Deserialize(ms);
            }
            catch (Exception ex)
            {
                throw new Exception("深拷贝对象实例出错", ex);
            }
            finally
            {
                ms.Close();
            }
            return copy;
        }
        #endregion


        /// <summary>
        /// 随机打算数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] RandomSort<T>(T[] array, int Seed)
        {
            int len = array.Length;
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
            T[] ret = new T[len];
            Random rand = Seed == 0 ? new Random() : new Random(DateTime.Now.Millisecond + Seed);
            int i = 0;
            while (list.Count < len)
            {
                int iter = rand.Next(0, len);
                if (!list.Contains(iter))
                {
                    list.Add(iter);
                    ret[i] = array[iter];
                    i++;
                }
            }
            return ret;
        }
        /// <summary>
        /// 获得字符枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static T GetEnum<T>(String strEnum, T Default)
        {
            if (String.IsNullOrEmpty(strEnum)) return Default;
            try
            {
                T EnumValue = (T)Enum.Parse(typeof(T), strEnum);
                return EnumValue;
            }
            catch (Exception)
            {
                return Default;
            }
        }
        /// <summary>
        /// 数字字符转数字，错误则返回默认值
        /// </summary>
        /// <param name="StringInt"></param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt(String StringInt, int DefaultValue = 0)
        {
            if (String.IsNullOrEmpty(StringInt)) return DefaultValue;
            return int.Parse(StringInt);
        }
    }
}

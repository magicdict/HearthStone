using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Engine.Utility.CardUtility;
using Engine.Client;
using Engine.Control;

namespace Engine.Utility
{
    /// <summary>
    /// C# 关联工具方法
    /// </summary>
    public static class CSharpUtility
    {
        #region"扩展方法"
        /// <summary>
        /// 转JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 文字列[最后带有分隔符]转字典
        /// </summary>
        /// <param name="DicString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToStringDictionary(this string DicString)
        {
            Dictionary<string, string> RtnDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(DicString))
            {
                string[] DicItem = DicString.TrimEnd(strSplitArrayMark.ToCharArray()).Split(strSplitArrayMark.ToCharArray());
                foreach (var item in DicItem)
                {
                    var t = item.Split(strSplitKeyValueMark.ToCharArray());
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
        public static string FromStringDictionary(this Dictionary<string, string> Dic)
        {
            string Rtn = string.Empty;
            List<string> t = new List<string>();
            if (t.Count != 0)
            {
                foreach (var item in Dic)
                {
                    t.Add(item.Key + strSplitKeyValueMark + item.Value);
                }
                Rtn = t.ToListString() + strSplitArrayMark;
            }
            return Rtn;
        }
        /// <summary>
        /// 字符串列表转字符串
        /// </summary>
        /// <param name="StringList"></param>
        /// <returns></returns>
        public static string ToListString(this List<string> StringList)
        {
            if (StringList.Count == 0) return string.Empty;
            var rtn = string.Empty;
            for (int i = 0; i < StringList.Count; i++)
            {
                rtn += StringList[i] + strSplitArrayMark;
            }
            return rtn.TrimEnd(strSplitArrayMark.ToCharArray());
        }
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

        #region"委托"
        /// <summary>
        /// 抉择
        /// </summary>
        /// <param name="First">第一效果</param>
        /// <param name="Second">第二效果</param>
        /// <returns>是否为第一效果</returns>
        public delegate 抉择枚举 delegatePickEffect(string First, string Second);
        /// <summary>
        /// 抽牌委托
        /// </summary>
        /// <param name="IsFirst">先后手区分</param>
        /// <param name="Ability">法术定义</param>
        public delegate List<string> delegateDrawCard(bool IsFirst, int DrawCount);
        /// <summary>
        /// 获得位置
        /// </summary>
        /// <returns></returns>
        public delegate 指定位置结构体 deleteGetTargetPosition(位置选择用参数结构体 选择参数);
        /// <summary>
        /// 随从进场位置
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public delegate int delegateGetMinionPos(BattleFieldInfo battleInfo);
        /// <summary>
        /// 中断处理
        /// </summary>
        /// <param name="interrupt"></param>
        public delegate void delegateInterrupt(FullServerManager.Interrupt interrupt);
        /// <summary>
        /// 日志接口
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="isHost"></param>
        /// <param name="Info"></param>
        public delegate void delegateLogDetail(int GameId, bool isHost, string Info);
        /// <summary>
        /// 日志接口
        /// </summary>
        /// <param name="Info"></param>
        public delegate void delegateLog(string Info);
        #endregion

        #region"辅助方法"
        /// <summary>
        /// 随机打算数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] RandomSort<T>(T[] array, int Seed)
        {
            int len = array.Length;
            List<int> list = new List<int>();
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
        public static T GetEnum<T>(string strEnum, T Default)
        {
            if (string.IsNullOrEmpty(strEnum)) return Default;
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
        public static int GetInt(string StringInt, int DefaultValue = 0)
        {
            if (string.IsNullOrEmpty(StringInt)) return DefaultValue;
            return int.Parse(StringInt);
        }
        #endregion
    }
}

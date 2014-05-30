using System;

namespace Card.Client
{
    public class Crystal
    {
        /// <summary>
        /// 最大水晶数
        /// </summary>
        public const int MaxPoint = 10;
        /// <summary>
        /// 当前满值水晶数
        /// </summary>
        public int CurrentFullPoint = 0;
        /// <summary>
        /// 当前可用水晶数
        /// </summary>
        public int CurrentRemainPoint = 0;
        /// <summary>
        /// 新的回合
        /// </summary>
        public void NewTurn()
        {
            AddFullPoint();
            CurrentRemainPoint = CurrentFullPoint;
        }
        /// <summary>
        /// 增加一个空水晶
        /// </summary>
        /// <remarks>野性成长</remarks>
        public void AddFullPoint()
        {
            if (CurrentFullPoint < MaxPoint) CurrentFullPoint++;
        }
        /// <summary>
        /// 增加多个空水晶
        /// </summary>
        /// <param name="Point"></param>
        public void AddFullPoint(int Point)
        {
            CurrentFullPoint += Point;
            if (CurrentFullPoint > MaxPoint) CurrentFullPoint = MaxPoint;
        }
        /// <summary>
        /// 增加一个可用水晶
        /// </summary>
        public void AddCurrentPoint()
        {
            if (CurrentRemainPoint < MaxPoint) CurrentRemainPoint++;
        }
        /// <summary>
        /// 增加多个可用水晶
        /// </summary>
        /// <param name="Point"></param>
        public void AddCurrentPoint(int Point)
        {
            CurrentRemainPoint += Point;
            if (CurrentRemainPoint > MaxPoint) CurrentRemainPoint = MaxPoint;

        }
        /// <summary>
        /// 减少一个可用水晶
        /// </summary>
        public void ReduceCurrentPoint()
        {
            if (CurrentRemainPoint > 0) CurrentRemainPoint--;
        }
        /// <summary>
        /// 减少多个可用水晶
        /// </summary>
        public void ReduceCurrentPoint(int Point)
        {
            CurrentRemainPoint -= Point;
            if (CurrentRemainPoint < 0) CurrentRemainPoint = 0;

        }
        /// <summary>
        /// 减少一个空水晶
        /// </summary>
        public void ReduceFullPoint()
        {
            if (CurrentFullPoint > 0) CurrentFullPoint--;
        }
        /// <summary>
        /// 减少多个空水晶
        /// </summary>
        /// <param name="Point"></param>
        public void ReduceFullPoint(int Point)
        {
            CurrentFullPoint -= Point;
            if (CurrentFullPoint < 0) CurrentFullPoint = 0;
        }
    }
}

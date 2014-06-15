using Engine.Effect;
using System;
namespace Engine.Card
{
    /// <summary>
    /// 武器卡牌
    /// </summary>
    [Serializable]
    public class WeaponCard : CardBasicInfo
    {
        /// <summary>
        /// 攻击力（实际）
        /// </summary>
        public int 攻击力 = -1;
        /// <summary>
        /// 耐久（）
        /// </summary>
        public int 耐久度 = -1;
        /// <summary>
        /// 武器的附加效果
        /// 真银圣剑：每当你的英雄进攻时，为其恢复2点生命值。
        /// </summary>
        public EffectDefine AdditionEffect = new EffectDefine();
        /// <summary>
        /// 设置初始状态
        /// </summary>
        public new void Init()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new string GetInfo()
        {
            return Name + "：" + 攻击力 + "/" + 耐久度;
        }
    }
}

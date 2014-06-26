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
        /// 攻击力
        /// </summary>
        public int 攻击力 = 0;
        /// <summary>
        /// 耐久（）
        /// </summary>
        public int 耐久度 = 0;
        /// <summary>
        /// 武器的附加效果
        /// 真银圣剑：每当你的英雄进攻时，为其恢复2点生命值。
        /// </summary>
        public EffectDefine AdditionEffect = new EffectDefine();
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public string 状态
        {
            get{
                return 名称 + "：" + 攻击力 + "/" + 耐久度;
            }
        }
    }
}

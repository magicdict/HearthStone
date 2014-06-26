using Engine.Effect;
using System;
namespace Engine.Card
{
    /// <summary>
    /// 武器卡牌[WeaponCard]
    /// </summary>
    [Serializable]
    public class WeaponCard : CardBasicInfo
    {
        /// <summary>
        /// 攻击力[Attack Point]
        /// </summary>
        public int 攻击力 = 0;
        /// <summary>
        /// 耐久[Durability Point]
        /// </summary>
        public int 耐久度 = 0;
        /// <summary>
        /// 武器的附加效果[Addition Effect]
        /// </summary>
        ///<remarks>
        /// 真银圣剑：每当你的英雄进攻时，为其恢复2点生命值。
        ///</remarks>
        public EffectDefine 武器的附加效果 = new EffectDefine();
        /// <summary>
        /// 状态[Status]
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

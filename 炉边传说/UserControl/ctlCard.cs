using Engine.Effect;
using Engine.Utility;
using System;
using System.Windows.Forms;

namespace 炉边传说
{
    public partial class ctlCard : UserControl
    {
        /// <summary>
        /// 随从
        /// </summary>
        public Engine.Card.CardBasicInfo CardInfo
        {
            set
            {
                lblName.Text = value.名称;
                lblDescription.Text = value.描述;
                switch (value.CardType)
                {
                    case Engine.Card.CardBasicInfo.CardTypeEnum.随从:
                        lblHealthPoint.Visible = true;
                        lblAttackPoint.Visible = true;
                        var minion = ((Engine.Card.MinionCard)value);
                        lblAttackPoint.Text = minion.TotalAttack().ToString();
                        lblHealthPoint.Text = minion.生命值.ToString();
                        lblStatus.Text = "[状]" + (minion.圣盾特性 ? "圣" : String.Empty) +
                           (minion.嘲讽特性 ? "|嘲" : String.Empty) +
                           (minion.风怒特性 ? "|风" : String.Empty) +
                           (minion.冲锋特性 ? "|冲" : String.Empty) +
                           (minion.冰冻状态 != CardUtility.效果回合枚举.无效果 ? "冻" : String.Empty);
                        CanAttack = minion.能否成为动作对象;
                        break;
                    case Engine.Card.CardBasicInfo.CardTypeEnum.法术:
                        lblHealthPoint.Visible = false;
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Engine.Card.AbilityCard)value).使用成本.ToString();
                        break;
                    case Engine.Card.CardBasicInfo.CardTypeEnum.奥秘:
                        break;
                    case Engine.Card.CardBasicInfo.CardTypeEnum.武器:
                        lblHealthPoint.Visible = true;
                        lblAttackPoint.Visible = true;
                        lblAttackPoint.Text = ((Engine.Card.WeaponCard)value).攻击力.ToString();
                        lblHealthPoint.Text = ((Engine.Card.WeaponCard)value).耐久度.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        public Boolean CanAttack
        {
            set
            {
                btnFight.Visible = value;
            }
        }
        public ctlCard()
        {
            InitializeComponent();
            btnFight.Click += (x, y) => { FightClick(x, y); };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void BtnClick(object sender, EventArgs e);
        /// <summary>
        /// 
        /// </summary>
        public BtnClick FightClick;
    }
}

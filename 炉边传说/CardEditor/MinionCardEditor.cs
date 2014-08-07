using Engine.Card;
using System;
using System.Windows.Forms;

namespace 炉边传说.CardEditor
{
    public partial class MinionCardEditor : Form
    {
        //EditorTarget
        MinionCard minion = new MinionCard();
        bool IsCreateMode = false;
        public MinionCardEditor(string CardSn)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(CardSn))
            {
                IsCreateMode = true;
            }
            else
            {
                IsCreateMode = false;
                minion = (MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(CardSn);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateMinionCard_Load(object sender, EventArgs e)
        {
            //Init Enum
            cmbSpecalEffect.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(MinionCard.特殊效果枚举)))
            {
                cmbSpecalEffect.Items.Add(item.ToString());
            }
            cmbSpecalEffect.SelectedIndex = 0;

            if (!IsCreateMode)
            {
                //Set UI 
                //Common Propery
                ctlMinionBasicInfo.GetBasicInfo(minion);
                //Basic Property
                numLifePoint.Value = minion.生命值上限;
                numAttackPoint.Value = minion.攻击力;
                //attr
                chkTaunt.Checked = minion.嘲讽特性;
                chkCharge.Checked = minion.冲锋特性;
                chkWindfury.Checked = minion.风怒特性;
                chkStealth.Checked = minion.潜行特性;
                chkDivineShield.Checked = minion.圣盾特性;
                //Misc
                cmbSpecalEffect.SelectedIndex = minion.特殊效果.GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Collect UI data to obj
            //Common Propery
            ctlMinionBasicInfo.SetBasicInfo(minion);
            //Basic Property
            minion.生命值上限 = (int)numLifePoint.Value;
            minion.攻击力 = (int)numAttackPoint.Value;
            //attr
            minion.嘲讽特性 = chkTaunt.Checked;
            minion.冲锋特性 = chkCharge.Checked;
            minion.风怒特性 = chkWindfury.Checked;
            minion.潜行特性 = chkStealth.Checked;
            minion.圣盾特性 = chkDivineShield.Checked;
            //Misc
            minion.特殊效果 = (MinionCard.特殊效果枚举)cmbSpecalEffect.SelectedIndex;
        }
    }
}

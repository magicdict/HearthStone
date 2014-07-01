using Engine.Card;
using Engine.Effect;
using Engine.Utility;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace 炉边传说
{
    public partial class frmExport : Form
    {
        public frmExport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportXml_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ExcelPicker.SelectedPathOrFileName)) return;
            if (String.IsNullOrEmpty(XmlFolderPicker.SelectedPathOrFileName)) return;
            Export();
            GC.Collect();
        }
        /// <summary>
        /// 导入
        /// </summary>
        private void Export()
        {
            dynamic excelObj = Interaction.CreateObject("Excel.Application");
            excelObj.Visible = true;
            dynamic workbook;
            workbook = excelObj.Workbooks.Open(ExcelPicker.SelectedPathOrFileName);
            if (chkMinion.Checked) Minion(workbook);
            if (chkAbility.Checked) Ability(workbook);
            if (chkWeapon.Checked) Weapon(workbook);
            if (chkSecret.Checked) Secret(workbook);
            workbook.Close();
            excelObj.Quit();
            excelObj = null;
            MessageBox.Show("导出结束");
        }
        /// <summary>
        /// 奥秘
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Secret(dynamic workbook)
        {
            if (Directory.Exists(XmlFolderPicker.SelectedPathOrFileName + "\\Secret\\"))
            {
                Directory.Delete(XmlFolderPicker.SelectedPathOrFileName + "\\Secret\\", true);
            }
            Directory.CreateDirectory(XmlFolderPicker.SelectedPathOrFileName + "\\Secret\\");
            //奥秘的导入
            dynamic worksheet = workbook.Sheets(4);
            int rowCount = 4;
            while (!String.IsNullOrEmpty(worksheet.Cells(rowCount, 2).Text))
            {
                Engine.Card.SecretCard Secret = new Engine.Card.SecretCard();
                Secret.序列号 = worksheet.Cells(rowCount, 2).Text;
                Secret.名称 = worksheet.Cells(rowCount, 3).Text;
                Secret.描述 = worksheet.Cells(rowCount, 4).Text;
                Secret.职业 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.职业枚举>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.职业枚举.中立);
                Secret.使用成本 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
                Secret.使用成本 = Secret.使用成本;
                Secret.稀有程度 = CSharpUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度枚举>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度枚举.白色);
                Secret.是否启用 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);
                Secret.Condition = CSharpUtility.GetEnum<Engine.Card.SecretCard.SecretCondition>(worksheet.Cells(rowCount, 14).Text, SecretCard.SecretCondition.对方召唤随从);
                Secret.AdditionInfo = worksheet.Cells(rowCount, 15).Text;
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.SecretCard));
                String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Secret\\" + Secret.序列号 + ".xml";
                xml.Serialize(new StreamWriter(XmlFilename), Secret);
                rowCount++;
            }
        }
        /// <summary>
        /// 随从的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Minion(dynamic workbook)
        {
            if (Directory.Exists(XmlFolderPicker.SelectedPathOrFileName + "\\Minion\\"))
            {
                Directory.Delete(XmlFolderPicker.SelectedPathOrFileName + "\\Minion\\", true);
            }
            Directory.CreateDirectory(XmlFolderPicker.SelectedPathOrFileName + "\\Minion\\");
            //随从的导入
            dynamic worksheet = workbook.Sheets(1);
            int rowCount = 4;
            while (!String.IsNullOrEmpty(worksheet.Cells(rowCount, 2).Text))
            {
                Engine.Card.MinionCard Minion = new Engine.Card.MinionCard();
                Minion.序列号 = worksheet.Cells(rowCount, 2).Text;
                Minion.名称 = worksheet.Cells(rowCount, 3).Text;
                Minion.描述 = worksheet.Cells(rowCount, 4).Text;
                Minion.职业 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.职业枚举>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.职业枚举.中立);
                Minion.种族 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.种族枚举>(worksheet.Cells(rowCount, 6).Text, Engine.Utility.CardUtility.种族枚举.无);
                Minion.使用成本 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 7).Text);

                Minion.攻击力 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 8).Text);
                Minion.生命值上限 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 9).Text);
                Minion.稀有程度 = CSharpUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度枚举>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度枚举.白色);
                Minion.是否启用 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);

                Minion.嘲讽特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 14).Text);
                Minion.冲锋特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 15).Text);
                Minion.无法攻击特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 16).Text);
                Minion.风怒特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 17).Text);
                Minion.潜行特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 18).Text);
                Minion.圣盾特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 19).Text);
                Minion.法术免疫特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 20).Text);
                Minion.英雄技能免疫特性 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 21).Text);

                Boolean HasBuff = false;
                for (int i = 22; i < 25; i++)
                {
                    if (!String.IsNullOrEmpty(worksheet.Cells(rowCount, i).Text))
                    {
                        HasBuff = true;
                        break;
                    }
                }
                if (HasBuff)
                {
                    Minion.光环效果.来源 = Minion.名称;
                    Minion.光环效果.范围 = CSharpUtility.GetEnum<Engine.Card.MinionCard.光环范围枚举>(worksheet.Cells(rowCount, 22).Text, Engine.Card.MinionCard.光环范围枚举.随从全体);
                    Minion.光环效果.类型 = CSharpUtility.GetEnum<Engine.Card.MinionCard.光环类型枚举>(worksheet.Cells(rowCount, 23).Text, Engine.Card.MinionCard.光环类型枚举.增加攻防);
                    Minion.光环效果.信息 = worksheet.Cells(rowCount, 24).Text;
                }
                Minion.战吼效果 = worksheet.Cells(rowCount, 25).Text;
                Minion.战吼类型 = CSharpUtility.GetEnum<Engine.Card.MinionCard.战吼类型枚举>(worksheet.Cells(rowCount, 26).Text, Engine.Card.MinionCard.战吼类型枚举.默认);

                Minion.亡语效果 = worksheet.Cells(rowCount, 27).Text;
                Minion.激怒效果 = worksheet.Cells(rowCount, 28).Text;
                Minion.连击效果 = worksheet.Cells(rowCount, 29).Text;
                Minion.回合开始效果 = worksheet.Cells(rowCount, 30).Text;
                Minion.回合结束效果 = worksheet.Cells(rowCount, 31).Text;
                Minion.过载 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 32).Text);
                Minion.自身事件效果.触发效果事件类型 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.事件类型枚举>(worksheet.Cells(rowCount, 33).Text, Engine.Utility.CardUtility.事件类型枚举.无);
                Minion.自身事件效果.效果编号 = worksheet.Cells(rowCount, 34).Text;
                Minion.自身事件效果.触发效果事件方向 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.目标选择方向枚举>(worksheet.Cells(rowCount, 35).Text, Engine.Utility.CardUtility.目标选择方向枚举.本方);
                Minion.自身事件效果.限制信息 = worksheet.Cells(rowCount, 36).Text;
                Minion.特殊效果 = CSharpUtility.GetEnum<Engine.Card.MinionCard.特殊效果枚举>(worksheet.Cells(rowCount, 37).Text, Engine.Card.MinionCard.特殊效果枚举.无效果);

                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.MinionCard));
                String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Minion\\" + Minion.序列号 + ".xml";
                xml.Serialize(new StreamWriter(XmlFilename), Minion);
                rowCount++;
            }
        }
        /// <summary>
        /// 法术的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Ability(dynamic workbook)
        {
            if (Directory.Exists(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\"))
            {
                Directory.Delete(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\", true);
            }
            Directory.CreateDirectory(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\");
            //法术的导入
            dynamic worksheet = workbook.Sheets(2);
            int rowCount = 4;
            Engine.Card.SpellCard Ability;
            while (!String.IsNullOrEmpty(worksheet.Cells(rowCount, 2).Text))
            {
                //当前行肯定是卡牌基本情报
                Ability = new SpellCard();
                Ability.序列号 = worksheet.Cells(rowCount, 2).Text;
                Ability.名称 = worksheet.Cells(rowCount, 3).Text;
                Ability.描述 = worksheet.Cells(rowCount, 4).Text;
                Ability.职业 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.职业枚举>(worksheet.Cells(rowCount, 9).Text, Engine.Utility.CardUtility.职业枚举.中立);
                Ability.使用成本 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 10).Text);
                Ability.过载 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 11).Text);
                Ability.是否启用 = true;
                rowCount++;
                //当前行肯定是选择条件
                Ability.效果选择类型 = CSharpUtility.GetEnum<SpellCard.效果选择类型枚举>(worksheet.Cells(rowCount, 3).Text,
                    Engine.Card.SpellCard.效果选择类型枚举.无需选择);
                Ability.FirstAbilityDefine.描述 = worksheet.Cells(rowCount, 4).Text;
                Ability.SecondAbilityDefine.描述 = worksheet.Cells(rowCount, 5).Text;
                rowCount++;
                GetAbilityDefine(ref Ability.FirstAbilityDefine, worksheet, ref rowCount);
                if (Ability.效果选择类型 != Engine.Card.SpellCard.效果选择类型枚举.无需选择)
                {
                    rowCount++;
                    //当前行是第一效果的标题栏
                    GetAbilityDefine(ref Ability.SecondAbilityDefine, worksheet, ref rowCount);
                }
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.SpellCard));
                String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\" + Ability.序列号 + ".xml";
                xml.Serialize(new StreamWriter(XmlFilename), Ability);
                rowCount++;
            }
        }
        /// <summary>
        /// 获得法术定义
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private static void GetAbilityDefine(ref SpellCard.AbilityDefine Ability, dynamic worksheet, ref int rowCount)
        {
            Ability.MainAbilityDefine = GetEffectDefine(worksheet, ref rowCount);
            //追加效果
            String NextLine = worksheet.Cells(rowCount + 1, 2).Text;
            if (!String.IsNullOrEmpty(NextLine) && NextLine == "追加条件")
            {
                rowCount++;
                //当前行是追加条件
                Ability.AppendEffectCondition = worksheet.Cells(rowCount, 3).Text;
                rowCount++;
                //当前行是第一效果的标题栏
                Ability.AppendAbilityDefine = GetEffectDefine(worksheet, ref rowCount);
            }
        }
        /// <summary>
        /// 获得效果定义
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private static EffectDefine GetEffectDefine(dynamic worksheet, ref int rowCount)
        {
            EffectDefine effect = new EffectDefine();
            //当前行是第一效果的标题栏
            rowCount++;
            //当前行是第一效果的内容栏
            effect.AbliltyPosPicker.EffictTargetSelectMode =
                CSharpUtility.GetEnum<Engine.Utility.CardUtility.目标选择模式枚举>(worksheet.Cells(rowCount, 3).Text, Engine.Utility.CardUtility.目标选择模式枚举.不用选择);
            effect.AbliltyPosPicker.EffectTargetSelectDirect =
                CSharpUtility.GetEnum<Engine.Utility.CardUtility.目标选择方向枚举>(worksheet.Cells(rowCount, 4).Text, Engine.Utility.CardUtility.目标选择方向枚举.双方);
            effect.AbliltyPosPicker.EffectTargetSelectRole =
                CSharpUtility.GetEnum<Engine.Utility.CardUtility.目标选择角色枚举>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.目标选择角色枚举.英雄);
            effect.AbliltyPosPicker.EffectTargetSelectCondition = worksheet.Cells(rowCount, 6).Text;
            effect.EffectCount = CSharpUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
            effect.效果条件 = worksheet.Cells(rowCount, 8).Text;
            effect.TrueAtomicEffect.描述 = worksheet.Cells(rowCount, 9).Text;
            effect.TrueAtomicEffect.AtomicEffectType = CSharpUtility.GetEnum<Engine.Effect.AtomicEffectDefine.AtomicEffectEnum>(worksheet.Cells(rowCount, 10).Text, Engine.Effect.AtomicEffectDefine.AtomicEffectEnum.未定义);

            for (int i = 11; i < 15; i++)
            {
                if (String.IsNullOrEmpty(worksheet.Cells(rowCount, i).Text)) break;
                effect.TrueAtomicEffect.InfoArray.Add((worksheet.Cells(rowCount, i).Text));
            }
            if (effect.效果条件 != CardUtility.strIgnore)
            {
                rowCount++;
                //当前行是第二效果的标题栏
                rowCount++;
                //当前行是第二效果的内容栏
                effect.FalseAtomicEffect.描述 = worksheet.Cells(rowCount, 9).Text;
                effect.FalseAtomicEffect.AtomicEffectType = CSharpUtility.GetEnum<Engine.Effect.AtomicEffectDefine.AtomicEffectEnum>(worksheet.Cells(rowCount, 10).Text, Engine.Effect.AtomicEffectDefine.AtomicEffectEnum.未定义);
                for (int i = 11; i < 15; i++)
                {
                    if (String.IsNullOrEmpty(worksheet.Cells(rowCount, i).Text)) break;
                    effect.FalseAtomicEffect.InfoArray.Add((worksheet.Cells(rowCount, i).Text));
                }
            }
            return effect;
        }

        /// <summary>
        /// 武器的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Weapon(dynamic workbook)
        {
            if (Directory.Exists(XmlFolderPicker.SelectedPathOrFileName + "\\Weapon\\"))
            {
                Directory.Delete(XmlFolderPicker.SelectedPathOrFileName + "\\Weapon\\", true);
            }
            Directory.CreateDirectory(XmlFolderPicker.SelectedPathOrFileName + "\\Weapon\\");
            //武器的导入
            dynamic worksheet = workbook.Sheets(3);
            int rowCount = 4;
            while (!String.IsNullOrEmpty(worksheet.Cells(rowCount, 2).Text))
            {
                Engine.Card.WeaponCard Weapon = new Engine.Card.WeaponCard();
                Weapon.序列号 = worksheet.Cells(rowCount, 2).Text;
                Weapon.名称 = worksheet.Cells(rowCount, 3).Text;
                Weapon.描述 = worksheet.Cells(rowCount, 4).Text;
                Weapon.职业 = CSharpUtility.GetEnum<Engine.Utility.CardUtility.职业枚举>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.职业枚举.中立);
                Weapon.使用成本 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
                Weapon.使用成本 = Weapon.使用成本;

                Weapon.攻击力 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 8).Text);
                Weapon.耐久度 = CSharpUtility.GetInt(worksheet.Cells(rowCount, 9).Text);
                Weapon.稀有程度 = CSharpUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度枚举>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度枚举.白色);
                Weapon.是否启用 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);

                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.WeaponCard));
                String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Weapon\\" + Weapon.序列号 + ".xml";
                xml.Serialize(new StreamWriter(XmlFilename), Weapon);
                rowCount++;
            }
        }
        /// <summary>
        /// 加载窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmExport_Load(object sender, EventArgs e)
        {
            Engine.Utility.CardUtility.CardXmlFolder = @"C:\炉石Git\炉石设计\Card";
            XmlFolderPicker.SelectedPathOrFileName = Engine.Utility.CardUtility.CardXmlFolder;
            ExcelPicker.SelectedPathOrFileName = @"C:\炉石Git\炉石设计\卡牌整理版本.xls";
        }
    }
}

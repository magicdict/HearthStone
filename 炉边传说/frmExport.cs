using Engine.Card;
using Engine.Effect;
using Engine.Utility;
using Microsoft.VisualBasic;
//using MongoDB.Driver;
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
        //private static MongoServer innerServer;
        //private static MongoDatabase innerDatabase;
        //private static MongoCollection innerCollection;
        /// <summary>
        /// 导出到MongoDB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportMongoDB_Click(object sender, EventArgs e)
        {
            //innerServer = MongoServer.Create(@"mongodb://localhost:28030");
            //innerServer.Connect();
            //innerDatabase = innerServer.GetDatabase("HearthStone");
            //innerCollection = innerDatabase.GetCollection("Card");
            if (String.IsNullOrEmpty(ExcelPicker.SelectedPathOrFileName)) return;
            Export(TargetType.MongoDB);
            GC.Collect();
            //innerServer.Disconnect();
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
            Export(TargetType.Xml);
        }
        /// <summary>
        /// 导出类型
        /// </summary>
        private enum TargetType
        {
            /// <summary>
            /// MongoDataBase
            /// </summary>
            MongoDB,
            /// <summary>
            /// XmlFile
            /// </summary>
            Xml
        }
        /// <summary>
        /// 导入
        /// </summary>
        private void Export(TargetType target)
        {
            dynamic excelObj = Interaction.CreateObject("Excel.Application");
            excelObj.Visible = true;
            dynamic workbook;
            workbook = excelObj.Workbooks.Open(ExcelPicker.SelectedPathOrFileName);
            //Minion(target, workbook);
            Ability(target, workbook);
            //AbilityNewFormat(target, workbook);
            //Weapon(target, workbook);
            //Secret(target, workbook);
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
        private void Secret(TargetType target, dynamic workbook)
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
                Secret.SN = worksheet.Cells(rowCount, 2).Text;
                Secret.Name = worksheet.Cells(rowCount, 3).Text;
                Secret.Description = worksheet.Cells(rowCount, 4).Text;
                Secret.Class = CardUtility.GetEnum<Engine.Utility.CardUtility.ClassEnum>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.ClassEnum.中立);
                Secret.StandardCostPoint = CardUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
                Secret.ActualCostPoint = Secret.StandardCostPoint;
                Secret.Rare = CardUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度.白色);
                Secret.IsCardReady = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);
                Secret.Condition = CardUtility.GetEnum<Engine.Card.SecretCard.SecretCondition>(worksheet.Cells(rowCount, 14).Text, SecretCard.SecretCondition.对方召唤随从);
                Secret.AdditionInfo = worksheet.Cells(rowCount, 15).Text;
                switch (target)
                {
                    case TargetType.MongoDB:
                        //innerCollection.Insert<Card.SecretCard>(Secret);
                        break;
                    case TargetType.Xml:
                        XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.SecretCard));
                        String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Secret\\" + Secret.SN + ".xml";
                        xml.Serialize(new StreamWriter(XmlFilename), Secret);
                        break;
                    default:
                        break;
                }
                rowCount++;
            }
        }
        /// <summary>
        /// 随从的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Minion(TargetType target, dynamic workbook)
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
                Minion.SN = worksheet.Cells(rowCount, 2).Text;
                Minion.Name = worksheet.Cells(rowCount, 3).Text;
                Minion.Description = worksheet.Cells(rowCount, 4).Text;
                Minion.Class = CardUtility.GetEnum<Engine.Utility.CardUtility.ClassEnum>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.ClassEnum.中立);
                Minion.种族 = CardUtility.GetEnum<Engine.Utility.CardUtility.种族Enum>(worksheet.Cells(rowCount, 6).Text, Engine.Utility.CardUtility.种族Enum.无);
                Minion.StandardCostPoint = CardUtility.GetInt(worksheet.Cells(rowCount, 7).Text);

                Minion.标准攻击力 = CardUtility.GetInt(worksheet.Cells(rowCount, 8).Text);
                Minion.标准生命值上限 = CardUtility.GetInt(worksheet.Cells(rowCount, 9).Text);
                Minion.Rare = CardUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度.白色);
                Minion.IsCardReady = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);

                Minion.Standard嘲讽 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 14).Text);
                Minion.Standard冲锋 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 15).Text);
                Minion.Standard不能攻击 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 16).Text);
                Minion.Standard风怒 = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 17).Text);
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
                    Minion.光环效果.Name = Minion.Name;
                    Minion.光环效果.Scope = CardUtility.GetEnum<Engine.Card.MinionCard.光环范围>(worksheet.Cells(rowCount, 22).Text, Engine.Card.MinionCard.光环范围.随从全体);
                    Minion.光环效果.EffectType = CardUtility.GetEnum<Engine.Card.MinionCard.光环类型>(worksheet.Cells(rowCount, 23).Text, Engine.Card.MinionCard.光环类型.增加攻防);
                    Minion.光环效果.BuffInfo = worksheet.Cells(rowCount, 24).Text;
                }
                Minion.战吼效果 = worksheet.Cells(rowCount, 25).Text;
                Minion.战吼类型 = CardUtility.GetEnum<Engine.Card.MinionCard.战吼类型列表>(worksheet.Cells(rowCount, 26).Text, Engine.Card.MinionCard.战吼类型列表.默认);

                Minion.亡语效果 = worksheet.Cells(rowCount, 27).Text;
                Minion.激怒效果 = worksheet.Cells(rowCount, 28).Text;
                Minion.连击效果 = worksheet.Cells(rowCount, 29).Text;
                Minion.回合开始效果 = worksheet.Cells(rowCount, 30).Text;
                Minion.回合结束效果 = worksheet.Cells(rowCount, 31).Text;
                Minion.Overload = CardUtility.GetInt(worksheet.Cells(rowCount, 32).Text);
                Minion.自身事件.事件类型 = CardUtility.GetEnum<Engine.Utility.CardUtility.事件类型列表>(worksheet.Cells(rowCount, 33).Text, Engine.Utility.CardUtility.事件类型列表.无);
                Minion.自身事件.事件效果 = worksheet.Cells(rowCount, 34).Text;
                Minion.自身事件.触发方向 = CardUtility.GetEnum<Engine.Utility.CardUtility.TargetSelectDirectEnum>(worksheet.Cells(rowCount, 35).Text, Engine.Utility.CardUtility.TargetSelectDirectEnum.本方);
                Minion.自身事件.附加信息 = worksheet.Cells(rowCount, 36).Text;
                Minion.特殊效果 = CardUtility.GetEnum<Engine.Card.MinionCard.特殊效果列表>(worksheet.Cells(rowCount, 37).Text, Engine.Card.MinionCard.特殊效果列表.无效果);

                switch (target)
                {
                    case TargetType.MongoDB:
                        //innerCollection.Insert<Card.MinionCard>(Minion);
                        break;
                    case TargetType.Xml:
                        XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.MinionCard));
                        String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Minion\\" + Minion.SN + ".xml";
                        xml.Serialize(new StreamWriter(XmlFilename), Minion);
                        break;
                    default:
                        break;
                }
                rowCount++;
            }
        }
        /// <summary>
        /// 法术的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Ability(TargetType target, dynamic workbook)
        {
            if (Directory.Exists(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\"))
            {
                Directory.Delete(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\", true);
            }
            Directory.CreateDirectory(XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\");
            //法术的导入
            dynamic worksheet = workbook.Sheets(2);
            int rowCount = 4;
            Engine.Card.AbilityCard Ability;
            while (!String.IsNullOrEmpty(worksheet.Cells(rowCount, 2).Text))
            {
                //这行肯定是卡牌基本情报
                Ability = new AbilityCard();
                Ability.SN = worksheet.Cells(rowCount, 2).Text;
                Ability.Name = worksheet.Cells(rowCount, 3).Text;
                Ability.Description = worksheet.Cells(rowCount, 4).Text;
                Ability.Class = CardUtility.GetEnum<Engine.Utility.CardUtility.ClassEnum>(worksheet.Cells(rowCount, 9).Text, Engine.Utility.CardUtility.ClassEnum.中立);
                Ability.StandardCostPoint = CardUtility.GetInt(worksheet.Cells(rowCount, 10).Text);
                Ability.Overload = CardUtility.GetInt(worksheet.Cells(rowCount, 11).Text);
                rowCount++;
                //这行肯定是选择条件
                Ability.效果选择类型 = CardUtility.GetEnum<AbilityCard.效果选择类型枚举>(worksheet.Cells(rowCount, 3).Text,
                    Engine.Card.AbilityCard.效果选择类型枚举.无需选择);
                rowCount++;
                Ability.FirstAbilityDefine = GetEffectDefine(worksheet, ref rowCount);
                if (Ability.效果选择类型 != Engine.Card.AbilityCard.效果选择类型枚举.无需选择)
                {
                    Ability.SecondAbilityDefine = GetEffectDefine(worksheet, ref rowCount);
                }
                XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.AbilityCard));
                String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Ability\\" + Ability.SN + ".xml";
                xml.Serialize(new StreamWriter(XmlFilename), Ability);
                rowCount++;
            }
        }
        /// <summary>
        /// GetEffectDifine
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private static AbilityCard.AbilityDefine GetEffectDefine(dynamic worksheet, ref int rowCount)
        {
            AbilityCard.AbilityDefine effect = new AbilityCard.AbilityDefine();
            effect.Init();
            //这行是第一效果的标题栏
            rowCount++;
            //这行是第一效果的内容栏
            effect.MainAbilityDefine.AbliltyPosPicker.EffictTargetSelectMode =
                CardUtility.GetEnum<Engine.Utility.CardUtility.TargetSelectModeEnum>(worksheet.Cells(rowCount, 3).Text, Engine.Utility.CardUtility.TargetSelectModeEnum.不用选择);
            effect.MainAbilityDefine.AbliltyPosPicker.EffectTargetSelectDirect =
                CardUtility.GetEnum<Engine.Utility.CardUtility.TargetSelectDirectEnum>(worksheet.Cells(rowCount, 4).Text, Engine.Utility.CardUtility.TargetSelectDirectEnum.双方);
            effect.MainAbilityDefine.AbliltyPosPicker.EffectTargetSelectRole =
                CardUtility.GetEnum<Engine.Utility.CardUtility.TargetSelectRoleEnum>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.TargetSelectRoleEnum.英雄);
            effect.MainAbilityDefine.AbliltyPosPicker.EffectTargetSelectCondition = worksheet.Cells(rowCount, 6).Text;
            effect.MainAbilityDefine.EffectCount = CardUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
            if (worksheet.Cells(rowCount, 8).Text == CardUtility.strIgnore)
            {

            }
            else
            {

            }
            return effect;
        }

        /// <summary>
        /// 武器的导入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="workbook"></param>
        private void Weapon(TargetType target, dynamic workbook)
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
                Weapon.SN = worksheet.Cells(rowCount, 2).Text;
                Weapon.Name = worksheet.Cells(rowCount, 3).Text;
                Weapon.Description = worksheet.Cells(rowCount, 4).Text;
                Weapon.Class = CardUtility.GetEnum<Engine.Utility.CardUtility.ClassEnum>(worksheet.Cells(rowCount, 5).Text, Engine.Utility.CardUtility.ClassEnum.中立);
                Weapon.StandardCostPoint = CardUtility.GetInt(worksheet.Cells(rowCount, 7).Text);
                Weapon.ActualCostPoint = Weapon.StandardCostPoint;

                Weapon.StandardAttackPoint = CardUtility.GetInt(worksheet.Cells(rowCount, 8).Text);
                Weapon.标准耐久度 = CardUtility.GetInt(worksheet.Cells(rowCount, 9).Text);
                Weapon.Rare = CardUtility.GetEnum<Engine.Card.CardBasicInfo.稀有程度>(worksheet.Cells(rowCount, 12).Text, CardBasicInfo.稀有程度.白色);
                Weapon.IsCardReady = !String.IsNullOrEmpty(worksheet.Cells(rowCount, 13).Text);

                switch (target)
                {
                    case TargetType.MongoDB:
                        //innerCollection.Insert<Card.WeaponCard>(Weapon);
                        break;
                    case TargetType.Xml:
                        XmlSerializer xml = new XmlSerializer(typeof(Engine.Card.WeaponCard));
                        String XmlFilename = XmlFolderPicker.SelectedPathOrFileName + "\\Weapon\\" + Weapon.SN + ".xml";
                        xml.Serialize(new StreamWriter(XmlFilename), Weapon);
                        break;
                    default:
                        break;
                }
                rowCount++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportXML_Click(object sender, EventArgs e)
        {
            Engine.Utility.CardUtility.Init(@"C:\炉石Git\CardHelper\CardXML");
        }
        private void frmExport_Load(object sender, EventArgs e)
        {
            Engine.Utility.CardUtility.CardXmlFolder = @"C:\炉石Git\炉石设计\Card";
            XmlFolderPicker.SelectedPathOrFileName = Engine.Utility.CardUtility.CardXmlFolder;
            ExcelPicker.SelectedPathOrFileName = @"C:\炉石Git\炉石设计\卡牌整理版本.xls";
        }
    }
}

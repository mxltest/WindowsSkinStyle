using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.XtraBars;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using DevExpress.XtraEditors.Controls;

namespace WindowsFormsApplication3
{
    public partial class Form2 : XtraForm
    {
        public Form2()
        {
            InitializeComponent();
            LoadWindowStyle();
            InitSkins();
            
        }
        #region 变量
        /// <summary>
        /// 皮肤名称前缀
        /// </summary>
        private string skinMask = "皮肤: ";
        public uWindowStyle _winStyle;
        System.Windows.Forms.MdiLayout MdiLayout;
        private static string[] skinCodes = new string[] { "Office 2013", "DevExpress Style", "Caramel", "Money Twins", "Lilian", "DevExpress Dark Style", "iMaginary", "Black", "Blue", "Office 2007 Blue", "Office 2007 Black", "Office 2007 Silver", "Office 2007 Green", "Office 2007 Pink", "Office 2010 Blue", "Office 2010 Black", "Office 2010 Silver", "Coffee", "Liquid Sky", "London Liquid Sky", "Glass Oceans", "Stardust", "Xmas 2008 Blue", "Valentine", "McSkin", "Summer 2008", "Pumpkin", "Dark Side", "Springtime", "Darkroom", "Foggy", "High Contrast", "Seven", "Seven Classic", "Sharp", "Sharp Plus", "The Asphalt World", "Blueprint", "Whiteprint", "VS2010" };
            
        private const string styleFileName = "WindowStyle.user";

        #endregion

        #region 加载窗口样式
        public void LoadWindowStyle()
        {
            try
            {
                string path = Path.Combine(GetAssemblyPath(), styleFileName);
                if (File.Exists(path))
                    _winStyle = (uWindowStyle)LoadXmlFormat(path, typeof(uWindowStyle));
                if (_winStyle != null)
                {
                    defaultLookAndFeel.LookAndFeel.SkinName = _winStyle.SkinName;
                    barTop.DockStyle = _winStyle.BarMenuDock;
                    xtraTabbedMdiMgr.MdiParent = this;
                    MdiLayout = _winStyle.MdiLayout;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(""+ ex);
            }
        }
        #endregion

        #region 保存窗口样式

        public void SaveWindowStyle()
        {
            try
            {
               
                _winStyle = new uWindowStyle()
                {
                    SkinName = defaultLookAndFeel.LookAndFeel.SkinName,
                    PaintStyleName = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane,
                    BarMenuDock = barTop.DockStyle,
                    IsTabbedMdi = true ,
                    MdiLayout = MdiLayout
                };
                string path = Path.Combine(GetAssemblyPath(), styleFileName);
                SaveAsXmlFormat(_winStyle, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        #endregion

        #region 切换皮肤

        void OnSkinClick(object sender, ItemClickEventArgs e)
        {
            try
            {
               
                SetSkinItemUnChecked();
                string skinName = e.Item.Tag as string;
                defaultLookAndFeel.LookAndFeel.SkinName = skinName;
                barMainMenu.GetController().PaintStyleName = "Skin";
                barSkinItem.Caption = "皮肤: " + e.Item.Caption;
                barSkinItem.Hint = barSkinItem.Caption;
                ((BarCheckItem)e.Item).Checked = true;
                SaveWindowStyle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
              
            }
        }

        private void SetSkinItemUnChecked()
        {
            foreach (BarItemLink ItemLink in barSkinItem.ItemLinks)
            {
                ((BarCheckItem)ItemLink.Item).Checked = false;
            }
        }
        #endregion

        #region 初始化皮肤

        /// <summary>
        /// 初始化皮肤
        /// </summary>
        void InitSkins()
        {
            bool isCheck = false;
          
            foreach (DevExpress.Skins.SkinContainer cnt in DevExpress.Skins.SkinManager.Default.Skins)
            {
                if (!skinCodes.Contains(cnt.SkinName)) continue;
                BarCheckItem item = new BarCheckItem(barMainMenu, false);
                item.Tag = cnt.SkinName;
                item.Caption = GetSkinName(cnt.SkinName, true);
                barSkinItem.AddItem(item);
                item.ItemClick += new ItemClickEventHandler(OnSkinClick);
                //默认选择一种皮肤
                if (FormatNullValue(item.Tag) == defaultLookAndFeel.LookAndFeel.SkinName)
                {
                    item.Checked = true;//
                    isCheck = true;
                    barSkinItem.Caption = skinMask + item.Tag;
                    barSkinItem.Hint = barSkinItem.Caption;
                   
                }
              
            }
            if (isCheck == false)
            {
                foreach (BarCheckItemLink link in barSkinItem.ItemLinks)
                {
                    BarCheckItem citem = link.Item as BarCheckItem;
                    if (citem != null && Convert.ToString(citem.Tag) == "Office 2013")
                    {
                        citem.Checked = true;
                        barSkinItem.Caption = skinMask + citem.Tag;
                        barSkinItem.Hint = Convert.ToString(barSkinItem.Tag);
                        defaultLookAndFeel.LookAndFeel.SkinName = Convert.ToString(citem.Tag);

                        break;
                    }
                }
            }
        }

        string GetSkinName(string strskin, bool transFlag = false)
        {
            if (!transFlag) return strskin;
            string name = "默认皮肤";
            Dictionary<string, string> dicSkins = new Dictionary<string, string>();
           // dicSkins.Add("Office 2013", "Office2013");
            string[] SKINCODE = { "Office 2013", "DevExpress Style", "Caramel", "Money Twins", "Lilian", "DevExpress Dark Style", "iMaginary", "Black", "Blue", "Office 2007 Blue", "Office 2007 Black", "Office 2007 Silver", "Office 2007 Green", "Office 2007 Pink", "Office 2010 Blue", "Office 2010 Black", "Office 2010 Silver", "Coffee", "Liquid Sky", "London Liquid Sky", "Glass Oceans", "Stardust", "Xmas 2008 Blue", "Valentine", "McSkin", "Summer 2008", "Pumpkin", "Dark Side", "Springtime", "Darkroom", "Foggy", "High Contrast", "Seven", "Seven Classic", "Sharp", "Sharp Plus", "The Asphalt World", "Blueprint", "Whiteprint", "VS2010" };
            string[] SKINNAME = { "Office2013","高雅灰", "大漠飞鹰", "贵族蓝", "纯洁白", "炫酷黑", "梦幻星空", "水晶黑", "水晶蓝", "办公蓝", "办公黑", "办公银", "办公绿", "办公粉", "办公蓝1", "办公黑1", "办公银1", "咖啡主题", "湛蓝天空", "湛蓝天空1", "玻璃海洋", "魔幻星空", "圣诞节", "情人节", "苹果主题", "夏日炎炎", "万圣节", "水晶黑", "春意盎然", "暗室之谜", "雾色撩人", "反光主题", "蓝色经典", "蓝色经典1", "灰色经典", "灰色经典1", "都市之光", "设计蓝图", "设计蓝图1", "深蓝主题" };
            for (int i = 0; i < SKINCODE.Length; i++)
            {
                dicSkins.Add(SKINCODE[i], SKINNAME[i]);
            }
            for (int i = 0; i < SKINCODE.Length; i++)
            {
                 if (SKINCODE[i] == strskin)
                {
                    name = SKINNAME[i];
                    break;
                }
            }
            if (dicSkins.ContainsKey(strskin))
                name = dicSkins[strskin];
            return name;
        }

        #endregion
        #region 获取Assembly的运行路径
        /// <summary>
        /// 获取Assembly的运行路径
        /// </summary>
        /// <returns>运行路径</returns>
        public static string GetAssemblyPath()
        {
            return GetAssemblyPath(1);
        }

        /// <summary>
        /// 获取Assembly的运行路径
        /// </summary>
        /// <param name="pLength">忽略路径层次</param>
        /// <returns>运行路径</returns>
        private static string GetAssemblyPath(int pLength)
        {
            string _CodeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            _CodeBase = _CodeBase.Substring(8, _CodeBase.Length - 8);    // 8是 file:// 的长度

            string[] arrSection = _CodeBase.Split(new char[] { '/' });

            string _FolderPath = "";
            for (int i = 0; i < arrSection.Length - pLength; i++)
            {
                _FolderPath += arrSection[i] + "\\";
            }

            return _FolderPath;
        }

        /// <summary>
        /// 获取Exe文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationPath()
        {
            return Application.StartupPath;
        }


        /// <summary>
        /// XML文件反序列化成实体 
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="type">实体类型</param>
        /// <returns>实体</returns>
        public static object LoadXmlFormat(string filePath, System.Type type)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                object obj = xs.Deserialize(reader);
                reader.Close();
                return obj;
            }
        }
        #endregion
        /// <summary>
        /// 对象保存为XML序列化文件
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="fileName">文件名</param>
        public static void SaveAsXmlFormat(object obj, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer xmlFormat = new XmlSerializer(obj.GetType());
                xmlFormat.Serialize(fStream, obj);
                fStream.Close();
            }
        }
        #region 格式化(返回String)
        /// <summary>
        /// 格式化(返回String)
        /// </summary>
        /// <param name="objValue">对象</param>
        /// <returns>返回String</returns>
        public static string FormatNullValue(object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
            {
                return string.Empty;
            }
            else
                return objValue.ToString();
        }
        #endregion

        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize); ////// 释放内存 

        protected static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #endregion

        /// <summary>
        /// 向父窗体增加子窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            xtraTabbedMdiMgr.MdiParent = this;   //设置控件的父表单..
            Form1 frm = new Form1();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiMgr.SelectedPage = xtraTabbedMdiMgr.Pages[frm];    //使得标签的选择为当前新建的窗口
              this.xtraTabbedMdiMgr.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;    //设置标签后面添加删除按钮 ,  多个标签只需要设置一次..
    
        }
        /// <summary>
        /// 获取对象颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void riColor_ColorChanged(object sender, EventArgs e)
        {
            this.Appearance.BackColor = ((System.Drawing.Color)(((ColorEdit)sender).EditValue));
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            i++;
            System.Diagnostics.Debug.Write(i);//调试窗口输出数据
        }
        private static int i = 0;
    }

}

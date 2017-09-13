using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DevExpress.XtraNavBar;

namespace WindowsFormsApplication3
{
    [Serializable]
    public class uWindowStyle
    {
        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName { get; set; }
        /// <summary>
        /// 导航栏样式
        /// </summary>
        public DevExpress.XtraNavBar.NavBarViewKind PaintStyleName { get; set; }
        /// <summary>
        /// 导航栏位置
        /// </summary>
        public DevExpress.XtraBars.Docking.DockingStyle PanelMenuDock { get; set; }
        /// <summary>
        /// 菜单栏位置
        /// </summary>
        public DevExpress.XtraBars.BarDockStyle BarMenuDock { get; set; }
        /// <summary>
        /// 是否MDI窗口
        /// </summary>
        public bool IsTabbedMdi { get; set; }
        /// <summary>
        /// MDI窗口排列方式
        /// </summary>
        public System.Windows.Forms.MdiLayout MdiLayout { get; set; }
    }
}

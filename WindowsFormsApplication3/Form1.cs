using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public const int SetColumns = 5;
        public const int SetRows = 5;
        public Form1()
        {
            InitializeComponent();
            //InitGridViewColumns();
            Table_getvalue();
            //gridView1.InitNewRow += gridView1_InitNewRow;

            gridView1_DataSourceChanged();
          //  RaiseCustomDrawEmptyForeground(null );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_DataSourceChanged()
        {
            //DataTable dt = ((DataTable)this.gridControl1.DataSource).Copy();//不能复制表只能克隆表结构，因为无法修改表中已有的数据类型
            DataTable dt = ((DataTable)this.gridControl1.DataSource).Clone();
            string columnname = dt.Columns[1].ColumnName;

            if ((dt.Columns.Contains("年龄") && dt.Columns["年龄"].DataType == typeof(Image)))
            { return; }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns["年龄"].DataType == typeof(string))
                {
                    dt.Columns["年龄"].DataType = typeof(Image);
                }
            }
            foreach (DataRow dr in ((DataTable)this.gridControl1.DataSource).Rows)
            {
                DataRow drNew = dt.NewRow();
             
               drNew["姓名"] = dr["姓名"];
               drNew["sex"] = dr["sex"];
               drNew["a"] = dr["a"];

                if (dr["年龄"] != null && !string.IsNullOrEmpty(dr["年龄"].ToString()))
                {
                    drNew["年龄"] = Image.FromFile(@"Resource\" + dr["年龄"].ToString());
                }
               
                dt.Rows.Add(drNew);
            }
            this.gridControl1.DataSource = dt;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            
        }

        /// <summary>
        /// GridView初始化
        /// </summary>
        protected virtual void InitGridViewColumns( )
        {
            //方法一
            GridView gv = ((DevExpress.XtraGrid.Views.Grid.GridView)gridControl1.MainView);
            gv.Columns.Clear();
            //添加列  并 设置列名
            for (int i = 0; i <= SetColumns; i++)
            {
                GridColumn gc = new GridColumn();
                gc.FieldName = string.Format("{0}", i);
                gc.Caption = string.Format("{0}.0", i);
                gc.Tag = i;
                gc.Visible = true;
                if (!gc.Visible) gc.VisibleIndex = -1;
                else gc.VisibleIndex = i;

                gc.OptionsColumn.AllowEdit = true;
                gc.OptionsColumn.AllowFocus = true;
                gv.Columns.Add(gc);              
            }
                //设置gv属性
                gv.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;//滚动条设置
                gv.OptionsView.ColumnAutoWidth = true;
                gv.OptionsBehavior.Editable = true;//保存更改内容
                gv.BestFitColumns();
                gv.OptionsNavigation.AutoFocusNewRow = true;
                gv.OptionsNavigation.EnterMoveNextColumn = true;
                gv.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
                gridControl1.DataSource = gv;//绑定数据源
               
                Random ran = new Random();
                //添加行数
                for (int i = 0; i <= SetRows; i++)
                {
                   gridView1.AddNewRow();
                    //添加列  并 设置列名
                    for (int j = 0; j <= SetColumns; j++)
                    {
                        //设置行值
                        this.gridView1.SetRowCellValue(i, gridView1.Columns[j], ran.Next(10, 1000).ToString());
                        // this.gridView1.GetDataRow(j)[string.Format("{0}.0", i)] = ran.Next(10, 1000).ToString();
                    }
                   
                }
       }

        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Table_getvalue()
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("姓名");
            _dt.Columns.Add("年龄");
            _dt.Columns.Add("sex");
            _dt.Columns.Add("a");
            _dt.Rows.Add("aa", "21.bmp", 12, "21.bmp");
            _dt.Rows.Add("bb", "22.bmp", 12, "22.bmp");
            _dt.Rows.Add("cc", "23.bmp", 12, "23.bmp");
            _dt.Rows.Add("dd", "24.bmp", 12, "24.bmp");
            _dt.Rows.Add("ee", "25.bmp", 12, "25.bmp");
            _dt.Rows.Add("ff", "26.bmp", 12, "26.bmp");
            gridControl1.DataSource = _dt;
        }

        /// <summary>
        /// 将GirdView 转换为 DataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            
            // 列强制转换
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }

            // 循环行
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected  void RaiseCustomDrawEmptyForeground(CustomDrawEventArgs e)
        {
            if ( !e.Handled)
            {
                //Image img = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("Image1.bmp");
                Image img = Image.FromFile(@"Resource\Image1.bmp");
                RectangleF actualRectF;
                int actualHeight = e.Bounds.Height - 26;
                if (e.Bounds.Width < img.Width || actualHeight < img.Height)
                {
                    // 当前区域小于图片大小，进行缩放。  
                    float factor1 = e.Bounds.Width * 1f / img.Width;
                    float factor2 = actualHeight * 1f / img.Height;
                    float factor = Math.Min(factor1, factor2);
                    float x = (e.Bounds.Width - img.Width * factor) / 2;
                    float y = (e.Bounds.Height - img.Height * factor) + 26 / 2;
                    actualRectF = new RectangleF(x, y, img.Width * factor, img.Height * factor);
                }
                else
                {
                    actualRectF = new RectangleF((e.Bounds.Width - img.Width) / 2f, (actualHeight - img.Height) / 2f + 26, img.Width, img.Height);
                }
                e.Graphics.DrawImage(img, actualRectF);
                e.Handled = true;
            }
          //  base.RaiseCustomDrawEmptyForeground(e);
        }  
    }

}

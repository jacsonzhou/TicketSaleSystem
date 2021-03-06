﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using TicketSaleSystem.SystemOperate;
using DevExpress.XtraTab;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using ToolsHelper;
using TSS_BLL;


namespace TicketSaleSystem
{
    public partial class Frm_MainForm : RibbonForm
    {
        private MainFormBLL mainFormBLL = new MainFormBLL();
        private string FieldName = "COLNAME"; // 用于TreeList显示文字的列，设计界面右键单击RunDesign
        private int I_ImageCount = 0;
        private Frm_Login frm_Login = null;
        public Frm_MainForm(Frm_Login frm)
        {
            frm_Login = frm;
            InitializeComponent();
            InitStatus();
            InitSkinGallery();
            BindMainFormTreeList();
        }

        private void InitStatus()
        {
            DateTime dt = DateTime.Now;
            this.barStaticItem4.Caption = "工号：" + SystemInfo.UserID;
            this.barStaticItem5.Caption = "姓名：" + SystemInfo.UserName;
            this.barStaticItem6.Caption = "时间：" + string.Format("{0:yyyy年MM月dd日 HH:mm:ss}", dt);
        }
        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        #region 登录
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frm_Login frm_login = new Frm_Login();
            frm_login.ShowDialog();
        }
        #endregion

        #region 注销
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您确定要注销吗?", "注销用户", messButton);
                if (dr == DialogResult.OK)
                {
                    // 重置全局变量
                    // 重置按钮
                    // 重置树
                    // 关闭除主页外的全部标签页
                    for (int i = xtraTabControl1.TabPages.Count - 1; i > 0 ; i--)
                    {
                        XtraTabPage page = xtraTabControl1.TabPages[i];
                        if (page.Text != "首 页")
                        {
                            xtraTabControl1.TabPages.Remove(page);
                            page.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Log写入失败：" + ex.Message);
            }
        }
        #endregion

        #region 退出
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 退出
            try
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您确定要退出吗?", "退出系统", messButton);
                if (dr == DialogResult.OK)
                {
                    frm_Login.Close();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Log写入失败：" + ex.Message);
            }
        }
        #endregion

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs EArg = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            string name = EArg.Page.Text; // 得到关闭的选项卡的text
            foreach (XtraTabPage page in xtraTabControl1.TabPages) // 遍历得到和关闭的选项卡一样的Text
            {
                if (page.Text == name)
                {
                    xtraTabControl1.TabPages.Remove(page);
                    page.Dispose();
                    return;
                }
            }
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            TreeListNode clickedNode = this.treeList1.FocusedNode;
            if (clickedNode != null && !clickedNode.HasChildren)
            {
                string disPlayText = clickedNode.GetDisplayText(FieldName); // 显示的汉字，目前无法取到绑定时的数字
                Object obj = GetDisplayObject(disPlayText);
                if (obj != null)
                    UIHelper.AddUserControl(xtraTabControl1, obj, disPlayText, disPlayText);
            }
        }

        /// <summary>
        /// 根据DisPlayText打开相应标签页内容
        /// </summary>
        /// <param name="DisPlayText"></param>
        /// <returns></returns>
        private object GetDisplayObject(string DisPlayText)
        {
            Object obj = null;
            if (string.IsNullOrEmpty(DisPlayText))
                return obj;

            switch(DisPlayText)
            {
                case "门票操作": break;
                case "财务入库": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "财务出库": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockOut(); break;
                case "门票出库": obj = new TicketSaleSystem.TicketOperate.Frm_TicketOut(); break;
                case "售票": obj = new TicketSaleSystem.TicketOperate.Frm_Sale(); break;
                case "退票": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "PDA销票": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "人工销票": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "异常退票": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "出库门票": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "销售查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "个人结存": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "团体购票查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "高级查询": break;
                case "门票状态查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "门票综合查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "游客查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "IC卡统计": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "销售汇总": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "进销汇总": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "时段汇总": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "门票检入查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "IC卡检入查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "销售明细": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "缴款明细": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "报表查询": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "管理": break;
                case "密码修改": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "售票员结算": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "门票项目": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "人员管理": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "团体管理": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                case "使用时间管理": obj = new TicketSaleSystem.TicketOperate.Frm_FinanceStockIn(); break;
                default:
                    // Write Error Log
                    break;
            }
            return obj;
        }

        /// <summary>
        /// 获取树结构数据
        /// </summary>
        private void BindMainFormTreeList()
        {
            this.Cursor = Cursors.WaitCursor;
            string errorCode = "";
            try
            {
                DataTable dt = mainFormBLL.BindMainFormTreeList(SystemInfo.UserID, ref errorCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtMenu = dt;
                    treeList1.Nodes.Clear();
                    treeList1.DataSource = dtMenu;
                    treeList1.ParentFieldName = "PID";
                    treeList1.KeyFieldName = "ID";
                    treeList1.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                // Error
                LogFile.WriteLine("BindTreeListData Error: " + ex.Message);
                MessageBox.Show("加载树数据时发生错误！错误代码：" + errorCode);
            }
            this.Cursor = Cursors.Default;
        }

        private void barButtonItem35_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 门票操作 刷新树
            RefreshTreeList("门票操作");
        }

        private void barButtonItem36_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 高级查询 刷新树
            RefreshTreeList("高级查询");
        }

        private void barButtonItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 管理 刷新树
            RefreshTreeList("管理");
        }

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 关于 弹窗
        }

        // 点击菜单大类时刷新对应树结构
        private void RefreshTreeList(string nodeName)
        {
            if (nodeName == null)
                return;
            TreeListNodes allNode = this.treeList1.Nodes;
            if (allNode != null)
            {
                this.treeList1.CollapseAll();
                for (int i = 0; i < allNode.Count; i++)
                {
                    TreeListNode tempTreeListNode = allNode[i];
                    if (tempTreeListNode.GetDisplayText(FieldName).Equals(nodeName)) 
                    {
                        tempTreeListNode.Expanded = true;
                    }
                }
            }
        }

        private void treeList1_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            switch (e.Node.GetDisplayText(FieldName))
            {
                case "门票操作": I_ImageCount = 0; break;
                case "财务入库": I_ImageCount = 1; break;
                case "财务出库": I_ImageCount = 2; break;
                case "门票出库": I_ImageCount = 3; break;
                case "售票": I_ImageCount = 4; break;
                case "退票": I_ImageCount = 5; break;
                case "PDA销票": I_ImageCount = 6; break;
                case "人工销票": I_ImageCount = 7; break;
                case "异常退票": I_ImageCount = 8; break;
                case "出库门票": I_ImageCount = 9; break;
                case "销售查询": I_ImageCount = 10; break;
                case "个人结存": I_ImageCount = 11; break;
                case "团体购票查询": I_ImageCount = 12; break;
                case "高级查询": I_ImageCount = 12; break;
                case "门票状态查询": I_ImageCount = 13; break;
                case "门票综合查询": I_ImageCount = 13; break;
                case "游客查询": I_ImageCount = 14; break;
                case "IC卡统计": I_ImageCount = 15; break;
                case "销售汇总": I_ImageCount = 16; break;
                case "进销汇总": I_ImageCount = 17; break;
                case "时段汇总": I_ImageCount = 18; break;
                case "门票检入查询": I_ImageCount = 13; break;
                case "IC卡检入查询": I_ImageCount = 15; break;
                case "销售明细": I_ImageCount = 20; break;
                case "缴款明细": I_ImageCount = 21; break;
                case "报表查询": I_ImageCount = 22; break;
                case "管理": I_ImageCount = 23; break;
                case "密码修改": I_ImageCount = 24; break;
                case "售票员结算": I_ImageCount = 25; break;
                case "门票项目": I_ImageCount = 26; break;
                case "人员管理": I_ImageCount = 27; break;
                case "团体管理": I_ImageCount = 28; break;
                case "使用时间管理": I_ImageCount = 29; break;
                default:
                    I_ImageCount = -1;
                    break;
            }
            e.SelectImageIndex = I_ImageCount;
            e.StateImageIndex = I_ImageCount; // e.SelectImageIndex为图片在ImageList中的index
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            InitStatus();
        }
    }
}
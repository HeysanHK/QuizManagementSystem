﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Pages_PortoQuiz_Home : System.Web.UI.Page
{
    private string user;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty((string)Session["user"]))
        {
            user = (string)Session["user"];
        }
        else
        {
            Response.Redirect("../Login/Login.aspx");
        }

        lblAdmin_USername.Text = (string)Session["name"];
        lblAccountUsername.Text = (string)Session["name"];
        lblDesignation.Text = (string)Session["des_id"];
        imgUser.ImageUrl = (string)Session["pic_name"];
        lblSignIn_Time.Text = (string)Session["signin"];
        SelectRight();
        SelectDepartment();
    }

    public void SelectDepartment()
    {
        List<BLLDepartmentDesignation> objlist = new List<BLLDepartmentDesignation>();
        BLLDepartmentDesignation objbll = new BLLDepartmentDesignation();
        objlist = objbll.SelectDepartment();
        if (objlist.Count > 0)
        {
            dgdepartment.DataSource = objlist;
            //dgdepartment.Columns[3].Visible = false;
            dgdepartment.DataBind();
            for (int i = 0; i <= dgdepartment.Rows.Count - 1; i++)
            {
                string update = (string)Session["update"];
                string delete = (string)Session["delete"];
                if (update == "yes")
                {
                    dgdepartment.Rows[i].Cells[2].Enabled = true;
                }
                else
                {
                    dgdepartment.Rows[i].Cells[2].Enabled = false;
                }
                if (delete == "yes")
                {
                    dgdepartment.Rows[i].Cells[3].Enabled = true;
                }
                else
                {
                    dgdepartment.Rows[i].Cells[3].Enabled = false;
                }
            }
        }
    }
    protected void linkProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("profile.aspx");
    }
    protected void linkChangePassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Login/ChangePasword.aspx");
    }
    protected void linkSignOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session["user"] = null;
            Session.Abandon();
            Response.Redirect("../Login/Login.aspx");
        }
        catch
        {

        }
    }

    public void SelectRight()
    {
        List<BLLUserRights> objlist = new List<BLLUserRights>();
        BLLUserRights objbll = new BLLUserRights();
        objbll.Emp_code = user;
        objbll.App_detail = "Department";
        objlist = objbll.Selectuserrights(objbll);
        if (objlist.Count > 0)
        {
            string r_add = objlist[0].Rr_add;
            string r_visible = objlist[0].Rr_visible;
            string r_edit = objlist[0].Rr_edit;
            string r_delete = objlist[0].Rr_delete;

            if (r_edit == "1")
            {
                Session["update"] = "yes";
            }
            if (r_delete == "1")
            {
                Session["delete"] = "yes";
            }
            if (r_visible == "1")
            {
                SelectDepartment();
            }
        }
        else
        {
            Response.Redirect("norights.aspx");
        }

    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        LinkButton obj = (LinkButton)(sender);
        string val = obj.CommandArgument.ToString();
        Session["dep_update"] = obj.CommandArgument.ToString();
        Session["mode"] = "ed";
        Response.Redirect("department.aspx");
        
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        LinkButton obj = (LinkButton)(sender);
        string val = obj.CommandArgument.ToString();
        BLLDepartmentDesignation objbll = new BLLDepartmentDesignation();
        objbll.Dep_id = val;
        int a = objbll.DeleteDepartment(objbll);
        SelectDepartment();
    }
}
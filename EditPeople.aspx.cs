﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class EditPeople : System.Web.UI.Page
{
   
    xData sql = new xData();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //зарежда статусите
            Load_Ddl_Status();
            //зарежда апартаментите
            Load_Ddl_Apartaments();

            string EID = Request.QueryString["id"];
            ReadElements(EID);
           
        }
    }

    /// <summary>
    /// зарежда елементите
    /// </summary>
    /// <param name="EID"></param>

    protected void ReadElements(string EID)
    {

        string query = "Select * , hs.Home from CONTACTS cntct left join HOUSES hs ON hs.ID = cntct.Hous_ID where cntct.ID='" + EID + "'";

        SqlDataReader dataRead = sql.ExecuteReader(query);

        if (dataRead.Read())
        {
            txtID.Text          = EID;
            ddl_Home.Text       = dataRead["Hous_ID"].ToString().Trim();
            txtFirstName.Text   = dataRead["FirstName"].ToString().Trim();
            txtLastName.Text    = dataRead["LastName"].ToString().Trim();
            txtAddress.Text     = dataRead["Address"].ToString().Trim();
            txtContact.Text     = dataRead["ContactTitle"].ToString().Trim();
            txtNotes.Text       = dataRead["Notes"].ToString().Trim();
            ddl_Status.Text     = dataRead["Status"].ToString().Trim();
            txtPhone.Text       = dataRead["Phone"].ToString().Trim();
            txtCity.Text        = dataRead["City"].ToString().Trim();
            txtEmail.Text       = dataRead["Email"].ToString().Trim();
        }

        int Rezultat = sql.CloseCommand(dataRead);

    }

    /// <summary>
    /// Тук се зареждата статусите в ddl списък
    /// </summary>
    protected void Load_Ddl_Status()
    {

        DataSet dataS = sql.ExecuteToDataSet("select * from STATUSES");
        ddl_Status.DataTextField = dataS.Tables[0].Columns["StatusName"].ToString();
        ddl_Status.DataValueField = dataS.Tables[0].Columns["ID"].ToString();
        ddl_Status.DataSource = dataS.Tables[0];
        ddl_Status.DataBind();
    }

    /// <summary>
    /// Тук се зареждата апартаментите в ddl списък
    /// </summary>
    protected void Load_Ddl_Apartaments()
    {

        DataSet dataS           = sql.ExecuteToDataSet("select * from HOUSES WHERE User_ID = " + Session["UserID"] + "");
        ddl_Home.DataTextField  = dataS.Tables[0].Columns["Home"].ToString();
        ddl_Home.DataValueField = dataS.Tables[0].Columns["ID"].ToString();
        ddl_Home.DataSource     = dataS.Tables[0];
        ddl_Home.DataBind();
    }

    /// <summary>
    /// При натискане на бутон Съхрани се ъпдейтват данните в базата
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string query = "Update CONTACTS  Set FirstName='" + txtFirstName.Text.Trim() +
                "', LastName='" + txtLastName.Text.Trim() + "', Address='" + txtAddress.Text.Trim() +
                "', ContactTitle='" + txtContact.Text.Trim() + "', Notes='" + txtNotes.Text.Trim() +
                "', Status='" + int.Parse(ddl_Status.Text.Trim()) + "', Phone='" + txtPhone.Text.Trim() +
                "', City='" + txtCity.Text.Trim() + "', Hous_ID='" + ddl_Home.Text.Trim() + "' where ID='" + txtID.Text.Trim() + "'";

            int rezult = sql.ExecuteNonQuery(query);

            if (rezult == 1)
            {
                lblResult.Text = "Вашите данни са съхранени!";
                pnlEdit.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblResult.Text = "Грешка: " + ex.Message;
            lblResult.ForeColor = System.Drawing.Color.Red;
        }

    }
 }

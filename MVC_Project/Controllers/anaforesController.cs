using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Project.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MVC_Project.Controllers
{
    public class anaforesController : Controller
    {
        // GET: anafores
        public ActionResult Index()
        {

            return View("Index");
        }

        public ActionResult firstanafora(string X,string date_start,string date_end,display_anafores da)
        {
            var constring = System.Configuration.ConfigurationManager.ConnectionStrings["pubsEntities"].ConnectionString;
            if (constring.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(constring);
                constring = efBuilder.ProviderConnectionString;
            }
            string query = "SELECT";
            SqlConnection con = new SqlConnection(constring);
            if (X != "")
            {
                query = query + " TOP(" + X + ")";
            }
            query = query + " sales.title_id,authors.au_fname,authors.address,authors.au_lname,authors.city,authors.phone,authors.zip,authors.state,SUM(sales.qty) as qty,sales.ord_date " +
                "FROM[pubs].[dbo].sales INNER JOIN titleauthor on titleauthor.title_id = sales.title_id " +
                "INNER JOIN authors on authors.au_id = titleauthor.au_id ";
            if (date_start != "" && date_end != "")
            {
                query = query + "WHERE sales.ord_date>='" + date_start + "' AND sales.ord_date<='" + date_end + "'";
            }
            else if(date_start != ""){
                query = query + "WHERE sales.ord_date>='" + date_start + "'";
            }
            else if (date_end != "")
            {
                query = query + "WHERE sales.ord_date<='" + date_end + "'";
            }
            query = query + " GROUP BY sales.title_id,authors.au_fname,authors.address,authors.au_lname,authors.city,authors.phone,authors.zip,authors.state,ord_date ORDER BY qty DESC";
            SqlCommand sqlcomm = new SqlCommand(query);
            sqlcomm.Connection = con;
            con.Open();
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            List<display_anafores> objmodel = new List<display_anafores>();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    var details = new display_anafores();
                    details.auth_name = sdr["au_fname"].ToString();
                    details.auth_lastname = sdr["au_lname"].ToString();
                    details.phone = sdr["phone"].ToString();
                    details.address = sdr["address"].ToString();
                    details.city = sdr["city"].ToString();
                    details.state = sdr["state"].ToString();
                    details.zip = sdr["zip"].ToString();
                    objmodel.Add(details);
                }
                da.info = objmodel;
                con.Close();
            }

            return View("firstanafora",da);
        }

        public ActionResult secondAnafora(string a, string b,string date_start, string date_end, display_anafores da)
        {
            var constring = System.Configuration.ConfigurationManager.ConnectionStrings["pubsEntities"].ConnectionString;
            if (constring.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(constring);
                constring = efBuilder.ProviderConnectionString;
            }
            string query = "SELECT sales.ord_num,sales.stor_id,sales.ord_date,sales.title_id,titles.title,stores.stor_name " +
                "FROM [pubs].[dbo].sales " +
                "INNER JOIN stores on stores.stor_id = sales.stor_id " +
                "INNER JOIN titles on titles.title_id = sales.title_id ";
            SqlConnection con = new SqlConnection(constring);
            if (a != "" || b != "" || date_start != "" || date_end != "")
            {
                Boolean flag = false;
                query += "WHERE ";
                if (date_start != "" && date_end != "")
                {
                    query = query + "(sales.ord_date>='" + date_start + "' AND sales.ord_date<='" + date_end + "')";
                    flag = true;
                }
                else if (date_start != "")
                {
                    flag = true;
                    query = query + "(sales.ord_date>='" + date_start + "')";
                }
                else if (date_end != "")
                {
                    flag = true;
                    query = query + "(sales.ord_date<='" + date_end + "')";
                }

                if (a != "" && b != "")
                {
                    if (flag)
                    {
                        query = query + " AND ";
                    }
                    query = query + "(stores.stor_name LIKE '[" + a + "-" + b + "]%')";
                }
                else if (a != "")
                {
                    if (flag)
                    {
                        query = query + " AND ";
                    }
                    query = query + "(stores.stor_name LIKE '[" + a + "-Z]%')";
                }
                else if (b != "")
                {
                    if (flag)
                    {
                        query = query + " AND ";
                    }
                    query = query + "(stores.stor_name LIKE '[A-" + b + "]%')";
                }
            }
            
            query = query + " GROUP BY sales.ord_num,sales.stor_id,sales.ord_date,sales.title_id,titles.title,stores.stor_name ORDER BY sales.ord_num DESC;";
            SqlCommand sqlcomm = new SqlCommand(query);
            sqlcomm.Connection = con;
            con.Open();
            SqlDataReader sdr = sqlcomm.ExecuteReader();
            List<display_anafores> objmodel = new List<display_anafores>();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    var details = new display_anafores();
                    details.order_id = sdr["ord_num"].ToString();
                    details.title_name = sdr["title"].ToString();
                    details.store_name = sdr["stor_name"].ToString();
                    objmodel.Add(details);
                }
                da.info = objmodel;
                con.Close();
            }

            return View("secondanafora", da);
        }
    }


}
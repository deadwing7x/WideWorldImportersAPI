namespace WebApplication1.Views
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using WebApplication1.Models;
    #endregion

    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult GetSalesByStateProvince()
        {
            List<Sales> totalSale = new List<Sales>();
            string connectionString = WebConfigurationManager.ConnectionStrings["main"].ConnectionString;
            string query = "Select c.[State Province], Sum([Total Including Tax]) as TotalSale From  Fact.Sale s Join Dimension.City c On s.[City Key] = c.[City Key] Group By c.[State Province]";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var sales = new Sales();
                    sales.stateProvince = Convert.ToString(rdr["State Province"]);
                    if (sales.stateProvince == "N/A")
                        continue;
                    else
                    {
                        sales.totalSale = Convert.ToDouble(rdr["TotalSale"]);
                        totalSale.Add(sales);
                    }
                }
            }
            return View(totalSale);
        }

        public void Update(int year)
        {
            Sales sales = new Sales();
            year = sales.year;
        }

        public ActionResult GetTotalSalesByStateProvinceForAYear(Sales sales)
        {
            Update(sales.year);
            List<Sales> totalSale = new List<Sales>();
            if (sales.year >= 2013 && sales.year <= 2016)
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["main"].ConnectionString;
                string query = "Select c.[State Province], Sum([Total Including Tax]) as TotalSale from  Fact.Sale s join Dimension.City c on s.[City Key] = c.[City Key] where year(s.[Delivery Date Key]) = @date group by c.[State Province]";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlParameter unitsParam = command.Parameters.AddWithValue("@date", sales.year);
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var salesObj = new Sales();
                        salesObj.stateProvince = Convert.ToString(reader["State Province"]);
                        salesObj.totalSale = Convert.ToDouble(reader["TotalSale"]);
                        totalSale.Add(salesObj);
                    }
                    return View(totalSale);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter a valid year between 2013 and 2016 to view the sales data.";
                return View(totalSale);
            }
        }

        public ActionResult GetTotalSalesByColor(Sales sales)
        {
            Update(sales.year);
            List<Sales> totalSale = new List<Sales>();
            if (sales.year >= 2013 && sales.year <= 2016)
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["main"].ConnectionString;
                string query = "select si.Color, Sum([Total Including Tax]) as TotalSale from  Fact.Sale s join Dimension.[Stock Item] si on s.[Stock Item Key] = si.[Stock Item Key] where year(s.[Delivery Date Key]) = @date group by si.Color";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlParameter unitsParam = command.Parameters.AddWithValue("@date", sales.year);
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var salesObj = new Sales();
                        salesObj.color = Convert.ToString(reader["Color"]);
                        if (salesObj.color == "N/A")
                            continue;
                        else
                        {
                            salesObj.totalSale = Convert.ToDouble(reader["TotalSale"]);
                            totalSale.Add(salesObj);
                        }
                    }
                    return View(totalSale);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter a valid year between 2013 and 2016 to view the sales data.";
                return View(totalSale);
            }
        }

        public ActionResult GetProfitDataByStateProvince()
        {
            List<Sales> totalSale = new List<Sales>();
            string connectionString = WebConfigurationManager.ConnectionStrings["main"].ConnectionString;
            string query = "Select Top(100) c.[State Province], Sum([Total Including Tax]) as TotalSale, s.Profit from  Fact.Sale s join Dimension.City c on s.[City Key] = c.[City Key] group by c.[State Province], S.Profit";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var salesObj = new Sales();
                    salesObj.stateProvince = Convert.ToString(reader["State Province"]);
                    salesObj.totalSale = Convert.ToDouble(reader["TotalSale"]);
                    salesObj.profit = Convert.ToDouble(reader["Profit"]);
                    totalSale.Add(salesObj);
                }
                return View(totalSale);
            }
        }
    }
}

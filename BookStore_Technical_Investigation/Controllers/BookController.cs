using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore_Technical_Investigation.Data;
using BookStore_Technical_Investigation.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BookStore_Technical_Investigation.Controllers
{
    public class BookController : Controller
    {
        /*private readonly BookStore_Technical_InvestigationContext _context;*/
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookStore_Technical_InvestigationContext")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SPViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

        
        public IActionResult Start()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookStore_Technical_InvestigationContext")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SPViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }


        public IActionResult CreateOrEdit(int? id)
        {
            BookModel BookModel = new BookModel();
            if (id > 0)
            {
                BookModel = FetchBookById(id);
            }
            return View(BookModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, [Bind("BookID,Title,Author,Price")] BookModel BookModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookStore_Technical_InvestigationContext")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlcmd = new SqlCommand("SPAddorEdit", sqlConnection);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("BookID", BookModel.BookID);
                    sqlcmd.Parameters.AddWithValue("Title", BookModel.Title);
                    sqlcmd.Parameters.AddWithValue("Author", BookModel.Author);
                    sqlcmd.Parameters.AddWithValue("Price", BookModel.Price);
                    sqlcmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(BookModel);
        }

        
        public IActionResult Delete(int? id)
        {
            BookModel BookModel = FetchBookById(id);
            return View(BookModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookStore_Technical_InvestigationContext")))
            {
                sqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand("SPDeleteByID", sqlConnection);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("BookID", id);
                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookModel FetchBookById(int? id)
        {
            BookModel BookModel = new BookModel();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("BookStore_Technical_InvestigationContext")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SPViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    BookModel.BookID = Convert.ToInt32(dtbl.Rows[0]["BookID"].ToString());
                    BookModel.Title = dtbl.Rows[0]["Title"].ToString();
                    BookModel.Author = dtbl.Rows[0]["Author"].ToString();
                    BookModel.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                }
                return BookModel;
            }
        }
    }
}


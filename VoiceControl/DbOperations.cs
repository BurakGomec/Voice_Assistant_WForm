using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections;
using System.Data;

namespace VoiceControl
{
    class DbOperations
    {
       
        public static void GetList(DataGridView dataGridView1)
        {
            SQLiteConnection connect = new SQLiteConnection("Data Source=products.db");
            SQLiteDataAdapter adapter = new SQLiteDataAdapter("Select * from products", connect);
            connect.Open();
            DataSet data = new DataSet();
            adapter.Fill(data, "products");
            dataGridView1.AutoResizeColumns();
           dataGridView1.AutoResizeColumns();
            //dataGridView1.AutoResizeRows();
            dataGridView1.DataSource = data.Tables["products"];
            adapter.Dispose();
            connect.Dispose();
        }


     public static void AddProduct(TextBox textBox_name, TextBox textBox_brand, TextBox textBox_price, TextBox textBox_piece)
     {
            try
            {
                string add = "insert into products(Name,Brand,Price,Piece)" + "values(@Name,@Brand,@Price,@Piece)";
                using (var connection = new SQLiteConnection("Data Source=products.db"))
                {
                    using (var command = new SQLiteCommand(add,connection))
                    {
                        command.Connection.Open();
                        command.Parameters.AddWithValue("@Name", textBox_name.Text);
                        command.Parameters.AddWithValue("@Brand", textBox_brand.Text);
                        command.Parameters.AddWithValue("@Price", textBox_price.Text);
                        command.Parameters.AddWithValue("@Piece", textBox_piece.Text);
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Database de sorun var");
            }
            
     }





    }
}

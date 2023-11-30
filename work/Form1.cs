using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.Design.Behavior;
using System.Linq;
using System.Xml.Linq;
using System.Data.Common;
using System.Data;
using static System.Net.WebRequestMethods;
using System.Diagnostics.Metrics;
using System.Drawing;
using System;

namespace work
{
    public partial class Form1 : Form
    {
        AutoCompleteTextBox tb = new AutoCompleteTextBox();
        static char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' };
        public static bool isAlphaNumeric(char c)
        {
            foreach (var e in delimiterChars)
            {
                if (c == e)
                    return true;
                return false;
            }
            return false;
        }
        int lastSimbol = 0;
        Dictionary<string, int> words = new Dictionary<string, int>();
        string connect = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=workdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            tb.TextChanged += tb_TextChanged;
            ñîçäàíèåÑëîâàðÿToolStripMenuItem.Click += new EventHandler(creatingDictionaryItem_Click);
            îáíîâëåíèåÑëîâàðÿToolStripMenuItem.Click += new EventHandler(updatingDictionaryItem_Click);
            î÷èñòêàÑëîâàðÿToolStripMenuItem.Click += new EventHandler(deleteDictionaryItem_Click);

            try
            {
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    string sqlCom = "Select * from dictionary";
                    SqlCommand command = new SqlCommand(sqlCom, connection);
                    connection.Open();
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        
                        while (oReader.Read())
                        {
                            if (!words.ContainsKey(oReader["keys"].ToString()))
                                words.Add(oReader["keys"].ToString(), int.Parse(oReader["value"].ToString()));
                            else
                                words[oReader["keys"].ToString()] += 1;
                        }

                        command.Connection = connection;
                        connection.Close();
                    }

                    
                }
                tb.Values = words;
            }
            catch
            {
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb.Location = new Point(10, 50);
            tb.Size = new Size(700, 400);
            tb.Multiline = true;
            Controls.Add(tb);
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            if (tb.TextLength != 0 && isAlphaNumeric(tb.Text[tb.TextLength - 1]) && tb.TextLength - lastSimbol - 1 >= 3)
            {
                var word = tb.Text.Substring(lastSimbol, tb.TextLength - lastSimbol).Trim(delimiterChars);
                if (words.ContainsKey(word))
                {
                    words[word] += 1;
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        SqlCommand command = new SqlCommand();
                        string sqlCom = "UPDATE dictionary SET value = value + 1 WHERE keys = @word";
                        using (command = new SqlCommand(sqlCom, connection))
                        {
                            connection.Open();
                            SqlParameter keyParam = new SqlParameter("@word", word);
                            command.Parameters.Add(keyParam);
                            command.Connection = connection;
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                else
                {
                    words.Add(word, 1);
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        SqlCommand command = new SqlCommand();

                        string sqlExpression = "INSERT INTO dictionary (keys,value) VALUES (@key,1);";
                        using (command = new SqlCommand(sqlExpression, connection))
                        {
                            connection.Open();
                            SqlParameter keyParam = new SqlParameter("@key", word);
                            command.Parameters.Add(keyParam);
                            command.Connection = connection;
                            command.ExecuteNonQuery();
                            connection.Close();
                        }

                    }
                }
                tb.Values = words;
                lastSimbol = tb.TextLength;
            }
        }

        private void creatingDictionaryItem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    var fileContent = string.Empty;
                    var filePath = string.Empty;
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    using (SqlConnection connection = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "CREATE DATABASE workdb";
                        command.Connection = connection;
                        command.ExecuteNonQuery();
                    }
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "CREATE TABLE dictionary (keys NVARCHAR(255),value INT);";
                        command.Connection = connection;
                        command.ExecuteNonQuery();
                    }
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        var text = fileContent.Split(delimiterChars);
                        foreach (var word in text)
                        {
                            using (SqlConnection connection = new SqlConnection(connect))
                            {
                                string sqlCom = "Select * from dictionary";
                                SqlCommand command = new SqlCommand(sqlCom, connection);
                                connection.Open();
                                using (SqlDataReader oReader = command.ExecuteReader())
                                {
                                    
                                    while (oReader.Read())
                                    {
                                        if (!words.ContainsKey(oReader["keys"].ToString()))
                                            words.Add(oReader["keys"].ToString(), int.Parse(oReader["value"].ToString()));
                                        else
                                            words[oReader["keys"].ToString()] += 1;
                                    }

                                    command.Connection = connection;
                                }
                            }
                            if (words.ContainsKey(word) && word.Length >= 3)
                            {
                                using (SqlConnection connection = new SqlConnection(connect))
                                {
                                    SqlCommand command = new SqlCommand();
                                    string sqlCom = "UPDATE dictionary SET value = value + 1 WHERE keys = @word";
                                    using (command = new SqlCommand(sqlCom, connection))
                                    {
                                        connection.Open();
                                        SqlParameter keyParam = new SqlParameter("@word", word);
                                        command.Parameters.Add(keyParam);
                                        command.Connection = connection;
                                        command.ExecuteNonQuery();
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                using (SqlConnection connection = new SqlConnection(connect))
                                {
                                    SqlCommand command = new SqlCommand();

                                    string sqlExpression = "INSERT INTO dictionary (keys,value) VALUES (@key,1);";
                                    using (command = new SqlCommand(sqlExpression, connection))
                                    {
                                        connection.Open();
                                        SqlParameter keyParam = new SqlParameter("@key", word);
                                        command.Parameters.Add(keyParam);
                                        command.Connection = connection;
                                        command.ExecuteNonQuery();
                                        connection.Close();
                                    }

                                }
                            }
                        }
                    }
                    tb.Values = words;
                }

            }
        }
        private void updatingDictionaryItem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    var fileContent = string.Empty;
                    var filePath = string.Empty;
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        var text = fileContent.Split(delimiterChars);
                        foreach (var word in text)
                        {
                            using (SqlConnection connection = new SqlConnection(connect))
                            {
                                string sqlCom = "Select * from dictionary";
                                SqlCommand command = new SqlCommand(sqlCom, connection);
                                connection.Open();
                                using (SqlDataReader oReader = command.ExecuteReader())
                                {

                                    while (oReader.Read())
                                    {
                                        if (!words.ContainsKey(oReader["keys"].ToString()))
                                            words.Add(oReader["keys"].ToString(), int.Parse(oReader["value"].ToString()));
                                        else
                                            words[oReader["keys"].ToString()] += 1;
                                    }

                                    command.Connection = connection;
                                }
                            }
                            if (words.ContainsKey(word) && word.Length >= 3)
                            {
                                using (SqlConnection connection = new SqlConnection(connect))
                                {
                                    SqlCommand command = new SqlCommand();
                                    string sqlCom = "UPDATE dictionary SET value = value + 1 WHERE keys = @word";
                                    using (command = new SqlCommand(sqlCom, connection))
                                    {
                                        connection.Open();
                                        SqlParameter keyParam = new SqlParameter("@word", word);
                                        command.Parameters.Add(keyParam);
                                        command.Connection = connection;
                                        command.ExecuteNonQuery();
                                        connection.Close();

                                    }
                                }
                            }
                            else
                            {
                                using (SqlConnection connection = new SqlConnection(connect))
                                {
                                    SqlCommand command = new SqlCommand();

                                    string sqlExpression = "INSERT INTO dictionary (keys,value) VALUES (@key,1);";
                                    using (command = new SqlCommand(sqlExpression, connection))
                                    {
                                        connection.Open();
                                        SqlParameter keyParam = new SqlParameter("@key", word);
                                        command.Parameters.Add(keyParam);
                                        command.Connection = connection;
                                        command.ExecuteNonQuery();
                                        connection.Close();
                                    }

                                }
                            }
                        }
                    }
                    tb.Values = words;
                }

            }
        }
        private void deleteDictionaryItem_Click(object? sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                SqlCommand command = new SqlCommand();

                string sqlExpression = "DELETE FROM dictionary;";
                using (command = new SqlCommand(sqlExpression, connection))
                {
                    connection.Open();

                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                    
                
            }
            words.Clear();
        }

    }
}
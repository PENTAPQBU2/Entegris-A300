using HMI_Master_KX7500.MachinePath;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Modules.UserAccount
{
    /// <summary>
    /// Interaction logic for UserAccountAccessView.xaml
    /// </summary>
    public partial class UserAccountAccessView : UserControl
    {
        #region PrivateFields

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion PrivateFields

        #region PublicFields

        public static OleDbConnection gl_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FolderPath.MachineSettings + @"\UserAccountSetting.mdb");

        #endregion PublicFields

        #region Constructor

        public UserAccountAccessView()
        {
            InitializeComponent();
            initializeUserAccountContent();
            displayUser();
        }

        #endregion Constructor

        private void initializeUserAccountContent()
        {
            InitializeDataGridUserAccount();
        }

        private void InitializeDataGridUserAccount()
        {
            try
            {
                Reconnectgl_conn();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM UserAccountModel", gl_conn);
                OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
                DataTable dt = new DataTable("UserAccountModel");
                adapter.Fill(dt);
                DataGridUserAccount.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        //private void initializeUserAccountContent()
        //{
        //    try
        //    {
        //        if(gl_conn.State.Equals(ConnectionState.Open))
        //        {
        //            gl_conn.Close();
        //        }
        //        gl_conn.Open();
        //        OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM UserAccountModel", gl_conn);
        //        OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
        //        DataTable dt = new DataTable("UserAccountModel");
        //        adapter.Fill(dt);
        //        DataGridUserAccount.ItemsSource = dt.DefaultView;
        //    }
        //    catch(Exception ex) { logger.Error(ex); }
        //}

        private static void Reconnectgl_conn()
        {
            if (gl_conn.State.Equals(ConnectionState.Open))
            {
                gl_conn.Close();
            }
            gl_conn.Open();
        }

        private void displayUser()
        {
            try
            {
                //cont here
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        private void SaveButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                Dictionary<int, bool> idCheck = new Dictionary<int, bool>();
                Dictionary<string, bool> userNameCheck = new Dictionary<string, bool>();
                foreach (object item in DataGridUserAccount.Items)
                {
                    if (item is DataRowView drv)
                    {
                        if (drv[0] is DBNull || drv[1] is DBNull || drv[2] is DBNull || drv[3] is DBNull || (string)drv[1] == "" || (string)drv[2] == "" || (string)drv[3] == "")
                        {
                            MessageBox.Show("Item cannot be null.");
                            throw new ArgumentNullException();
                        }
                        if (idCheck.ContainsKey((int)drv[0]))
                        {
                            MessageBox.Show("Id must be unique");
                            throw new ArgumentNullException();
                        }
                        idCheck[(int)drv[0]] = true;
                        if (userNameCheck.ContainsKey((string)drv[1]))
                        {
                            MessageBox.Show("User name must be unique");
                            throw new ArgumentNullException();
                        }
                    }
                }

                Reconnectgl_conn();
                OleDbCommand dropCommand = new OleDbCommand("DELETE FROM UserAccountModel");
                dropCommand.Connection = gl_conn;
                dropCommand.ExecuteNonQuery();
                foreach (object item in DataGridUserAccount.Items)
                {
                    if (item is DataRowView drv)
                    {
                        OleDbCommand insertCommand = new OleDbCommand($"INSERT INTO UserAccountModel VALUES({drv[0]}, '{drv[1]}', '{drv[2]}', '{drv[3]}')");
                        insertCommand.Connection = gl_conn;
                        insertCommand.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Save Done");

            }

            catch (Exception ex) { logger.Error(ex); }
        }

        private void RestoreButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            InitializeDataGridUserAccount();
        }
    }
}
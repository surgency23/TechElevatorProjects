
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Web.Models;


namespace Capstone.Web.Tests.DAL
{
    [TestClass()]
    public class UserDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source =.\sqlexpress;Initial Catalog = Potholes; Integrated Security = True";
        private int userID;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand(@"
                     INSERT INTO [dbo].[Users]
                     ([Username]
                     ,[Password]
                     ,[FirstName]
                     ,[LastName]
                     ,[Is_Employee]
                     ,[Email])
                        VALUES
                    ('testUser',
                    'Password123',
                    'FirstName',
                    'LastName',
                    1,
                    'TestEmail@test.com')", conn);
                userID = cmd.ExecuteNonQuery();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void FindingTestUser()
        {
            UserSQLDAL userDAL = new UserSQLDAL(connectionString);
            Users user = new Users
            {
                Username = "testUser",
                Password = "Password123",
                FirstName = "Test",
                LastName = "LastName",
                Is_Employee = 1,
                Email = "TestEmail@test.com"

            };
            Assert.AreEqual(true, userDAL.CreateUser(user));
        }

        [TestMethod]
        public void TestingChangePassword()
        {
            UserSQLDAL userDAL = new UserSQLDAL(connectionString);
            Users user = new Users
            {
                Username = "testUser",
                Password = "Password123",
                FirstName = "Test",
                LastName = "LastName",
                Is_Employee = 1,
                Email = "TestEmail@test.com"
            };
            userDAL.ChangePassword("testUser", "NewPassword");

            Assert.AreEqual("NewPassword", user.Password);
        }
    }
}


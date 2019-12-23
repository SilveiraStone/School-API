﻿using Dapper;
using School.Core.Repositories;
using StoneCo.Buy4.School.DataContracts.Subscription.EnrolledStudents;
using StoneCo.Buy4.School.DataContracts.Subscription.InformationsSubscription;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace School.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly string _connectionString;

        public SubscriptionRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IEnumerable<InfomationsViewData> InformationsView()
        {
            const string sql = @"SELECT * FROM SubscriptionsInformations";

            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                List<InfomationsViewData> informationView = sqlConnection.Query<InfomationsViewData>(sql).ToList();

                return informationView;
            }
        }

        public IEnumerable<EnrolledStudentsViewData> EnrolledStudentsView()
        {
            const string sql = @"SELECT * FROM Enrolled_Students";

            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                List<EnrolledStudentsViewData> enrolledStudents = sqlConnection.Query<EnrolledStudentsViewData>(sql).ToList();

                return enrolledStudents;
            }
        }

        public bool ExistByClassId(byte classId)
        {
            const string sql = @"SELECT
                1
            FROM
                dbo.Class
            WHERE
                ClassId = @ClassId;";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("ClassId",classId,DbType.Byte);

            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                bool exist = sqlConnection.ExecuteScalar<bool>(sql,parameter);

                return exist;
            }
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(this._connectionString);
        }
    }
}
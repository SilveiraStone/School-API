﻿using Dapper;
using Microsoft.Extensions.Configuration;
using School.Core.Models;
using School.Core.Repositories;
using StoneCo.Buy4.School.DataContracts;
using StoneCo.Buy4.School.DataContracts.DeleteTeacher;
using StoneCo.Buy4.School.DataContracts.InsertTeacher;
using StoneCo.Buy4.School.DataContracts.UpdateTeacher;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace School.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public Teacher Get(long cpf)
        {
            //Quando tem a mais, não tem problema
            //Quando tem a menos, a coluna que a aplicação faz referência pode ter sido deletada e a aplicação vai quebrar
            string sql = @"
                SELECT 
	                TeacherId,
	                Name,
	                Gender,
	                LevelId,
	                Salary,
	                AdmitionDate
                FROM 
	                dbo.Teacher
                WHERE
	                TeacherId = @Cpf";

            //Para passar um parâmetro na query
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Cpf", cpf, DbType.Int64);

            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString)) 
            {
                sqlConnection.Open();

                return sqlConnection.QuerySingleOrDefault<Core.Models.Teacher>(sql, parameters);
            }
        }

        //Criar novo ListAll para ser paginável e poder aplicar filtro
        public IEnumerable<Teacher> ListAll()
        {
            string sql = @"
                SELECT 
	                TeacherId,
	                Name,
	                Gender,
	                LevelId,
	                Salary,
	                AdmitionDate
                FROM 
	                dbo.Teacher";

            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<Teacher>(sql);
            }
        }

        public void Delete(long cpf)
        {
            string sqlDelete = @"
                DELETE
                FROM 
                	dbo.Teacher
                WHERE
                	TeacherId = @Cpf";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Cpf", cpf, DbType.Int64);

            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
            {
                sqlConnection.Open();

                sqlConnection.QuerySingleOrDefault<Teacher>(sqlDelete, parameters);
            }
        }

        public void Insert(Teacher teacherToInsert)
        {
            string sql = @"
                INSERT INTO dbo.Teacher
                (
                	TeacherId,
                	Name,
                	Gender,
                	LevelId,
                	Salary,
                	AdmitionDate
                )
                VALUES
                (
                	@TeacherId,
                	@Name,
                	@Gender,
                	@LevelId,
                	@Salary,
                	@AdmitionDate
                )
            ";
            
            DynamicParameters parameters = new DynamicParameters();

            parameters.AddDynamicParams(teacherToInsert);
            
            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString)) 
            {
                sqlConnection.Open();
                
                sqlConnection.Query(sql, parameters);
            }
        }
        
        public void Update(Teacher requestData)
        {
            string sqlUpdate = @"
                UPDATE dbo.Teacher
                SET Name = @Name,
                    Gender = @Gender,
                    LevelId = @LevelId,
                    Salary = @Salary,
                    AdmitionDate = @AdmitionDate
                WHERE TeacherId = @TeacherId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(requestData);

            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
            {
                sqlConnection.Open();
                
                sqlConnection.QueryFirstOrDefault(sqlUpdate, parameters);
            }
        }
    }
}

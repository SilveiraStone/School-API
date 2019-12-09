﻿using System;
using System.Collections.Generic;
using System.Text;
using School.Core.Models;
using School.Core.Validators.DataBaseValidator;
using School.Core.Validators.Page;
using School.Core.Validators.ValidateTeacherParameters;
using StoneCo.Buy4.School.DataContracts;
using StoneCo.Buy4.School.DataContracts.SearchTeacher;

namespace School.Core.Validators.SearchTeacher
{
    public class SearchTeacherValidator : ISearchTeacherValidator
    {
        private readonly ITeacherParametersValidator _parameterValidator;
        private readonly IDataBaseValidator _dataBaseValidator;
        private readonly IPageValidator _pageValidator;

        public SearchTeacherValidator(ITeacherParametersValidator parameterValidator, IDataBaseValidator dataBaseValidator, IPageValidator pageValidator)
        {
            this._parameterValidator = parameterValidator;
            this._dataBaseValidator = dataBaseValidator;
            this._pageValidator = pageValidator;
        }


        public SearchTeacherResponse ValidateParameters(SearchTeacherRequestData requestData)
        {
            SearchTeacherResponse response = new SearchTeacherResponse
            { 
                Notifications = new List<OperationNotification>(),
                Errors = new List<OperationError>()
            };

            int count = 0;

            foreach(char? gender in requestData.Gender)
            {
                if(this._parameterValidator.ValidateGender(gender) == false)
                {
                    count += 1;
                }
            }

            if(count > 0)
            {
                response.Errors.Add(new OperationError("017", $"{count} values in {nameof(requestData.Gender)} list invalid "));
                count = 0;
            }

            foreach (char? level in requestData.Level)
            {
                if (this._parameterValidator.ValidateGender(level) == false)
                {
                    count += 1;
                }
            }
                        
            if (count > 0)
            {
                response.Errors.Add(new OperationError("017", $"{count} values in {nameof(requestData.Level)} list invalid "));
                count = 0;
            }

            if (requestData.MinAdmitionDate != null)
            {
                this._parameterValidator.ValidateAdmitionDate(requestData.MinAdmitionDate, response);
            }

            if (requestData.MaxAdmitionDate != null)
            {
                this._parameterValidator.ValidateAdmitionDate(requestData.MaxAdmitionDate, response);
            }

            if (requestData.MaxSalary != null) 
            { 
                this._parameterValidator.ValidateMinMaxSalary(requestData.MaxSalary, ModelConstants.Teacher.MinSalary, ModelConstants.Teacher.MaxSalary, response);
            }

            if (requestData.MinSalary != null)
            {
                this._parameterValidator.ValidateMinMaxSalary(requestData.MinSalary, ModelConstants.Teacher.MinSalary, ModelConstants.Teacher.MaxSalary, response);
            }

            //refinar começando aqui
            //Atual --> trata o erro de buscar um dado em uma página vazia, tendo como base todos os dados
            //Ideal --> tratar o erro de buscar um dado em uma página vazia, tendo como base a quantidade de valores encontrados
            //Testar --> assim que achar os valores desejados, contar quantos vieram e depois fazer a verificação da página
            long maxElements = this._dataBaseValidator.NumberOfElements();

            long offset = (requestData.PageNumber.Value - 1) *(requestData.PageSize.Value);

            if (requestData.PageSize > ModelConstants.Teacher.MaxTeachersPerPage)
            {
                response.Errors.Add(new OperationError("015", "Number of teachers exceeded the limit"));
            }

            if (offset >= maxElements)
            {
                response.Errors.Add(new OperationError("016", "Values can't be find! Incorrect search local"));
            }
            //terminando aqui

            return null;
        }
        
        public void ValidatePage(long maxElements, long elementsPerPage, long pageNumber, SearchTeacherResponse response)
        {
            this._pageValidator.ValidatePage(elementsPerPage, (pageNumber - 1)*elementsPerPage, maxElements, response);
        }
    }
}

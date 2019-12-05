﻿using School.Core.Mapping;
using School.Core.Models;
using School.Core.Repositories;
using School.Core.Validators.GetTeachersPerPage;
using StoneCo.Buy4.School.DataContracts.GetTeacherPerPage;
using System.Collections.Generic;

namespace School.Core.Operations
{
    public class GetTeachersPerPage : IGetTeachersPerPage
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISchoolMappingResolver _mappingResolver;
        private readonly IGetTeachersPerPageValidator _validator;

        public GetTeachersPerPage(ITeacherRepository teacherRepository, ISchoolMappingResolver mappingResolver, IGetTeachersPerPageValidator validator)
        {
            this._teacherRepository = teacherRepository;
            this._mappingResolver = mappingResolver;
            this._validator = validator;
        }

        public GetTeachersPerPageResponse ProcessOperation(GetTeachersPerPageRequest request)
        {
            GetTeachersPerPageResponse response = new GetTeachersPerPageResponse();

            this._validator.NumberOfElementsValiator(request.Data.PageNumber, request.Data.TeachersPerPage, response);

            if (response.Success == false)
            {
                return response;
            }

            List<Teacher> teacher = _teacherRepository.GetPerPage(request.Data);

            response.Data = this._mappingResolver.BuildFrom(teacher);

            return response;
        }
    }
}
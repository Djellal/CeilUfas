using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CeilUfas.Data;

namespace CeilUfas.Controllers
{
    public partial class ExportceilufasController : ExportController
    {
        private readonly ceilufasContext context;
        private readonly ceilufasService service;

        public ExportceilufasController(ceilufasContext context, ceilufasService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ceilufas/appsettings/csv")]
        [HttpGet("/export/ceilufas/appsettings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAppSettingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAppSettings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/appsettings/excel")]
        [HttpGet("/export/ceilufas/appsettings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAppSettingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAppSettings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courselevels/csv")]
        [HttpGet("/export/ceilufas/courselevels/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseLevelsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourseLevels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courselevels/excel")]
        [HttpGet("/export/ceilufas/courselevels/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseLevelsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourseLevels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courseregistrations/csv")]
        [HttpGet("/export/ceilufas/courseregistrations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseRegistrationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourseRegistrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courseregistrations/excel")]
        [HttpGet("/export/ceilufas/courseregistrations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseRegistrationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourseRegistrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courses/csv")]
        [HttpGet("/export/ceilufas/courses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCoursesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/courses/excel")]
        [HttpGet("/export/ceilufas/courses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCoursesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/coursetypes/csv")]
        [HttpGet("/export/ceilufas/coursetypes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourseTypes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/coursetypes/excel")]
        [HttpGet("/export/ceilufas/coursetypes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCourseTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourseTypes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/municipalities/csv")]
        [HttpGet("/export/ceilufas/municipalities/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMunicipalitiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMunicipalities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/municipalities/excel")]
        [HttpGet("/export/ceilufas/municipalities/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMunicipalitiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMunicipalities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/professions/csv")]
        [HttpGet("/export/ceilufas/professions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProfessionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProfessions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/professions/excel")]
        [HttpGet("/export/ceilufas/professions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProfessionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProfessions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/sessions/csv")]
        [HttpGet("/export/ceilufas/sessions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSessionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSessions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/sessions/excel")]
        [HttpGet("/export/ceilufas/sessions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSessionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSessions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/states/csv")]
        [HttpGet("/export/ceilufas/states/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ceilufas/states/excel")]
        [HttpGet("/export/ceilufas/states/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStates(), Request.Query, false), fileName);
        }
    }
}

using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using CeilUfas.Data;

namespace CeilUfas
{
    public partial class ceilufasService
    {
        ceilufasContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ceilufasContext context;
        private readonly NavigationManager navigationManager;

        public ceilufasService(ceilufasContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAppSettingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/appsettings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/appsettings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAppSettingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/appsettings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/appsettings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAppSettingsRead(ref IQueryable<CeilUfas.Models.ceilufas.AppSetting> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.AppSetting>> GetAppSettings(Query query = null)
        {
            var items = Context.AppSettings.AsQueryable();

            items = items.Include(i => i.Session);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAppSettingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAppSettingGet(CeilUfas.Models.ceilufas.AppSetting item);
        partial void OnGetAppSettingById(ref IQueryable<CeilUfas.Models.ceilufas.AppSetting> items);


        public async Task<CeilUfas.Models.ceilufas.AppSetting> GetAppSettingById(int id)
        {
            var items = Context.AppSettings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Session);
 
            OnGetAppSettingById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAppSettingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAppSettingCreated(CeilUfas.Models.ceilufas.AppSetting item);
        partial void OnAfterAppSettingCreated(CeilUfas.Models.ceilufas.AppSetting item);

        public async Task<CeilUfas.Models.ceilufas.AppSetting> CreateAppSetting(CeilUfas.Models.ceilufas.AppSetting appsetting)
        {
            OnAppSettingCreated(appsetting);

            var existingItem = Context.AppSettings
                              .Where(i => i.Id == appsetting.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AppSettings.Add(appsetting);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(appsetting).State = EntityState.Detached;
                throw;
            }

            OnAfterAppSettingCreated(appsetting);

            return appsetting;
        }

        public async Task<CeilUfas.Models.ceilufas.AppSetting> CancelAppSettingChanges(CeilUfas.Models.ceilufas.AppSetting item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAppSettingUpdated(CeilUfas.Models.ceilufas.AppSetting item);
        partial void OnAfterAppSettingUpdated(CeilUfas.Models.ceilufas.AppSetting item);

        public async Task<CeilUfas.Models.ceilufas.AppSetting> UpdateAppSetting(int id, CeilUfas.Models.ceilufas.AppSetting appsetting)
        {
            OnAppSettingUpdated(appsetting);

            var itemToUpdate = Context.AppSettings
                              .Where(i => i.Id == appsetting.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(appsetting);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAppSettingUpdated(appsetting);

            return appsetting;
        }

        partial void OnAppSettingDeleted(CeilUfas.Models.ceilufas.AppSetting item);
        partial void OnAfterAppSettingDeleted(CeilUfas.Models.ceilufas.AppSetting item);

        public async Task<CeilUfas.Models.ceilufas.AppSetting> DeleteAppSetting(int id)
        {
            var itemToDelete = Context.AppSettings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAppSettingDeleted(itemToDelete);


            Context.AppSettings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAppSettingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCourseLevelsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courselevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courselevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCourseLevelsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courselevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courselevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCourseLevelsRead(ref IQueryable<CeilUfas.Models.ceilufas.CourseLevel> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.CourseLevel>> GetCourseLevels(Query query = null)
        {
            var items = Context.CourseLevels.AsQueryable();

            items = items.Include(i => i.Course);
            items = items.Include(i => i.CourseLevel1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCourseLevelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseLevelGet(CeilUfas.Models.ceilufas.CourseLevel item);
        partial void OnGetCourseLevelById(ref IQueryable<CeilUfas.Models.ceilufas.CourseLevel> items);


        public async Task<CeilUfas.Models.ceilufas.CourseLevel> GetCourseLevelById(int id)
        {
            var items = Context.CourseLevels
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Course);
            items = items.Include(i => i.CourseLevel1);
 
            OnGetCourseLevelById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCourseLevelGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCourseLevelCreated(CeilUfas.Models.ceilufas.CourseLevel item);
        partial void OnAfterCourseLevelCreated(CeilUfas.Models.ceilufas.CourseLevel item);

        public async Task<CeilUfas.Models.ceilufas.CourseLevel> CreateCourseLevel(CeilUfas.Models.ceilufas.CourseLevel courselevel)
        {
            OnCourseLevelCreated(courselevel);

            var existingItem = Context.CourseLevels
                              .Where(i => i.Id == courselevel.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CourseLevels.Add(courselevel);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(courselevel).State = EntityState.Detached;
                throw;
            }

            OnAfterCourseLevelCreated(courselevel);

            return courselevel;
        }

        public async Task<CeilUfas.Models.ceilufas.CourseLevel> CancelCourseLevelChanges(CeilUfas.Models.ceilufas.CourseLevel item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseLevelUpdated(CeilUfas.Models.ceilufas.CourseLevel item);
        partial void OnAfterCourseLevelUpdated(CeilUfas.Models.ceilufas.CourseLevel item);

        public async Task<CeilUfas.Models.ceilufas.CourseLevel> UpdateCourseLevel(int id, CeilUfas.Models.ceilufas.CourseLevel courselevel)
        {
            OnCourseLevelUpdated(courselevel);

            var itemToUpdate = Context.CourseLevels
                              .Where(i => i.Id == courselevel.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(courselevel);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCourseLevelUpdated(courselevel);

            return courselevel;
        }

        partial void OnCourseLevelDeleted(CeilUfas.Models.ceilufas.CourseLevel item);
        partial void OnAfterCourseLevelDeleted(CeilUfas.Models.ceilufas.CourseLevel item);

        public async Task<CeilUfas.Models.ceilufas.CourseLevel> DeleteCourseLevel(int id)
        {
            var itemToDelete = Context.CourseLevels
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .Include(i => i.CourseLevels1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseLevelDeleted(itemToDelete);


            Context.CourseLevels.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseLevelDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCourseRegistrationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courseregistrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courseregistrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCourseRegistrationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courseregistrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courseregistrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCourseRegistrationsRead(ref IQueryable<CeilUfas.Models.ceilufas.CourseRegistration> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.CourseRegistration>> GetCourseRegistrations(Query query = null)
        {
            var items = Context.CourseRegistrations.AsQueryable();

            items = items.Include(i => i.Municipality);
            items = items.Include(i => i.State);
            items = items.Include(i => i.Course);
            items = items.Include(i => i.CourseLevel);
            items = items.Include(i => i.Profession);
            items = items.Include(i => i.Session);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCourseRegistrationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseRegistrationGet(CeilUfas.Models.ceilufas.CourseRegistration item);
        partial void OnGetCourseRegistrationById(ref IQueryable<CeilUfas.Models.ceilufas.CourseRegistration> items);


        public async Task<CeilUfas.Models.ceilufas.CourseRegistration> GetCourseRegistrationById(int id)
        {
            var items = Context.CourseRegistrations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Municipality);
            items = items.Include(i => i.State);
            items = items.Include(i => i.Course);
            items = items.Include(i => i.CourseLevel);
            items = items.Include(i => i.Profession);
            items = items.Include(i => i.Session);
 
            OnGetCourseRegistrationById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCourseRegistrationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCourseRegistrationCreated(CeilUfas.Models.ceilufas.CourseRegistration item);
        partial void OnAfterCourseRegistrationCreated(CeilUfas.Models.ceilufas.CourseRegistration item);

        public async Task<CeilUfas.Models.ceilufas.CourseRegistration> CreateCourseRegistration(CeilUfas.Models.ceilufas.CourseRegistration courseregistration)
        {
            OnCourseRegistrationCreated(courseregistration);

            var existingItem = Context.CourseRegistrations
                              .Where(i => i.Id == courseregistration.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CourseRegistrations.Add(courseregistration);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(courseregistration).State = EntityState.Detached;
                throw;
            }

            OnAfterCourseRegistrationCreated(courseregistration);

            return courseregistration;
        }

        public async Task<CeilUfas.Models.ceilufas.CourseRegistration> CancelCourseRegistrationChanges(CeilUfas.Models.ceilufas.CourseRegistration item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseRegistrationUpdated(CeilUfas.Models.ceilufas.CourseRegistration item);
        partial void OnAfterCourseRegistrationUpdated(CeilUfas.Models.ceilufas.CourseRegistration item);

        public async Task<CeilUfas.Models.ceilufas.CourseRegistration> UpdateCourseRegistration(int id, CeilUfas.Models.ceilufas.CourseRegistration courseregistration)
        {
            OnCourseRegistrationUpdated(courseregistration);

            var itemToUpdate = Context.CourseRegistrations
                              .Where(i => i.Id == courseregistration.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(courseregistration);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCourseRegistrationUpdated(courseregistration);

            return courseregistration;
        }

        partial void OnCourseRegistrationDeleted(CeilUfas.Models.ceilufas.CourseRegistration item);
        partial void OnAfterCourseRegistrationDeleted(CeilUfas.Models.ceilufas.CourseRegistration item);

        public async Task<CeilUfas.Models.ceilufas.CourseRegistration> DeleteCourseRegistration(int id)
        {
            var itemToDelete = Context.CourseRegistrations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseRegistrationDeleted(itemToDelete);


            Context.CourseRegistrations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseRegistrationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCoursesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCoursesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/courses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/courses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCoursesRead(ref IQueryable<CeilUfas.Models.ceilufas.Course> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.Course>> GetCourses(Query query = null)
        {
            var items = Context.Courses.AsQueryable();

            items = items.Include(i => i.CourseType);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCoursesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseGet(CeilUfas.Models.ceilufas.Course item);
        partial void OnGetCourseById(ref IQueryable<CeilUfas.Models.ceilufas.Course> items);


        public async Task<CeilUfas.Models.ceilufas.Course> GetCourseById(int id)
        {
            var items = Context.Courses
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.CourseType);
 
            OnGetCourseById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCourseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCourseCreated(CeilUfas.Models.ceilufas.Course item);
        partial void OnAfterCourseCreated(CeilUfas.Models.ceilufas.Course item);

        public async Task<CeilUfas.Models.ceilufas.Course> CreateCourse(CeilUfas.Models.ceilufas.Course course)
        {
            OnCourseCreated(course);

            var existingItem = Context.Courses
                              .Where(i => i.Id == course.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Courses.Add(course);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(course).State = EntityState.Detached;
                throw;
            }

            OnAfterCourseCreated(course);

            return course;
        }

        public async Task<CeilUfas.Models.ceilufas.Course> CancelCourseChanges(CeilUfas.Models.ceilufas.Course item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseUpdated(CeilUfas.Models.ceilufas.Course item);
        partial void OnAfterCourseUpdated(CeilUfas.Models.ceilufas.Course item);

        public async Task<CeilUfas.Models.ceilufas.Course> UpdateCourse(int id, CeilUfas.Models.ceilufas.Course course)
        {
            OnCourseUpdated(course);

            var itemToUpdate = Context.Courses
                              .Where(i => i.Id == course.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(course);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCourseUpdated(course);

            return course;
        }

        partial void OnCourseDeleted(CeilUfas.Models.ceilufas.Course item);
        partial void OnAfterCourseDeleted(CeilUfas.Models.ceilufas.Course item);

        public async Task<CeilUfas.Models.ceilufas.Course> DeleteCourse(int id)
        {
            var itemToDelete = Context.Courses
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .Include(i => i.CourseLevels)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseDeleted(itemToDelete);


            Context.Courses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCourseTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/coursetypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/coursetypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCourseTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/coursetypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/coursetypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCourseTypesRead(ref IQueryable<CeilUfas.Models.ceilufas.CourseType> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.CourseType>> GetCourseTypes(Query query = null)
        {
            var items = Context.CourseTypes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCourseTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseTypeGet(CeilUfas.Models.ceilufas.CourseType item);
        partial void OnGetCourseTypeById(ref IQueryable<CeilUfas.Models.ceilufas.CourseType> items);


        public async Task<CeilUfas.Models.ceilufas.CourseType> GetCourseTypeById(int id)
        {
            var items = Context.CourseTypes
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetCourseTypeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCourseTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCourseTypeCreated(CeilUfas.Models.ceilufas.CourseType item);
        partial void OnAfterCourseTypeCreated(CeilUfas.Models.ceilufas.CourseType item);

        public async Task<CeilUfas.Models.ceilufas.CourseType> CreateCourseType(CeilUfas.Models.ceilufas.CourseType coursetype)
        {
            OnCourseTypeCreated(coursetype);

            var existingItem = Context.CourseTypes
                              .Where(i => i.Id == coursetype.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CourseTypes.Add(coursetype);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(coursetype).State = EntityState.Detached;
                throw;
            }

            OnAfterCourseTypeCreated(coursetype);

            return coursetype;
        }

        public async Task<CeilUfas.Models.ceilufas.CourseType> CancelCourseTypeChanges(CeilUfas.Models.ceilufas.CourseType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseTypeUpdated(CeilUfas.Models.ceilufas.CourseType item);
        partial void OnAfterCourseTypeUpdated(CeilUfas.Models.ceilufas.CourseType item);

        public async Task<CeilUfas.Models.ceilufas.CourseType> UpdateCourseType(int id, CeilUfas.Models.ceilufas.CourseType coursetype)
        {
            OnCourseTypeUpdated(coursetype);

            var itemToUpdate = Context.CourseTypes
                              .Where(i => i.Id == coursetype.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(coursetype);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCourseTypeUpdated(coursetype);

            return coursetype;
        }

        partial void OnCourseTypeDeleted(CeilUfas.Models.ceilufas.CourseType item);
        partial void OnAfterCourseTypeDeleted(CeilUfas.Models.ceilufas.CourseType item);

        public async Task<CeilUfas.Models.ceilufas.CourseType> DeleteCourseType(int id)
        {
            var itemToDelete = Context.CourseTypes
                              .Where(i => i.Id == id)
                              .Include(i => i.Courses)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseTypeDeleted(itemToDelete);


            Context.CourseTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseTypeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMunicipalitiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/municipalities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/municipalities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMunicipalitiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/municipalities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/municipalities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMunicipalitiesRead(ref IQueryable<CeilUfas.Models.ceilufas.Municipality> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.Municipality>> GetMunicipalities(Query query = null)
        {
            var items = Context.Municipalities.AsQueryable();

            items = items.Include(i => i.State);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMunicipalitiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMunicipalityGet(CeilUfas.Models.ceilufas.Municipality item);
        partial void OnGetMunicipalityById(ref IQueryable<CeilUfas.Models.ceilufas.Municipality> items);


        public async Task<CeilUfas.Models.ceilufas.Municipality> GetMunicipalityById(int id)
        {
            var items = Context.Municipalities
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.State);
 
            OnGetMunicipalityById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMunicipalityGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMunicipalityCreated(CeilUfas.Models.ceilufas.Municipality item);
        partial void OnAfterMunicipalityCreated(CeilUfas.Models.ceilufas.Municipality item);

        public async Task<CeilUfas.Models.ceilufas.Municipality> CreateMunicipality(CeilUfas.Models.ceilufas.Municipality municipality)
        {
            OnMunicipalityCreated(municipality);

            var existingItem = Context.Municipalities
                              .Where(i => i.Id == municipality.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Municipalities.Add(municipality);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(municipality).State = EntityState.Detached;
                throw;
            }

            OnAfterMunicipalityCreated(municipality);

            return municipality;
        }

        public async Task<CeilUfas.Models.ceilufas.Municipality> CancelMunicipalityChanges(CeilUfas.Models.ceilufas.Municipality item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMunicipalityUpdated(CeilUfas.Models.ceilufas.Municipality item);
        partial void OnAfterMunicipalityUpdated(CeilUfas.Models.ceilufas.Municipality item);

        public async Task<CeilUfas.Models.ceilufas.Municipality> UpdateMunicipality(int id, CeilUfas.Models.ceilufas.Municipality municipality)
        {
            OnMunicipalityUpdated(municipality);

            var itemToUpdate = Context.Municipalities
                              .Where(i => i.Id == municipality.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(municipality);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMunicipalityUpdated(municipality);

            return municipality;
        }

        partial void OnMunicipalityDeleted(CeilUfas.Models.ceilufas.Municipality item);
        partial void OnAfterMunicipalityDeleted(CeilUfas.Models.ceilufas.Municipality item);

        public async Task<CeilUfas.Models.ceilufas.Municipality> DeleteMunicipality(int id)
        {
            var itemToDelete = Context.Municipalities
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMunicipalityDeleted(itemToDelete);


            Context.Municipalities.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMunicipalityDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProfessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/professions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/professions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProfessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/professions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/professions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProfessionsRead(ref IQueryable<CeilUfas.Models.ceilufas.Profession> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.Profession>> GetProfessions(Query query = null)
        {
            var items = Context.Professions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProfessionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProfessionGet(CeilUfas.Models.ceilufas.Profession item);
        partial void OnGetProfessionById(ref IQueryable<CeilUfas.Models.ceilufas.Profession> items);


        public async Task<CeilUfas.Models.ceilufas.Profession> GetProfessionById(int id)
        {
            var items = Context.Professions
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetProfessionById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProfessionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProfessionCreated(CeilUfas.Models.ceilufas.Profession item);
        partial void OnAfterProfessionCreated(CeilUfas.Models.ceilufas.Profession item);

        public async Task<CeilUfas.Models.ceilufas.Profession> CreateProfession(CeilUfas.Models.ceilufas.Profession profession)
        {
            OnProfessionCreated(profession);

            var existingItem = Context.Professions
                              .Where(i => i.Id == profession.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Professions.Add(profession);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(profession).State = EntityState.Detached;
                throw;
            }

            OnAfterProfessionCreated(profession);

            return profession;
        }

        public async Task<CeilUfas.Models.ceilufas.Profession> CancelProfessionChanges(CeilUfas.Models.ceilufas.Profession item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProfessionUpdated(CeilUfas.Models.ceilufas.Profession item);
        partial void OnAfterProfessionUpdated(CeilUfas.Models.ceilufas.Profession item);

        public async Task<CeilUfas.Models.ceilufas.Profession> UpdateProfession(int id, CeilUfas.Models.ceilufas.Profession profession)
        {
            OnProfessionUpdated(profession);

            var itemToUpdate = Context.Professions
                              .Where(i => i.Id == profession.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(profession);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProfessionUpdated(profession);

            return profession;
        }

        partial void OnProfessionDeleted(CeilUfas.Models.ceilufas.Profession item);
        partial void OnAfterProfessionDeleted(CeilUfas.Models.ceilufas.Profession item);

        public async Task<CeilUfas.Models.ceilufas.Profession> DeleteProfession(int id)
        {
            var itemToDelete = Context.Professions
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProfessionDeleted(itemToDelete);


            Context.Professions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProfessionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSessionsRead(ref IQueryable<CeilUfas.Models.ceilufas.Session> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.Session>> GetSessions(Query query = null)
        {
            var items = Context.Sessions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSessionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSessionGet(CeilUfas.Models.ceilufas.Session item);
        partial void OnGetSessionById(ref IQueryable<CeilUfas.Models.ceilufas.Session> items);


        public async Task<CeilUfas.Models.ceilufas.Session> GetSessionById(int id)
        {
            var items = Context.Sessions
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetSessionById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSessionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSessionCreated(CeilUfas.Models.ceilufas.Session item);
        partial void OnAfterSessionCreated(CeilUfas.Models.ceilufas.Session item);

        public async Task<CeilUfas.Models.ceilufas.Session> CreateSession(CeilUfas.Models.ceilufas.Session session)
        {
            OnSessionCreated(session);

            var existingItem = Context.Sessions
                              .Where(i => i.Id == session.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Sessions.Add(session);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(session).State = EntityState.Detached;
                throw;
            }

            OnAfterSessionCreated(session);

            return session;
        }

        public async Task<CeilUfas.Models.ceilufas.Session> CancelSessionChanges(CeilUfas.Models.ceilufas.Session item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSessionUpdated(CeilUfas.Models.ceilufas.Session item);
        partial void OnAfterSessionUpdated(CeilUfas.Models.ceilufas.Session item);

        public async Task<CeilUfas.Models.ceilufas.Session> UpdateSession(int id, CeilUfas.Models.ceilufas.Session session)
        {
            OnSessionUpdated(session);

            var itemToUpdate = Context.Sessions
                              .Where(i => i.Id == session.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(session);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSessionUpdated(session);

            return session;
        }

        partial void OnSessionDeleted(CeilUfas.Models.ceilufas.Session item);
        partial void OnAfterSessionDeleted(CeilUfas.Models.ceilufas.Session item);

        public async Task<CeilUfas.Models.ceilufas.Session> DeleteSession(int id)
        {
            var itemToDelete = Context.Sessions
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .Include(i => i.AppSettings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSessionDeleted(itemToDelete);


            Context.Sessions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSessionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/ceilufas/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/ceilufas/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStatesRead(ref IQueryable<CeilUfas.Models.ceilufas.State> items);

        public async Task<IQueryable<CeilUfas.Models.ceilufas.State>> GetStates(Query query = null)
        {
            var items = Context.States.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStateGet(CeilUfas.Models.ceilufas.State item);
        partial void OnGetStateById(ref IQueryable<CeilUfas.Models.ceilufas.State> items);


        public async Task<CeilUfas.Models.ceilufas.State> GetStateById(string id)
        {
            var items = Context.States
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStateById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStateCreated(CeilUfas.Models.ceilufas.State item);
        partial void OnAfterStateCreated(CeilUfas.Models.ceilufas.State item);

        public async Task<CeilUfas.Models.ceilufas.State> CreateState(CeilUfas.Models.ceilufas.State state)
        {
            OnStateCreated(state);

            var existingItem = Context.States
                              .Where(i => i.Id == state.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.States.Add(state);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(state).State = EntityState.Detached;
                throw;
            }

            OnAfterStateCreated(state);

            return state;
        }

        public async Task<CeilUfas.Models.ceilufas.State> CancelStateChanges(CeilUfas.Models.ceilufas.State item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStateUpdated(CeilUfas.Models.ceilufas.State item);
        partial void OnAfterStateUpdated(CeilUfas.Models.ceilufas.State item);

        public async Task<CeilUfas.Models.ceilufas.State> UpdateState(string id, CeilUfas.Models.ceilufas.State state)
        {
            OnStateUpdated(state);

            var itemToUpdate = Context.States
                              .Where(i => i.Id == state.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(state);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStateUpdated(state);

            return state;
        }

        partial void OnStateDeleted(CeilUfas.Models.ceilufas.State item);
        partial void OnAfterStateDeleted(CeilUfas.Models.ceilufas.State item);

        public async Task<CeilUfas.Models.ceilufas.State> DeleteState(string id)
        {
            var itemToDelete = Context.States
                              .Where(i => i.Id == id)
                              .Include(i => i.CourseRegistrations)
                              .Include(i => i.Municipalities)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStateDeleted(itemToDelete);


            Context.States.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStateDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}
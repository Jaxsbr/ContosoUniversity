#region snippet_All
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        // GOALS:
        // - Capture process of converting current non tested logic, to testable logic
        // - Demostrate benefits of code coverage and unit testing
        // - Demonstate in memory db

        // TODO:
        // - Capture use cases in OnGetAsync
        // - Write e2e test (to be run before and after modification to prove success of refactor)
        // - Illustate units of work within OnGetAsync (comment/region areas)
        // - Rewrite OnGetAsync with TDD

        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Students { get; set; }

        // Use case: Sort student list in acending last name order (default)
        // Use case: Sort student list in descending last name order
        // Use case: Sort student list in acending enrollment date order
        // Use case: Sort student list in descending enrollment date order
        // Use case: Filter student list where name OR last name contains search string
        // Use case: Display maximum of 3 students per page

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            // Set local property to user provide sort order
            CurrentSort = sortOrder;

            // Configure default name and date column sort
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            // If user provides search string, set page index to 1
            if (searchString != null)
            {
                pageIndex = 1;
            }
            // If no search string is provided, set text box to current filter
            else
            {
                searchString = currentFilter;
            }

            // Set current filter to value of the search string text box
            CurrentFilter = searchString;

            // Get all students in Students table
            IQueryable<Student> studentsIQ = from s in _context.Students select s;

            // Filters students list based on provide search string (name and last name is compared)
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }

            // Order students list based on provide order (defaults to last name ordering if non provided)
            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            // Set max page size
            int pageSize = 3;

            // Set local property with paged students list
            Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
#endregion
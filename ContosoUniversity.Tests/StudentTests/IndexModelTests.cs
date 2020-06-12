using ContosoUniversity.Models;
using ContosoUniversity.Pages.Students;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.Tests.StudentTests
{
    public class IndexModelTests
    {
        // Use case: Sort student list in acending last name order (default)
        // Use case: Sort student list in descending last name order
        // Use case: Sort student list in acending enrollment date order
        // Use case: Sort student list in descending enrollment date order
        // Use case: Filter student list where name OR last name contains case sensitive search string
        // Use case: Display maximum of 3 students per page

        [Fact]
        public async Task OnGetAsync_GivenFilter_ReturnsSingleStudent()
        {
            using (var context = TestUtility.GetPopulatedContext())
            {
                // Arrange
                var filter = "ram";
                var expectedStudent = new Student { FirstMidName = "John", LastName = "rambo" };
                context.Students.Add(expectedStudent);
                context.SaveChanges();
                var indexModel = new IndexModel(context);

                // Act
                await indexModel.OnGetAsync(null, filter, null, 0);

                // Assert
                Assert.Contains(indexModel.Students, s => 
                    s.LastName == expectedStudent.LastName && 
                    s.FirstMidName == expectedStudent.FirstMidName);
            }
        }

        [Fact]
        public async Task OnGetAsync_ReturnsStudentCount_EqualToMaxPageSize()
        {
            using (var context = TestUtility.GetPopulatedContext())
            {
                // Arrange
                var maxPageSize = 3;
                var indexModel = new IndexModel(context);

                // Act
                await indexModel.OnGetAsync(null, null, null, 0);

                // Assert
                Assert.Equal(maxPageSize, indexModel.Students.Count);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NoteProject.Models;
using NoteProject.Services;

namespace NoteProject.Tests
{
    public class DiaryServiceTests
    {
        [Fact]
        public async Task SearchDiaryByTime_ShouldReturnDiariesWithinTimeRange()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Diaries.Add(new Diaries { Title = "Diary 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                var startTime = new DateTime(2024, 1, 15);
                var endTime = new DateTime(2024, 2, 15);
                var result = await diaryService.SearchDiaryByTime(startTime, endTime, userId);
                // Assert
                Assert.Single(result);
                Assert.Equal("Diary 2", result[0].Title);
            }
        }
        [Fact]
        public async Task SearchDiaryByTitleAsync_ShouldReturnDiariesWithMatchingTitle()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Diaries.Add(new Diaries { Title = "Diary 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                var result = await diaryService.SearchDiaryByTitleAsync("Diary 2", userId);
                // Assert
                Assert.Single(result);
                Assert.Equal("Diary 2", result[0].Title);
            }
        }
        [Fact]
        public async Task SearchDiaryByTitleAsync_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                var userId = Guid.NewGuid();
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => diaryService.SearchDiaryByTitleAsync("", userId));
            }
        }

        [Fact]
        public async Task SearchDiaryByTime_ShouldThrowException_WhenStartDateIsGreaterThanEndDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                var userId = Guid.NewGuid();
                var startDate = new DateTime(2024, 3, 1);
                var endDate = new DateTime(2024, 2, 1);
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => diaryService.SearchDiaryByTime(startDate, endDate, userId));
            }
        }

        [Fact]
        public async Task SearchDiaryByTime_ShouldThrowArgumentException_WhenBothDatesAreNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                var userId = Guid.NewGuid();
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => diaryService.SearchDiaryByTime(null, null, userId));
            }
        }

        [Fact]
        public async Task SearchDiaryByTime_ShouldReturnDiaries_WhenOnlyStartDateIsProvided()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                var userId = Guid.NewGuid();
                context.Diaries.Add(new Diaries { Title = "Diary 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                var startDate = new DateTime(2024, 2, 1);
                // Act
                var result = await diaryService.SearchDiaryByTime(startDate, null, userId);
                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, d => d.Title == "Diary 2");
                Assert.Contains(result, d => d.Title == "Diary 3");
            }
        }

        [Fact]
        public async Task SearchDiaryByTime_ShouldReturnDiaries_WhenOnlyEndDateIsProvided()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var diaryService = new DiaryService(context);
                var userId = Guid.NewGuid();
                context.Diaries.Add(new Diaries { Title = "Diary 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Diaries.Add(new Diaries { Title = "Diary 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                var endDate = new DateTime(2024, 2, 15);
                // Act
                var result = await diaryService.SearchDiaryByTime(null, endDate, userId);
                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, d => d.Title == "Diary 1");
                Assert.Contains(result, d => d.Title == "Diary 2");
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NoteProject.Models;
using NoteProject.Services;

namespace NoteProject.Tests
{
    public class NoteServiceTests
    {
        [Fact]
        public async Task SearchNoteByTime_ShouldReturnNotesWithinTimeRange()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Notes.Add(new Notes { Title = "Note 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                var startTime = new DateTime(2024, 1, 15);
                var endTime = new DateTime(2024, 2, 15);
                var result = await noteService.SearchNoteByTime(startTime, endTime, userId);
                // Assert
                Assert.Single(result);
                Assert.Equal("Note 2", result[0].Title);
            }
        }
        [Fact]
        public async Task SearchNoteByTitleAsync_ShouldReturnNotesWithMatchingTitle()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Notes.Add(new Notes { Title = "Note 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                var result = await noteService.SearchNoteAsync("Note 2", userId);
                // Assert
                Assert.Single(result);
                Assert.Equal("Note 2", result[0].Title);
            }
        }

        [Fact]
        public async Task SearchNoteByTitleAsync_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                var userId = Guid.NewGuid();
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => noteService.SearchNoteAsync("", userId));
            }
        }

        [Fact]
        public async Task SearchNoteByTime_ShouldThrowException_WhenStartDateIsGreaterThanEndDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                var userId = Guid.NewGuid();
                var startTime = new DateTime(2024, 2, 15);
                var endTime = new DateTime(2024, 1, 15);
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => noteService.SearchNoteByTime(startTime, endTime, userId));
            }
        }

        [Fact]
        public async Task SearchNoteByTime_ShouldThrowArgumentException_WhenBothDatesAreNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                var userId = Guid.NewGuid();
                DateTime? startTime = null;
                DateTime? endTime = null;
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => noteService.SearchNoteByTime(startTime, endTime, userId));
            }
        }

        [Fact]
        public async Task SearchNoteByTime_ShouldReturnNotes_WhenOnlyStartDateIsProvided()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Notes.Add(new Notes { Title = "Note 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                var startTime = new DateTime(2024, 2, 1);
                DateTime? endTime = null;
                var result = await noteService.SearchNoteByTime(startTime, endTime, userId);
                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, n => n.Title == "Note 2");
                Assert.Contains(result, n => n.Title == "Note 3");
            }
        }
        [Fact]
        public async Task SearchNoteByTime_ShouldReturnNotes_WhenOnlyEndDateIsProvided()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new NoteDbContext(options))
            {
                var noteService = new NoteService(context);
                // Seed the database with test data
                var userId = Guid.NewGuid();
                context.Notes.Add(new Notes { Title = "Note 1", Content = "Content 1", CreatedAt = new DateTime(2024, 1, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 2", Content = "Content 2", CreatedAt = new DateTime(2024, 2, 1), UserId = userId });
                context.Notes.Add(new Notes { Title = "Note 3", Content = "Content 3", CreatedAt = new DateTime(2024, 3, 1), UserId = userId });
                await context.SaveChangesAsync();
                // Act
                DateTime? startTime = null;
                var endTime = new DateTime(2024, 2, 15);
                var result = await noteService.SearchNoteByTime(startTime, endTime, userId);
                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, n => n.Title == "Note 1");
                Assert.Contains(result, n => n.Title == "Note 2");
            }
        }
    }
}

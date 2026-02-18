using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class ProjectTasksViewModelTests
    {
        private class MockStorageService : IStorageService
        {
            private List<Project> _projects = new();

            public Task<List<Project>> LoadDataAsync()
            {
                return Task.FromResult(_projects);
            }

            public Task SaveDataAsync(List<Project> projects)
            {
                _projects = new List<Project>(projects);
                return Task.CompletedTask;
            }
        }

        [TestMethod]
        public void TestProjectCreation()
        {
            // Arrange
            var project = new Project("Домашние дела");

            // Act
            var task = new TaskItem(project.Id, "Купить молоко");
            project.Tasks.Add(task);

            // Assert
            Assert.AreEqual("Домашние дела", project.Name);
            Assert.AreEqual(1, project.Tasks.Count);
            Assert.AreEqual(TaskStatus.New, task.Status);
            Assert.AreEqual(TaskPriority.Medium, task.Priority);
        }

        [TestMethod]
        public void TestTaskStatusTransition()
        {
            // Arrange
            var task = new TaskItem("proj-1", "Test Task");
            Assert.AreEqual(TaskStatus.New, task.Status);

            // Act
            task.Status = TaskStatus.InProgress;

            // Assert
            Assert.AreEqual(TaskStatus.InProgress, task.Status);
        }

        [TestMethod]
        public void TestProjectCompletionStats()
        {
            // Arrange
            var project = new Project("Test Project");
            project.Tasks.Add(new TaskItem(project.Id, "Task 1") { Status = TaskStatus.Completed });
            project.Tasks.Add(new TaskItem(project.Id, "Task 2") { Status = TaskStatus.New });
            project.Tasks.Add(new TaskItem(project.Id, "Task 3") { Status = TaskStatus.InProgress });

            // Act
            var completed = project.GetCompletedTaskCount();
            var total = project.GetTotalTaskCount();

            // Assert
            Assert.AreEqual(1, completed);
            Assert.AreEqual(3, total);
        }

        [TestMethod]
        public async Task TestStorageService()
        {
            // Arrange
            var storage = new MockStorageService();
            var projects = new List<Project>
            {
                new Project("Project 1"),
                new Project("Project 2")
            };

            // Act
            await storage.SaveDataAsync(projects);
            var loaded = await storage.LoadDataAsync();

            // Assert
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("Project 1", loaded[0].Name);
        }

        [TestMethod]
        public void TestTaskPriorities()
        {
            // Verify enum values exist
            Assert.AreEqual(0, (int)TaskPriority.Low);
            Assert.AreEqual(1, (int)TaskPriority.Medium);
            Assert.AreEqual(2, (int)TaskPriority.High);
        }

        [TestMethod]
        public void TestTaskStatuses()
        {
            // Verify enum values exist
            Assert.AreEqual(0, (int)TaskStatus.New);
            Assert.AreEqual(1, (int)TaskStatus.InProgress);
            Assert.AreEqual(2, (int)TaskStatus.Completed);
        }
    }
}

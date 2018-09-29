using System.Collections.Generic;
using System.Diagnostics;
using MEFedMVVM.Testability.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MEFedMVVMDemo.Services.Contracts;
using MEFedMVVM.Services.Contracts;
using MEFedMVVMDemo.ViewModels;
using MEFedMVVMDemo.Services.Models;

namespace TestMEFedMVVMDemo
{
    /// <summary>
    /// name is kind of funny isn't it... These are the Tests for the TestViewModel
    /// </summary>
    [TestClass]
    public class TestViewModelTest
    {
        [TestMethod]
        public void Should_Load_Data_On_Loaded_Test()
        {
            //ARRANGE
			var autoStabber = new MoqAutoStabber();

        	var usersService = new Mock<IUsersService>();
        	autoStabber.Add(typeof(IUsersService), usersService.Object);

            var firstuser = new User();
            var mockUsersData = new List<User> { firstuser, new User() };

			usersService.Setup(us => us.GetAllUsers()).Returns(mockUsersData);

            //ACT
			var viewModel = autoStabber.Create<UsersViewModel>();
           

            //ASSERT
            Assert.AreEqual(mockUsersData.Count, viewModel.Users.Count, "View model did not load the users");
            
            //test that by default the first user in the list was selected
            Assert.AreEqual(firstuser, viewModel.SelectedUser, "The first user was not selected by default");

            //test that the correct visual state was invoked
			autoStabber.GetMock<IVisualStateManager>().Verify(x => x.GoToState("Welcome"));
            
            //make sure that when the selected user was set a message was sent via the mediator
			autoStabber.GetMock<IMediator>().Verify(m => m.NotifyColleagues(MEFedMVVMDemo.MediatorMessages.SelectedUser, firstuser));
        }

        [TestMethod]
        public void Should_Select_User_Test()
        {
            //ARRANGE
            var mockUser = new User();
			var autoStabber = new MoqAutoStabber();

            //ACT
			var viewModel = autoStabber.Create<UsersViewModel>();
			viewModel.SelectedUser = mockUser;

            //ASSERT
			Assert.AreEqual(mockUser, viewModel.SelectedUser, "The selected user was not selected");
			//make sure that when the selected user was set a message was sent via the mediator
			autoStabber.GetMock<IMediator>().Verify(m => m.NotifyColleagues(MEFedMVVMDemo.MediatorMessages.SelectedUser, mockUser));
        }
    }
}

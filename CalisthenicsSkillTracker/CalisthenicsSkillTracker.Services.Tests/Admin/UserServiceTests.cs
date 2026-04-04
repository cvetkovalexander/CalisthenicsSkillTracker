using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.Services.Core.Services.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CalisthenicsSkillTracker.Services.Tests.Admin;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _repositoryMock;

    private IUserService _service;

    [SetUp]
    public void SetUp()
    {
        this._repositoryMock = new Mock<IUserRepository>();
        this._service = new UserService(this._repositoryMock.Object);
    }

    [Test]
    public async Task GetAllManageableUsersAsync_NoUsersAreReturned_DoesReturnAnEmptyCollection()
    {
        Guid adminUserId = Guid.NewGuid();

        this._repositoryMock
            .Setup(r => r.GetUsersWithRolesAsync(
                u => u.Id != adminUserId, null))
            .ReturnsAsync(Enumerable.Empty<ApplicationUser>());

        IEnumerable<ManageUserViewModel> result = await this._service.GetAllManageableUsersAsync(adminUserId.ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);

        this._repositoryMock.Verify(r => r.GetUsersWithRolesAsync(u => u.Id != adminUserId, null), Times.Once);
    }

    [Test]
    public async Task GetAllManageableUsersAsync_UsersAreReturned_DoesReturnNonEmptyCollection()
    {
        Guid adminUserId = Guid.NewGuid();

        List<ApplicationUser> users = new List<ApplicationUser>
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "firstUser",
            Email = "first@example.com",
            UserRoles = new List<ApplicationUserRole>()
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "secondUser",
            Email = "second@example.com",
            UserRoles = new List<ApplicationUserRole>()
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "thirdUser",
            Email = "third@example.com",
            UserRoles = new List<ApplicationUserRole> 
            {
                new ApplicationUserRole
                {
                    Role = new IdentityRole<Guid>
                    {
                        Name = "Admin"
                    }
                }
            }
        }
    };

        this._repositoryMock
            .Setup(r => r.GetUsersWithRolesAsync(u => u.Id != adminUserId, null))
            .ReturnsAsync(users);

        IEnumerable<ManageUserViewModel> result = await this._service.GetAllManageableUsersAsync(adminUserId.ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(users.Count));

        this._repositoryMock.Verify(r => r.GetUsersWithRolesAsync(u => u.Id != adminUserId, null), Times.Once);
    }

    [Test]
    public async Task EditUsernameAsync_UserNameSuccessfullyEdited_DoesReturnTrue()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "OldUsername",
            NormalizedUserName = "OLDUSERNAME",
        };

        string newUsername = "NewUsername";

        this._repositoryMock
            .Setup(r => r.UpdateApplicationUserAsync(user))
            .ReturnsAsync(true);

        bool result = await this._service.EditUserUsernameAsync(user, newUsername);

        Assert.That(result, Is.True);
        Assert.That(user.UserName, Is.EqualTo(newUsername));
        Assert.That(user.NormalizedUserName, Is.EqualTo(newUsername.ToUpper()));

        this._repositoryMock.Verify(r => r.UpdateApplicationUserAsync(user), Times.Once);
    }

    [Test]
    public async Task EditUsernameAsync_UserNameFailEdit_DoesReturnFalse()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "OldUsername",
            NormalizedUserName = "OLDUSERNAME",
        };

        string newUsername = "NewUsername";

        this._repositoryMock
            .Setup(r => r.UpdateApplicationUserAsync(user))
            .ReturnsAsync(false);

        bool result = await this._service.EditUserUsernameAsync(user, newUsername);

        Assert.That(result, Is.False);
        Assert.That(user.UserName, Is.EqualTo(newUsername));
        Assert.That(user.NormalizedUserName, Is.EqualTo(newUsername.ToUpper()));

        this._repositoryMock.Verify(r => r.UpdateApplicationUserAsync(user), Times.Once);
    }

    [Test]
    public async Task DeleteUserAsync_UserSuccessfullyDeleted_DoesReturnTrue()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        this._repositoryMock
            .Setup(r => r.DeleteApplicationUserAsync(user))
            .ReturnsAsync(true);

        bool result = await this._service.DeleteUserAsync(user);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.DeleteApplicationUserAsync(user), Times.Once);
    }

    [Test]
    public async Task DeleteUserAsync_UserFailDelete_DoesReturnFalse()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        this._repositoryMock
            .Setup(r => r.DeleteApplicationUserAsync(user))
            .ReturnsAsync(false);

        bool result = await this._service.DeleteUserAsync(user);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.DeleteApplicationUserAsync(user), Times.Once);
    }

    [Test]
    public void CreateDeleteUserViewModel_ShouldReturnViewModelWithMappedProperties()
    {
        string userId = Guid.NewGuid().ToString();
        string username = "TestUser";

        DeleteUserViewModel result = this._service.CreateDeleteUserViewModel(userId, username);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.UserName, Is.EqualTo(username));
    }

    [Test]
    public void CreateEditUsernameViewModel_ShouldReturnViewModelWithMappedProperties()
    {
        string userId = Guid.NewGuid().ToString();
        string username = "TestUser";

        EditUsernameViewModel result = this._service.CreateEditUsernameViewModel(userId, username);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.UserName, Is.EqualTo(username));
    }

    [Test]
    public async Task GetUserByIdAsync_UserIsFound_DoesReturnFoundUser()
    {
        string userId = Guid.NewGuid().ToString();

        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            UserName = "TestUser"
        };

        this._repositoryMock
            .Setup(r => r.GetApplicationUserAsync(userId))
            .ReturnsAsync(user);

        ApplicationUser? result = await this._service.GetUserByIdAsync(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(user));

        this._repositoryMock.Verify(r => r.GetApplicationUserAsync(userId), Times.Once);
    }

    [Test]
    public async Task GetUserByIdAsync_UserIsNotFound_DoesReturnNull()
    {
        string userId = Guid.NewGuid().ToString();

        this._repositoryMock
            .Setup(r => r.GetApplicationUserAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        ApplicationUser? result = await this._service.GetUserByIdAsync(userId);

        Assert.That(result, Is.Null);

        this._repositoryMock.Verify(r => r.GetApplicationUserAsync(userId), Times.Once);
    }

    [Test]
    public async Task RoleAddedSuccessfullyAsync_RoleSuccessfullyAdded_DoesReturnTrue()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.AddRoleToUserAsync(user, role))
            .ReturnsAsync(true);

        bool result = await this._service.RoleAddedSuccessfullyAsync(user, role);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.AddRoleToUserAsync(user, role), Times.Once);
    }

    [Test]
    public async Task RoleAddedSuccessfullyAsync_RoleFailAdd_DoesReturnFalse()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.AddRoleToUserAsync(user, role))
            .ReturnsAsync(false);

        bool result = await this._service.RoleAddedSuccessfullyAsync(user, role);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.AddRoleToUserAsync(user, role), Times.Once);
    }

    [Test]
    public async Task RoleAlreadyAssignedAsync_RoleAlreadyAssigned_DoesReturnTrue()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RoleAlreadyAssignedAsync(user, role))
            .ReturnsAsync(true);

        bool result = await this._service.RoleAlreadyAssignedAsync(user, role);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.RoleAlreadyAssignedAsync(user, role), Times.Once);
    }

    [Test]
    public async Task RoleAlreadyAssignedAsync_RoleNotAssigned_DoesReturnFalse()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RoleAlreadyAssignedAsync(user, role))
            .ReturnsAsync(false);

        bool result = await this._service.RoleAlreadyAssignedAsync(user, role);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.RoleAlreadyAssignedAsync(user, role), Times.Once);
    }

    [Test]
    public async Task RoleExistsAsync_RoleDoesExist_DoesReturnTrue()
    {
        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RoleExistsAsync(role))
            .ReturnsAsync(true);

        bool result = await this._service.RoleExistsAsync(role);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.RoleExistsAsync(role), Times.Once);
    }

    [Test]
    public async Task RoleExistsAsync_RoleDoesNotExist_DoesReturnFalse()
    {
        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RoleExistsAsync(role))
            .ReturnsAsync(false);

        bool result = await this._service.RoleExistsAsync(role);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.RoleExistsAsync(role), Times.Once);
    }

    [Test]
    public async Task RoleRemovedSuccessfullyAsync_RoleSuccessfullyRemoved_DoesReturnTrue()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RemoveRoleFromUserAsync(user, role))
            .ReturnsAsync(true);

        bool result = await this._service.RoleRemovedSuccessfullyAsync(user, role);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.RemoveRoleFromUserAsync(user, role), Times.Once);
    }

    [Test]
    public async Task RoleRemovedSuccessfullyAsync_RoleFailRemove_DoesReturnFalse()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        string role = "Admin";

        this._repositoryMock
            .Setup(r => r.RemoveRoleFromUserAsync(user, role))
            .ReturnsAsync(false);

        bool result = await this._service.RoleRemovedSuccessfullyAsync(user, role);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.RemoveRoleFromUserAsync(user, role), Times.Once);
    }

    [Test]
    public async Task UserExistsAsync_UserDoesExist_DoesReturnTrue()
    {
        string userId = Guid.NewGuid().ToString();

        this._repositoryMock
            .Setup(r => r.UserExistsAsync(userId))
            .ReturnsAsync(true);

        bool result = await this._service.UserExistsAsync(userId);

        Assert.That(result, Is.True);

        this._repositoryMock.Verify(r => r.UserExistsAsync(userId), Times.Once);
    }

    [Test]
    public async Task UserExistsAsync_UserDoesNotExist_DoesReturnFalse()
    {
        string userId = Guid.NewGuid().ToString();

        this._repositoryMock
            .Setup(r => r.UserExistsAsync(userId))
            .ReturnsAsync(false);

        bool result = await this._service.UserExistsAsync(userId);

        Assert.That(result, Is.False);

        this._repositoryMock.Verify(r => r.UserExistsAsync(userId), Times.Once);
    }
}

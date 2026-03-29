namespace CalisthenicsSkillTracker.GCommon;

public static class OutputMessages
{
    public static class EntityMessages 
    {
        public const string EntitySaveError = "An error occurred while creating the {0}. Please try again in a few minutes.";

        public const string EntityEditError = "An error occurred while editing the {0}. Please try again in a few minutes.";

        public const string EntityDeleteError = "An error occurred while deleting the {0}. Please try again in a few minutes.";

        public const string EntitySuccessfullyDeleted = "The {0} was successfully deleted.";

        public const string EntitySuccessfullyEdited = "The {0} was successfully edited.";

        public const string EntitySuccessfullyCreated = "The {0} was successfully created";

        public const string ExerciseSuccessfullyLogged = "The exercise was successfully logged.";

        public const string SetSuccessfullyLogged = "The set was successfully logged.";   
    }

    public static class IdentityMessages 
    {
        public const string RoleSeedingExceptionMessage = "An error occurred while seeding the role {0}. Please try again in a few minutes.";

        public const string AdminEmailSeedingExceptionMessage = "Admin email not found in configuration.";

        public const string AdminPasswordSeedingExceptionMessage = "Admin password not found in configuration.";

        public const string AdminUserSeedingExceptionMessage = "An error occurred while seeding the admin user. Please try again in a few minutes.";

        public const string AdminUserRoleSeedingExceptionMessage = "An error occurred while adding the admin user to the {0} role. Please try again in a few minutes.";

        public const string InvalidRoleAssignmentMessage = "Invalid role assignment request. Please try again.";

        public const string InvalidRoleRemovalMessage = "Invalid role removal request. Please try again.";

        public const string UserNotFoundMessage = "User not found. Please try again.";

        public const string RoleNotFoundMessage = "Role not found. Please try again.";

        public const string RoleAlreadyAssignedMessage = "The user already has the {0} role assigned.";

        public const string RoleNotAssignedMessage = "The user does not have the {0} role assigned.";

        public const string RoleAssignmentFailedMessage = "An error occurred while assigning the {0} role to the user. Please try again in a few minutes.";

        public const string RoleRemovalFailedMessage = "An error occurred while removing the {0} role from the user. Please try again in a few minutes.";

        public const string RoleSuccessfullyAssignedMessage = "The {0} role was successfully assigned to the user.";

        public const string RoleSuccessfullyRemovedMessage = "The {0} role was successfully removed from the user.";
    }
}

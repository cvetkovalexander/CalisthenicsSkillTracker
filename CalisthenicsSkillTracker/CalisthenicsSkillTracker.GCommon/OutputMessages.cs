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
    }
}

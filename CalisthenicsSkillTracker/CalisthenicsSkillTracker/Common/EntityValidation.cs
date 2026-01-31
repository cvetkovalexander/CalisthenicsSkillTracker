namespace CalisthenicsSkillTracker.Common;

public static class EntityValidation
{
    public static class User
    {
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 30;

        public const int FirstNameMinLength = 5;
        public const int FirstNameMaxLength = 40;

        public const int LastNameMinLength = 5;
        public const int LastNameMaxLength = 40;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 64;

        public const int EmailMinLength = 8;
        public const int EmailMaxLength = 80;
    }

    public static class Skill
    {
        public const int NameMinLength = 5;
        public const int NameMaxLength = 50;
        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 500;
    }

    public static class SkillProgress
    {
        public const int NotesMinLength = 0;
        public const int NotesMaxLength = 1000;

        public const int RepetitionsMinValue = 0;
        public const int RepetitionsMaxValue = 1000;

        public const int DurationMinValue = 0;
        public const int DurationMaxValue = 3600;
    }
}

namespace CalisthenicsSkillTracker.GCommon;

public static class ApplicationConstants
{
    public const string DateTimeColumnType = "datetime2";

    public const int DefaultPageSize = 5;

    public const string UnexpectedErrorMessage = "An unexpected error occurred. Please contact support!";

    public const string InvalidOperationMessage = "An unexpected error occurred while trying to execute the {0} operation. Please try again later!";

    public const string ErrorTempDataKey = "ErrorMessage";
    public const string WarningTempDataKey = "WarningMessage";
    public const string InfoTempDataKey = "InfoMessage";
    public const string SuccessTempDataKey = "SuccessMessage";

    public static class EnumKeys
    {
        public const string MeasurementKey = "Measurement";
        public const string DifficultyKey = "Difficulty";
        public const string SkillTypeKey = "SkillType";
        public const string CategoryKey = "Category";
    }
}

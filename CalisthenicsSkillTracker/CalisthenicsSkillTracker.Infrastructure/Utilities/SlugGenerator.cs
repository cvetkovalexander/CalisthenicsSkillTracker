using CalisthenicsSkillTracker.Infrastructure.Utilities.Contracts;

namespace CalisthenicsSkillTracker.Infrastructure.Utilities;

public class SlugGenerator : ISlugGenerator
{
    public string GenerateSlug(string input)
    {
        string[] data = input
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToLowerInvariant())
            .ToArray();

        return string.Join("-", data);
    }
}

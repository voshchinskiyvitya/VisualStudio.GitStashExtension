using System.Linq;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace VisualStudio.GitStashExtension.Extensions
{
    /// <summary>
    /// Extension class for IGitExt.
    /// </summary>
    public static class GitServiceExtension
    {
        /// <summary>
        /// Checks existence of any active git repository.
        /// </summary>
        public static bool AnyActiveRepository(this IGitExt service)
        {
            var activeRepository = service.ActiveRepositories.FirstOrDefault();

            return activeRepository != null;
        }
    }
}

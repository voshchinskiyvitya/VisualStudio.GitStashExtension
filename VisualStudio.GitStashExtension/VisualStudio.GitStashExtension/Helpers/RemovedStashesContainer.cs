using System.Collections.Generic;

namespace VisualStudio.GitStashExtension.Helpers
{
    /// <summary>
    /// Represents container for the stashes that were removed during the last Stash List page visiting.
    /// </summary>
    /// <remarks>
    /// Team explorer extensibility doesn't provide simple and straightforward way for managing navigation history,
    /// so we need to save deleted stashes and check them bafore viewing Stash Info page to avoid navigation to deleted Stash page.
    /// </remarks>
    public static class RemovedStashesContainer
    {
        private static HashSet<int> _stashIds = new HashSet<int>();

        /// <summary>
        /// Adds new deleted stash id to the container.
        /// </summary>
        /// <param name="id">Stash id.</param>
        public static void AddDeletedStash(int id)
        {
            _stashIds.Add(id);
        }

        /// <summary>
        /// Checks whether or not stash with this Id was removed.
        /// </summary>
        /// <param name="id">Stash Id.</param>
        public static bool Contains(int id)
        {
            return _stashIds.Contains(id);
        }

        /// <summary>
        /// Resets container state (removes all deleted stash ids).
        /// </summary>
        public static void ResetContainer()
        {
            _stashIds.Clear();
        }
    }
}

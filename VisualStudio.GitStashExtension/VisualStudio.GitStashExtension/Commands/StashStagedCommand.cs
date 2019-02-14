using System;
using System.ComponentModel.Design;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using VisualStudio.GitStashExtension.UI.Commands;
using Task = System.Threading.Tasks.Task;

namespace VisualStudio.GitStashExtension.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class StashStagedCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("8d0a9a23-f158-4aa4-9b6e-3fcc779eb26d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="StashStagedCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private StashStagedCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);

            ToggleStashStagedSectionVisibilityCommand = new ToggleStashStagedSectionVisibilityCommand(TeamExplorer);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static StashStagedCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return package;
            }
        }

        private ITeamExplorer TeamExplorer => ServiceProvider?.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
        private ICommand ToggleStashStagedSectionVisibilityCommand { get; }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in StashStagedCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new StashStagedCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        public void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ToggleStashStagedSectionVisibilityCommand.Execute(null);
        }
    }
}

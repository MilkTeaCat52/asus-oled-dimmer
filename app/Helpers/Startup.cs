using GHelper.Helpers;
using Microsoft.Win32.TaskScheduler;
using System.Security.Principal;

public class Startup
{

    static string taskName = "GHelper";
    static string strExeFilePath = Application.ExecutablePath.Trim();

    public static bool IsScheduled()
    {
        try
        {
            using (TaskService taskService = new TaskService())
                return (taskService.RootFolder.AllTasks.Any(t => t.Name == taskName));
        }
        catch (Exception e)
        {
            Logger.WriteLine("Can't check startup task status: " + e.Message);
            return false;
        }
    }

    public static void StartupCheck()
    {
        using (TaskService taskService = new TaskService())
        {
            var task = taskService.RootFolder.AllTasks.FirstOrDefault(t => t.Name == taskName);
            if (task != null)
            {
                try
                {
                    string action = task.Definition.Actions.FirstOrDefault()!.ToString().Trim();
                    if (!strExeFilePath.Equals(action, StringComparison.OrdinalIgnoreCase) && !File.Exists(action))
                    {
                        Logger.WriteLine("File doesn't exist: " + action);
                        Logger.WriteLine("Rescheduling to: " + strExeFilePath);
                        UnSchedule();
                        Schedule();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"Can't check startup task: {ex.Message}");
                }

            }
        }
    }

    public static void Schedule()
    {

        using (TaskDefinition td = TaskService.Instance.NewTask())
        {

            td.RegistrationInfo.Description = "G-Helper Auto Start";
            td.Triggers.Add(new LogonTrigger { UserId = WindowsIdentity.GetCurrent().Name, Delay = TimeSpan.FromSeconds(2) });
            td.Actions.Add(strExeFilePath);

            if (ProcessHelper.IsUserAdministrator())
                td.Principal.RunLevel = TaskRunLevel.Highest;

            td.Settings.StopIfGoingOnBatteries = false;
            td.Settings.DisallowStartIfOnBatteries = false;
            td.Settings.ExecutionTimeLimit = TimeSpan.Zero;

            try
            {
                TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td);
            }
            catch (Exception e)
            {
                if (ProcessHelper.IsUserAdministrator())
                    MessageBox.Show("Can't create a start up task. Try running Task Scheduler by hand and manually deleting GHelper task if it exists there.", "Scheduler Error", MessageBoxButtons.OK);
                else
                    ProcessHelper.RunAsAdmin();
            }

            Logger.WriteLine("Startup task scheduled: " + strExeFilePath);
        }
    }

    public static void UnSchedule()
    {
        using (TaskService taskService = new TaskService())
        {
            try
            {
                taskService.RootFolder.DeleteTask(taskName);
            }
            catch (Exception e)
            {
                if (ProcessHelper.IsUserAdministrator())
                    MessageBox.Show("Can't remove task. Try running Task Scheduler by hand and manually deleting GHelper task if it exists there.", "Scheduler Error", MessageBoxButtons.OK);
                else
                    ProcessHelper.RunAsAdmin();
            }
        }
    }
}

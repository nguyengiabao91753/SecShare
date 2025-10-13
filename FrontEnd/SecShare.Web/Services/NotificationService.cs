namespace SecShare.Services;

public class NotificationService
{
    public event Func<string, string, Task> OnShow; // (message, type)
    public event Func<Task> OnHide;

    public async Task ShowSuccess(string message)
    {
        if (OnShow != null)
            await OnShow.Invoke(message, "success");
    }

    public async Task ShowError(string message)
    {
        if (OnShow != null)
            await OnShow.Invoke(message, "danger");
    }

    public async Task Hide()
    {
        if (OnHide != null)
            await OnHide.Invoke();
    }
}

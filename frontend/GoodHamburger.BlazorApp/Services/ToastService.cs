using System;

namespace GoodHamburger.BlazorApp.Services;

public class ToastNotification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = "info";
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class ToastService
{
    public event Action<ToastNotification>? OnShow;
    public event Action<Guid>? OnHide;

    public void ShowSuccess(string title, string message = "")
    {
        Show("success", title, message);
    }

    public void ShowError(string title, string message = "")
    {
        Show("error", title, message);
    }

    public void ShowInfo(string title, string message = "")
    {
        Show("info", title, message);
    }

    public void ShowWarning(string title, string message = "")
    {
        Show("warning", title, message);
    }

    private void Show(string type, string title, string message)
    {
        var notification = new ToastNotification
        {
            Type = type,
            Title = title,
            Message = message
        };

        OnShow?.Invoke(notification);
    }

    public void Hide(Guid id)
    {
        OnHide?.Invoke(id);
    }
}

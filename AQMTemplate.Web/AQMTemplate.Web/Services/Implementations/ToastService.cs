using AQMTemplate.Web.Enums;
using AQMTemplate.Web.Services.Interfaces;

namespace AQMTemplate.Web.Services.Implementations;

public class ToastService : IToastService
{
	public event Action<string, ToastLevel>? OnShow;

	public void ShowInfo(string message)
	{
		OnShow?.Invoke(message, ToastLevel.Info);
	}

	public void ShowSuccess(string message)
	{
		OnShow?.Invoke(message, ToastLevel.Success);
	}

	public void ShowError(string message)
	{
		OnShow?.Invoke(message, ToastLevel.Warning);
	}

	public void ShowWarning(string message)
	{
		OnShow?.Invoke(message, ToastLevel.Error);
	}
}

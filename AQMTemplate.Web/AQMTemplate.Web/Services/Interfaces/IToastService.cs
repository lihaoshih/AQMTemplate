using AQMTemplate.Web.Enums;

namespace AQMTemplate.Web.Services.Interfaces;

public interface IToastService
{
	event Action<string, ToastLevel>? OnShow;
	void ShowInfo(string message);
	void ShowSuccess(string message);
	void ShowWarning(string message);
	void ShowError(string message);
}

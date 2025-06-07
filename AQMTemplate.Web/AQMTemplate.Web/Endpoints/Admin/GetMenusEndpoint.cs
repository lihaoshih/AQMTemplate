using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.Services.Interfaces.Admin;

namespace AQMTemplate.Web.Endpoints.Admin;

public class GetMenusEndpoint : EndpointWithoutRequest<List<MenuDto>> 
{
	private readonly IMenuService _menuService;

	public GetMenusEndpoint(IMenuService menuService)
	{
		_menuService = menuService;
	}

	public override void Configure()
	{
		Get("/api/menus");
		AllowAnonymous();
		Description(b => b
		.WithName("GetAllMenus")
		.WithSummary("獲取所有選單")
		.WithDescription("傳回系統中所有功能選單列表")
		.Produces<List<MenuDto>>(200, "application/json"));			
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		var menus = await _menuService.GetAllMenusAsync(cancellationToken);
		await SendOkAsync(menus.ToList(), cancellationToken);
	}
}

using AQMTemplate.Application.DTOs.Admin;
using AQMTemplate.Application.Services.Interfaces.Admin;

namespace AQMTemplate.Web.Endpoints.Admin;

public class GetUsersEndpoint : EndpointWithoutRequest<List<UserDto>> 
{
	private readonly IUserService _userService;

	public GetUsersEndpoint(IUserService userService)
	{
		_userService = userService;		
	}

	public override void Configure()
	{
		Get("/api/users");
		AllowAnonymous();
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		var users = await _userService.GetAllUsersAsync(cancellationToken);
		await SendOkAsync(users.ToList(), cancellationToken);
	}
}

using Microsoft.AspNetCore.Mvc;
//لو هنشغل عليه هعمله
[ApiController]
[Route("api/mobile")]
public class MobileApiController : ControllerBase
{
	private readonly AcademyDbContext _context;
	public MobileApiController(AcademyDbContext context) => _context = context;

}
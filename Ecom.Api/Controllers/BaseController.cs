using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // protected يشوفها فقط الوارث منه
        protected readonly IUnitOfWork work;

        public BaseController(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}

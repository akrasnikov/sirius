using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace ProjectName.Auth.WebApi.Controllers
{
    public class MetaController : BaseController
    {
        public MetaController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

        [HttpGet("/debug/routes")]        
        public ActionResult<string> Get([FromServices] IEnumerable<EndpointDataSource> sources)
        {
            var sb = new StringBuilder();
            var endpoints =
              sources.SelectMany(s => s.Endpoints);
            foreach (var e in endpoints)
            {
                sb.AppendLine(e.DisplayName);

                if (e is RouteEndpoint re)
                    sb.AppendLine(re.RoutePattern.RawText);
            }

            return sb.ToString();
        }


    }
}
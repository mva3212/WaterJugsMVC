using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using WaterJugsMVC.Models;

namespace WaterJugsMVC.Controllers
{
    public class JugsProblemSpaceController : ApiController
    {

        [HttpPost]
        [Route("SolveJugProblem")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // Requires JSON object JugXCapacity, JugYCapacity, and GoalCapacity
        // Returns The defined problem space including shortest solution
        public HttpResponseMessage Solve([FromBody]JugsProblemSpace problemSpace)
        {
            if (problemSpace == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Must enter valid problem space.");
            }
            if (ModelState.IsValid)
            {
                problemSpace.SolveForShortestSolution();
                return this.Request.CreateResponse(HttpStatusCode.OK, problemSpace);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}

using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(UsuariosRepository usuariosRepos) : ControllerBase
    {

    }
}

using BibliotecaApi.Application.Dtos;
using BibliotecaApi.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroServicio _servicio;

    public LibrosController(ILibroServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibroLeerDto>>> Listar()
    {
        var libros = await _servicio.ListarAsync();
        return Ok(libros);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroLeerDto>> Obtener(int id)
    {
        var libro = await _servicio.ObtenerPorIdAsync(id);
        if (libro is null) return NotFound();
        return Ok(libro);
    }

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] LibroCrearDto dto)
    {
        var resultado = await _servicio.CrearAsync(dto);
        return Ok(resultado);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] LibroActualizarDto dto)
    {
        var resultado = await _servicio.ActualizarAsync(id, dto);

        if (resultado is null)
            return NotFound(new { Mensaje = "No existe un libro con ese ID" });

        return Ok(resultado);
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var resultado = await _servicio.EliminarAsync(id);

        if (resultado is null)
            return NotFound(new { Mensaje = "No existe un libro con ese ID" });

        return Ok(resultado);
    }


}

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
    public async Task<ActionResult<LibroLeerDto>> Crear([FromBody] LibroCrearDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] LibroActualizarDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ok = await _servicio.ActualizarAsync(id, dto);
        if (!ok) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await _servicio.EliminarAsync(id);
        if (!ok) return NotFound();

        return NoContent();
    }
}

public class RespuestaDto<T>
{
    public string Mensaje { get; set; } = string.Empty;
    public T? Datos { get; set; }
}

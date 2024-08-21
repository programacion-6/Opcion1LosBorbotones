namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public interface IMapper<out T, in TK>
{
    static abstract T ToEntity(TK response);
}
using Microsoft.EntityFrameworkCore;
using P2_AP1_FelixMunoz.Models;
using P2_AP1_FelixMunoz.DAL;
using System.Linq.Expressions;

namespace P2_AP1_FelixMunoz.Service;

public class RegistroService(IDbContextFactory<Contexto> DbFactory)
{
    /*public async Task<bool> Guardar(Registro registro)
    {
        if (!await Existe(registro.Id))
        {
            return await Insertar(registro);
        }
        else
        {
            return await Modificar(registro);
        }
    }*/

    public async Task<List<Registro>> Listar(Expression<Func<Registro, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registro.Where(criterio).AsNoTracking().ToListAsync();
    }

}

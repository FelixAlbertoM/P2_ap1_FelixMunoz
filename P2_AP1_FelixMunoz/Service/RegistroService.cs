﻿using P2_AP1_FelixMunoz.DAL;

namespace P2_AP1_FelixMunoz.Service;

public class RegistroService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Registros registro)
    {
        if (!await Existe(registro.Id))
        {
            return await Insertar(registro);
        }
        else
        {
            return await Modificar(registro);
        }
    }

    public async Task<List<Registros>> Listar(Expressions<Func<RegistroService, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Registros.Where(criterio).AsNoTracking().ToListAsync();
    }

}

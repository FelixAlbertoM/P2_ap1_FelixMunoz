using Microsoft.EntityFrameworkCore;
using P2_AP1_FelixMunoz.Models;
using P2_AP1_FelixMunoz.DAL;
using System.Linq.Expressions;

namespace P2_AP1_FelixMunoz.Service;

public class PedidosService(IDbContextFactory<Contexto> DbFactory)
{

    public async Task<bool> Guardar(Pedidos pedido)
    {
        if (!await Existe(pedido.PedidoId))
        {
            return await Insertar(pedido);
        }
        else
        {
            return await Modificar(pedido);
        }
    }
    private async Task<bool> Existe(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos.AnyAsync(p => p.PedidoId == pedidoId);
    }

    private async Task<bool> Insertar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        pedido.Total = pedido.PedidosDetalle?.Sum(d => d.Precio * d.Cantidad) ?? 0M;
        foreach (var detalle in pedido.PedidosDetalle)
        {
            detalle.PedidoId = pedido.PedidoId;
        }
        contexto.Pedidos.Add(pedido);
        var guardado = await contexto.SaveChangesAsync() > 0;

        if (guardado && pedido.PedidosDetalle != null && pedido.PedidosDetalle.Any())
        {
            await AfectarExistencia(pedido.PedidosDetalle.ToArray(), TipoOperacion.Resta);
        }

        return guardado;
    }


    private async Task<bool> Modificar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var anterior = await contexto.Pedidos
            .Include(p => p.PedidosDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PedidoId == pedido.PedidoId);

        if (anterior != null)
        {
          
            await AfectarExistencia(anterior.PedidosDetalle.ToArray(), TipoOperacion.Suma);
        }
        pedido.Total = pedido.PedidosDetalle?.Sum(d => d.Precio * d.Cantidad) ?? 0M;
        await AfectarExistencia(pedido.PedidosDetalle.ToArray(), TipoOperacion.Resta);

        contexto.Update(pedido);
        return await contexto.SaveChangesAsync() > 0;
    }
    private async Task AfectarExistencia(PedidosDetalle[] detalle, TipoOperacion tipoOperacion)
    {
        if (detalle == null || detalle.Length == 0) return;

        await using var contexto = await DbFactory.CreateDbContextAsync();

        foreach (var item in detalle)
        {
            var componente = await contexto.Componentes.SingleAsync(c => c.ComponenteId == item.ComponenteId);

            if (tipoOperacion == TipoOperacion.Resta)
                componente.Existencia -= item.Cantidad;
            else
                componente.Existencia += item.Cantidad;

        }

        await contexto.SaveChangesAsync();
    }
    public async Task<Pedidos?> Buscar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Include(p => p.PedidosDetalle)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
    }
    public async Task<bool> Eliminar(int pedidoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var pedido = await contexto.Pedidos
            .Include(p => p.PedidosDetalle)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

        if (pedido == null) return false;

        await AfectarExistencia(pedido.PedidosDetalle.ToArray(), TipoOperacion.Suma);

        contexto.PedidosDetalles.RemoveRange(pedido.PedidosDetalle);
        contexto.Pedidos.Remove(pedido);

        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<List<Pedidos>> Listar(Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos.Where(criterio).AsNoTracking().ToListAsync();
    }

}
public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}

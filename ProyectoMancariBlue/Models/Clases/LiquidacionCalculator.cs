using ProyectoMancariBlue.Models.Enum;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Clases
{
    public class LiquidacionCalculator
    {
        public static decimal CalcularAguinaldoProporcional(LiquidacionDTO liquidacion)
        {
            
            DateTime inicioPeriodo = new DateTime(liquidacion.FechaSalida.Value.Year - 1, 12, 1);
            int diasTrabajados = (liquidacion.FechaSalida - inicioPeriodo).Value.Days;
            decimal aguinaldoProporcional = (Convert.ToDecimal(liquidacion.empleadoObj.Salario.Value) * diasTrabajados / 30) / 12;
            return aguinaldoProporcional;
        }

        public static decimal CalcularVacaciones(LiquidacionDTO liquidacion)
        {
            decimal salarioDiario = (Convert.ToDecimal(liquidacion.empleadoObj.Salario) / 30);
            decimal vacaciones = salarioDiario * liquidacion.empleadoObj.DiasDisponibles.Value;
            return vacaciones;
        }

        public static decimal CalcularPreaviso(LiquidacionDTO liquidacion)
        {
            if (liquidacion.MotivoSalida.Equals(EReasonDeparture.Renuncia)|| liquidacion.MotivoSalida.Equals(EReasonDeparture.DespidoSinResponsabilidadPatronal) || !liquidacion.Preaviso)
            {
                return 0;
            }

            decimal salarioDiario = Convert.ToDecimal(liquidacion.empleadoObj.Salario.Value) / 30;
            int diasPreaviso = 0;

            int mesesTrabajados = (liquidacion.FechaSalida.Value.Year - liquidacion.empleadoObj.FechaIngreso.Value.Year) * 12 + liquidacion.FechaSalida.Value.Month - liquidacion.empleadoObj.FechaIngreso.Value.Month;

            if (mesesTrabajados >= 3 && mesesTrabajados < 6)
            {
                diasPreaviso = 7;
            }
            else if (mesesTrabajados >= 6 && mesesTrabajados < 12)
            {
                diasPreaviso = 15;
            }
            else if (mesesTrabajados >= 12)
            {
                diasPreaviso = 30;
            }

            decimal preaviso = salarioDiario * diasPreaviso;
            return preaviso;
        }

        public static decimal CalcularCesantia(LiquidacionDTO liquidacion)
        {
            if (liquidacion.MotivoSalida.Equals(EReasonDeparture.Renuncia) || liquidacion.MotivoSalida.Equals(EReasonDeparture.DespidoSinResponsabilidadPatronal))
            {
                return 0;
            }

            decimal salarioMensual = Convert.ToDecimal(liquidacion.empleadoObj.Salario.Value);
            int mesesTrabajados = (liquidacion.FechaSalida.Value.Year - liquidacion.empleadoObj.FechaIngreso.Value.Year) * 12 + liquidacion.FechaSalida.Value.Month - liquidacion.empleadoObj.FechaIngreso.Value.Month;

            decimal cesantia = 0;

            if (mesesTrabajados >= 3 && mesesTrabajados < 6)
            {
                cesantia = salarioMensual / 30 * 7;
            }
            else if (mesesTrabajados >= 6 && mesesTrabajados < 12)
            {
                cesantia = salarioMensual / 30 * 14;
            }
            else if (mesesTrabajados >= 12)
            {
                int añosTrabajados = mesesTrabajados / 12;
                cesantia = Math.Min(añosTrabajados, 8) * salarioMensual;
            }

            return cesantia;
        }
    }
}
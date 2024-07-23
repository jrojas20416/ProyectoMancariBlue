using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace ProyectoMancariBlue.Models.Enum
{
    public static class EnumExtention
    {
        public static string? GetDisplayName(this ECattle enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();
        }

        public static string? GetDescription(this ECattle enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription();
        }

        public static string? GetDisplayNameProductType(this EProductType enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();
        }

        public static string? GetDescriptionProductType(this EProductType enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription();
        }
        public static string? GetDisplayNameReason(this EReasonDeparture enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();
        }

        public static string? GetDescriptionReason(this EReasonDeparture enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription();
        }

    }
}

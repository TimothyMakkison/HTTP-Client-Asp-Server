using Client.Infrastructure;
using System;
using System.Reflection;

namespace Client.Models;

public enum TargetType
{
    Switch,
    Scalar,
    Sequence
}

public class Specification
{
    private readonly Type conversionType;
    private readonly TargetType targetType;

    public Specification(Type conversionType, TargetType targetType)
    {
        this.conversionType = conversionType;
        this.targetType = targetType;
    }

    public Type ConversionType => conversionType;

    public TargetType TargetType => targetType;

    public static Specification FromPropertyInfo(ParameterInfo info)
    {
        var type = info.ParameterType;
        return new Specification(type, type.ToTargetType());
    }
}

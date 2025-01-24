namespace Bitfields.CSharp.Generator.Parsing;

public static class InternalTypeUtil
{
    public enum InternalType
    {
        Bool,
        Byte,
        SByte,
        Char,
        UShort,
        Short,
        UInt,
        Int,
        ULong,
        Long
    }

    public static string ToTypeString(this InternalType internalType)
    {
        return internalType switch
        {
            InternalType.Bool => "bool",
            InternalType.Byte => "byte",
            InternalType.SByte => "sbyte",
            InternalType.Char => "char",
            InternalType.UShort => "ushort",
            InternalType.Short => "short",
            InternalType.UInt => "uint",
            InternalType.Int => "int",
            InternalType.ULong => "ulong",
            InternalType.Long => "long",
            _ => throw new ArgumentOutOfRangeException(nameof(internalType), internalType, null)
        };
    }

    public static string ToNamespaceType(this InternalType internalType)
    {
        return internalType switch
        {
            InternalType.Bool => "Boolean",
            InternalType.Byte => "Byte",
            InternalType.SByte => "SByte",
            InternalType.Char => "UInt16",
            InternalType.UShort => "UInt16",
            InternalType.Short => "Int16",
            InternalType.UInt => "UInt32",
            InternalType.Int => "Int32",
            InternalType.ULong => "UInt64",
            InternalType.Long => "Int64",
            _ => throw new ArgumentOutOfRangeException(nameof(internalType), internalType, null)
        };
    }


    public static bool IsIntegerType(string type)
    {
        return type switch
        {
            "byte" or "sbyte" or "char" or "ushort" or "short" or "uint" or "int" or "ulong"
                or "long" => true,
            _ => false
        };
    }

    public static bool IsPrimitiveType(string type)
    {
        return type switch
        {
            "bool" or "byte" or "sbyte" or "char" or "ushort" or "short" or "uint" or "int" or "ulong"
                or "long" => true,
            _ => false
        };
    }

    public static bool IsUnsupportedFieldType(string type)
    {
        return type switch
        {
            "float" or "double" or "decimal" => true,
            _ => false
        };
    }

    public static InternalType? GetInternalType(string type)
    {
        return type switch
        {
            "bool" => InternalType.Bool,
            "byte" => InternalType.Byte,
            "sbyte" => InternalType.SByte,
            "char" => InternalType.Char,
            "ushort" => InternalType.UShort,
            "short" => InternalType.Short,
            "uint" => InternalType.UInt,
            "int" => InternalType.Int,
            "ulong" => InternalType.ULong,
            "long" => InternalType.Long,
            _ => null
        };
    }

    public static int GetBitsFromInternalType(InternalType internalType)
    {
        return internalType switch
        {
            InternalType.Bool => 1,
            InternalType.Byte => 8,
            InternalType.SByte => 8,
            InternalType.Char => 16,
            InternalType.UShort => 16,
            InternalType.Short => 16,
            InternalType.UInt => 32,
            InternalType.Int => 32,
            InternalType.ULong => 64,
            InternalType.Long => 64,
            _ => throw new ArgumentOutOfRangeException(nameof(internalType), internalType, null)
        };
    }

    public static bool IsInternalTypeUnsigned(InternalType internalType)
    {
        return internalType is InternalType.Bool or InternalType.Byte or InternalType.Char or InternalType.UShort
            or InternalType.UInt or InternalType.ULong;
    }
}
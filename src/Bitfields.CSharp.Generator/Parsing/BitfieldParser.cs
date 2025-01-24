using Microsoft.CodeAnalysis;

namespace Bitfields.CSharp.Generator.Parsing;

public static class BitfieldParser
{
    private const int BitfieldTypeArgumentIndex = 0;
    private const int BitOrderArgumentIndex = 1;

    private const int ByteTypeId = 0;
    private const int CharTypeId = 1;
    private const int UshortTypeId = 2;
    private const int UintTypeId = 3;
    private const int UlongTypeId = 4;

    private const int MsbBitOrderId = 0;
    private const int LsbBitOrderId = 1;

    public static ParsedBitfield ParseBitfield(AttributeData bitfieldAttributeData, string className, string visibility)
    {
        var bitfieldType =
            ParseBitfieldType((int)bitfieldAttributeData.ConstructorArguments[BitfieldTypeArgumentIndex].Value!);
        var order = ParseBitOrder((int)bitfieldAttributeData.ConstructorArguments[BitOrderArgumentIndex].Value!);

        return new ParsedBitfield
        {
            Name = className,
            Type = bitfieldType,
            Visibility = visibility,
            Order = order
        };
    }

    private static InternalTypeUtil.InternalType ParseBitfieldType(int bitfieldTypeId)
    {
        return bitfieldTypeId switch
        {
            ByteTypeId => InternalTypeUtil.InternalType.Byte,
            CharTypeId => InternalTypeUtil.InternalType.Char,
            UshortTypeId => InternalTypeUtil.InternalType.UShort,
            UintTypeId => InternalTypeUtil.InternalType.UInt,
            UlongTypeId => InternalTypeUtil.InternalType.ULong,
            _ => throw new Exception($"Invalid bitfield type id: {bitfieldTypeId}")
        };
    }

    private static ParsedBitfield.BitOrder ParseBitOrder(int bitOrderId)
    {
        return bitOrderId switch
        {
            MsbBitOrderId => ParsedBitfield.BitOrder.Msb,
            LsbBitOrderId => ParsedBitfield.BitOrder.Lsb,
            _ => throw new Exception($"Invalid bit order id: {bitOrderId}")
        };
    }
}
#pragma warning disable CS9113 // These fields only exist to provide metadata for the BitfieldGenerator.

namespace Bitfields.CSharp;

/// <summary>
/// Marks a class or struct as a bitfield.
/// </summary>
/// <remarks>
/// Example usage:
/// <code>
/// using System.Diagnostics;
/// using Bitfields.CSharp;
///
/// namespace Main;
///
/// // All fields in the bitfield must sum up to the number of bits of the bitfield type
/// // and the class or struct must be marked as partial.
/// [Bitfield(BitfieldType.ULong)]
/// public partial class Bitfield
/// {
///   // Fields without bits specified default to the size of the field type.
///   // 8 bits.
///   private byte byteField;
///
///   // A field can have specified bits, but the bits must be greater than zero
///   // and fit in the field type.
///   [Bits(4)] private byte smallByte; // A byte is 8 bits, so 4 bits is valid.
///
///   // A field that is signed, will be sign-extended by the most significant 
///   // bit of its type.
///   private sbyte signedByte;
///
///   // If you specify bits, the field will be sign-extended by the most significant
///   // bit of the specified bits. In this case, the most significant bit of 4 bits.
///   // Also signed fields are 2's complement, meaning this field with 4 bits has
///   // the value range of `-8` to `7`. You can add more bits to increase this
///   // range!
///   [Bits(4)] 
///   private sbyte smallSignedByte;
///
///   // A field can be a bool type.
///   private bool boolField;
///
///   // A field can have a default value, which must fit in the field type.
///   private byte fieldWithDefault = 0x1F;
///
///   // A field can have a default value and specified bits. The default value
///   // must fit in the specified bits or a compile error will occur.
///   [Bits(4)]
///   private byte fieldWithBitsDefault = 0xF;
///
///   // Nested bitfields are supported, but must have their bits and
///   // its type specified and implement the `static [CustomType] FromBits([type] bits)`
///   // and `[type] ToBits()` methods.
///   [Bits(3, CustomFieldType.Byte)] 
///   private NestedBitfield nestedField;
///
///   // Custom types are supported, but must have their bits and
///   // type specified and implement the `static [CustomType] FromBits([type] bits)`
///   // and `[type] ToBits()` methods.
///   [Bits(3, CustomFieldType.Byte)] 
///   private CustomType customType;
///
///   // Custom enum types are supported. All custom types are assumed ot be a class
///   // of struct, but enums must be specified using the `CustomFieldBase.Enum` param.
///   // The enum must have an enum named `BfBase` and a extension class that implements
///   // both the static `static [CustomEnumType] FromBits(this [CustomEnumType] _, byte val)`
///   // and `static [type] ToBits(this [CustomEnumType] val)` methods.
///   [Bits(3, CustomFieldType.Byte, CustomFieldBase.Enum)]
///   private CustomEnumType customEnumType;
///   
///   // Fields suffixed with "_" are padding fields, which are inaccessible.
///   [Bits(18)] 
///   private int padding_ = 0x3;
/// }
///
/// [Bitfield(BitfieldType.Byte)]
/// public partial struct NestedBitfield
/// {
///   private byte field;
/// }
///
/// // Custom types must have 2 methods, `static [CustomType] FromBits([type] bits)`
/// // and `[type] ToBits()` methods.
/// public struct CustomType
/// {
///   private byte health;
///
///   public CustomType(byte health)
///   {
///     this.health = health;
///   }
///
///   public static CustomType FromBits(byte bits)
///   {
///     return new CustomType(bits);
///   }
///
///   public byte ToBits()
///   {
///     return health;
///   }
/// }
///
/// // Custom enum types must have an enum named `BfBase`, its value doesn't matter.
/// public enum CustomEnumType {
///     BfBase = int.MaxValue,
///     A = 1,
///     B = 2,
///     C = 3,
/// }
///
/// // Custom enum types must have a extension class that implements the `FromBits` and `ToBits`.
/// public static class CustomEnumTypeExtension
/// {
///     public static CustomEnumType FromBits(this CustomEnumType _, byte val)
///     {
///         return val switch
///         {
///             1 => CustomEnumType.A,
///             2 => CustomEnumType.B,
///             3 => CustomEnumType.C,
///             _ => CustomEnumType.A
///         };
///     }
///
///     public static byte ToBits(this CustomEnumType val)
///     {
///         return (byte)val;
///     }
/// }
///
/// class Program
/// {
///   static void Main(string[] args)
///   {
///     // Usage:
///     // Creates a new bitfield using a builder pattern, unset fields default to 0 
///     // or their provided default value.
///     var bitfield = new Bitfield.BitfieldBuilder()
///       .WithByteField(5)
///       .WithSmallByte(0xF)
///       .WithCustomType(CustomType.FromBits(0x3))
///       .WithCustomEnumType(CustomEnumType.A)
///       .WithSignedByte(-5)
///       .WithSmallSignedByte(0xF)
///       .Build();
///
///     // var bitfield = new Bitfield(); // Bitfield with defaults
///     // var bitfield = new Bitfield(withoutDefaults: true); // Bitfield without defaults
///
///     // var builder = new Bitfield.BitfieldBuilder(); // Builder with defaults
///     // var builder = new Bitfield.BitfieldBuilder(withoutDefaults: true); // Builder without defaults
///
///     // var builder = bitfield.ToBuilder(); // Builder from existing bitfield
///
///     // Accessing fields:
///     var byteField = bitfield.GetByteField();
///     Debug.Assert(byteField == 5);
///     var smallSignedByte = bitfield.GetSmallSignedByte(); // Signed-types are sign-extended by the MSB.
///     Debug.Assert(smallSignedByte == -1);
///     var enumType = bitfield.GetCustomEnumType();
///     Debug.Assert(enumType == CustomEnumType.A);
///
///     // Converting into bits:
///     var bits = bitfield.ToBits();
///
///     // Converting from bits:
///     var bitfieldFromBits = Bitfield.FromBits(0x3); // Converts from bits
///     // var fromBitsWithDefaults = Bitfield.FromBitsWithDefaults(0x3); // Converts from bits with defaults
///
///     // Constants:
///     Debug.Assert(Bitfield.BYTE_FIELD_BITS == 8); // Number of bits of the field.
///     Debug.Assert(Bitfield.BOOL_FIELD_OFFSET == 0); // The offset of the field in the bitfield.
///   }
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class BitfieldAttribute(
    BitfieldType type,
    BitOrder bitOrder = BitOrder.Lsb) : Attribute;
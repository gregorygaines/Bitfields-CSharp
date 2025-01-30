# 🌻 Bitfields-CSharp

[![GitHub][github-badge]][github-url]
[![License][license-badge]][license-url]
[![Stars][stars-badge]][github-url]
[![Ko-fi][kofi-badge]][kofi-url]

[github-badge]: https://img.shields.io/badge/github-gregorygaines/Bitfields--CSharp-8da0cb?labelColor=555555&logo=github
[github-url]: https://github.com/gregorygaines/Bitfields-CSharp
[license-badge]: https://img.shields.io/github/license/Naereen/StrapDown.js.svg
[license-url]: #%EF%B8%8F-license
[stars-badge]: https://img.shields.io/github/stars/gregorygaines/Bitfields-CSharp?style=flat
[kofi-badge]: https://img.shields.io/badge/Ko--fi-FF5E5B?logo=kofi&logoColor=fff&style=flat
[kofi-url]: https://ko-fi.com/T6T07SXPV

A C# package that provides a source generator for generating bitfields from classes, structs, or
custom types, which is useful for defining schemas when working with low-level environments
or concepts (e.g. embedded or writing an emulator).

- Efficient and safe code like you would write by hand.
- No unsafe code or runtime dependencies.
- Compile-time checks for fields, types, and bits bounds checking.
- Supports most primitive and user-defined custom types.
- Signed fields are treated as 2's complement data types.

## 🔧 Usage

Bitfields is available as a NuGet package. You can install it using the following command:

```bash
nuget install Bitfields.CSharp
```

Or using the .NET Core command-line interface:

```bash
dotnet add package Bitfields.CSharp
```

## 🚀 Getting Started

You're emulating the N1nt3nd0 GameChild and come across the Display
Control Register (DISPCNT) which is an 8-bit register:

```text
  Bit   Expl.
  0-1   BG Mode                    (0-7=Video Mode)
  2-3   Display BG (0-1)           (0=BGx Off, 1=BGx On)
  4     OBJ Character VRAM Mapping (0=Two dimensional, 1=One dimensional)
  5-7   Always 0x3                 Padding
```

In table form, the bits are as follows:

| 7 - 5      | 4   | 3 -2       | 1 - 0   |
|------------|-----|------------|---------|
| Always 0x3 | OBJ | Display BG | BG Mode |

You can define the register as follows:

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.Byte)]
public partial class DisplayControl
{
  // We specify gg mode occupies the first 2 bits (0-1) of the bitfield
  // using the `[Bits]` attribute.
  [Bits(2)]
  private byte bgMode;

  // Custom type fields must implement the `FromBits` and `ToBits`
  // methods and declare its size and its type using the `[Bits]`
  // attribute.
  [Bits(2, CustomFieldType.Byte)] 
  private DisplayMode displayMode;

  // We can omit the `[Bits]` attribute for non-custom types, the generator
  // will assume the number of bits is the size of the field type. Here,
  // its 1 bit for a `bool` type.
  private bool objCharVramMapping;

  // Suffixing a field with "_" marks it as a padding field which
  // are inaccessible. Padding fields are 0 by default, unless a default value
  // is provided.
  [Bits(3)] 
  private byte always0X3Padding_ = 0x3;
}

/// <summary>
///  Implement the `FromBits` and `ToBits` methods for the custom type.
/// </summary>
public partial struct DisplayMode
{
  public bool bg0On;
  public bool bg1On;

  public DisplayMode(bool bg0On, bool bg1On)
  {
    this.bg0On = bg0On;
    this.bg1On = bg1On;
  }

  /// <summary>
  /// Convert bits to the custom type.
  /// </summary>
  public static DisplayMode FromBits(byte bits)
  {
    return new DisplayMode
    {
      bg0On = (bits & 0b01) != 0,
      bg1On = (bits & 0b10) != 0,
    };
  }

  /// <summary>
  /// Convert the custom type into bits.
  /// </summary>
  public byte ToBits()
  {
    byte bits = 0;
    if (bg0On) bits |= 0b01;
    if (bg1On) bits |= 0b10;
    return bits;
  }
}

class Program
{
  static void Main(string[] args)
  {
    // Creating the display mode custom type.
    var displayMode = new DisplayMode
    {
      bg0On = true,
      bg1On = false
    };

    // Building the display control
    var displayControl = new DisplayControl.DisplayControlBuilder()
      .WithBgMode(0b1)
      .WithDisplayMode(displayMode)
      .WithObjCharVramMapping(true)
      .Build();

    // Converting into bits.
    var bits = displayControl.ToBits();
    Debug.Assert(bits == 0b01110101);
  }
}
```

## 🤔 What Other Features Does Bitfields Offer?

Bitfields offers a wide range of features to help you define and work with bitfields.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

// All fields in the bitfield must sum up to the number of bits of the bitfield type
// and the class or struct must be marked as partial.
[Bitfield(BitfieldType.ULong)]
public partial class Bitfield
{
  // Fields without bits specified default to the size of the field type.
  // 8 bits.
  private byte byteField;

  // A field can have specified bits, but the bits must be greater than zero
  // and fit in the field type.
  [Bits(4)] private byte smallByte; // A byte is 8 bits, so 4 bits is valid.

  // A field that is signed, will be sign-extended by the most significant 
  // bit of its type.
  private sbyte signedByte;

  // If you specify bits, the field will be sign-extended by the most significant
  // bit of the specified bits. In this case, the most significant bit of 4 bits.
  // Also signed fields are 2's complement, meaning this field with 4 bits has
  // the value range of `-8` to `7`. You can add more bits to increase this
  // range!
  [Bits(4)] 
  private sbyte smallSignedByte;

  /// A field can be a bool type.
  private bool boolField;

  // A field can have a default value, which must fit in the field type.
  private byte fieldWithDefault = 0x1F;

  /// A field can have a default value and specified bits. The default value
  /// must fit in the specified bits or a compile error will occur.
  [Bits(4)]
  private byte fieldWithBitsDefault = 0xF;

  /// Nested bitfields are supported, but must have their bits and
  /// its type specified and implement the `static [CustomType] FromBits([type] bits)`
  /// and `[type] ToBits()` methods.
  [Bits(3, CustomFieldType.Byte)] 
  private NestedBitfield nestedField;

  /// Custom types are supported, but must have their bits and
  /// type specified and implement the `static [CustomType] FromBits([type] bits)`
  /// and `[type] ToBits()` methods.
  [Bits(3, CustomFieldType.Byte)] 
  private CustomType customType;

  /// Fields suffixed with "_" are padding fields, which are inaccessible.
  [Bits(21)] 
  private int padding_ = 0x3;
}

[Bitfield(BitfieldType.Byte)]
public partial struct NestedBitfield
{
  private byte field;
}

/// Custom types must have 2 methods, `static [CustomType] FromBits([type] bits)`
/// and `[type] ToBits()` methods.
public partial struct CustomType
{
  private byte a;

  public CustomType(byte a)
  {
    this.a = a;
  }

  public static CustomType FromBits(byte bits)
  {
    return new CustomType(bits);
  }

  public byte ToBits()
  {
    return a;
  }
}

class Program
{
  static void Main(string[] args)
  {
    // Usage:
    // Creates a new bitfield using a builder pattern, unset fields default to 0 
    // or their provided default value.
    var bitfield = new Bitfield.BitfieldBuilder()
      .WithByteField(5)
      .WithSmallByte(0xF)
      .WithCustomType(CustomType.FromBits(0x3))
      .WithSignedByte(-5)
      .WithSmallSignedByte(0xF)
      .Build();

    // var bitfield = new Bitfield(); // Bitfield with defaults
    // var bitfield = new Bitfield(withoutDefaults: true); // Bitfield without defaults

    // var builder = new Bitfield.BitfieldBuilder(); // Builder with defaults
    // var builder = new Bitfield.BitfieldBuilder(withoutDefaults: true); // Builder without defaults

    // var builder = bitfield.ToBuilder(); // Builder from existing bitfield

    // Accessing fields:
    var byteField = bitfield.GetByteField();
    Debug.Assert(byteField == 5);
    var smallSignedByte = bitfield.GetSmallSignedByte(); // Signed-types are sign-extended by the MSB.
    Debug.Assert(smallSignedByte == -1);

    // Converting into bits:
    var bits = bitfield.ToBits();

    // Converting from bits:
    var bitfieldFromBits = Bitfield.FromBits(0x3); // Converts from bits
    // var fromBitsWithDefaults = Bitfield.FromBitsWithDefaults(0x3); // Converts from bits with defaults

    // Constants:
    Debug.Assert(Bitfield.BYTE_FIELD_BITS == 8); // Number of bits of the field.
    Debug.Assert(Bitfield.BOOL_FIELD_OFFSET == 0); // The offset of the field in the bitfield.
  }
}
```

### Bitfield Types

A bitfield can represent unsigned types (`Byte`, `UShort`, `Char`, `UInt`, `ULong`). The field bits
of a bitfield  must add up to the number of bits of the bitfield type.

```csharp
using Bitfields.CSharp;

[Bitfield(BitfieldType.Byte)]
public partial class BitfieldByte
{
  private byte field;
}

[Bitfield(BitfieldType.UShort)]
public partial class BitfieldUShort
{
  private ushort field;
}

[Bitfield(BitfieldType.ULong)]
public partial class BitfieldULong
{
  private ulong field;
}
```

### Bitfield Field Types

A bitfield field can be any unsigned (`byte`, `short`, `char`, `uint`, `ulong`), signed
type (`sbyte`, `short`, `int`, `long`), or a custom type that implements the
methods `FromBits` and `ToBits`.

Signed types are treated as 2's complement data types, meaning the most significant
represents the sign bit. For example, if you had a field with 5 bits, the value range
would be `-16` to `15`. The more bits you include, the larger the value range.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt)]
public partial class Bitfield
{
  [Bits(8)]
  private byte a = 0xFF;
  [Bits(8)] 
  private sbyte b = -127;
  [Bits(4)] 
  private sbyte cSignExtended = 9;
  [Bits(8, CustomFieldType.Byte)]
  private CustomType customType = CustomType.FromBits(0x11);
  [Bits(4)]
  private byte padding_ = 0;
}

public class CustomType
{
  private byte val;

  private CustomType(byte val)
  {
    this.val = val;
  }
  
  public static CustomType FromBits(byte val)
  {
    return new CustomType(val);
  }

  public byte ToBits()
  {
    return val;
  }
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = new Bitfield();
    
    Debug.Assert(bitfield.GetA() == 0xFF);
    Debug.Assert(bitfield.GetB() == -127);
    Debug.Assert(bitfield.GetCSignExtended() == -7);
    Debug.Assert(bitfield.GetCustomType().ToBits() == CustomType.FromBits(0x11).ToBits());
  }
}
```

### Constructing a Bitfield

A bitfield can be constructed using its `Bitfield()` and `Bitfield(bool withoutDefaults)` constructors.
The former initializes the bitfield with default values, while the latter initializes the bitfield without default values,
except for padding fields which always keep their default value or 0.

A bitfield can also be constructed using a fluent builder pattern using the `new <Bitfield>Builder()` or
`new <Bitfield>Builder(bool withoutDefaults)` constructors. They operate the same as the `Bitfield()` and 
`Bitfield(bool withoutDefaults)` constructors.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt)]
public partial class Bitfield
{
  [Bits(8)]
  private byte a = 0x12;
  [Bits(8)] 
  private byte b = 0x34;
  [Bits(8)]
  private byte c = 0x56;
  [Bits(8)]
  private byte d_ = 0x78; // Padding field
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = new Bitfield();
    Debug.Assert(bitfield.GetA() == 0x12);
    Debug.Assert(bitfield.GetB() == 0x34);
    Debug.Assert(bitfield.GetC() == 0x56);
    Debug.Assert(bitfield.ToBits() == 0x78563412);

    var bitfieldWithoutDefaults = new Bitfield(withoutDefaults: true);
    Debug.Assert(bitfieldWithoutDefaults.GetA() == 0);
    Debug.Assert(bitfieldWithoutDefaults.GetB() == 0);
    Debug.Assert(bitfieldWithoutDefaults.GetC() == 0);
    Debug.Assert(bitfieldWithoutDefaults.ToBits() == 0x78000000);

    var customBitfield = new Bitfield.BitfieldBuilder()
      .WithA(0x12)
      .WithB(0x34)
      .WithC(0x56)
      .Build();
    Debug.Assert(customBitfield.GetA() == 0x12);
    Debug.Assert(customBitfield.GetB() == 0x34);
    Debug.Assert(customBitfield.GetC() == 0x56);
    Debug.Assert(customBitfield.ToBits() == 0x78563412);
  }
}
```

### To Builder

A constructed bitfield can be converted back to a builder using the `ToBuilder()` method.

```csharp
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt)]
public partial class Bitfield
{
  [Bits(8)]
  private byte a = 0x12;
  [Bits(8)] 
  private byte b = 0x34;
  [Bits(8)]
  private byte c = 0x56;
  [Bits(8)]
  private byte d_ = 0x78; // Padding field
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = new Bitfield();

    var builder = bitfield.ToBuilder();
  }
}

```

### Bitfield Conversions

A bitfield can be converted from bits using the `FromBits()` or `FromBitsWithoutDefaults()` methods. The former
ignores default values, while the latter respects them. Padding fields are always 0 or their default value. The
bitfield can also be converted to bits using the `ToBits()` function.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt)]
public partial class Bitfield
{
  [Bits(8)]
  private byte a = 0x12;
  [Bits(8, CustomFieldType.Byte)] 
  private CustomType b;
  private byte c;
  private byte d_ = 0x78;
}

public class CustomType
{
  private byte val;

  private CustomType(byte val)
  {
    this.val = val;
  }
  
  // Only takes the first bit of the val.
  public static CustomType FromBits(byte val)
  {
    return new CustomType((byte)(val & 1));
  }

  public byte ToBits()
  {
    return val;
  }
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = Bitfield.FromBits(0x11223344);
    Debug.Assert(bitfield.GetA() == 0x44);
    Debug.Assert(bitfield.GetB().ToBits() == 1);
    Debug.Assert(bitfield.GetC() == 0x22);
    Debug.Assert(bitfield.ToBits() == 0x78220144);
    
    var bitfieldRespectDefaults = Bitfield.FromBitsWithDefaults(0x11223344);
    Debug.Assert(bitfieldRespectDefaults.GetA() == 0x12); // Default value respected
    Debug.Assert(bitfieldRespectDefaults.GetB().ToBits() == 1);
    Debug.Assert(bitfieldRespectDefaults.GetC() == 0x22);
    Debug.Assert(bitfieldRespectDefaults.ToBits() == 0x78220112);
  }
}
```

### Field Order

By default, fields are ordered from the least significant bit (lsb) to the most significant bit (msb).
The order can be changed by specifying the second `BitOrder` arg on the bitfield attribute.
There are two field orderings, `BitOrder.Lsb` and `BitOrder.Msb`.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt, BitOrder.Msb)]
public partial class Bitfield
{
  [Bits(8)] 
  private byte a = 0x12;
  [Bits(8)] 
  private byte b = 0x34;
  [Bits(8)]
  private byte c = 0x56;
  [Bits(8)]
  private byte d = 0x78;
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = new Bitfield();
    Debug.Assert(bitfield.GetA() == 0x12);
    Debug.Assert(bitfield.GetB() == 0x34);
    Debug.Assert(bitfield.GetC() == 0x56);
    Debug.Assert(bitfield.GetD() == 0x78);
    var bits = bitfield.ToBits();
    
    //                      .- a
    //                      |  .- b
    //                      |  |  .- c
    //                      |  |  |  .- d
    Debug.Assert(bits == 0x12_34_56_78);
    Debug.Assert(Bitfield.A_OFFSET == 24); // Offset of the field in the bitfield.
  }
}
```

### Padding Fields

Fields suffixed with an underscore `_` are padding fields, which are inaccessible. Meaning the field is always
0/false or a default value. They are useful for padding the bits of a bitfield.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UShort)]
public partial class Bitfield
{
  [Bits(8)] 
  private byte a;
  [Bits(8)] 
  private byte padding_ = 0xFF;
}

class Program
{
  static void Main(string[] args)
  {
    var bitfield = new Bitfield();
    Debug.Assert(bitfield.GetA() == 0);
    // Padding field is inaccessible, so no direct access or setting.
    Debug.Assert(bitfield.ToBits() == 0xFF00);
  }
}

```

### Field Constants

Fields have constants generated for their number of bits and offset in the bitfield.

```csharp
using System.Diagnostics;
using Bitfields.CSharp;

namespace Main;

[Bitfield(BitfieldType.UInt)]
public partial class Bitfield
{
  [Bits(8)] 
  private byte a = 0x12;
  [Bits(8)] 
  private byte b = 0x34;
  [Bits(8)]
  private byte c = 0x56;
  [Bits(8)]
  private byte d = 0x78;
}

class Program
{
  static void Main(string[] args)
  {
    Debug.Assert(Bitfield.A_OFFSET == 0); // Number of bits of the a field.
    Debug.Assert(Bitfield.A_BITS == 8); // The offset of the a field in the bitfield.
    Debug.Assert(Bitfield.B_OFFSET == 8); // Number of bits of the b field.
    Debug.Assert(Bitfield.B_BITS == 8); // The offset of the b field in the bitfield.
    Debug.Assert(Bitfield.C_OFFSET == 16); // Number of bits of the c field.
    Debug.Assert(Bitfield.C_BITS == 8); // The offset of the c field in the bitfield.
    Debug.Assert(Bitfield.D_OFFSET == 24); // Number of bits of the d field.
    Debug.Assert(Bitfield.D_BITS == 8); // The offset of the d field in the bitfield.
  }
}
```

## ⚖️ License

Distributed under the MIT License. See [LICENSE](/LICENSE) for more information.

## 🤝 Contributing

Unless you explicitly state otherwise, any contribution intentionally submitted
for inclusion in the work by you, as defined in the MIT license, shall be licensed as
above, without any additional terms or conditions.

## 🥶 Employment Disclaimer

As of Jan 2025, I am a Google employee; "Bitfields" is my own work, not
affiliated with Google, its subsidiaries, nor endorsing any Google-owned
products or tools. "Bitfields" was written without any proprietary knowledge,
tools, or resources of Google.

## Other Versions

- [Rust - bitfields-rs](https://github.com/gregorygaines/bitfields-rs)
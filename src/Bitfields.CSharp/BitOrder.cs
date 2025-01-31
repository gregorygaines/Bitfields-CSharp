namespace Bitfields.CSharp;

/// <summary>
/// The order of the bits in the bitfield.
/// <remarks>
/// Msb means the top-most field represents the most significant bit and Lsb means the top-most field represents the least significant bit.
/// </remarks>
/// </summary>
public enum BitOrder
{
    Lsb,
    Msb
}
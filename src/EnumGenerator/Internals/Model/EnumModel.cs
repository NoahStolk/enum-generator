using EnumGenerator.Internals.Extensions;
using System.Numerics;

namespace EnumGenerator.Internals.Model;

internal sealed record EnumModel
{
	public EnumModel(
		string enumName,
		string enumTypeName,
		string namespaceName,
		string accessibility,
		List<EnumMemberModel> members,
		bool hasFlagsAttribute,
		string? generatedClassName,
		string enumUnderlyingTypeName,
		string binaryReaderMethodName)
	{
		EnumName = enumName;
		EnumTypeName = enumTypeName;
		NamespaceName = namespaceName;
		Accessibility = accessibility;
		HasFlagsAttribute = hasFlagsAttribute;
		GeneratedClassName = generatedClassName;
		EnumUnderlyingTypeName = enumUnderlyingTypeName;
		BinaryReaderMethodName = binaryReaderMethodName;

		if (hasFlagsAttribute)
		{
			UniqueMembers = members
				.Where(member => member.ConstantValue == 0 || IsPowerOfTwo(member.ConstantValue))
				.DistinctBy(m => m.ConstantValue)
				.ToList();
		}
		else
		{
			UniqueMembers = members.DistinctBy(m => m.ConstantValue).ToList();
		}

		static bool IsPowerOfTwo(BigInteger value)
		{
			return value != 0 && (value & (value - 1)) == 0;
		}
	}

	public string EnumName { get; }

	public string EnumTypeName { get; }

	public string NamespaceName { get; }

	public string Accessibility { get; }

	public bool HasFlagsAttribute { get; }

	public string? GeneratedClassName { get; }

	public string EnumUnderlyingTypeName { get; }

	public string BinaryReaderMethodName { get; }

	public List<EnumMemberModel> UniqueMembers { get; }
}

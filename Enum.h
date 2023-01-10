/*
 *  Copyright 2022 Fox Cutter
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

#pragma once
#include <type_traits>

// Using type traits to allow binary operations on Enum Class types.
// These work by casting down to the underlying type, then casting the result back up to the Enum type. 
// 
// On release builds, this only adds symantic overhead to the compiler, the resulting binary
// will be the same as for any other integer type (usually a single opcode)
template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator | (EnumType lhs, EnumType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) | static_cast<BaseType>(rhs));
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator & (EnumType lhs, EnumType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) & static_cast<BaseType>(rhs));
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator ^ (EnumType lhs, EnumType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) ^ static_cast<BaseType>(rhs));
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator ~ (EnumType lhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(~static_cast<BaseType>(lhs));
}

template<typename EnumType, typename ShiftType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator << (EnumType lhs, ShiftType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) << rhs);
}

template<typename EnumType, typename ShiftType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator >> (EnumType lhs, ShiftType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) >> rhs);
}



// Assignment Operators
template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator |= (EnumType& lhs, EnumType rhs)
{
	lhs = lhs | rhs;
	return lhs;
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator &= (EnumType& lhs, EnumType rhs)
{
	lhs = lhs & rhs;
	return lhs;
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator ^= (EnumType& lhs, EnumType rhs)
{
	lhs = lhs ^ rhs;
	return lhs;
}

template<typename EnumType, typename ShiftType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator <<= (EnumType& lhs, ShiftType rhs)
{
	lhs = lhs << rhs;
	return lhs;
}

template<typename EnumType, typename ShiftType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator >>= (EnumType& lhs, ShiftType rhs)
{
	lhs = lhs >> rhs;
	return lhs;
}


// Math operators
template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator - (EnumType lhs, EnumType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) - static_cast<BaseType>(rhs));
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator + (EnumType lhs, EnumType rhs)
{
	using BaseType = std::underlying_type_t <EnumType>;
	return static_cast<EnumType>(static_cast<BaseType>(lhs) + static_cast<BaseType>(rhs));
}


template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator ++ (EnumType& lhs) // ++lhs
{
	using BaseType = std::underlying_type_t <EnumType>;	
	lhs = static_cast<EnumType>(static_cast<BaseType>(lhs) + 1);
	return lhs;
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator ++ (EnumType& lhs, int)  // lhs++
{
	using BaseType = std::underlying_type_t <EnumType>;
	EnumType val = lhs;
	++lhs;
	return val;
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator -- (EnumType& lhs) // --lhs
{
	using BaseType = std::underlying_type_t <EnumType>;
	lhs = static_cast<EnumType>(static_cast<BaseType>(lhs) - 1);
	return lhs;
}

template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr EnumType operator -- (EnumType& lhs, int) // lhs++
{
	using BaseType = std::underlying_type_t <EnumType>;
	EnumType val = lhs;
	--lhs;
	return val;
}

// Generic test function
template<typename EnumType, std::enable_if_t<std::is_enum<EnumType>::value, bool> = true>
constexpr bool HasFlag(EnumType Value, EnumType Flag)
{
	return (Value & Flag) == Flag;
}

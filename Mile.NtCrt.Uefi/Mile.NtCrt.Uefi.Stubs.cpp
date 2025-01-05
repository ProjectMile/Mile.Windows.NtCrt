/*
 * PROJECT:   Mouri Internal Library Essentials
 * FILE:      Mile.NtCrt.Uefi.Stubs.cpp
 * PURPOSE:   Stubs for Windows Driver Kit C Runtime for UEFI
 *
 * LICENSE:   The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

#include <stdint.h>

namespace
{
    int g_errno = 0;
}

extern "C" int* __cdecl _errno()
{
    return &g_errno;
}

extern "C" void __cdecl _invalid_parameter_noinfo()
{
    // Do nothing.
}

extern "C" void __cdecl _invalid_parameter(
    wchar_t const* const expression,
    wchar_t const* const function_name,
    wchar_t const* const file_name,
    unsigned int const line_number,
    uintptr_t const reserved)
{
    expression;
    function_name;
    file_name;
    line_number;
    reserved;
}

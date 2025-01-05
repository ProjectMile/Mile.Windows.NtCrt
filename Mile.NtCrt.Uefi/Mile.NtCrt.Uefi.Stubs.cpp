/*
 * PROJECT:   Mouri Internal Library Essentials
 * FILE:      Mile.NtCrt.Uefi.Stubs.cpp
 * PURPOSE:   Stubs for Windows Driver Kit C Runtime for UEFI
 *
 * LICENSE:   The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

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

#ifndef _UINTPTR_T_DEFINED
#define _UINTPTR_T_DEFINED
#ifdef _WIN64
typedef unsigned __int64 uintptr_t;
#else
typedef unsigned int uintptr_t;
#endif
#endif

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

extern "C" __declspec(noreturn) void __cdecl __report_gsfailure(
    uintptr_t stack_cookie)
{
    stack_cookie;
    for (;;) {}
}

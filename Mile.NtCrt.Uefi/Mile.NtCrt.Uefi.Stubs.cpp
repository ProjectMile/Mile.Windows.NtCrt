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

__pragma(comment(linker, "/alternatename:__lookuptable_s=__lookuptable"));

#ifndef _SIZE_T_DEFINED
#ifdef  _WIN64
typedef unsigned __int64 size_t;
#else
typedef _W64 unsigned int size_t;
#endif
#define _SIZE_T_DEFINED
#endif

typedef int errno_t;

#ifndef EILSEQ
#define EILSEQ 42
#endif // !EILSEQ

extern "C" errno_t __cdecl wctomb_s(
    int* const return_value,
    char* const destination,
    size_t const destination_count,
    wchar_t const wchar)
{
    return_value;
    destination;
    destination_count;
    wchar;
    return EILSEQ;
}

extern "C" int __cdecl mbtowc(
    wchar_t* pwc,
    const char* s,
    size_t n)
{
    pwc;
    s;
    n;
    return -1;
}

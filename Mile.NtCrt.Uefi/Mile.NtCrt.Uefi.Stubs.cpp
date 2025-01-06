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

#ifndef EINVAL
#define EINVAL 22
#endif // !EINVAL

#ifndef ERANGE
#define ERANGE 34
#endif // !ERANGE

#ifndef EILSEQ
#define EILSEQ 42
#endif // !EILSEQ

// Workaround Implementation
// Only support Unicode Basic Multilingual Plane
extern "C" errno_t __cdecl wctomb_s(
    int* const return_value,
    char* const mbchar,
    size_t const mbchar_count,
    wchar_t const wchar)
{
    if (!mbchar && mbchar_count)
    {
        if (return_value)
        {
            *return_value = 0;
        }
        return 0;
    }

    if (return_value)
    {
        *return_value = -1;
    }

    if (wchar < 0x80)
    {
        if (mbchar_count < 1)
        {
            return ERANGE;
        }
        mbchar[0] = static_cast<char>(wchar);
        if (return_value)
        {
            *return_value = 1;
        }
        return 0;
    }
    else if (wchar < 0x800)
    {
        if (mbchar_count < 2)
        {
            return ERANGE;
        }
        mbchar[0] = static_cast<char>(0xC0 | (wchar >> 6));
        mbchar[1] = static_cast<char>(0x80 | (wchar & 0x3F));
        if (return_value)
        {
            *return_value = 2;
        }
        return 0;
    }
    else if (wchar < 0x10000)
    {
        if (mbchar_count < 3)
        {
            return ERANGE;
        }
        mbchar[0] = static_cast<char>(0xE0 | (wchar >> 12));
        mbchar[1] = static_cast<char>(0x80 | ((wchar >> 6) & 0x3F));
        mbchar[2] = static_cast<char>(0x80 | (wchar & 0x3F));
        if (return_value)
        {
            *return_value = 3;
        }
        return 0;
    }

    return EILSEQ;
}

// Workaround Implementation
// Only support Unicode Basic Multilingual Plane
extern "C" int __cdecl mbtowc(
    wchar_t* wchar,
    const char* mbchar,
    size_t count)
{
    if (!mbchar || !count)
    {
        return 0;
    }

    if (!*mbchar)
    {
        if (wchar)
        {
            *wchar = 0;
        }
        return 0;
    }

    if (mbchar[0] < 0x80)
    {
        if (wchar)
        {
            *wchar = mbchar[0];
        }
        return 1;
    }
    else if ((mbchar[0] & 0xE0) == 0xC0)
    {
        if ((count < 2) ||
            (mbchar[1] & 0xC0) != 0x80)
        {
            return -1;
        }
        if (wchar)
        {
            *wchar = (mbchar[0] & 0x1F) << 6;
            *wchar |= mbchar[1] & 0x3F;
        }
        return 2;
    }
    else if ((mbchar[0] & 0xF0) == 0xE0)
    {
        if ((count < 3) ||
            (mbchar[1] & 0xC0) != 0x80 ||
            (mbchar[2] & 0xC0) != 0x80)
        {
            return -1;
        }
        if (wchar)
        {
            *wchar = (mbchar[0] & 0x0F) << 12;
            *wchar |= (mbchar[1] & 0x3F) << 6;
            *wchar |= mbchar[2] & 0x3F;
        }
        return 3;
    }

    return -1;
}

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

extern "C" void __cdecl _invalid_parameter_noinfo()
{
    // Do nothing.
}

extern "C" int* __cdecl _errno()
{
    return &g_errno;
}

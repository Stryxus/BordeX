#pragma once

#include "pch.h"

extern "C"
{
	__declspec(dllexport) bool InitializeSteam(DWORD processID);
	__declspec(dllexport) void ShutdownSteam();
}

bool IsSteamInitialized = false;
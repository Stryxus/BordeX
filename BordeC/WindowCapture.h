#pragma once
#include "pch.h"

extern "C" {
	__declspec(dllexport) bool Initialize();
	__declspec(dllexport) bool ReInitialize();
	__declspec(dllexport) void Uninitialize();
	__declspec(dllexport) bool Update();
}
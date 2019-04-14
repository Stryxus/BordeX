#include "pch.h"
#include "Steam.h"

bool InitializeSteam(DWORD processID) {
	if (!IsSteamInitialized) {
		IsSteamInitialized = SteamAPI_Init();
		if (!IsSteamInitialized) {
			int returnValue = MessageBox(NULL, L"[Native] BordeX has failed to attach to steam!\n\nReasons for this are:\n\n1. Steam is not running\n2. Steam can not detect BordeX's SteamID\n3. Steam is not running under the administrator context\n\nFix Suggestions:\n\n1. Reboot your Computer\n2. Restart Steam\n3. Reinstall BordeX\n\nIf the error still persists, please post the issue on the BordeX's steam forum!", L"Steam failed to initialize!", MB_ICONEXCLAMATION | MB_RETRYCANCEL);
			switch (returnValue) {
			case IDRETRY:
				return InitializeSteam(processID);
				break;
			case IDCANCEL:
				MessageBox(NULL, L"[Native] Because BordeX requires Steam to function, BordeX will now shutdown!", L"Shutdown", MB_ICONINFORMATION | MB_OK);
				TerminateProcess(OpenProcess(PROCESS_TERMINATE, FALSE, processID), 0);
				return false;
				break;
			}
		}
	}
	return IsSteamInitialized;
}

void ShutdownSteam() {
	SteamAPI_Shutdown();
}
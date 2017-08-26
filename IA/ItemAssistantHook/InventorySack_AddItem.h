#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"
#include "GetPrivateStash.h"
#include <set>
#define MOD_FUNCTIONALITY_ENABLED 0
/************************************************************************
AddItem called from DLL:
 Option #1:
 => Send data to IA
 => Skip adding it (if IA is not running, this is bad)
 (This may be problematic if the caller code uses the ID for anything)


 Option #2:
 => Add item (dll code)
 => Send Item + Id to IA
 => Have IA store the item in a cache
 => Have IA ask for a deletion by Id
 => Calls RemoveItem(id)
 (May be problematic in quick place & pickup situations)
 (May be problematic if player closes the stash? obj deleted? maybe not since crafting uses it?)
 => Signal IA a successful removal, so IA can activate the item?


 Sending items to GD:
 => IA marks item as 'inactive'
 => IA signals hook
 => Check IsSpaceForItem
 => Call AddItem on dll
 => Call SetItemAddedWhileNotTheCurrentlySelectedInventoryTab
 => Signal IA successful add, so IA can delete the item
 : All inactives are activated on start, or after N seconds.

 How do I
 char __thiscall GAME::GameEngine::AddTransferSack(GAME::GameEngine *this)
 void GAME::GameEngine::SetSelectedTransferSackNumber(unsigned int)

 I can always know when a transfer is being created/destroyed via its (des)constructor
 So I will know if stash+2 still exists



bool GAME::InventorySack::AddItem(class GAME::Item *,bool,bool)
bool GAME::InventorySack::AddItem(class GAME::Vec2 const &,class GAME::Item *,bool)
bool GAME::InventorySack::FindNextPosition(class GAME::Item const *,class GAME::Rect &,bool)
bool GAME::InventorySack::IsSpaceForItem(class GAME::Item const *,bool)
class GAME::Vec2 GAME::InventorySack::AddItemAndReturnPoint(class GAME::Item *)

class GAME::Item * GAME::Item::CreateItem(struct GAME::ItemReplicaInfo const &)

void GAME::Character::GiveItemToCharacter(class GAME::Item *,bool)

bool GAME::Player::IsInventorySpaceAvailable(class GAME::Item const *)
void GAME::Player::GiveItemToCharacter(class GAME::Item *,bool)

bool GAME::InventorySack::RemoveItem(unsigned int)
bool GAME::Player::RemoveItemFromPrivateStash(unsigned int)
void GAME::Inventory::RemoveItemFromInventory(unsigned int)

GAME::InventorySack::SetItemAddedWhileNotTheCurrentlySelectedInventoryTab(bool)
/************************************************************************/

class ItemInjectorHook;
class InventorySack_AddItem : public BaseMethodHook {
public:
	InventorySack_AddItem();
	InventorySack_AddItem(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();

private:
	struct Vec2f {
		float x,y;
	};
	//static OriginalAttachItem dll_AttachItem;
	static int REPLICA_PTR_OFFSET;
	static DataQueue* m_dataQueue;
	static HANDLE m_hEvent;
	static void* m_gameEngine;
	static ItemInjectorHook itemInjector;
	static GetPrivateStash privateStashHook;

	static bool CanTransferItems();
	static bool IsTransferStash(void* stash, int idx);
	static int GetStashIndex(void* stash);
	static std::set<void*> inventorySacks;

#if MOD_FUNCTIONALITY_ENABLED
	static DataItemPtr GetModName(void* This);
	static std::string previousModSetting;

	static bool m_hasModName;
	static char m_modName[256];
#endif

	static int m_isHardcore;

	// void GAME::Item::GetItemReplicaInfo(struct GAME::ItemReplicaInfo &)
	typedef void* (__thiscall *Item_GetItemReplicaInfo)(void* This, void* replicaInfo);

	typedef int* (__thiscall *InventorySack_AddItem01)(void*, void* item, bool findPosition, bool playSound);
	typedef int* (__thiscall *InventorySack_AddItem02)(void*, Vec2f const &, void* item, bool);
	typedef int* (__thiscall *GameEngine_GetTransferSack)(void* ge, int idx);
	typedef int* (__thiscall *GameEngine_AddItemToTransfer_01)(void*, unsigned int, void* Vec2, unsigned int index, bool);
	typedef int* (__thiscall *GameEngine_AddItemToTransfer_02)(void*, unsigned int, unsigned int index, bool);
	typedef int(__thiscall *GameEngine_SetTransferOpen)(void*, bool);
	typedef int*(__thiscall *GameInfo_GameInfo_Param)(void*, void* info);
	typedef int*(__thiscall *GameInfo_SetHardcore)(void*, bool isHardcore);

	typedef int*(__thiscall *InventorySack_InventorySack)(void*);
	typedef int*(__thiscall *InventorySack_InventorySackParam)(void*, void* stdstring);
	typedef int*(__thiscall *InventorySack_Deconstruct)(void*);

	typedef bool(__thiscall *InventorySack_Sort)(void*, unsigned int unknown);

	typedef bool(__thiscall *GameInfo_GetHardcore)(void*);
	static GameInfo_GetHardcore dll_GameInfo_GetHardcore;


	typedef char* (__thiscall *GameEngine_GetGameInfo)(void* This);
	static GameEngine_GetGameInfo dll_GameEngine_GetGameInfo;


	
	static GameEngine_GetTransferSack dll_GameEngine_GetTransferSack;
	static InventorySack_AddItem01 dll_InventorySack_AddItem01;
	static InventorySack_AddItem02 dll_InventorySack_AddItem02;
	static GameEngine_AddItemToTransfer_01 dll_GameEngine_AddItemToTransfer_01;
	static GameEngine_AddItemToTransfer_02 dll_GameEngine_AddItemToTransfer_02;
	static GameEngine_SetTransferOpen dll_GameEngine_SetTransferOpen;
	static GameInfo_GameInfo_Param dll_GameInfo_GameInfo_Param;
	static GameInfo_SetHardcore dll_GameInfo_SetHardcore;
	static InventorySack_InventorySackParam dll_InventorySack_InventorySackParam;
	static InventorySack_InventorySack dll_InventorySack_InventorySack;
	static InventorySack_Deconstruct dll_InventorySack_Deconstruct;
	static InventorySack_Sort dll_InventorySack_Sort;
	static Item_GetItemReplicaInfo dll_GetItemReplicaInfo;


	// bool GAME::InventorySack::AddItem(class GAME::Item *,bool,bool)
	static void* __fastcall Hooked_InventorySack_AddItem_01(void* This, void* notUsed, void* item, bool findPosition, bool playSound);
	// bool GAME::InventorySack::AddItem(class GAME::Vec2 const &, class GAME::Item *, bool)
	static void* __fastcall Hooked_InventorySack_AddItem_02(void* This, void* notUsed, Vec2f const&, void* item, bool);

	// Previously in a separate class
	// void GAME::GameEngine::SetTransferOpen(bool)
	static void __fastcall Hooked_GameEngine_SetTransferOpen(void* This, void* notUsed, bool firstParam);



	// Game info is used to monitor IsHardcore and ModLabel
	//GAME::GameInfo::GameInfo(class GAME::GameInfo const &)
	static void* __fastcall Hooked_GameInfo_GameInfo_Param(void* This, void* notUsed, void* info);

	//void GAME::GameInfo::SetHardcore(bool)
	static void* __fastcall Hooked_GameInfo_SetHardcore(void* This, void* notUsed, bool isHardcore);


	//GAME::InventorySack::InventorySack(class GAME::InventorySack const &)
	static void* __fastcall Hooked_InventorySack_InventorySackParam(void* This, void* Other);
	//GAME::InventorySack::InventorySack(void)
	static void* __fastcall Hooked_InventorySack_InventorySack(void* This);
	//GAME::InventorySack::~InventorySack(void)
	static void* __fastcall Hooked_InventorySack_Deconstruct(void* This);

	static bool __fastcall Hooked_InventorySack_Sort(void* This, void* notUsed, unsigned int unknown);


	/* NEVER CALLED: To be removed */
	
	//bool GAME::GameEngine::AddItemToTransfer(unsigned int, class GAME::Vec2 const &, unsigned int, bool)
	static void* __fastcall Hooked_GameEngine_AddItemToTransfer_01(void* This, void* notUsed, unsigned int, void* Vec2, unsigned int index, bool);

	//bool GAME::GameEngine::AddItemToTransfer(unsigned int, unsigned int, bool)
	static void* __fastcall Hooked_GameEngine_AddItemToTransfer_02(void* This, void* notUsed, unsigned int, unsigned int index, bool);

	static int* __fastcall Hooked_GameEngine_GetTransferSack(void* This, void* discarded, int idx);
	
};
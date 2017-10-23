// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#include "JuanitoGameMode.h"
#include "JuanitoPlayerController.h"
#include "JuanitoCharacter.h"
#include "UObject/ConstructorHelpers.h"

AJuanitoGameMode::AJuanitoGameMode()
{
	// use our custom PlayerController class
	PlayerControllerClass = AJuanitoPlayerController::StaticClass();

	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/TopDownCPP/Blueprints/TopDownCharacter"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}
}
/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#pragma once

#include "stdbool.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <limits.h>

void LogV(char const* const Message, ...);
void Log(char const* const Message);

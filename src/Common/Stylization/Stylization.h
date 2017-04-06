//
//  Copyright (C) 2004-2011 by Autodesk, Inc.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of version 2.1 of the GNU Lesser
//  General Public License as published by the Free Software Foundation.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

#ifndef STYLIZATION_H_
#define STYLIZATION_H_

// OS specific DLL options
#include "StylizationAPI.h"

// basic definitions
#include "StylizationDefs.h"

// MDF headers
#include "MdfModel/MdfCommon.pb.h"
#include "MdfModel/LayerDefinition.pb.h"
#include "MdfModel/SymbolDefinition.pb.h"
#include "MdfModel/WatermarkDefinition.pb.h"

//#include "Base64.h"

// FDO headers
#pragma warning(push)
#pragma warning(disable: 4201)
#include "Fdo.h"
#include "FdoExpressionEngine.h"
#include "FdoExpressionEngineFunctionCollection.h"
#include "FdoExpressionEngineINonAggregateFunction.h"
#pragma warning(pop)


#ifndef __WFILE__
// Wide character version of __FILE__ macro
#define WIDEN2(x) L ## x
#define WIDEN(x) WIDEN2(x)
#define __WFILE__ WIDEN(__FILE__)
#endif

//Non-fatal FDO exception logging mechanism
//Implementation is defined in Stylizer.cpp
typedef void (*StylizerExceptionCallback)(FdoException* exception, int line, wchar_t* file);
void ProcessStylizerException(FdoException* exception);
STYLIZATION_API void SetStylizerExceptionCallback(StylizerExceptionCallback callbackFunction);

// avoid linux warnings
#ifndef _WIN32
static const void* avoid_warning1 = (void*)FDO_ACTIVELONGTRANSACTION;
static const void* avoid_warning2 = (void*)FDO_ROOTLONGTRANSACTION;
#endif

#endif

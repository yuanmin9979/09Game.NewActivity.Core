//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Enums/ItemActionType 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 16:49:24
//  公司：                          09game.com
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Enums
{
    internal enum ItemActionType
    {
        Query = 21,
        Add = 22,
        Sub = 23,
        SubRollback = 24,
        RollBack = 16
    }
}

﻿/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.MODEL;
using HotCoreUtils.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
    public class SystemLogic
    {

        private static string GetCacheKey(int type)
        {
            return "SYSTEM_MENU_" + type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0总后台菜单，1总门店菜单，2分店菜单</param>
        /// <returns></returns>
        public static List<SystemMenuModel> GetMenuList(int type)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                //读取缓存数据
                List<SystemMenuModel> menuData = WebCacheHelper<List<SystemMenuModel>>.Get(GetCacheKey(type));
                if (menuData == null)
                {
                    menuData = dal.GetMenuList(type);
                    //将数据插入缓存中
                    if (menuData != null)
                        WebCacheHelper.Insert(GetCacheKey(type), menuData, "bameng/cacheMenuKey_" + type);

                }

                return menuData;
            }
        }


        /// <summary>
        /// 添加我的位置
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="myLocation">My location.</param>
        /// <param name="lnglat">The lnglat.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool AddMyLocation(int userId, string myLocation, string lnglat)
        {
            using (var dal = FactoryDispatcher.SystemFactory())
            {
                return dal.AddMyLocation(userId, myLocation, lnglat);
            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    public class OrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ct_BelongId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ShopId { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        /// <value>The name of the shop.</value>
        public string ShopName { get; set; }

        /// <summary>
        /// 盟友姓名
        /// </summary>
        public string BelongOneName { get; set; }
        /// <summary>
        /// 盟主姓名
        /// </summary>
        public string BelongTwoName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime orderTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 订单状态 0 未成交 1 已成交 2退单
        /// </summary>
        /// <value>The order status.</value>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        /// <value>The name of the order status.</value>
        public string OrderStatusName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SuccessImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ct_Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal CashCouponAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CashCouponBn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FianlAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }
    }

    public class OrderListModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public long id;

        /// <summary>
        /// 图片地址
        /// </summary>
        public string pictureUrl { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 盟豆
        /// </summary>
        public decimal money { get; set; }

        /// <summary>
        /// 订单状态 0 未成交 1 已成交 2退单
        /// </summary>
        public int status { get; set; }

        public string orderId { get; set; }
        /// <summary>
        /// 订单状态名称
        /// </summary>
        /// <value>The name of the status.</value>
        public string statusName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value>The remark.</value>
        public string remark { get; set; }
        /// <summary>
        /// 补充说明
        /// </summary>
        /// <value>The note.</value>
        public string note { get; set; }
    }


    public class OrderDetailModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId;
        /// <summary>
        /// 下单时间
        /// </summary>
        public long orderTime;

        /// <summary>
        /// 图片地址
        /// </summary>
        public string pictureUrl { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string address { get; set; }


        /// <summary>
        /// 订单状态 0 未成交 1 已成交 2退单
        /// </summary>
        public int status { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        /// <value>The remark.</value>
        public string remark { get; set; }


        /// <summary>
        /// 补充说明
        /// </summary>
        /// <value>The note.</value>
        public string note { get; set; }


    }
}

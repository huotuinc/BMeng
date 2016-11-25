﻿using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// app 的摘要说明
    /// </summary>
    public class app : BaseLogicFactory, IHttpHandler
    {

        private HttpContext ctx { get; set; }

        /// <summary>
        /// 设置 <see cref="T:System.Web.UI.Page" /> 对象的内部服务器对象，如 <see cref="P:System.Web.UI.Page.Context" />、<see cref="P:System.Web.UI.Page.Request" />、<see cref="P:System.Web.UI.Page.Response" /> 和 <see cref="P:System.Web.UI.Page.Application" /> 属性。
        /// </summary>
        /// <param name="context">一个 <see cref="T:System.Web.HttpContext" /> 对象，它提供对用于为 HTTP 请求提供服务的内部服务器对象（如 <see cref="P:System.Web.HttpContext.Request" />、<see cref="P:System.Web.HttpContext.Response" />、<see cref="P:System.Web.HttpContext.Session" />）的引用。</param>
        public new void ProcessRequest(HttpContext context)
        {
            string resultMsg = string.Format(@"{0} header:{1} Form:{2} UserAgent:{3} IP:{4};referrer:{5}"
                 , context.Request.Url.ToString()
                 , context.Request.Headers.ToString()
                 , context.Request.Form.ToString()
                 , StringHelper.ToString(context.Request.UserAgent)
                 , StringHelper.GetClientIP()
                 , context.Request.UrlReferrer != null ? StringHelper.ToString(context.Request.UrlReferrer.AbsoluteUri) : ""
                );
            try
            {
                ctx = context;
                DoRequest(context);
                LogHelper.Log(resultMsg, LogHelperTag.INFO, WebConfig.debugMode());
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("{0} StackTrace:{1} Message:{2}", resultMsg, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
            }
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string action { get { return GetFormValue("action", ""); } }

        public string json { get; set; }

        public void DoRequest(HttpContext context)
        {
            try
            {
                switch (action.ToUpper())
                {
                    case "ALLYAPPLY":
                        AllyApply();
                        break;
                    case "MYQRCODE":
                        MyQrcode();
                        break;
                    case "GETCOUPONINFO":
                        GetCouponInfo();
                        break;
                    case "COUPONGET":
                        CouponGet();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("action:{0} StackTrace:{1} Message:{2}", action, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        /// <summary>
        /// 盟友注册申请
        /// </summary>
        private void AllyApply()
        {

            int uid = GetFormValue("userid", 0);
            string nickName = GetFormValue("nickname", "");
            string pwd = GetFormValue("pwd", "");
            string username = GetFormValue("username", "");
            string usermobile = GetFormValue("usermobile", "");
            int sex = GetFormValue("sex", 0);
            if (uid > 0)
            {
                ApiStatusCode code = ApiStatusCode.OK;
                bool flag = UserLogic.AllyApply(uid, usermobile, pwd, nickName, username, sex, ref code);
                json = JsonHelper.JsonSerializer(new ResultModel(code));
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.账户已存在));
        }


        /// <summary>
        /// 我的二维码
        /// </summary>
        private void MyQrcode()
        {
            string auth = GetFormValue("auth", "");
            int userId = 0;
            if (!string.IsNullOrEmpty(auth))
            {
                userId = UserLogic.GetUserIdByAuthToken(auth);
                if (userId == 0)
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.令牌失效));
                else
                {
                    string sourceFileName = Server.MapPath("/app/myqrcodetemplate.html");
                    string destPath = Server.MapPath(string.Format("/resource/app/qrcode/{0}", userId));
                    if (!Directory.Exists(destPath))
                        Directory.CreateDirectory(destPath);
                    File.Copy(sourceFileName, destPath + "/index.html", true);
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
                }
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.令牌失效));
        }


        /// <summary>
        /// 领取优惠券
        /// </summary>
        private void CouponGet()
        {
            try
            {
                int uid = GetFormValue("userid", 0);
                int cpid = GetFormValue("cpid", 0);
                string username = GetFormValue("username", "");
                string usermobile = GetFormValue("usermobile", "");
                string requestSign = GetFormValue("sign", "");

                Dictionary<string, string> paramters = new Dictionary<string, string>();
                paramters.Add("userid", uid.ToString());
                paramters.Add("cpid", cpid.ToString());
                string currentSign = SignatureHelper.BuildSign(paramters, ConstConfig.SECRET_KEY);
                if (!requestSign.Equals(currentSign))
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.未授权));
                else
                {
                    CashCouponLogModel logModel = CouponLogic.GetCashCouponLogIDByUserID(uid, cpid);
                    if (logModel != null)
                    {
                        bool flag = CouponLogic.UpdateUserCashCouponGetLog(new CashCouponLogModel()
                        {
                            UserId = uid,
                            ID = logModel.ID,
                            Name = username,
                            Mobile = usermobile
                        });
                        if (flag)
                        {
                            var user = UserLogic.GetModel(uid);
                            if (user != null)
                            {
                                CashCouponModel model = CouponLogic.GetModel(cpid, true);
                                //添加优惠券领取操作日志
                                LogLogic.AddCouponLog(new LogBaseModel()
                                {
                                    UserId = user.UserId,
                                    ShopId = logModel.ShopId,
                                    OperationType = 1,//0创建 1领取 2使用
                                    Money = logModel.Money
                                });
                            }
                        }
                        if (flag)
                            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.现金券已领完));
                        else
                            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.SERVICEERROR));
                    }
                    else
                    {
                        json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.现金券已领完));
                    }
                }

            }
            catch (Exception ex)
            {
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.现金券已领完));
            }

        }



        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        private void GetCouponInfo()
        {
            int uid = GetFormValue("userid", 0);
            int cpid = GetFormValue("cpid", 0);
            string requestSign = GetFormValue("sign", "");
            Dictionary<string, string> paramters = new Dictionary<string, string>();
            paramters.Add("userid", uid.ToString());
            paramters.Add("cpid", cpid.ToString());
            string currentSign = SignatureHelper.BuildSign(paramters, ConstConfig.SECRET_KEY);
            if (!requestSign.Equals(currentSign))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.未授权));
            else
            {
                // CashCouponModel model = CouponLogic.GetModel(cpid, true);
                CashCouponLogModel model = CouponLogic.GetCashCouponLogIDByUserID(uid, cpid);
                if (model != null)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data["time"] = model.StartTime.ToString("yyyy.MM.dd") + "-" + model.EndTime.ToString("yyyy.MM.dd");
                    data["money"] = model.Money;
                    data["url"] = "http://" + ctx.Request.Url.Host + string.Format("/app/getcoupon.html?userid={0}&cpid={1}&sign={2}", uid, cpid, currentSign);
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
                }
                else
                {
                    json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.现金券已领完));
                }
            }
        }
    }
}